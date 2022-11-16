namespace SnakeF

open System
open Essentials

type Field (x: int, y: int) as fie=
    let _width = x
    let _height = y
    let mutable field: int [,] = Array2D.zeroCreate<int> _width _height
    
    // create the base of the field (walls and open)
    do
        fie.LayBase
        
    member public _.GetField = field
    
    member private _.BaseLaying row col =
        if row = 0 or col = 0 or row = _height-1 or col = _width-1 then
            (int)MapBlocks.Wall
        else
            (int)MapBlocks.Open
            
    member private _.LayBase =
        field <- Array2D.init<int> _width _height (fie.BaseLaying)
        
    member public _.LayApple =
        let ran = new Random()
        let x = ran.Next(1, _width-2)
        let y = ran.Next(1, _height-2)
        field[x,y] <- (int)MapBlocks.Apple
        
    member public _.PlaceSnake =
        let ran = new Random()
        let mutable x: int = 0
        let mutable y: int = 0
        
        while field[x,y] = (int)MapBlocks.Wall or field[x,y] = (int)MapBlocks.Apple do
            x <- ran.Next(1, _width-2)
            y <- ran.Next(1, _height-2)
            
        field[x,y] <- (int)MapBlocks.Head