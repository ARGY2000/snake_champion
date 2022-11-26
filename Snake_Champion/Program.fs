// For more information see https://aka.ms/fsharp-console-apps

open OpenTK.Mathematics
open OpenTK.Windowing.Desktop
open SnakeF
open Displayinator

let sc = new SnakeController(20, 20, true)
sc.Start()