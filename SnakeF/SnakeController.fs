namespace SnakeF

open Displayinator
open OpenTK.Mathematics
open OpenTK.Windowing.Desktop

type SnakeController (width: int, height: int, visible: bool) as SC =
    let _visible = visible
    let _field = new Field(width, height)
    let _board = new SnakeBoard((float)width, (float)height)
    
    do
        _field.LayApple()
        _field.PlaceSnake()
        _board.SetNewBoard <| _field.FieldArray()
    let _display =
        if _visible then
            let nativset = new NativeWindowSettings(Size = new Vector2i(800, 800), Title = "hey")
            new SnakeWindow(GameWindowSettings.Default, nativset, _board.GetBaseVertices(), _board.GetLineArray(),
                            _board.GetWalls(), _board.GetFloors(), _board.GetHead(), _board.GetApple())
        else
            null
        
    member public _.Board = _board
    member public _.Field = _field
    
    
    member public _.Start() =
        if visible then
            _display.Run()