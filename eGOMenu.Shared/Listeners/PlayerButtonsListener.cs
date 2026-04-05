using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared.Listeners;

public static class PlayerButtonsListener {
  public static void Register()
    => NativeAPI.AddListener("OnPlayerButtonsChanged",
      FunctionReference.Create(OnPlayerButtonsChanged));


  private static void OnPlayerButtonsChanged(CCSPlayerController player,
    PlayerButtons oldButtons, PlayerButtons newButtons) {
    // Only handle input if the player currently has an open menu
    if (!MenuManager.Instance.hasActiveMenu(player)) return;

    foreach (var (menuButton, requiredFlag) in ButtonMappings
     .MENU_TO_PLAYER_BUTTON) {
      if ((newButtons & requiredFlag) == 0) continue;
      MenuManager.Instance.HandleButtonPress(player, menuButton);
      return;
    }
  }
}