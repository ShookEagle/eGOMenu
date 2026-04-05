using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared.Elements;

/// <summary>
/// Represents a blank line or horizontal separator within the menu.  Spacers
/// allow menu authors to visually group related items or insert vertical space.
/// </summary>
public class SpacerElement : IMenuElement {
  private readonly int height;

  /// <summary>
  /// Creates a spacer with a given height.  A height of 1 inserts a single
  /// blank line; larger values insert multiple blank lines.
  /// </summary>
  /// <param name="height">The number of blank lines to insert.</param>
  public SpacerElement(int height = 1) { this.height = Math.Max(1, height); }

  public MenuItemType Type => MenuItemType.SPACER;

  public bool IsEnabled => false;

  public bool IsSelectable => false;

  public string Render(int selectableIndex) {
    // Use <br> tags to insert blank lines.  HTML requires one <br> per blank line.
    return string.Concat(Enumerable.Repeat("<br>", height));
  }

  public void Activate(CounterStrikeSharp.API.Core.CCSPlayerController player,
    string? input = null) {
    // Spacers cannot be activated.
  }
}