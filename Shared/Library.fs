namespace Shared

open System

[<AbstractClass; Sealed>]
type LittleHelpers private () =
    
    // TODO: fix this    
    static member GetSquareDisplayArray (width: int, height: int): float[] =
        let halfW: float = ((float)width / (float)2)
        let halfH: float = ((float)height / (float)2)
        let mutable row = 0
        let mutable col = 0
        let mutable spot = 0
        let tempy: float[] = [|
            for i in 0 .. ((width*height)*3) ->
                row <- (int)(i/width)
                col <- (int)(i - (row * width))
                match spot with
                | 0 ->
                    spot <- spot + 1
                    ((float)col - halfW)
                | 1 ->
                    spot <- spot + 1
                    (halfH - (float)row)
                | 2 ->
                    spot <- 0
                    0
                | _ -> raise (new Exception("Something went wrong"))
        |]
        tempy