using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared;

/// <summary>
/// Fluent API for constructing a <see cref="WASDMenu"/>.  The builder collects
/// lines and global options before creating the final menu instance.  See
/// <see cref="WASDMenu.Create(string)"/> to obtain a new builder.
/// </summary>
public class MenuBuilder {
  private readonly string title;
  private readonly List<MenuLine> lines = new();
  private string? footer;
  private int linesPerPage = 6;

  internal MenuBuilder(string title) {
    if (string.IsNullOrWhiteSpace(title))
      throw new ArgumentException("Title cannot be empty", nameof(title));
    this.title = title;
  }

  /// <summary>
  /// Adds a line to the menu.  The provided action is invoked with a new
  /// <see cref="MenuLine"/> allowing the caller to add elements to it.
  /// </summary>
  public MenuBuilder AddLine(Action<MenuLine> configure) {
    var line = new MenuLine();
    configure(line);
    lines.Add(line);
    return this;
  }

  /// <summary>
  /// Adds a footer to the menu.  The footer appears at the bottom of each
  /// page and typically displays control hints.
  /// </summary>
  public MenuBuilder AddFooter(string text, MenuStyle style = MenuStyle.NONE,
    System.Drawing.Color? color = null) {
    var c       = color ?? System.Drawing.Color.Gray;
    var hex     = System.Drawing.ColorTranslator.ToHtml(c);
    var content = text;
    content = style switch {
      MenuStyle.BOLD   => "<b>" + content + "</b>",
      MenuStyle.ITALIC => "<i>" + content + "</i>",
      MenuStyle.MONO => "<span style='font-family: monospace'>" + content
        + "</span>",
      _ => content
    };
    footer = $"<font color='{hex}'>{content}</font>";
    return this;
  }

  /// <summary>
  /// Sets the maximum number of lines to display per page.  If more lines
  /// are added than fit on a page, the menu will automatically paginate.
  /// </summary>
  public MenuBuilder SetLinesPerPage(int lineCount) {
    linesPerPage = Math.Max(1, lineCount);
    return this;
  }

  /// <summary>
  /// Finalises the builder and returns a constructed <see cref="WASDMenu"/>.
  /// </summary>
  public WASDMenu Build() {
    return new WASDMenu(title, lines, footer, linesPerPage);
  }
}