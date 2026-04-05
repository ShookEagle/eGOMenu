using System.Text;
using CounterStrikeSharp.API.Core;
using eGOMenu.Shared.Elements;

namespace eGOMenu.Shared.Rendering;

/// <summary>
/// Responsible for converting a <see cref="WASDMenu"/> and the current menu state
/// into an HTML string suitable for printing with CPrintToCenter.  This class
/// encapsulates the rendering rules such as line breaks, colours and
/// page indicators.
/// </summary>
public static class HtmlRenderer {
  /// <summary>
  /// Renders the given menu for the specified page and selectable index.
  /// </summary>
  /// <param name="player">The player this menu is being rendered for.</param>
  /// <param name="menu">The menu instance.</param>
  /// <param name="page">The zero‑based page index.</param>
  /// <param name="selectedIndex">The zero‑based index of the selected element within the page.</param>
  public static string Render(CCSPlayerController player, WASDMenu menu,
    int page, int selectedIndex) {
    var sb = new StringBuilder();
    // Title
    sb.Append("<center><b>").Append(menu.Title).Append("</b><br>");
    int startLine = page * menu.LinesPerPage;
    int endLine = Math.Min(startLine + menu.LinesPerPage, menu.Lines.Count);
    int selectableIndex = 1;
    // Render lines
    for (int i = startLine; i < endLine; i++) {
      var line = menu.Lines[i];
      foreach (var element in line.Elements) {
        // Inform the element of the current player if it implements prepare
        if (element is ButtonElement b)
          b.prepareForRender(player);
        else if (element is ToggleElement t)
          t.prepareForRender(player);
        else if (element is InputElement input) input.prepareForRender(player);
        // Determine index for selectable items
        int indexForElement = element.IsSelectable ? selectableIndex++ : 0;
        sb.Append(element.Render(indexForElement));
      }

      // After each line add a break
      sb.Append("<br>");
    }

    // Page indicator line (e.g. "Page 2/3")
    int totalPages =
      (menu.Lines.Count + menu.LinesPerPage - 1) / menu.LinesPerPage;
    if (totalPages > 1) {
      int currentPage = page + 1;
      sb.Append("Page ")
       .Append(currentPage)
       .Append("/")
       .Append(totalPages)
       .Append("<br>");
    }

    // Footer
    if (!string.IsNullOrEmpty(menu.Footer)) { sb.Append(menu.Footer); }

    sb.Append("</center>");
    return sb.ToString();
  }
}