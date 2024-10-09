
namespace GamesSystem
open GameSystem
open Sharpino.Definitions
open Sharpino.Utils
open Sharpino
open Sharpino.Storage
open Sharpino.CommandHandler
open Sharpino.Core
open FSharpPlus
open FsToolkit.ErrorHandling
open System

module GameSystemService =
    open Sharpino.CommandHandler
    let doNothingBroker: IEventBroker<_> =
        {
            notify = None
            notifyAggregate = None
        }

    type GameService 
        (
            eventStore: IEventStore<string>, 
            eventBroker: IEventBroker<string>,
            gameBucketViewer: AggregateViewer<GameBucket>,
            gameViewer: AggregateViewer<Game>,
            gameSpaceViewer: StateViewer<GamesManagement>
        ) =
        new (eventStore, eventBroker) =
            GameService
                (
                    eventStore, 
                    eventBroker,
                    getAggregateStorageFreshStateViewer<GameBucket, GameBucketEvents, string> eventStore,
                    getAggregateStorageFreshStateViewer<Game, GameEvents, string> eventStore,
                    getStorageFreshStateViewer<GamesManagement, GamesManagementEvents, string> eventStore
                )

        member this.CreateNewBucket(name: string) =
            result {
                let bucket = GameBucket.mkNew name
                let addBucketCommand = GamesMagementCommands.AddGameBucketReference bucket.Id 
                let! added =
                    addBucketCommand
                    |> runInitAndCommand eventStore eventBroker bucket
                return added
            }

        member this.CreateNewGame (name: string, bucketId: Guid) =
            result {
                let game = Game.mkGame name bucketId
                let addGameCommand = GamesMagementCommands.AddGameReference game.Id
                let! added =
                    addGameCommand
                    |> runInitAndCommand eventStore eventBroker game
                return added
            }
        member this.MoveGameToBucket (gameId: Guid, bucketId: Guid) =
            result {
                let! _, game = gameViewer gameId
                let! _, bucketFrom = gameBucketViewer game.GameBucketId 
                let! _, bucketTo = gameBucketViewer bucketId

                // let changeBucketCommand : List<AggregateCommand<Game, GameEvents>> = [GameCommands.ChangeBucket bucketId :> AggregateCommand<Game, GameEvents>]
                let changeBucketCommand  = [(GameCommands.ChangeBucket bucketId) :> AggregateCommand<Game, GameEvents>]
                let removeFromBucketCommand = [(GameBucketCommands.RemoveGame gameId) :> AggregateCommand<GameBucket, GameBucketEvents>]
                let addToBucketCommand = [(GameBucketCommands.AddGame gameId) :> AggregateCommand<GameBucket, GameBucketEvents>]

                return! 
                    runThreeNAggregateCommands<Game, GameEvents, GameBucket, GameBucketEvents, GameBucket, GameBucketEvents, string> 
                        [gameId] [bucketFrom.Id] [bucketId] eventStore eventBroker changeBucketCommand removeFromBucketCommand addToBucketCommand

            }



