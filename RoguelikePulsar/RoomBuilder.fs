namespace Pulsar

module RoomBuilder =
    open Enumerations
    open Types
    open Constants
    open Defaults
    open Utils

    let BuildRoom ( mask : Size ) : ( EntityBase list ) =
        let BuildBorder ( tile : EntityBase ) : ( EntityBase ) =  
            if IsxoryAllign tile.Location defaultCoordinate || IsxoryAllign tile.Location ( GetRightDownLocation defaultCoordinate mask ) then
                { defaultMapTileEntity with Location = tile.Location ; Sprite = '#' }
            else
                { defaultMapTileEntity with Location = tile.Location ; Sprite = '´' }

        List.init ( mask.Width * mask.Height ) ( fun n -> 
            BuildBorder { defaultMapTileEntity with Location = { x = n % mask.Width  ; y = n / mask.Width  } } )

    let SetEntity ( e' : EntityBase ) ( map : EntityBase list ) : ( EntityBase list ) =
        List.map ( fun ( e : EntityBase ) -> if e.Location.x = e'.Location.x && e.Location.y = e'.Location.y then e' else e ) <| map

    let GetRoomUpdated ( room : Room ) : ( EntityBase list ) =
        List.map ( fun e -> { e with Location = e.Location >+ room.Location } ) <| room.Map

    //Random Settings
    let rec GetRandomSize ( ) : ( Size ) =
        let size = {
            Width = defaultRandom.Next( MIN_ROOM_SIZE , MAX_ROOM_SIZE )
            Height = defaultRandom.Next( MIN_ROOM_SIZE , MAX_ROOM_SIZE )
        }
        if size.Width * size.Height > MAX_ROOM_AREA then GetRandomSize ( ) else size

    let GetRandomRoom ( location : Coordinate ) : ( Room ) = 
        let randomSize = GetRandomSize ( )
        {
            Location    = location
            Mask        = randomSize
            Map         = BuildRoom randomSize
        }

    //Doors
    let GetDoorLocations ( room : Room ) : ( EntityBase list ) =
        room.Map |> List.filter ( fun tile -> tile.Sprite = '^' ) 

    let GetNewDoorLocation ( rm1 : Room ) ( rm2 : Room ) : ( Coordinate ) =
        let GetDirection ( ) : ( Cardinals ) =
            match ( rm1 , rm2 ) with
                | ( rm1 , rm2 ) when rm1.Location.y = rm2.Location.y - rm1.Mask.Height + 1  -> North
                | ( rm1 , rm2 ) when rm1.Location.x = rm2.Location.x - rm1.Mask.Width + 1   -> West
                | ( rm1 , rm2 ) when rm1.Location.x = rm2.Location.x + rm2.Mask.Width - 1   -> East
                | ( rm1 , rm2 ) when rm1.Location.y = rm2.Location.y + rm2.Mask.Height - 1  -> South
                | ( _ , _ ) -> NonCardinal
        
        let maxx' = Max rm2.Location.x rm1.Location.x
        let maxy' = Max rm2.Location.y rm1.Location.y
        let minx' = Min ( rm2.Location.x + rm2.Mask.Width ) ( rm1.Location.x + rm1.Mask.Width )
        let miny' = Min ( rm2.Location.y + rm2.Mask.Height ) ( rm1.Location.y + rm1.Mask.Height )

        match GetDirection ( ) with
            | North -> 
                { x = defaultRandom.Next ( maxx' + 1 , minx' - 1 ) ; y = rm2.Location.y } 
            | South -> 
                { x = defaultRandom.Next ( maxx' + 1 , minx' - 1 ) ; y = rm1.Location.y }
            | West  -> 
                { x = rm2.Location.x ; y = defaultRandom.Next ( maxy' + 1 , miny' - 1 ) }  
            | East  -> 
                { x = rm1.Location.x ; y = defaultRandom.Next ( maxy' + 1 , miny' - 1 ) }
            | NonCardinal -> { x = 0 ; y = 0 }

    //Rooms Validations
    let roomIntersect ( rmA : Room ) ( rmB : Room ) : ( bool ) =
        // the +1/-1 here are to allow the rooms one tile of overlap. this is to allow the rooms to share walls
        // instead of always ending up with two walls between the rooms
        if  rmA.Location.x + rmA.Mask.Width <= rmB.Location.x + 1   || 
            rmA.Location.x >= rmB.Location.x + rmB.Mask.Width - 1   || 
            rmA.Location.y + rmA.Mask.Height <= rmB.Location.y + 1  || 
            rmA.Location.y >= rmB.Location.y + rmB.Mask.Height - 1 then
            false else true

    let AreRoomsConnected ( rm1 : Room ) ( rm2 : Room ) : ( bool ) =
        let IsTheSameDoor ( door : EntityBase ) : ( bool ) =
            let d' = door.Location >+ ( rm1.Location >- rm2.Location )
            ( d'.x < 0 || d'.x > rm2.Mask.Width - 1 || d'.y < 0 || d'.y > rm2.Mask.Height - 1 ) && ( GetTileMap rm2.Map d' ).Sprite = '^'

        GetDoorLocations rm1 |> List.exists ( fun door -> IsTheSameDoor door )

    let HasStairs ( room : Room ) : ( bool ) =
        room.Map |> List.exists ( fun tile -> tile.Sprite = '%' ) 