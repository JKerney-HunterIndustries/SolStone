namespace ThingStead.Tests.Verification
open ThingStead.Core.SharedTypes
open ThingStead.TestBuilder.Scripting
open ThingStead.Verification
open ThingStead.Verification.GoldenMaster
open ThingStead.Verification.GoldenMaster.Raw

module GoldenMaster = 

    //let tests : Test list = 
    let comparison = 
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

    let tests = 
        suite "Golden Master Verification" (
            [
                comparison
            ] |> List.concat
        )

