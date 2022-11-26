namespace SnakeF

open SnakeF

type Snake (x: int, y: int) =
    let mutable _field = new Field(x,y)
    
    let mutable _length = 1
    let mutable _score = 0
    
    do
        _field.LayApple()
    