namespace SnakeF

open Displayinator
open Essentials
open OpenTK.Mathematics
open OpenTK.Windowing.Desktop
open OpenTK.Windowing.GraphicsLibraryFramework

type SnakeController (width: int, height: int, visible: bool) as SC =
    let _visible = visible
    let _field = new Field(width, height)
    let _board = new SnakeBoard((float)width, (float)height)
    
    let _display =
        if _visible then
            _board.SetNewBoard <| _field.FieldArray()
            let floors = _board.GetFloors()
            _field.LayApple()
            _field.PlaceSnake()
            _board.SetNewBoard <| _field.FieldArray()
            let nativset = new NativeWindowSettings(Size = new Vector2i(800, 800), Title = "hey")
            new SnakeWindow(GameWindowSettings.Default, nativset, _board.GetBaseVertices(), _board.GetLineArray(),
                            _board.GetWalls(), floors, _board.GetHead(), _board.GetApple(), _board.GetLineVertices())
        else
            _field.LayApple()
            _field.PlaceSnake()
            _board.SetNewBoard <| _field.FieldArray()
            null
        
    do
        _display.KeyInput.Add(SC.MoveSnake)
        
    member public _.Board = _board
    member public _.Field = _field
    
    member public _.Start() =
        if visible then
            _display.Run()
            
    member public _.MoveSnake(key: Keys) =
        let updated =
            match key with
            | Keys.Up ->
                _field.MoveHead(Direction.Up)
                true
            | Keys.Down ->
                _field.MoveHead(Direction.Down)
                true
            | Keys.Left ->
                _field.MoveHead(Direction.Left)
                true
            | Keys.Right ->
                _field.MoveHead(Direction.Right)
                true
            | _ -> false
        
        if updated then
            _board.SetNewBoard <| _field.FieldArray()
            _display.UpdateHeadIndices(_board.GetHead())