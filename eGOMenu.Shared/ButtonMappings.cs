using CounterStrikeSharp.API;
using eGOMenu.Shared.Enums;

namespace eGOMenu.Shared;

public static class ButtonMappings
{
  public static ReadOnlySpan<(MenuButton Button, PlayerButtons Flag)> Map => new[]
  {
    (MenuButton.UP, PlayerButtons.Forward),       // W
    (MenuButton.DOWN, PlayerButtons.Back),        // S
    (MenuButton.LEFT, PlayerButtons.Moveleft),    // A
    (MenuButton.RIGHT, PlayerButtons.Moveright),  // D
    (MenuButton.SELECT, PlayerButtons.Use),       // E
    (MenuButton.BACK, PlayerButtons.Duck),        // Crouch
    (MenuButton.EXIT, (PlayerButtons)8589934592 ) // Tab
  };
}