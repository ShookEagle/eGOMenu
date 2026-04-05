using System.Drawing;
using CounterStrikeSharp.API.Core;
using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared.Elements;

/// <summary>
/// Represents a toggleable boolean value.  When activated, the toggle flips its
/// state and invokes a callback.  The current state is obtained from a
/// delegate so that values can be stored externally.
/// </summary>
public class ToggleElement(string label,
  Func<CCSPlayerController, bool> getState,
  Action<CCSPlayerController, bool> onChanged,
  Func<CCSPlayerController, bool>? enabledPredicate = null,
  Color? onColor = null, Color? offColor = null) : IMenuElement {
  private readonly Color onColor = onColor ?? Color.Lime;
  private readonly Color offColor = offColor ?? Color.Red;

  private CCSPlayerController? currentPlayer;

  public MenuItemType Type => MenuItemType.CHOICE;

  public bool IsEnabled
    => currentPlayer != null
      && (enabledPredicate?.Invoke(currentPlayer) ?? true);

  public bool IsSelectable => IsEnabled;

  public string Render(int selectableIndex) {
    if (currentPlayer == null) { return string.Empty; }

    var state     = getState(currentPlayer);
    var indicator = state ? "[X]" : "[ ]";
    var color     = state ? onColor : offColor;
    var hex       = ColorTranslator.ToHtml(color);
    var prefix = selectableIndex > 0 ?
      "!" + selectableIndex + " " :
      string.Empty;
    if (IsEnabled)
      return prefix + "<font color='" + hex + "'>" + indicator + "</font> "
        + label;
    var disabledHex = ColorTranslator.ToHtml(Color.Gray);
    return "<font color='" + disabledHex + "'>" + indicator + " " + label
      + "</font>";
  }

  public void Activate(CCSPlayerController player, string? input = null) {
    if (!IsEnabled) return;
    var current = getState(player);
    var next    = !current;
    onChanged(player, next);
  }

  internal void prepareForRender(CCSPlayerController player) {
    currentPlayer = player;
  }
}