namespace Pulsar

module Graphics =
    open System
    open Types
    open Utils

    let DrawSprite ( entity : EntityBase ) : Unit =
        Console.SetCursorPosition ( entity.Location.x , entity.Location.y )
        let originalForegroundColor = Console.BackgroundColor
        let originalColor = Console.ForegroundColor
        Console.BackgroundColor <- ( match entity.BackgroundColor with | None -> ConsoleColor.Black | Some c -> c )
        Console.ForegroundColor <- ( match entity.Color with | None -> ConsoleColor.White | Some c -> c )
        Console.Write ( entity.Sprite )
        Console.BackgroundColor <- originalForegroundColor
        Console.BackgroundColor <- originalColor

    let DrawEntity ( world : EntityBase list ) ( entity : EntityBase ) : Unit =
        match entity.PrevLocation with
            | Some location -> 
                Console.SetCursorPosition ( location.x , location.y )
                DrawSprite <| GetTileMap world location
            | None -> ( )
        DrawSprite entity
        Console.SetCursorPosition ( 0 , 0 )

    let DrawWorld ( world : EntityBase list ) : Unit = 
        Console.Clear ( )
        Console.CursorVisible <- false
        world |> List.map (fun tileMap -> DrawEntity world tileMap ) |> ignore  