namespace Pulsar

module Defaults =
    open System
    open Enumerations
    open Types

    let defaultColor = ConsoleColor.White
    let defaultRandom = Random ( )

    let defaultCoordinate = {
        x       = 0
        y       = 0
    }

    let dafaultSize = {
        Width   = 0
        Height  = 0
    }

    let defaultMapTileEntity = { 
        Entity          = MapTileEntity { MapVisibility = MapVisibility.Visible }
        PrevLocation    = None
        Location        = defaultCoordinate
        Color           = Some defaultColor
        BackgroundColor = None
        Sprite          = '.'
        IsVisible       = true
        IsEnabled       = true 
    }

