namespace Champion

open Keras
open Keras.Layers
open Keras.Models
open Numpy

module Say =
    let hello name =
        //printfn "Hello %s" name
        //
        //let inputShape = new Shape(51,51)
        //
        //let inputs = new Layers.Input(inputShape)
        //let layer1 = new Layers.Conv2D(32, (8,8), strides = (4,4), activation = "relu", input_shape = inputShape)
        //layer1
        //printf "gh"
        let bb : NDarray = np.array([])
        let cc = np.array([|0;1;1;0|])
        let i: int = 32
        let mutable model = new Sequential()
        model.Add(new Dense(i, activation = "relu", input_shape = new Shape(2)))
        model.Add(new Dense(64, activation = "relu"))
        model.Add(new Dense(1, activation = "sigmoid"))
        
        model.Compile(optimizer = "sgd", loss = "binary_crossentropy", metrics = [|"accuracy"|])
        model.Fit(bb,cc,batch_size = 2, epochs = 1000, verbose = 1) |> ignore
        
        let json = model.ToJson()
        System.IO.File.WriteAllText("model.json", json)
        model.SaveWeight("model.h5")
        
        let mutable loaded = Sequential.ModelFromJson(System.IO.File.ReadAllText("model.json"))
        loaded.LoadWeight("model.h5")
        