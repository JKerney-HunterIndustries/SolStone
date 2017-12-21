﻿namespace ThingStead.Tests.Verification
open ThingStead.Core.SharedTypes
open ThingStead.TestBuilder.Scripting
open ThingStead.Verification
open ThingStead.Verification.GoldenMaster
open ThingStead.Verification.GoldenMaster.Raw

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
            feature "build reporter" [
                "gives a method that calls the report method"
                    |> testedWith (fun _ ->
                        let mutable wasCalled = false
                        let reportFunction = (fun _ -> wasCalled <- true; ())
                        let reporter = buildReporter reportFunction
                        { Standard = (); Recieved = () } |> reporter |> ignore
                        wasCalled |> expectsToBe true
                    )

                "returns the result of the report method wrapped in an Ok if it has no error"
                    |> testedWith (fun _ -> 
                        let data = "Done"
                        let expected = data |> Ok
                        let reportFunction = (fun _ -> data)
                        let reporter = buildReporter reportFunction
                        let actual = 
                            { Standard = (); Recieved = () } |> reporter
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

