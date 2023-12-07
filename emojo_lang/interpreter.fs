module interpreter
open parser
open Microsoft.FSharp.Collections

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
        if not (context.ContainsKey(oper)) || not (isFunction context[oper]) then
            failwith $"Operation not defined: %s{oper}"
        else 
            let eval_args_folder aggr item = 
                        List.append (snd aggr) [evaluate_expr (fst aggr) item]     
            let new_args = Seq.map (evaluate_expr context) args
            match context[oper] with
                | CalcFunction (oper_args, oper_ast) -> 
                    evaluate_expr (context_add_list2 context oper_args new_args) oper_ast
                | CalcBuiltin (func) ->  func (Seq.toList new_args)
                | _ -> invalidstate()
    | CalcIdentifier x -> 
        if context.ContainsKey(x) then
            context[x]
        else
            failwith $"Variable not defined: %s{x}"
    | _ -> invalidstate()

let evaluate_line context  parsed= 
    match parsed with
    | CalcIdentifier x, ast -> Map.add x (evaluate_expr context ast) context
    | _, ast -> 
        ignore (evaluate_expr context ast)
        context