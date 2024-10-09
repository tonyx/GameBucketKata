

namespace GamesSystem
open GameSystem
open Sharpino.Definitions
open Sharpino.Utils
open Sharpino
open Sharpino.Core
open System

type GameState = 
    | Open
    | Closed
    

type Game =
    { Id: Guid
      Name: string
      GameBucketId: Guid 
      State: GameState }
    with
    static member mkGame (name: string) (gameBucketId: Guid) =
        { Id = Guid.NewGuid()
          Name = name
          GameBucketId = gameBucketId 
          State = Open}
        
    member this.ChangeBucket bucketId =
        { this with GameBucketId = bucketId } |> Ok

    member this.Close () =
        { this with State = Closed } |> Ok

    static member StorageName =
        "_game"
    static member Version =
        "_01"
    static member SnapshotsInterval =
        15
    static member Deserialize x =
        serializer.Deserialize<Game> x
    member this.Serialize  =
        this
        |> serializer.Serialize

    interface Aggregate<string> with
        member this.Id = this.Id
        member this.Serialize = this.Serialize