using System.Drawing;
using CounterStrikeSharp.API.Core;
using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared.Elements;

/// <summary>
/// Represents a menu element that prompts the player for textual input.  When
/// the element is selected, the menu manager should switch to an input mode
/// (e.g. capture the next chat message) and then call Activate with the
/// submitted text.
/// </summary>
public class InputElement(string label, string placeholder,
  Action<CCSPlayerController, string> onSubmitted,
  Func<CCSPlayerController, bool>? enabledPredicate = null, Color? color = null)
  : IMenuElement {
  private readonly Color color = color ?? Color.Cyan;

  private CCSPlayerController? currentPlayer;

  public MenuItemType Type => MenuItemType.INPUT;

  public bool IsEnabled
    => currentPlayer != null
      && (enabledPredicate?.Invoke(currentPlayer) ?? true);

  public bool IsSelectable => IsEnabled;

  public string Render(int selectableIndex) {
    var prefix = selectableIndex > 0 ?
      "!" + selectableIndex + " " :
      string.Empty;
    var hex     = ColorTranslator.ToHtml(color);
    var content = string.IsNullOrEmpty(label) ? placeholder : label;
    if (IsEnabled)
      return prefix + "<font color='" + hex + "'>" + content + "</font>";
    var disabledHex = ColorTranslator.ToHtml(Color.Gray);
    return "<font color='" + disabledHex + "'>" + content + "</font>";
  }

  public void Activate(CCSPlayerController player, string? input = null) {
    if (!IsEnabled) return;
    // If input is null then we expect the manager to capture a subsequent message.
    if (input != null) { onSubmitted(player, input); }
  }

  internal void prepareForRender(CCSPlayerController player) {
    currentPlayer = player;
  }
}