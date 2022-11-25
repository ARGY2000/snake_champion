namespace SnakeF

open System
open Microsoft.FSharp.Core

type Point (index: int, cordx: float, cordy: float, row: int, col: int) =
    let _index = index
    let _coords = (cordx, cordy)
    let _row = row
    let _col = col
    
    member public _.Index = _index
    
    member public _.Row = _row
    member public _.Col = _col
    
    member public _.X = fst _coords
    member public _.Y = snd _coords
    member public _.Z = 0.0