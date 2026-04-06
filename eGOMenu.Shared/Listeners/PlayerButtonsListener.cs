using System.Runtime.CompilerServices;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared.Listeners;

// I understand how utterly obese this looks, and I'm sorry. But this is the most
// server performant way with multiple open menus I could think of to implement
// the 'hold to activate' behavior. Enjoy this brainfuck for a QOL feature 
// that no one will even notice :D
public class PlayerButtonsListener {
  public readonly Dictionary<int, ButtonHoldState> Held = new();
  private readonly List<int> removeBuffer = [];

  private const int INITIAL_DELAY_MS = 300;
  private const int REPEAT_DELAY_MS = 90;

  private static long Now => Environment.TickCount64;

  public PlayerButtonsListener Register() {
    NativeAPI.AddListener("OnPlayerButtonsChanged",
      FunctionReference.Create(OnPlayerButtonsChanged));
    NativeAPI.AddListener("OnTick", FunctionReference.Create(onTick));
    return this;
  }


  private void OnPlayerButtonsChanged(CCSPlayerController player,
    PlayerButtons oldButtons, PlayerButtons newButtons) {
    if (!player.IsValid) return;

    if (!MenuManager.Instance.hasActiveMenu(player) || newButtons == 0) {
      Held.Remove(player.Slot);
      return;
    }

    var pressedButtons = newButtons & ~oldButtons;

    if (!tryResolveMenuButton(pressedButtons, out var menuBtn, out var flag))
      return;

    Held[player.Slot] =
      new ButtonHoldState { Button = flag, StartTime = Now, RepeatCount = 0 };

    MenuManager.Instance.HandleButtonPress(player, menuBtn);
  }

  private void onTick() {
    if (Held.Count == 0) return;

    var now = Now;
    removeBuffer.Clear();

    foreach (var pair in Held) {
      var slot = pair.Key;
      var hold = pair.Value;

      var player = Utilities.GetPlayerFromSlot(slot);

      if (player == null || !player.IsValid
        || !MenuManager.Instance.hasActiveMenu(player)) {
        removeBuffer.Add(slot);
        continue;
      }

      var current = player.Buttons;

      if ((current & hold.Button) == 0) {
        removeBuffer.Add(slot);
        continue;
      }

      var heldTime = now - hold.StartTime;
      if (heldTime < INITIAL_DELAY_MS) continue;

      var repeat = (int)((heldTime - INITIAL_DELAY_MS) / REPEAT_DELAY_MS);
      if (repeat <= hold.RepeatCount) continue;

      if (!tryResolveMenuButton(hold.Button, out var menuBtn, out _)) continue;
      MenuManager.Instance.HandleButtonPress(player, menuBtn);

      hold.RepeatCount = repeat;
      Held[slot]       = hold;
    }

    foreach (var t in removeBuffer) Held.Remove(t);
  }

  // Fastest Resolver I Could create
  // Fighting hard to avoid 64/64 player lag
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static bool tryResolveMenuButton(PlayerButtons buttons,
    out MenuButton menuButton, out PlayerButtons matchedFlag) {
    foreach (var (btn, flag) in ButtonMappings.Map) {
      if ((buttons & flag) == 0) continue;

      menuButton  = btn;
      matchedFlag = flag;
      return true;
    }

    menuButton  = default;
    matchedFlag = 0;
    return false;
  }
}

public struct ButtonHoldState {
  public PlayerButtons Button;
  public long StartTime;
  public int RepeatCount;
}