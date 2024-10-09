namespace GamesSystem
open GameSystem
open Sharpino.Definitions
open Sharpino.Utils
open Sharpino
open Sharpino.Core
open System

type GameBucket = {
    Id: Guid
    Name: string
    Games: List<Guid>
} 

    with 
    static member mkNew name =
        {
            Id = Guid.NewGuid()
            Name = name
            Games = []
        }
    member this.AddGame game =
        { 
            this 
                with Games = game :: this.Games 
        }  
        |> Ok
    member this.RemoveGame game =
        {
            this 
                with Games = this.Games |> List.filter (fun x -> x <> game) 
        }
        |> Ok

    static member StorageName =
        "_game_bucket"
    static member Version =
        "_01"
    static member SnapshotsInterval =
        15
    static member Deserialize x =
        serializer.Deserialize<GameBucket> x
    member this.Serialize  =
        this
        |> serializer.Serialize

    interface Aggregate<string> with
        member this.Id = this.Id
        member this.Serialize = this.Serialize
