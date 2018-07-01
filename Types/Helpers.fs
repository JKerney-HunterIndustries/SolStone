namespace Thingstead.Types

[<AutoOpen>]
module Helpers = 
    let Success : TestResult = Ok ()

    let Failure failureType : TestResult = Error failureType

    let emptyEnvironment : Environment = Map.empty<string, string list>
    let testTemplate = 
            {
                Name = "[need a name for this test]"
                Path = None
                TestMethod = fun _ ->
                    "Not Yet Implimented"
                    |> Ignored
                    |> Failure
                Before = fun env -> Ok env
                After = fun _ -> Ok ()
            }

    let applyToTemplate template testMethod name = 
        { template with
            Name = name
            TestMethod = testMethod
        }

    let ``Not Yet Implimented`` : TestResult = 
        "Not Yet Implimented"
        |> Ignored
        |> Error 

    let Not_Yet_Implimented = ``Not Yet Implimented``     

    let withFailComment comment result =
        match result with
        | Error f -> FailureWithComment (f, comment) |> Failure
        | r -> r

    let withFailMessage message = withFailComment message    

    let combine (resultB) (resultA) =
        match resultA, resultB with
        | Error (BeforeFailure before), Error (AfterFailure after)
        | Error (AfterFailure after), Error (BeforeFailure before) ->
            MultiFailure (before |> BeforeFailure, after |> AfterFailure)
            |> Error
        | Error (BeforeFailure before), _
        | _, Error (BeforeFailure before) ->
            before
            |> BeforeFailure
            |> Error
        | Error (AfterFailure after), _
        | _, Error (AfterFailure after) ->
            after
            |> AfterFailure
            |> Error
        | Error error, Ok _
        | Ok _, Error error ->
            error
            |> Error
        | _ -> resultA

    let stage = 
        {
            BeforeStage = (fun env _ -> Ok env)
            Steps = []
            AfterStage = (fun _ _ -> Ok ())
            Filter = (fun input -> 
                    match input with
                    | Tests tests -> tests
                    | Results (tests, _) -> tests
                )
        }

    let pipeline = 
        {
            Name = None
            Tests = []
            BeforePipeline = fun env _ -> Ok env
            Stages = []
            AfterPipeline = fun _ _ -> Ok ()
        }