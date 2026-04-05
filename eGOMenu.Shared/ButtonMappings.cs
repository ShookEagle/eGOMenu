using CounterStrikeSharp.API;
using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared;

public static class ButtonMappings {
  public static readonly IReadOnlyDictionary<string, PlayerButtons> DEFAULT =
    new Dictionary<string, PlayerButtons> {
      { "W", PlayerButtons.Forward },
      { "A", PlayerButtons.Moveleft },
      { "S", PlayerButtons.Back },
      { "D", PlayerButtons.Moveright },
      { "E", PlayerButtons.Use },
      { "Crouch", PlayerButtons.Duck },
      { "Tab", (PlayerButtons)8589934592 }
    };

  public static readonly IReadOnlyDictionary<MenuButton, PlayerButtons>
    MENU_TO_PLAYER_BUTTON = new Dictionary<MenuButton, PlayerButtons> {
      { MenuButton.UP, PlayerButtons.Forward },
      { MenuButton.DOWN, PlayerButtons.Back },
      { MenuButton.LEFT, PlayerButtons.Moveleft },
      { MenuButton.RIGHT, PlayerButtons.Moveright },
      { MenuButton.SELECT, PlayerButtons.Use },
      { MenuButton.BACK, PlayerButtons.Duck },
      { MenuButton.EXIT, (PlayerButtons)8589934592 },
    };
}