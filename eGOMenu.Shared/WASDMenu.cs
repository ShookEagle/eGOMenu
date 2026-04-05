using CounterStrikeSharp.API.Core;

namespace eGOMenu.Shared;

/// <summary>
/// Represents a composed menu instance.  Once constructed via the builder,
/// it contains a title, a list of lines, a footer and paging settings.  The
/// menu can be displayed to players via <see cref="Display(CCSPlayerController)"/>.
/// </summary>
public class WASDMenu
{
  internal WASDMenu(string title, IReadOnlyList<MenuLine> lines, string? footer, int linesPerPage)
  {
    Title        = title;
    Lines        = lines;
    Footer       = footer;
    LinesPerPage = linesPerPage;
  }

  public string Title { get; }
  public IReadOnlyList<MenuLine> Lines { get; }
  public string? Footer { get; }
  public int LinesPerPage { get; }

  /// <summary>
  /// Begins constructing a new menu with the provided title.
  /// </summary>
  public static MenuBuilder Create(string title) => new MenuBuilder(title);

  /// <summary>
  /// Displays this menu to the specified player.  The menu manager handles
  /// paging, input, rendering and disposal.
  /// </summary>
  public void Display(CCSPlayerController player)
  {
    MenuManager.Instance.OpenMenu(player, this);
  }
}