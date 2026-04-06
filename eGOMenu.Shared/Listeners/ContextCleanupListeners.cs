using CounterStrikeSharp.API.Core;

namespace eGOMenu.Shared.Listeners;

public class ContextCleanupListeners {
  public static void Register() {
    NativeAPI.HookEvent("player_disconnect",
      FunctionReference.Create(OnPlayerDisconnect), false);

    NativeAPI.AddListener("OnMapEnd", FunctionReference.Create(OnMapEnd));
  }

  // Close any open menus when a player disconnects.
  private static void OnPlayerDisconnect(EventPlayerDisconnect @event) {
    if (@event.Userid == null
      || MenuManager.Instance.hasActiveMenu(@event.Userid))
      return;
    MenuManager.Instance.CloseMenu(@event.Userid);
  }
  
  // Close any open menus when the map ends.
  private static void OnMapEnd() { MenuManager.Instance.CloseAllMenus(); }
}