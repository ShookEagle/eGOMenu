namespace eGOMenu.Shared;

public sealed class MenuFrame(WASDMenu menu) {
  public WASDMenu Menu { get; } = menu;
  public int Page { get; set; }
  public int SelectedIndex { get; set; }
}