module interpreter
open parser
open Microsoft.FSharp.Collections
open System

module builtins = 
    let calc_float num =
        match num with
        | CalcFloat x -> x
        | CalcInt x -> x

    let calc_sum2 a b =
        match (a, b) with
        | CalcNum n_a, CalcNum n_b -> 
            match (n_a, n_b) with
            | CalcInt i_a, CalcInt i_b -> CalcNum(CalcInt(i_a + i_b))
            | (_, _) -> CalcNum(CalcFloat(calc_float n_a + calc_float n_b))
        | CalcString s_a, CalcString s_b -> CalcString(s_a + s_b)
        | (_, _) -> failwith $"Incorrect argument types: cannot sum {a.GetType()} with {b.GetType()}."
    let calc_sum args = List.reduce calc_sum2 args

    let calc_mul2 a b =
        match (a, b) with
        | CalcNum n_a, CalcNum n_b -> 
            match (n_a, n_b) with
            | CalcInt i_a, CalcInt i_b -> CalcNum(CalcInt(i_a * i_b))
            | (_, _) -> CalcNum(CalcFloat(calc_float n_a + calc_float n_b))
        | (_, _) -> failwith $"Incorrect argument types: cannot multiply {a.GetType()} with {b.GetType()}."
    let calc_mul args = List.reduce calc_mul2 args


    let calc_negate num =
        match num with
        | CalcFloat x -> CalcNum(CalcFloat(-x))
        | CalcInt x -> CalcNum(CalcInt(-x))

    let calc_minus args = 
        match args with
        | [CalcNum x] -> calc_negate x
        | [CalcNum x; CalcNum y] -> calc_sum2 (CalcNum(x)) (calc_negate y)
        | [a] -> failwith $"Incorrect argument type: cannot negate {a.GetType()}."
        | [a; b] -> failwith $"Incorrect argument types: cannot subtract from {a.GetType()} a value of {b.GetType()}."
        | _ -> failwith $"Incorrect argument count for minus: expected 1 or 2, got {args.Length}."

    let calc_divide args = 
        match args with
        | [CalcNum x; CalcNum y] -> CalcNum(CalcFloat(calc_float x / calc_float y))
        | [a; b] -> failwith $"Incorrect argument types: cannot divide {a.GetType()} by {b.GetType()}."
        | _ -> failwith $"Incorrect argument count for division: expected 2, got {args.Length}."

    
    let calc_equal2 a b =
        match (a, b) with
        | CalcNum x, CalcNum y  -> (calc_float x) = (calc_float y)
        | CalcString x, CalcString y  -> x = y
        | CalcNone, CalcNone -> true
        | _ -> false

    let calc_equal args = 
        match args with
        | head::tail -> if Seq.forall (calc_equal2 head) tail then CalcNum(CalcInt(1)) else CalcNum(CalcInt(0))
        | _ -> failwith $"Incorrect argument count for equality: expected at least 2, got {args.Length}."


    let calc_print (args: CalcNode list) =
        match args with
        | [arg] -> 
            match arg with 
            | CalcString s -> printf "%s" s
            | CalcNum num -> 
                match num with
                | CalcInt i -> printf "%d" i
                | CalcFloat f -> printf "%f" f
            | CalcNone -> printf "None"
            | _ -> failwith $"{arg.ToString}"
            CalcNone
        | _ -> failwith $"Incorrect argument count for print: expected 1, got {args.Length}." 

    let read_int args = 
        match args with
        | [CalcNone] -> ()
        | [CalcString s] -> printf "%s" s
        | [arg] -> failwith $"Incorrect argument type for read_int: expected STRING or NONE, got {arg}."
        | _ -> failwith $"Incorrect argument count for read_int: expected 1, got {args.Length}."
        try System.Console.ReadLine() |> (int >> CalcInt >> CalcNum)
        with
        | _ -> CalcNone
    let read_float args = 
        match args with
        | [CalcNone] -> ()
        | [CalcString s] -> printf "%s" s
        | [arg] -> failwith $"Incorrect argument type for read_float: expected STRING or NONE, got {arg}."
        | _ -> failwith $"Incorrect argument count for read_int: expected 1, got {args.Length}."
        try System.Console.ReadLine() |> (float >> CalcFloat >> CalcNum)
        with
        | _ -> CalcNone
    let read_line args = 
        match args with
        | [CalcNone] -> ()
        | [CalcString s] -> printf "%s" s
        | [arg] -> failwith $"Incorrect argument type for read_line: expected STRING or NONE, got {arg}."
        | _ -> failwith $"Incorrect argument count for read_int: expected 1, got {args.Length}."
        try System.Console.ReadLine() |> CalcString
        with
        | _ -> CalcNone

    let builtins: Map<string,CalcNode> = Map [
        ("➕",CalcBuiltin(calc_sum));
        ("➖",CalcBuiltin(calc_minus));
        ("➗",CalcBuiltin(calc_divide));
        ("❌",CalcBuiltin(calc_mul));
        ("⚖️",CalcBuiltin(calc_equal));
        ("🖨️",CalcBuiltin(calc_print));
        ("📖🅰️",CalcBuiltin(read_line));
        ("📖⚪️",CalcBuiltin(read_float));
        ("📖1️⃣",CalcBuiltin(read_int));
        ]

