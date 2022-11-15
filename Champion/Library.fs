namespace Champion

open Keras

module Say =
    let hello name =
        printfn "Hello %s" name
        
        let inputShape = new Shape(51,51)
        
        let inputs = new Layers.Input(inputShape)
        let layer1 = new Layers.Conv2D(32, (8,8), strides = (4,4), activation = "relu", input_shape = inputShape)
        layer1
        printf "gh"
        