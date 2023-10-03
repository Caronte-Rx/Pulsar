namespace Pulsar

module Levels =
    open System
    open Types
    open Constants
    open Utils
    open MapBuilder

    let lvl01 =
        
        let playerEntity = {
                Entity          = ActorEntity { lives = 3 } 
                PrevLocation    = None
                Location        = { x = WORLD_SIZE.Width  / 2 + 1 ; y = WORLD_SIZE.Height / 2 + 1 }
                Color           = Some ConsoleColor.Cyan
                BackgroundColor = None
                Sprite          = '@'
                IsVisible       = true
                IsEnabled       = true 
            }

        {
            GameStateKind   = GameStateKind.Level
            ChildEntities   = [ playerEntity ]
            MapGrid         = BuildWorld ( )
        }