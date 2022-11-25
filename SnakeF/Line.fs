namespace SnakeF

type Line (start: Point, ending: Point) =
    let _origin = start
    let _destination = ending
    
    member public _.Orig = _origin
    member public _.Dest = _destination