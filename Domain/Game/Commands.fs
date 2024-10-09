
namespace GamesSystem
open GameSystem
open Sharpino.Definitions
open Sharpino.Utils
open Sharpino
open Sharpino.Core
open System

type GameCommands =
    | ChangeBucket of Guid
    | Close

    interface AggregateCommand<Game, GameEvents> with
        member this.Execute game =
            match this with
            | ChangeBucket bucket ->
                game.ChangeBucket bucket
                |> Result.map (fun s -> (s, [BucketChanged bucket]))
            | Close ->
                game.Close ()
                |> Result.map (fun s -> (s, [Closed]))
        member this.Undoer = None