namespace eGOMenu.Shared.Enums;

public enum MenuAction {
  /// <summary>Begins displaying the menu for the first time.</summary>
  START,

  /// <summary>Invoked when a selectable item is activated.</summary>
  SELECT,

  /// <summary>Invoked when a choice among many options is made.</summary>
  CHOOSE,

  /// <summary>Invoked when a menu element updates a value.</summary>
  UPDATE,

  /// <summary>Invoked when the user exits the menu.</summary>
  EXIT,

  /// <summary>Additional helper action (reserved for future use).</summary>
  ASSIST,

  /// <summary>Invoked when textual input is submitted.</summary>
  INPUT
}