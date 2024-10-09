
namespace GamesSystem
open GameSystem
open Sharpino.Definitions
open Sharpino.Utils
open Sharpino
open Sharpino.Core
open System

type GameBucketEvents =
    | GameAdded of Guid
    | GameRemoved of Guid

    interface Event<GameBucket> with
        member this.Process gameBucket =
            match this with
            | GameAdded game ->
                gameBucket.AddGame game
            | GameRemoved game ->
                gameBucket.RemoveGame game
    member this.Serialize =
        serializer.Serialize this
    static member Deserialize x =
        serializer.Deserialize<GameBucketEvents> x