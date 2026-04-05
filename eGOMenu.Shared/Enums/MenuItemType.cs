namespace eGOMenu.Shared.Enums;

public enum MenuItemType {
  /// <summary>Simple read‑only text.</summary>
  TEXT,

  /// <summary>A spacer used to insert blank lines or horizontal separators.</summary>
  SPACER,

  /// <summary>An element that allows the user to choose from multiple options.</summary>
  CHOICE,

  /// <summary>A clickable button.</summary>
  BUTTON,

  /// <summary>An element prompting the user for textual input.</summary>
  INPUT
}