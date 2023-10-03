namespace Pulsar

module Main =
    open System
    open Enumerations
    open Types
    open Inputs 
    open Graphics
    open Moves
    open Levels
    open Constants

    let gameStateController = {
        GameStates = [ lvl01 ]
    }

    let Update ( gameState : GameState ) : GameState =
        let childEntitiesUpdated = gameState.ChildEntities |> List.map ( fun e -> UpdateEntity gameState.MapGrid e )    
        { GameStateKind = GameStateKind.Level ; ChildEntities = childEntitiesUpdated ; MapGrid = gameState.MapGrid }

    let Draw ( gameState : GameState ) : GameState =
        gameState.ChildEntities |> List.map (fun e -> DrawEntity gameState.MapGrid e ) |> ignore  
        gameState

    let Initialize ( ) : Unit =
        let gameState = List.head gameStateController.GameStates
        Console.SetBufferSize ( WORLD_SIZE.Width , WORLD_SIZE.Height )
        Console.SetWindowSize ( 145 ,  40 )
        Console.SetWindowPosition ( 0 , 0 )
        DrawWorld <| gameState.MapGrid
        Draw <| gameState |> ignore
        ( )
         
    let LoadContent ( ) : Unit = 
        ( )

    let UnloadContent ( ) : Unit =
        ( )

    let rec GameLoop ( gameState : GameState ) =
        gameState |> Update |> Draw |> GameLoop

    [<EntryPoint>]
    let Main ( argv : string [] ) : int = 
        Initialize ( )
        LoadContent ( )        
        GameLoop <| List.head gameStateController.GameStates
        UnloadContent ( )
        0 // return an integer exit code