using CounterStrikeSharp.API.Core;
using eGOMenu.Shared.Enums;
using eGOMenu.Shared.Rendering;

namespace eGOMenu.Shared;

/// <summary>
/// Encapsulates the runtime state of a menu for a particular player. It
/// tracks a stack of menu frames, renders the current frame, and activates
/// elements when the user interacts.
/// </summary>
internal sealed class MenuContext {
  private readonly CCSPlayerController player;
  private readonly Stack<MenuFrame> frames = new();

  private MenuFrame CurrentFrame => frames.Peek();
  private WASDMenu CurrentMenu => CurrentFrame.Menu;

  private int CurrentPage {
    get => CurrentFrame.Page;
    set => CurrentFrame.Page = value;
  }

  private int CurrentSelectedIndex {
    get => CurrentFrame.SelectedIndex;
    set => CurrentFrame.SelectedIndex = value;
  }

  public MenuContext(CCSPlayerController player, WASDMenu menu) {
    this.player = player;
    frames.Push(new MenuFrame(menu));
  }

  /// <summary>
  /// Opens a submenu and pushes it onto the frame stack.
  /// </summary>
  public void OpenSubMenu(WASDMenu subMenu) {
    frames.Push(new MenuFrame(subMenu));
    Render();
  }

  /// <summary>
  /// Goes back to the previous menu frame if one exists; otherwise closes the menu.
  /// </summary>
  public void GoBack() {
    if (frames.Count > 1) {
      frames.Pop();
      Render();
      return;
    }

    Close();
  }

  /// <summary>
  /// Renders the current menu state to the player's center screen.
  /// </summary>
  public void Render() {
    var html = HtmlRenderer.Render(player, CurrentMenu, CurrentPage,
      CurrentSelectedIndex);
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
        GoBack();
        return;

      case MenuButton.EXIT:
        Close();
        return;
    }

    Render();
  }

  /// <summary>
  /// Closes the menu and clears it from the player's screen.
  /// </summary>
  public void Close() {
    player.PrintToCenterHtml(string.Empty);
    MenuManager.Instance.CloseMenu(player);
  }

  private void moveSelection(int delta) {
    var selectableCount = countSelectableInPage(CurrentPage);
    if (selectableCount == 0) return;

    CurrentSelectedIndex = (CurrentSelectedIndex + delta + selectableCount)
      % selectableCount;
  }

  private void changePage(int delta) {
    var totalPages = (CurrentMenu.Lines.Count + CurrentMenu.LinesPerPage - 1)
      / CurrentMenu.LinesPerPage;

    if (totalPages <= 0) return;

    CurrentPage          = (CurrentPage + delta + totalPages) % totalPages;
    CurrentSelectedIndex = 0;
  }

  private int countSelectableInPage(int page) {
    var start = page * CurrentMenu.LinesPerPage;
    var end = Math.Min(start + CurrentMenu.LinesPerPage,
      CurrentMenu.Lines.Count);
    var count = 0;

    for (var i = start; i < end; i++) {
      var elements = CurrentMenu.Lines[i].Elements;
      for (var j = 0; j < elements.Count; j++) {
        if (elements[j].IsSelectable) count++;
      }
    }

    return count;
  }

  private void activateSelected(string? input) {
    var targetIndex = CurrentSelectedIndex;
    var start       = CurrentPage * CurrentMenu.LinesPerPage;
    var end = Math.Min(start + CurrentMenu.LinesPerPage,
      CurrentMenu.Lines.Count);
    var currentIndex = 0;

    for (var i = start; i < end; i++) {
      var elements = CurrentMenu.Lines[i].Elements;
      for (var j = 0; j < elements.Count; j++) {
        var elem = elements[j];
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