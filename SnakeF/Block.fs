namespace SnakeF

open Essentials

type Block (topLeft: Point, topRight: Point, bottomLeft: Point, bottomRight: Point, style: MapBlocks) =
    let _topLeft = topLeft
    let _topRight = topRight
    let _bottomLeft = bottomLeft
    let _bottomRight = bottomRight
    let _style = style
    
    member public _.Style = _style
    
    member public _.GetElementArray =
        [|(uint)_topLeft.Index; (uint)_topRight.Index; (uint)_bottomLeft.Index
          (uint)_topRight.Index; (uint)_bottomRight.Index; (uint)_bottomLeft.Index|]