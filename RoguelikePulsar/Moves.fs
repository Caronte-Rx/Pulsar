namespace Pulsar

module Moves =
    open Enumerations
    open Types
    open Utils
    open Inputs

    let UpdateActor ( world : EntityBase list ) ( entity : EntityBase ) ( direction : Input ) : ( EntityBase ) =
        let nextLocation = GetNextCoordinate entity direction
        let tile = GetTileMap world nextLocation
        match tile.Sprite with
            | '#' ->   entity
            | _   -> { entity with PrevLocation = Some entity.Location ; Location = nextLocation } 

    let UpdateEntity ( world : EntityBase list ) ( entity : EntityBase ) : ( EntityBase ) =
        let input = GetInput ()
        match entity.Entity with
            | ActorEntity e -> UpdateActor world entity input
            | _             -> entity