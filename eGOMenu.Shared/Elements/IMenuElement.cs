using CounterStrikeSharp.API.Core;
using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared.Elements;

/// <summary>
/// Represents a renderable component within a <see cref="WASDMenu"/>. Elements
/// can be interactive (buttons, toggles, inputs) or purely informational (text, spacer).
/// Implementations should avoid holding expensive state; all states should live in
/// an external context to allow the same element definition to be reused across
/// players. Elements may be disabled or non‑selectable based on runtime conditions.
/// </summary>
public interface IMenuElement {
  /// <summary>
  /// Gets the type of the element. This value influences how the renderer
  /// displays the element and whether it is included in the selectable list.
  /// </summary>
  MenuItemType Type { get; }

  /// <summary>
  /// Indicates whether the element is currently enabled. Disabled elements
  /// will be rendered differently and cannot be activated by the user.
  /// </summary>
  bool IsEnabled { get; }

  /// <summary>
  /// Indicates whether the element should be included in the selectable index.
  /// For example, spacers and plain text are non‑selectable.
  /// </summary>
  bool IsSelectable { get; }

  /// <summary>
  /// Builds the HTML fragment for this element. The provided index is the
  /// current selectable index (1‑based) and should be included in the
  /// rendered text when the element is selectable.
  /// </summary>
  /// <param name="selectableIndex">The 1‑based index for this element or 0 if not selectable.</param>
  string Render(int selectableIndex);

  /// <summary>
  /// Called by the menu manager when the element is activated (e.g. when the
  /// user presses the select key on the element). Input elements may be
  /// activated after the player submits text.
  /// </summary>
  /// <param name="player">The player who triggered the activation.</param>
  /// <param name="input">Optional input supplied by the player.</param>
  void Activate(CCSPlayerController player, string? input = null);
}