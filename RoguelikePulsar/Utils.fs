namespace Pulsar

module Utils =

    open Enumerations
    open Types
    open Defaults

    //Math
    let Min ( x : _ ) ( y : _ ) : ( _ ) = if x < y then x else y
    let Max ( x : _ ) ( y : _ ) : ( _ ) = if x > y then x else y
    let Clamp ( v : int ) ( min : int ) ( max : int ) : ( int ) = Min max <| Max v min
    
    /// Exclusive 'between' operator:
    let (><) ( x : _ ) ( min : _ , max : _ ) : ( bool ) =
        (x > min) && (x < max)
    /// Inclusive 'between' operator:
    let (>=<) ( x : _ ) ( min : _ , max : _ ) : ( bool ) =
        (x >= min) && (x <= max)
    /// Exclusive 'not between' operator:
    let (<->) ( x : _ ) ( min : _ , max : _ ) : ( bool ) =
        (x < min) && (x > max)
    /// Inclusive 'between' operator:
    let (<=>) ( x : _ ) ( min : _ , max : _ ) : ( bool ) =
        (x <= min) && (x >= max)
    //General
    let PickRandomFrom ( lst : _ list ) =
        List.nth lst <| defaultRandom.Next ( 0 , lst.Length )

    //Map
    let (>+) ( c1 : Coordinate ) ( c2 : Coordinate ) : ( Coordinate ) =
        { x = c1.x + c2.x ; y = c1.y + c2.y }
    let (>-) ( c1 : Coordinate ) ( c2 : Coordinate ) : ( Coordinate ) =
        { x = c1.x - c2.x ; y = c1.y - c2.y }

    let GetTileMap ( tilesList : EntityBase list ) ( location : Coordinate ) : ( EntityBase ) = 
        tilesList |> List.find ( fun tile -> tile.Location = location  ) 

    let GetRightDownLocation ( location : Coordinate ) ( mask : Size ) : ( Coordinate ) = 
        { x = location.x + mask.Width - 1 ; y = location.y + mask.Height - 1 }

    let GetNextCoordinate ( entity : EntityBase ) ( direction : Input ) : ( Coordinate ) =
        match direction with
            | Up    -> entity.Location >+ { x = 0 ; y = -1 }
            | Right -> entity.Location >+ { x = 1 ; y = 0  } 
            | Down  -> entity.Location >+ { x = 0 ; y = 1  }
            | Left  -> { x = entity.Location.x - 1; y = entity.Location.y;      }
            | _ -> entity.Location

    let IsxoryAllign ( c1 : Coordinate ) ( c2 : Coordinate ) : ( bool ) = c1.x = c2.x || c1.y = c2.y