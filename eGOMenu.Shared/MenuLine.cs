using System.Drawing;
using CounterStrikeSharp.API.Core;
using eGOMenu.Shared.Elements;
using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared;

/// <summary>
/// Represents a single line within a menu.  A line can contain one or more
/// <see cref="IMenuElement"/>s which are rendered sequentially.  Lines are
/// created via the <see cref="MenuBuilder"/> fluent API.
/// </summary>
public class MenuLine {
  private readonly List<IMenuElement> elements = [];

  internal IReadOnlyList<IMenuElement> Elements => elements;

  /// <summary>
  /// Adds a label to this line.
  /// </summary>
  public MenuLine AddLabel(string text, MenuStyle style = MenuStyle.NONE,
    Color? color = null) {
    elements.Add(new TextElement(text, style, color ?? Color.White));
    return this;
  }

  /// <summary>
  /// Adds a spacer to this line.  The spacer inserts a blank line when rendered.
  /// </summary>
  public MenuLine AddSpacer(int height = 1) {
    elements.Add(new SpacerElement(height));
    return this;
  }

  /// <summary>
  /// Adds a clickable button to this line.
  /// </summary>
  public MenuLine AddButton(string text,
    Action<CCSPlayerController> onPressed,
    Func<CCSPlayerController, bool>? enabledPredicate = null,
    MenuStyle style = MenuStyle.NONE, Color? color = null) {
    elements.Add(new ButtonElement(text, onPressed, enabledPredicate, style,
      color ?? Color.Yellow));
    return this;
  }

  /// <summary>
  /// Adds a toggle to this line.
  /// </summary>
  public MenuLine AddToggle(string label,
    Func<CCSPlayerController, bool> getState,
    Action<CCSPlayerController, bool> onChanged,
    Func<CCSPlayerController, bool>?
      enabledPredicate = null, Color? onColor = null, Color? offColor = null) {
    elements.Add(new ToggleElement(label, getState, onChanged,
      enabledPredicate, onColor, offColor));
    return this;
  }

  /// <summary>
  /// Adds an input prompt to this line.
  /// </summary>
  public MenuLine AddInput(string label, string placeholder,
    Action<CCSPlayerController, string> onSubmitted,
    Func<CCSPlayerController, bool>?
      enabledPredicate = null, Color? color = null) {
    elements.Add(new InputElement(label, placeholder, onSubmitted,
      enabledPredicate, color));
    return this;
  }
}