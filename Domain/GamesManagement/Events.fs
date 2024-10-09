namespace GamesSystem
open GameSystem
open Sharpino.Definitions
open Sharpino.Utils
open Sharpino
open Sharpino.Core
open System

type GamesManagementEvents =
    | GameReferenceAdded of Guid
    | GameReferenceRemoved of Guid
    | GameBucketReferenceAdded of Guid
    | GameBucketReferenceRemoved of Guid

    interface Event<GamesManagement> with

        member this.Process gamesManagement =
            match this with
            | GameReferenceAdded gameReference ->
                gamesManagement.AddGameReference gameReference
            | GameReferenceRemoved gameReference ->
                gamesManagement.RemoveGameReference gameReference
            | GameBucketReferenceAdded gameBucketReference ->
                gamesManagement.AddGameBucketReference gameBucketReference
            | GameBucketReferenceRemoved gameBucketReference ->
                gamesManagement.RemoveGameBucketReference gameBucketReference
    member this.Serialize =
        serializer.Serialize this
    static member Deserialize x =
        serializer.Deserialize<GamesManagementEvents> x