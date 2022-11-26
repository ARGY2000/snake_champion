namespace SnakeF

open System
open System.Linq
open Essentials

type Field (x: int, y: int) as fie =
    let _width = x
    let _height = y
    let mutable _field: MapBlocks [,] = Array2D.zeroCreate<MapBlocks> _width _height
    
    // create the base of the field (walls and open)
    do
        fie.LayBase()
        
    member public _.GetField() = _field
    
    member private _.BaseLaying row col =
        if row = 0 || col = 0 || row = _height-1 || col = _width-1 then
            MapBlocks.Wall
        else
            MapBlocks.Open
            
    member private _.LayBase() =
        _field <- Array2D.init<MapBlocks> _width _height (fie.BaseLaying)
        
    member public _.LayApple() =
        let ran = new Random()
        let x = ran.Next(1, _width-2)
        let y = ran.Next(1, _height-2)
        _field[x,y] <- MapBlocks.Apple
        
    member public _.PlaceSnake() =
        let ran = new Random()
        let mutable x: int = 0
        let mutable y: int = 0
        
        while _field[x,y] = MapBlocks.Wall || _field[x,y] = MapBlocks.Apple do
            x <- ran.Next(1, _width-2)
            y <- ran.Next(1, _height-2)
            
        _field[x,y] <- MapBlocks.Head
        
    member public _.MoveHead(direct: Direction) =
        let mutable row = 0
        let mutable col = 0
        let mutable break = false
        while not break && row < _height do
            col <- 0
            while not break && col < _width do
                if _field[col,row] = MapBlocks.Head then
                    let destrow =
                        match direct with
                        | Direction.Up -> row-1
                        | Direction.Down -> row+1
                        | _ -> row
                    let destcol =
                        match direct with
                        | Direction.Left -> col-1
                        | Direction.Right -> col+1
                        | _ -> col
                    if _field[destcol, destrow] <> MapBlocks.Wall && _field[destcol, destrow] <> MapBlocks.Body then
                        _field[destcol, destrow] <- MapBlocks.Head
                        _field[col,row] <- MapBlocks.Open
                    break <- true
                col <- col+1
            row <- row+1
        
    member public _.FieldArray() : MapBlocks[] =
        [|
            for i in 0 .. _height-1 do
                for j in 0 .. _width-1 ->
                    _field[j,i]
        |]