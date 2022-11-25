namespace SnakeF

open System
open Essentials

type Field (x: int, y: int) as fie =
    let _width = x
    let _height = y
    let mutable _field: MapBlocks [,] = Array2D.zeroCreate<MapBlocks> _width _height
    let _snakeBoard = new SnakeBoard((float)_width, (float)_height)
    
    // create the base of the field (walls and open)
    do
        fie.LayBase
        _snakeBoard.SetNewBoard fie.FieldArray
        
    member public _.GetField = _field
    member public _.GetBoard = _snakeBoard
    member public _.GetWalls = _snakeBoard.GetWalls
    member public _.GetFloors = _snakeBoard.GetFloors
    
    member private _.BaseLaying row col =
        if row = 0 or col = 0 or row = _height-1 or col = _width-1 then
            MapBlocks.Wall
        else
            MapBlocks.Open
            
    member private _.LayBase =
        _field <- Array2D.init<MapBlocks> _width _height (fie.BaseLaying)
        
    member public _.LayApple =
        let ran = new Random()
        let x = ran.Next(1, _width-2)
        let y = ran.Next(1, _height-2)
        _field[x,y] <- MapBlocks.Apple
        
    member public _.PlaceSnake =
        let ran = new Random()
        let mutable x: int = 0
        let mutable y: int = 0
        
        while _field[x,y] = MapBlocks.Wall or _field[x,y] = MapBlocks.Apple do
            x <- ran.Next(1, _width-2)
            y <- ran.Next(1, _height-2)
            
        _field[x,y] <- MapBlocks.Head
        
    member public _.FieldArray: MapBlocks[] =
        [|
            for i in 0 .. _height-1 do
                for j in 0 .. _width-1 ->
                    _field[j,i]
        |]