namespace Champion

open Essentials
open Keras
open Keras.Layers
open Keras.Models
open Keras.Optimizers
open Numpy

type SnakeMind(x:int, y:int, ?load: bool) as SM =
    let load = defaultArg load false
    let _model =
        if load then
            let loaded = Sequential.ModelFromJson(System.IO.File.ReadAllText("model.json"))
            loaded.LoadWeight("model.h5")
            loaded
        else
            let mutable model = new Sequential()
            model.Add(new Dense(32, activation = "relu", input_shape = new Shape(x*y)))
            model.Add(new Dense(64, activation = "relu"))
            model.Add(new Dense(1, activation = "sigmoid"))
            let adam = new Adam(lr = 0.002f)
            model.Compile(loss = "mse", optimizer = adam.ToString())
            model
    let mutable _reward = 0
    
    member public _.GiveReward(block: MapBlocks) =
        match block with
        | MapBlocks.Apple -> _reward <- 10
        | MapBlocks.Wall
        | MapBlocks.Body -> _reward <- -10
        | _ -> _reward <- 0
        _reward
        
    //member public _.Remember() =
        