namespace SnakeF

open System
open System.Linq
open Essentials

type SnakeBoard (width: float, height: float) as SB =
    
    let _width = width + 1.0
    let _height = height + 1.0
    let mutable _board: Point[] = Array.empty
    let mutable _graph: Line[] = Array.empty
    let mutable _blocks: Block[] = Array.empty
    
    do
       SB.InitBoard
       SB.CreateLineGraph
    
    // TODO: this may be one short both ways
    member private _.InitBoard =
        let maxW = (_width/2.0)
        let maxH = (_height/2.0)
        _board <- [|
            for i in 0 .. (((int)_width * (int)_height) - 1)  ->
                let row: int = (int)(Math.Floor ((float)i)/((float)_width))
                let col: int = i - (row * (int)_width)
                let x: float = (((float)col+0.5) - (_width/2.0)) / maxW
                let y: float = ((_height/2.0) - ((float)row+0.5)) / maxH
                new Point(i, x, y, row, col)
        |]
        let max = _board.Max(fun b -> b.X)
        let min = _board.Min(fun b -> b.X)
        let maxy = _board.Max(fun b -> b.Y)
        let miny = _board.Min(fun b -> b.Y)
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
        
    member public _.SetNewBoard (map: MapBlocks[]) =
        _blocks <- [|
            for i in 0 .. map.Length-1 ->
                let row = (int)(Math.Floor((float)i/(_width-1.0)))
                let col = i - (row * (int)(_width-1.0))
                let tl = _board.First(fun p -> p.Row = row && p.Col = col)
                let tr = _board.First(fun p -> p.Row = tl.Row && p.Col = tl.Col+1)
                let bl = _board.First(fun p -> p.Row = tl.Row+1 && p.Col = tl.Col)
                let br = _board.First(fun p -> p.Row = bl.Row && p.Col = tr.Col)
                new Block(tl, tr, bl, br, map[i])
        |]
        
    member public _.GetWalls =
        let mutable walls: seq<uint> = Seq.empty
        for i in _blocks.Where(fun b -> b.Style = MapBlocks.Wall) do
            walls <- Seq.append walls i.GetElementArray
        Seq.toArray walls
        
    member public _.GetFloors =
        let mutable walls: seq<uint> = Seq.empty
        for i in _blocks.Where(fun b -> b.Style = MapBlocks.Open) do
            walls <- Seq.append walls i.GetElementArray
        Seq.toArray walls
        
    member public _.GetSnake =
        let mutable walls: seq<uint> = Seq.empty
        for i in _blocks.Where(fun b -> b.Style = MapBlocks.Body) do
            walls <- Seq.append walls i.GetElementArray
        Seq.toArray walls
        
    member public _.GetHead =
        let mutable walls: seq<uint> = Seq.empty
        for i in _blocks.Where(fun b -> b.Style = MapBlocks.Head) do
            walls <- Seq.append walls i.GetElementArray
        Seq.toArray walls
        
    member public _.GetApple =
        let mutable walls: seq<uint> = Seq.empty
        for i in _blocks.Where(fun b -> b.Style = MapBlocks.Apple) do
            walls <- Seq.append walls i.GetElementArray
        Seq.toArray walls