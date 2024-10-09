namespace GamesSystem
open GameSystem
open Sharpino.Definitions
open Sharpino.Utils
open Sharpino
open System

type GamesManagement = {
    GameReferences: List<Guid>
    GameBucketsReferences: List<Guid>
} with 

    member this.AddGameReference gameReference =
        { 
            this 
                with GameReferences = gameReference :: this.GameReferences 
        }  
        |> Ok
    member this.RemoveGameReference gameReference =
        { 
            this 
                with GameReferences = this.GameReferences |> List.filter (fun x -> x <> gameReference) 
        }  
        |> Ok

    member this.AddGameBucketReference gameBucketReference =
        { 
            this 
                with GameBucketsReferences = gameBucketReference :: this.GameBucketsReferences 
        }  
        |> Ok
    member this.RemoveGameBucketReference gameBucketReference =
        { 
            this 
                with GameBucketsReferences = this.GameBucketsReferences |> List.filter (fun x -> x <> gameBucketReference) 
        }  
        |> Ok

    static member Zero =
        {
            GameReferences = []
            GameBucketsReferences = []
        }
    static member StorageName =
        "_customer_space"
    static member Version =
        "_01"
    static member SnapshotsInterval =
        15
    static member Deserialize x =
        serializer.Deserialize<GamesManagement> x
    member this.Serialize  =
        this
        |> serializer.Serialize

