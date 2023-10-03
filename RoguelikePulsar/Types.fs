namespace Pulsar

module Types =
    open Enumerations
    open System

    type Coordinate = {
        x : int 
        y : int
    }
    
    type Size = {
        Width   : int 
        Height  : int
    } 

    type ActorEntity = {
        lives   : int
    }

    type MapTileEntity = {
        MapVisibility   : MapVisibility
    }

    type Entity =
        | ActorEntity           of ActorEntity
        | MapTileEntity         of MapTileEntity

    type EntityBase = {
        Entity          : Entity
        PrevLocation    : Coordinate option
        Location        : Coordinate
        Color           : ConsoleColor option
        BackgroundColor : ConsoleColor option
        Sprite          : char
        IsVisible       : bool
        IsEnabled       : bool
    }

    type LabelUI = {
        Text            : string
    }

    type ControlUI = 
        | LabelUI               of LabelUI

    type ControlUIBase = {
        ControlUI       : ControlUI
        HasFocus        : bool
        HasTabStop      : bool
    }

    type ComponentComponent = 
        | EntityBase            of EntityBase
        | ControlUIBase         of ControlUIBase

    type GameStateKind = MenuScreen | MainMenuScreen | Level
    type GameState = {
        GameStateKind   : GameStateKind
        ChildEntities   : EntityBase list
        MapGrid         : EntityBase list
    }

    type GameStateController = {
        GameStates      : GameState list
    }

    type Room = {
        Location        : Coordinate
        Mask            : Size
        Map             : EntityBase list
    }
 