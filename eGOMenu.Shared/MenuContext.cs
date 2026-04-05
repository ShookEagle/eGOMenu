using CounterStrikeSharp.API.Core;
using eGOMenu.Shared.Enums;
using eGOMenu.Shared.Rendering;

namespace eGOMenu.Shared;

/// <summary>
/// Encapsulates the runtime state of a menu for a particular player.  It
/// tracks the current page and selection index, renders the menu, and
/// activates elements when the user interacts.
/// </summary>
internal class MenuContext(CCSPlayerController player, WASDMenu menu) {
  private int page;

  private int
    selectedIndex; // zero‑based index within selectable items on the current page

  /// <summary>
  /// Renders the current menu state to the player's center screen using the
  /// underlying API (PrintToCenterHtml).
  /// </summary>
  public void Render() {
    var html = HtmlRenderer.Render(player, menu, page, selectedIndex);
    // Use PrintToCenterHtml to display the menu.  If the API is not available
    // at compile time, consumers will need to provide an implementation via
    // dependency injection.
    player.PrintToCenterHtml(html);
  }

  /// <summary>
  /// Responds to a button press from the player.
  /// </summary>
  public void OnButton(MenuButton button, string? input) {
    switch (button) {
      case MenuButton.UP:
        moveSelection(-1);
        break;
      case MenuButton.DOWN:
        moveSelection(1);
        break;
      case MenuButton.LEFT:
        changePage(-1);
        break;
      case MenuButton.RIGHT:
        changePage(1);
        break;
      case MenuButton.SELECT:
        activateSelected(input);
        break;
      case MenuButton.BACK:
      case MenuButton.EXIT:
        Close();
        break;
    }

    Render();
  }

  /// <summary>
  /// Closes the menu and clears it from the player's screen.
  /// </summary>
  public void Close() {
    // Clear the center message by sending an empty string.  Do not route
    // the exit button back into the manager to avoid recursion.
    player.PrintToCenterHtml(string.Empty);
  }

  private void moveSelection(int delta) {
    var selectableCount = countSelectableInPage(page);
    if (selectableCount == 0) return;
    selectedIndex = (selectedIndex + delta + selectableCount) % selectableCount;
  }

  private void changePage(int delta) {
    var totalPages = (menu.Lines.Count + menu.LinesPerPage - 1)
      / menu.LinesPerPage;
    page          = (page + delta + totalPages) % totalPages;
    selectedIndex = 0;
  }

  private int countSelectableInPage(int currentPage) {
    var start = currentPage * menu.LinesPerPage;
    var end   = Math.Min(start + menu.LinesPerPage, menu.Lines.Count);
    var count = 0;
    for (var i = start; i < end; i++)
      count += menu.Lines[i].Elements.Count(elem => elem.IsSelectable);

    return count;
  }

  private void activateSelected(string? input) {
    var targetIndex  = selectedIndex;
    var start        = page * menu.LinesPerPage;
    var end          = Math.Min(start + menu.LinesPerPage, menu.Lines.Count);
    var currentIndex = 0;
    for (var i = start; i < end; i++) {
      foreach (var elem in menu.Lines[i].Elements) {
        if (!elem.IsSelectable) continue;
        if (currentIndex == targetIndex) {
          elem.Activate(player, input);
          return;
        }

        currentIndex++;
      }
    }
  }
}