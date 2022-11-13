namespace Champion

open Keras

module Say =
    let hello name =
        printfn "Hello %s" name
        
        let inputs = new Layers.Input(new Shape(51,51))
        let layer 1 = new Layers.Conv2D(32, 8, strides = 4, activation = "relu", input_shape = (51,51))
        printf "gh"
        