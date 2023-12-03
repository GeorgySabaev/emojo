module Program
open parser
open interpreter
//printfn "%A" (parser.build_ast "▶️➕⏸️1️⃣⏸️▶️✖⏸️3️⃣ⓕ3️⃣◀️⏸️🧵This is a test.🧵◀️")
//printfn "%A" (parser.build_ast "▶️➕⏸️1️⃣⏸️▶️➕⏸️3️⃣ⓕ3️⃣◀️⏸️🧵This is ➕⏸️ daw3️⃣◀️ test.🧵◀️")
//printfn "%A" (parser.build_ast "▶️➕⏸️🅱️⏸️▶️✖⏸️3️⃣ⓕ3️⃣◀️⏸️🧵This is ➕⏸️ a ⓕ3️⃣◀️ test.🧵◀️")
//printfn "%A" (parser.build_ast "▶️➕⏸️1️⃣⏸️▶️❌⏸️3️⃣ⓕ3️⃣◀️⏸️🧵This is ➕⏸️ fe test.🧵◀️")

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
    | (_, _) -> failwith $"Incorrect operand types: cannot sum {a.GetType()} with {b.GetType()}."

let calc_negate num =
    match num with
    | CalcFloat x -> CalcNum(CalcFloat(-x))
    | CalcInt x -> CalcNum(CalcInt(-x))

let calc_minus args = 
    match args with
    | [CalcNum x] -> calc_negate x
    | [CalcNum x; CalcNum y] -> calc_sum2 (CalcNum(x)) (calc_negate y)
    | [a] -> failwith $"Incorrect operand type: cannot negate {a.GetType()}."
    | [a; b] -> failwith $"Incorrect operand types: cannot subtract from {a.GetType()} a value of {b.GetType()}."
    | _ -> failwith $"Incorrect operand count for minus: expected 1 or 2, got {args.Length}."

let calc_divide args = 
    match args with
    | [CalcNum x; CalcNum y] -> CalcNum(CalcFloat(calc_float x / calc_float y))
    | [a; b] -> failwith $"Incorrect operand types: cannot divide {a.GetType()} by {b.GetType()}."
    | _ -> failwith $"Incorrect operand count for division: expected 2, got {args.Length}."

let calc_sum args = List.reduce calc_sum2 args


let builtins: Map<string,CalcNode> = Map [("➕",CalcBuiltin(calc_sum));]

printf "%A" (evaluate_expr builtins (parser.build_ast "▶️➕⏸️🧵Hello 🧵⏸️🧵world!🧵◀️"))