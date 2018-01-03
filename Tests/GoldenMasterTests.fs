namespace ThingStead.Tests.Verification
open ThingStead.TestBuilder.Scripting
open ThingStead.Verification
open ThingStead.Verification.GoldenMaster
open ThingStead.Verification.GoldenMaster.Raw
open System

module GoldenMaster = 

    //let tests : Test list = 
    let rawTests =   
        let comparisonTests = 
            feature "comparison" [
                "compares 2 matching strings and returns that the standard was met"
                    |> testedWith (fun _ ->
                        let value = "Hello tests"
                        let result = value |> isVerifiedAgainst value
                        result |> expectsToBe StandardMet
                    )

                "copares 2 dissimular strings and returns that the standard was not met"
                    |> testedWith (fun _ ->
                        let value = "Hello"
                        let standard = "world"

                        let result = value |> isVerifiedAgainst standard
                        result |> expectsToBe (StandardNotMet { Standard = standard; Recieved = value })

                    )

                "compares 2 matching integers and returns that the standard was met"
                    |> testedWith (fun _ ->
                        let value = 43
                        let result = value |> isVerifiedAgainst value
                        result |> expectsToBe StandardMet
                    )

                "copares 2 dissimular integers and returns that the standard was not met"
                    |> testedWith (fun _ ->
                        let value = 12
                        let standard = 60

                        let result = value |> isVerifiedAgainst standard
                        result |> expectsToBe (StandardNotMet { Standard = standard; Recieved = value })
                    )

                "compares 2 matching byte lists and returns that the standard was met"
                    |> testedWith (fun _ ->
                        let value = [ 100uy; 99uy; 255uy; ]
                        let result = value |> isVerifiedAgainst value
                        result |> expectsToBe StandardMet
                    )

                "copares 2 dis-simular byte lists and returns that the standard was not met"
                    |> testedWith (fun _ ->
                        let value = [ 1uy; 2uy; 3uy; 4uy; 5uy ]
                        let standard = [ 40uy; 41uy; 42uy; ]

                        let result = value |> isVerifiedAgainst standard
                        result |> expectsToBe (StandardNotMet { Standard = standard; Recieved = value })
                    )

                "compares 2 matching byte arrays and returns that the standard was met"
                    |> testedWith (fun _ ->
                        let value = [| 100uy; 99uy; 255uy; |]
                        let expected = [| 100uy; 99uy; 255uy; |]
                        let result = value |> isVerifiedAgainst expected
                        result |> expectsToBe StandardMet
                    )

                "copares 2 dis-simular byte arrays and returns that the standard was not met"
                    |> testedWith (fun _ ->
                        let value = [| 1uy; 2uy; 3uy; 4uy; 5uy |]
                        let standard = [| 40uy; 41uy; 42uy; |]

                        let result = value |> isVerifiedAgainst standard
                        result |> expectsToBe (StandardNotMet { Standard = standard; Recieved = value })
                    )
            ]

        let buildReporterTests = 
            feature "as reporter" [
                "gives a method that calls the report method"
                    |> testedWith (fun _ ->
                        let mutable wasCalled = false
                        let reportFunction = (fun _ -> wasCalled <- true; ())
                        let reporter = asReporter reportFunction
                        { Standard = (); Recieved = () } |> reporter |> ignore
                        wasCalled |> expectsToBe true
                    )

                "returns the result of the report method wrapped in an Ok if it has no error"
                    |> testedWith (fun _ -> 
                        let data = "Done"
                        let expected = data |> Ok
                        let reportFunction = (fun _ -> data)
                        let reporter = asReporter reportFunction
                        let actual = 
                            { Standard = (); Recieved = () } |> reporter
                        actual |> expectsToBe expected
                    )

                "returns the result Error with the exception if the report function throws an error"
                    |> testedWith (fun _ ->
                        let ex = Exception "New Exception"
                        let expected = ex |> Error
                        let reportFunction = fun _ -> raise ex
                        let reporter = asReporter reportFunction

                        let actual = 
                            { Standard = (); Recieved = () } |> reporter

                        actual |> expectsToBe expected
                    )

                "calls the report function with the data passed to it"
                    |> testedWith (fun _ ->
                        let mutable actual = { Standard = ""; Recieved = "" }
                        let data = { Standard = "Standard"; Recieved = "Recieved" }
                        let expected = data 
                        let reportFunction = fun input -> actual <- input; ()
                        let reporter = asReporter reportFunction

                        data |> reporter |> ignore

                        actual |> expectsToBe expected
                    )
            ]

        groupedBy "raw" (
            [
                comparisonTests
                buildReporterTests
            ] |> List.concat
        )

    let tests = 
        suite "Golden Master Verification" (
            [
                rawTests
            ] |> List.concat
        )

