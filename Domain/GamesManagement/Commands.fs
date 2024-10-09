namespace GamesSystem
open GameSystem
open Sharpino.Definitions
open Sharpino.Utils
open Sharpino
open Sharpino.Core
open System

type GamesMagementCommands =
    | AddGameReference of Guid
    | RemoveGameReference of Guid
    | AddGameBucketReference of Guid
    | RemoveGameBucketReference of Guid

    interface Command<GamesManagement, GamesManagementEvents> with
            member this.Execute gamesManagement =
                match this with
                | AddGameReference id ->
                    gamesManagement.AddGameReference id
                    |> Result.map (fun s -> (s, [GameReferenceAdded id]))
                | RemoveGameReference id ->
                    gamesManagement.RemoveGameReference id
                    |> Result.map (fun s -> (s, [GameReferenceRemoved id]))
                | AddGameBucketReference id ->
                    gamesManagement.AddGameBucketReference id
                    |> Result.map (fun s -> (s, [GameBucketReferenceAdded id]))
                | RemoveGameBucketReference id ->
                    gamesManagement.RemoveGameBucketReference id
                    |> Result.map (fun s -> (s, [GameBucketReferenceRemoved id]))
            member this.Undoer = None    