let isFunction = function
        | CalcBuiltin (_) -> true
        | CalcFunction (_, _) -> true
        | _ -> false

let context_add_list2 context argnames argvals = 
    Seq.fold2 (fun args name arg -> Map.add name arg args) 
                                        context argnames argvals
let context_sum context addition = 
    let argnames, argvals  = List.unzip (Map.toList addition)
    context_add_list2 context argnames argvals

let invalidstate () = failwith $"if this code was ever reached i have failed as a developer and a human being and you are on your own"


// gotta love immutable data
// makes this so much easier
// </sarcasm>
let rec evaluate_expr (context: Map<string, CalcNode>) ast =
    match ast with
    | CalcNum x -> ast
    | CalcString _ -> ast
    | CalcFunction (_,_) -> ast
    | CalcExpression (oper, args)  -> 
        let eval_args_folder aggr item = 
                    List.append (snd aggr) [evaluate_expr (fst aggr) item]     
        let new_args = Seq.map (evaluate_expr context) args
        match oper with
            | CalcFunction (oper_args, oper_ast) -> 
                evaluate_expr (context_add_list2 context oper_args new_args) oper_ast
            | CalcIdentifier s ->
                match context[s] with
                | CalcFunction (oper_args, oper_ast) -> 
                    evaluate_expr (context_add_list2 context oper_args new_args) oper_ast
                | CalcBuiltin (func) ->  func (Seq.toList new_args)
                | _ -> failwith $"Error: variable {s} of type {context[s].ToString()} is not a function!"
            | _ -> invalidstate()
    | CalcIdentifier x -> 
        if context.ContainsKey(x) then
            context[x]
        else
            failwith $"Variable not defined: %s{x}"
    | CalcBranching (c, t, f) -> 
        match evaluate_expr context c with
        | CalcNum(CalcFloat(0.0)) | CalcNum(CalcInt(0)) | CalcNone -> evaluate_expr context f
        | _ -> evaluate_expr context t
    | _ -> invalidstate()

let evaluate_line context  parsed= 
    match parsed with
    | CalcIdentifier x, ast -> 
        if Map.containsKey x context then
            failwith $"Variable {x} has already been defined"
        
        Map.add x (evaluate_expr context ast) context
    | _, ast -> 
        ignore (evaluate_expr context ast)
        context


let rec run_statements context statements = 
    match statements with
    | head::tail -> 
        let new_context = evaluate_line context head
        run_statements new_context tail
    | [] ->
        context

let run_program = parse_program >> run_statements builtins.builtins