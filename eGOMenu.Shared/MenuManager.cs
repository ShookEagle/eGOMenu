using CounterStrikeSharp.API.Core;
using eGOMenu.Shared.Enums;
using eGOMenu.Shared.Listeners;

namespace eGOMenu.Shared;

/// <summary>
/// Maintains active menu contexts for all players and routes input to the
/// appropriate menu.  This class is a singleton because only one instance
/// should manage menus on the server at any time.
/// </summary>
public class MenuManager : IDisposable {
  public static MenuManager Instance { get; } = new MenuManager();

  private readonly Dictionary<CCSPlayerController, MenuContext>
    contexts = new();

  private MenuManager() { PlayerButtonsListener.Register(); }

  /// <summary>
  /// Returns true if there is an active menu open for the specified player.
  /// </summary>
  internal bool hasActiveMenu(CCSPlayerController player)
    => contexts.ContainsKey(player);

  /// <summary>
  /// Attempts to get the active menu context for the specified player.
  /// </summary>
  internal bool tryGetContext(CCSPlayerController player,
    out MenuContext context)
    => contexts.TryGetValue(player, out context);


  /// <summary>
  /// Opens the specified menu for the player.  If a menu is already open
  /// for this player it will be closed first.
  /// </summary>
  public void OpenMenu(CCSPlayerController player, WASDMenu menu) {
    if (contexts.TryGetValue(player, out var existing)) { existing.Close(); }

    var context = new MenuContext(player, menu);
    contexts[player] = context;
    context.Render();
  }

  /// <summary>
  /// Routes a button press to the active menu for the given player.  If
  /// there is no menu open for the player, this method does nothing.
  /// </summary>
  public void HandleButtonPress(CCSPlayerController player, MenuButton button,
    string? input = null) {
    if (contexts.TryGetValue(player, out var context)) {
      context.OnButton(button, input);
    }
  }

  /// <inheritdoc/>
  public void Dispose() {
    foreach (var context in contexts.Values) { context.Close(); }

    contexts.Clear();
  }
}