
namespace GamesSystem
open GameSystem
open Sharpino.Definitions
open Sharpino.Utils
open Sharpino
open Sharpino.Core
open System

type GameEvents =
    | BucketChanged of Guid
    | Closed
    interface Event<Game> with
        member this.Process game =
            match this with
            | BucketChanged bucket ->
                game.ChangeBucket bucket
            | Closed ->
                game.Close ()
    member this.Serialize =
        serializer.Serialize this
    static member Deserialize x =
        serializer.Deserialize<GameEvents> x
