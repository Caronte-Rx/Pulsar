namespace Pulsar

module Inputs =
    open Enumerations
    
    let rec GetInput ( ) : Input = 
        let input = System.Console.ReadKey ( true ) 
        match input.Key with
            | System.ConsoleKey.RightArrow  -> Right
            | System.ConsoleKey.DownArrow   -> Down
            | System.ConsoleKey.LeftArrow   -> Left
            | System.ConsoleKey.UpArrow     -> Up
            | System.ConsoleKey.Q           -> Exit
            | _                             -> GetInput ( )
