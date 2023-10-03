namespace Pulsar

module MapBuilder =
    open Enumerations
    open Types
    open Constants
    open Defaults
    open Utils
    open RoomBuilder

    let CanFitRoom ( room : Room ) ( roomList : Room list ) : ( bool ) =
        match room with
            | r when r.Location.x < 0 
                || r.Location.x + r.Mask.Width > WORLD_SIZE.Width - 1 -> false
            | r when r.Location.y < 0 
                || r.Location.y + r.Mask.Height > WORLD_SIZE.Height - 1 -> false
            | _ -> not( List.exists ( fun r' -> roomIntersect room r' ) <| roomList )


    let findRoomAttachment ( room : Room ) ( roomsList : Room list ) : ( Room list ) =
        let rm = PickRandomFrom <| roomsList
        // Randomly position this room on one of the sides of the random room
        let GetRoomLocation ( ) : ( Coordinate ) =
            match PickRandomFrom [ North ; East ; South ; West ] with
                | North ->
                    { x = defaultRandom.Next ( rm.Location.x - room.Mask.Width + 3 , rm.Location.x + rm.Mask.Width - 3 ) ; y = rm.Location.y - room.Mask.Height + 1 }
                | West -> 
                    { x = rm.Location.x - room.Mask.Width + 1 ; y = defaultRandom.Next ( rm.Location.y - room.Mask.Height + 3 , rm.Location.y + rm.Mask.Height - 2 ) }     
                | East ->
                    { x = rm.Location.x + rm.Mask.Width - 1 ; y = defaultRandom.Next ( rm.Location.y - room.Mask.Height + 3 , rm.Location.y + rm.Mask.Height - 2 ) }
                | South -> 
                    { x = defaultRandom.Next ( rm.Location.x - room.Mask.Width + 3 , rm.Location.x + rm.Mask.Width - 3 ) ; y = rm.Location.y + rm.Mask.Height - 1 }
            
        let newRoom = { room with Location = GetRoomLocation ( ) }
        if CanFitRoom newRoom roomsList then
            let doorInNewRoom = { defaultMapTileEntity with Location = GetNewDoorLocation rm newRoom >- newRoom.Location ; Sprite = '^' } 
            let doorInOldRoom = { defaultMapTileEntity with Location = { x = doorInNewRoom.Location.x + newRoom.Location.x ; y = doorInNewRoom.Location.y + newRoom.Location.y } >- rm.Location ; Sprite = '^' }
            let newRoom' = { newRoom with Map = SetEntity doorInNewRoom <| newRoom.Map }

            let UpdateRoom ( rm' : Room ) : ( Room ) = { rm' with Map = SetEntity doorInOldRoom <| rm'.Map }
            newRoom' :: ( List.map ( fun r -> if r.Location.x = rm.Location.x && r.Location.y = rm.Location.y then UpdateRoom r else r ) <| roomsList )
        else
            roomsList

    let GenerateRoom ( roomList : Room list ) : ( Room list ) =
        let newRoom = GetRandomRoom defaultCoordinate

        let rec tryToPlace ( iter : int ) : ( Room list ) = 
            if iter <= 0 then
                roomList
            else
                let list' = findRoomAttachment newRoom roomList
                if list'.Length > roomList.Length then list' else tryToPlace ( iter - 1 )

        tryToPlace ( 150 )

    let rec GenerateRooms ( roomList : Room list ) : ( Room list ) =
        let rec tryToGenerate ( roomList' : Room list ) ( iter : int ) : ( Room list ) = 
            if iter <= 0  || roomList'.Length >= MAX_NUM_ROOMS then
                roomList'
            else
                tryToGenerate <| GenerateRoom roomList' <| iter - 1 
                
        tryToGenerate roomList <| MAX_NUM_ROOMS * 5

    let BuildWorld ( ) : ( EntityBase list ) =
        let centerOfMap = { x = WORLD_SIZE.Width  / 2 ; y = WORLD_SIZE.Height / 2 }
        let roomList = GenerateRooms [ GetRandomRoom centerOfMap ]

        List.collect ( fun r -> GetRoomUpdated r ) <| roomList
              