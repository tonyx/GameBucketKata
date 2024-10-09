namespace GamesSystem
open GameSystem
open Sharpino.Definitions
open Sharpino.Utils
open Sharpino
open Sharpino.Core
open System

type GameBucketCommands =
    | AddGame of Guid
    | RemoveGame of Guid

    interface AggregateCommand<GameBucket, GameBucketEvents> with
            member this.Execute gameBucket =
                match this with
                | AddGame id ->
                    gameBucket.AddGame id
                    |> Result.map (fun s -> (s, [GameAdded id]))
                | RemoveGame id ->
                    gameBucket.RemoveGame id
                    |> Result.map (fun s -> (s, [GameRemoved id]))
            member this.Undoer = None
