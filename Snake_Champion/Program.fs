// For more information see https://aka.ms/fsharp-console-apps

open OpenTK.Mathematics
open OpenTK.Windowing.Desktop
open SnakeF
open Displayinator

let nativset = new NativeWindowSettings(Size = new Vector2i(800, 800), Title = "hey")

let dis = new Displayinator.SnakeWindow(GameWindowSettings.Default, nativset)

dis.Run()