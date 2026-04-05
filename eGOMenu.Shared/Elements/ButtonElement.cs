using System.Drawing;
using CounterStrikeSharp.API.Core;
using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared.Elements;

/// <summary>
/// Represents a clickable button in the menu.  Buttons are selectable when
/// enabled and invoke a callback when activated.
/// </summary>
public class ButtonElement(string text, Action<CCSPlayerController> onPressed,
  Func<CCSPlayerController, bool>? enabledPredicate = null,
  MenuStyle style = MenuStyle.NONE, Color? color = null) : IMenuElement {
  private readonly Action<CCSPlayerController> onPressed =
    onPressed ?? throw new ArgumentNullException(nameof(onPressed));

  private readonly Color color = color ?? Color.White;

  public MenuItemType Type => MenuItemType.BUTTON;

  public bool IsEnabled
    => currentPlayer != null
      && (enabledPredicate?.Invoke(currentPlayer) ?? true);

  public bool IsSelectable => IsEnabled;

  // The current player is captured before rendering to evaluate IsEnabled.
  private CCSPlayerController? currentPlayer;

  public string Render(int selectableIndex) {
    // Build HTML with index indicator.  If the element is disabled then use a
    // disabled color (grey) and do not include the index.
    var label  = text;
    var prefix = selectableIndex > 0 ? $"!{selectableIndex} " : string.Empty;
    if (!IsEnabled) {
      var disabledHex = ColorTranslator.ToHtml(Color.Gray);
      return $"<font color='{disabledHex}'>{label}</font>";
    }

    label = style switch {
      // Apply style
      MenuStyle.BOLD   => "<b>" + label + "</b>",
      MenuStyle.ITALIC => "<i>" + label + "</i>",
      MenuStyle.MONO => "<span style='font-family: monospace'>" + label
        + "</span>",
      _ => label
    };
    var hex = ColorTranslator.ToHtml(color);
    label = $"<font color='{hex}'>{label}</font>";
    return prefix + label;
  }

  public void Activate(CCSPlayerController player, string? input = null) {
    if (IsEnabled) { onPressed(player); }
  }

  /// <summary>
  /// Internal method called by the menu manager to set the current player
  /// before rendering.  This allows the element to evaluate dynamic states
  /// (e.g. IsEnabled) on a per‑player basis.
  /// </summary>
  internal void prepareForRender(CCSPlayerController player) {
    currentPlayer = player;
  }
}