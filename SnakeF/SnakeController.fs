namespace SnakeF

open Champion
open Displayinator
open Essentials
open OpenTK.Mathematics
open OpenTK.Windowing.Desktop
open OpenTK.Windowing.GraphicsLibraryFramework

type SnakeController (width: int, height: int, ?visible: bool, ?smart: bool) as SC =
    let visible = defaultArg visible false
    let smart = defaultArg smart false
    let _visible = visible
    let _smart = smart
    let _field = new Field(width, height)
    let _board = new SnakeBoard((float)width, (float)height)
    let _mind: SnakeMind = new SnakeMind(width,height)
        
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
        if not _smart then _display.KeyInput.Add(fun x -> SC.MoveSnake(x))
        _field.SendDestination.Add(fun x -> _mind.GiveReward(x) |> ignore)
        
    member public _.Board = _board
    member public _.Field = _field
    
    member public _.Start() =
        if visible then
            _display.Run()
            
    member _.ReStart() =
        _field.LayBase()
        _board.SetNewBoard <| _field.FieldArray()
        let floors = _board.GetFloors()
        _field.LayApple()
        _field.PlaceSnake()
        _board.SetNewBoard <| _field.FieldArray()
    
    member public _.MoveSnake (?key: Keys, ?dir: int) =
        let key = defaultArg key Keys.Unknown
        let dir = defaultArg dir -1
        let updated =
            match key, dir with
            | key, dir when (key = Keys.Up || dir = (int)Direction.Up) ->
                true, _field.MoveHead(Direction.Up)
            | key, dir when (key = Keys.Down || dir = (int)Direction.Down) ->
                true, _field.MoveHead(Direction.Down)
            | key, dir when (key = Keys.Left || dir = (int)Direction.Left) ->
                true, _field.MoveHead(Direction.Left)
            | key, dir when (key = Keys.Right || dir = (int)Direction.Right) ->
                true, _field.MoveHead(Direction.Right)
            | _ -> (false, false)
            
        let updateall() =
            _display.UpdateHeadIndices(_board.GetHead())
            _display.UpdateSnakeIndices(_board.GetSnake())
            _display.UpdateAppleIndices(_board.GetApple())
        
        if snd(updated) then
            SC.ReStart()
            updateall()
        elif fst(updated) then
            _board.SetNewBoard <| _field.FieldArray()
            updateall()