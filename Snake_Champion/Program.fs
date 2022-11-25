// For more information see https://aka.ms/fsharp-console-apps

open OpenTK.Mathematics
open OpenTK.Windowing.Desktop
open SnakeF
open Displayinator

let nativset = new NativeWindowSettings(Size = new Vector2i(800, 800), Title = "hey")


let fieldy = new Field(20,20)
let boardy = fieldy.GetBoard
let verts = boardy.GetBaseVertices
let lineys = boardy.GetLineArray

let dis = new Displayinator.SnakeWindow(GameWindowSettings.Default, nativset, verts, lineys)

dis.Run()