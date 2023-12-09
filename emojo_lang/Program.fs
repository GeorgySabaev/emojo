module Program
open interpreter

[<EntryPoint>]
let main args =
    match Seq.toList args with
    |  [path] -> 
        let program = 
            try
            System.IO.File.ReadAllText(path)
            with
            | _ -> 
                printfn $"Error while reading file {path}"
                ""
        ignore <| run_program program
        ()
    | _ -> printfn $"Incorrect argument count, expected 1, got {args.Length}"
    0



