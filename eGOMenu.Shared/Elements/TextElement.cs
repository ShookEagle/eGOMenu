using System.Drawing;
using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared.Elements;

/// <summary>
/// Represents a read‑only text label.  Text elements are non‑selectable and
/// cannot be activated by the user.  A text element may specify a custom
/// colour and style which will be applied by the renderer.
/// </summary>
public class TextElement(string text, MenuStyle style, Color color)
  : IMenuElement {
  public MenuItemType Type => MenuItemType.TEXT;

  public bool IsEnabled => false;

  public bool IsSelectable => false;

  public string Render(int selectableIndex) {
    // Build the HTML fragment.  Non‑selectable elements ignore the index.
    var html = text;
    html = style switch {
      // Apply style tags
      MenuStyle.BOLD   => "<b>" + html + "</b>",
      MenuStyle.ITALIC => "<i>" + html + "</i>",
      MenuStyle.MONO => "<span style='font-family: monospace'>" + html
        + "</span>",
      _ => html
    };
    // Apply colour
    var hex = ColorTranslator.ToHtml(color);
    html = $"<font color='{hex}'>{html}</font>";
    return html;
  }

  public void Activate(CounterStrikeSharp.API.Core.CCSPlayerController player,
    string? input = null) {
    // Text elements have no activation.
  }
}