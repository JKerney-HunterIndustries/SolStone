namespace ThingStead.Verification.GoldenMaster

type StandardsData<'a when 'a : equality> =
    {
        Standard : 'a
        Recieved : 'a
    }

type StandardsVerificationResult<'a when 'a : equality> = 
    | StandardMet
    | StandardNotMet of StandardsData<'a>

module Raw =
    let isVerifiedAgainst standard recieved = 
        if standard = recieved then StandardMet
        else StandardNotMet { Standard = standard; Recieved = recieved }

    let asReporter fn data = 
        try
            fn data |> Ok
        with
        | ex -> ex |> Error
