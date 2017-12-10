﻿namespace SolStone.Tests

module Program =
    [<EntryPoint>]
    let main _argv =
        [
            TestRunners.DefaultRunner.run ()
            TestBuilders.Scripting.run ()
            Core.VerificationTests.run ()
        ] |> List.sum
