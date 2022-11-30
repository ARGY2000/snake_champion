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
            
    member public _.LayBase() =
        _field <- Array2D.init<MapBlocks> _width _height (fie.BaseLaying)
        
    member public _.LayApple() =
        let ran = new Random()
        let mutable x = 0
        let mutable y = 0
        while _field[x,y] <> MapBlocks.Open do
            x <- ran.Next(1, _width-2)
            y <- ran.Next(1, _height-2)
        _field[x,y] <- MapBlocks.Apple
        
    member public _.PlaceSnake() =
        let ran = new Random()
        let mutable x: int = 0
        let mutable y: int = 0
        
        while _field[x,y] <> MapBlocks.Open do
            x <- ran.Next(1, _width-2)
            y <- ran.Next(1, _height-2)
            
        _field[x,y] <- MapBlocks.Head
        
    member public _.MoveHead(direct: Direction) =
        let mutable crash = false
        let mutable placing = false
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
                        if _field[destcol, destrow] = MapBlocks.Apple then
                            _field[destcol, destrow] <- MapBlocks.Head
                            _field[col,row] <- MapBlocks.Body
                            placing <- true
                        else
                            _field[destcol, destrow] <- MapBlocks.Head
                            _field[col,row] <- MapBlocks.Open
                            fie.MoveBody destcol destrow col row
                    else
                        crash <- true
                    break <- true
                col <- col+1
            row <- row+1
            
        if placing then
            fie.LayApple()
        
        crash
            
    member private _.MoveBody (col: int) (row: int) (ncol: int) (nrow: int) =
        let mutable Spots: seq<int * int> = Seq.empty
        let rec MoveThatBody (c:int) (r: int) (nc:int) (nr: int) =
            let ToTheNextOne destC destR =
                Spots <- Seq.append Spots [(nc, nr)]
                _field[nc, nr] <- MapBlocks.Body
                _field[destC,destR] <- MapBlocks.Open
                MoveThatBody nc nr destC destR
                
            let spot = (1, 1)
                
            if _field[nc-1,nr] = MapBlocks.Body && not (Spots |> Seq.exists (fun x -> x = ((nc-1),nr))) then
                ToTheNextOne (nc-1) nr
            else if _field[nc+1,nr] = MapBlocks.Body && not (Spots |> Seq.exists (fun x -> x = ((nc+1),nr))) then
                ToTheNextOne (nc+1) nr
            else if _field[nc,nr-1] = MapBlocks.Body && not (Spots |> Seq.exists (fun x -> x = (nc,(nr-1)))) then
                ToTheNextOne nc (nr-1)
            else if _field[nc,nr+1] = MapBlocks.Body && not (Spots |> Seq.exists (fun x -> x = (nc,(nr+1)))) then
                ToTheNextOne nc (nr+1)
                
        MoveThatBody col row ncol nrow
        
    member public _.FieldArray() : MapBlocks[] =
        [|
            for i in 0 .. _height-1 do
                for j in 0 .. _width-1 ->
                    _field[j,i]
        |]