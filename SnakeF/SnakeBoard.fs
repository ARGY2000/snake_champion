namespace SnakeF

open System
open System.Linq

type SnakeBoard (width: float, height: float) as SB =
    
    let _width = width + 1.0
    let _height = height + 1.0
    let mutable _board: Point[] = Array.empty
    let mutable _graph: Line[] = Array.empty
    
    do
       SB.InitBoard
       SB.CreateLineGraph
    
    // TODO: this may be one short both ways
    member private _.InitBoard =
        let maxW = (_width-1.0) - (_width/2.0)
        let maxH = (_height-1.0) - (_height/2.0)
        _board <- [|
            for i in 0 .. (((int)_width * (int)_height) - 1)  ->
                let row: int = (int)(Math.Floor ((float)i)/((float)_width))
                let col: int = i - (row * (int)_width)
                let x: float = ((float)col - (_width/2.0)) / maxW
                let y: float = ((_height/2.0) - (float)row) / maxH
                new Point(i, x, y, row, col)
        |]
        let max = _board.Max(fun b -> b.X)
        let min = _board.Min(fun b -> b.X)
        printf ""
        
    member private _.CreateLineGraph =
        let mutable lines: seq<Line> = Seq.empty
        for i in _board do
            if i.Col < (int)_width-1 then
                lines <- Seq.append lines [new Line(i, _board.First(fun b -> b.Row = i.Row && b.Col = i.Col+1))]
            if i.Row < (int)_height-1 then
                lines <- Seq.append lines [new Line(i, _board.First(fun b -> b.Col = i.Col && b.Row = i.Row+1))]
        _graph <- Seq.toArray lines
        
    member public _.GetBaseVertices =
        let mutable vertices: seq<float> = Seq.empty
        for i in _board do
            vertices <- Seq.append vertices [i.X; i.Y; i.Z]
        Seq.toArray vertices
        
    member public _.GetLineArray: uint[] =
        let mutable dots: seq<uint> = Seq.empty
        for i in _graph do
            dots <- Seq.append dots [(uint)(i.Orig.Index); (uint)(i.Dest.Index)]
        Seq.toArray dots