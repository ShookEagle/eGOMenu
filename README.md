# eGOMenu

eGOMenu is a lightweight, high-performance WASD menu library for CounterStrikeSharp (CS2).  
It is designed to be easy to consume from other plugins while remaining fast, flexible, and fully extensible.

---

## Overview

eGOMenu provides a simple API for building and displaying interactive menus using center HTML rendering (`PrintToCenterHtml`).  

Menus are composed of lines and elements, support pagination automatically, and allow full control over behavior and layout.

---

## Installation

### NuGet
dotnet add package eGOMenu

### Server Setup

Place the compiled DLL in:
`addons/counterstrikesharp/shared/eGOMenu/`

---

## Basic Usage (From a Plugin)

Create and open a menu from within your plugin:

```csharp
var menu = WASDMenu.Create("Main Menu")
    .AddLine(l => l.AddButton("Start Game", p =>
    {
        p.PrintToChat("Game starting...");
    }))
    .AddLine(l => l.AddButton("Settings", p =>
    {
        MenuManager.Instance.OpenSubMenu(p, BuildSettingsMenu());
    }))
    .AddFooter("W/S = Navigate | E = Select | R = Back")
    .Build();

MenuManager.Instance.OpenMenu(player, menu);
``` 

Menus are created once and passed directly to the `MenuManager`.
There is no required registration or capability system.

## Submenus

Submenus are handled automatically using a stack-based system.

```csharp
private WASDMenu BuildSettingsMenu()
{
    return WASDMenu.Create("Settings")
        .AddLine(l => l.AddButton("Audio", p => { }))
        .AddLine(l => l.AddButton("Video", p => { }))
        .Build();
}
```
To open a submenu:

```csharp 
MenuManager.Instance.OpenSubMenu(player, submenu);
```

Behavior:
- Back returns to the previous menu
- Exit closes the entire menu stack
- Each submenu preserves its own page and selection state

## Menu Structure

Menus are built using a fluent builder pattern:

```csharp
WASDMenu.Create("Title")
    .AddLine(...)
    .AddFooter(...)
    .Build();
```
Each menu consists of:
- Header (title)
- Lines (collections of elements)
- Footer (optional)
- Automatic pagination when exceeding vertical limits 

## Elements

### Button
```csharp
.AddButton("Label", player =>
{
    // logic
})
```

### Conditional / Disabled
```csharp
.AddButton("Admin Only", p => { }, p => p.IsAdmin)
```

If the predicate returns false, the item is skipped during selection.

### Spacer
```csharp
.AddSpacer()
```
Useful for layout control.

## Input Handling

eGOMenu uses a combination of:

- `OnPlayerButtonsChanged` (event-driven input)
- `OnTick` (for held key repeat)

This ensures:

- Immediate response on press
- Smooth scrolling when holding keys
- Minimal overhead (only active menus are processed)

## Default Controls
| Action | 	Key   |
|--------|--------|
| Up     | 	W     |
| Down   | 	S     |
| Left   | 	A     |
| Right  | 	D     |
| Select | 	E     |
| Back   | 	R     |
| Exit   | 	Space |

Button mappings are configurable internally and can be extended if needed.

## Rendering

Menus are rendered using:

```csharp
player.PrintToCenterHtml(...)
```
Formatting is HTML-based and supports:

- colors
- font sizes
- styling (bold, italic)

You can extend or replace the renderer if needed.

Architecture
- `WASDMenu`
  - Immutable menu definition built via the fluent API
- `MenuManager`
  - Singleton, responsible for managing active menus per player
- `MenuContext`
  - Per-player runtime state (page, selection, submenu stack)
- `MenuFrame`
  - Represents a single menu layer in the stack
- `PlayerButtonsListener`
  - Handles input and key repeat logic

## Extending

The system is designed to be extended from consumer plugins:

You can build:
- custom elements (sliders, toggles, inputs)
- dynamic menus generated at runtime
- reusable menu builders
- alternative renderers