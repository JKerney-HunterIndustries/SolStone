namespace SolStone.TestRunner.Default
open SolStone.Core.SharedTypes
open System

module Framework =
    let shuffle<'a> (getRandom: (int * int) -> int) (items: Test list) =
        let arr = items |> List.toArray

        let rec shuffle pt (arr: Test []) =
            if pt >= arr.Length
            then arr
            else
                let pt2 = getRandom (0, arr.Length - 1)
                let hold = arr.[pt]
                arr.[pt] <- arr.[pt2]
                arr.[pt2] <- hold
                shuffle (pt + 1) arr

        let shuffleTimes times arr = 
            seq { for _ in 1 .. times do yield 0 }
                |> Seq.map shuffle
                |> Seq.fold (fun a fn -> fn a) arr

        shuffleTimes 3 arr |> List.ofArray

    let addTest (result : TestExecutionReport) test =
        try
            match test.TestFunction () with
            | Success
                -> result |> addSuccess test Success
            | Failure failure -> 
                result |> addFailure test (Failure failure)
        with
        | e -> 
            let failure = e |> ExceptionFailure |> Failure
            result |> addFailure test failure


    let executerWithSeed tests seed =
        let rand = Random (seed) 
        
        tests
            |> shuffle (rand.Next)
            |> List.fold addTest { startingReport with Seed = Some seed } 

    let executer (tests : Test list) = 
        let dateTimeAsIntSeed = int(DateTime.Now.Ticks &&& 0x0000FFFFL)
        dateTimeAsIntSeed |> executerWithSeed tests