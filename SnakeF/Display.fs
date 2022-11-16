module SnakeF.Display

open OpenTK.Graphics.OpenGL4
open OpenTK.Graphics
open OpenTK.Mathematics
open OpenTK.Windowing.Desktop

let CreateImage =
    let g = new GameWindow(GameWindowSettings.Default, new NativeWindowSettings(Size = new Vector2i(500, 500), Title = "Test"))
    g.Run()