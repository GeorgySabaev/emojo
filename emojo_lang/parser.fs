module parser
open FParsec
open NeoSmart.Unicode

type CalcNumType = 
        | CalcInt of int
        | CalcFloat of float

type CalcNode =
        | CalcNum of CalcNumType
        | CalcIdentifier of string
        | CalcExpression of oper: string * args: CalcNode list
        | CalcFunction of args: string list * CalcNode
        | CalcString of string
        | CalcBuiltin of (list<CalcNode> -> CalcNode)

module constants = 
    let digits = ["0️⃣"; "1️⃣"; "2️⃣"; "3️⃣"; "4️⃣"; "5️⃣"; "6️⃣"; "7️⃣"; "8️⃣"; "9️⃣"]
    let lbracket = "▶️"
    let rbracket = "◀️"
    let dot = "ⓕ"
    let sbracket = "🧵"
    let comma = "⏸️"
    let reserved = [lbracket; rbracket; dot; sbracket; comma]

let emoji = List.map string (Seq.toList Emoji.All)
let emoji_set = new Set<string>(emoji)

let emoji_identifier = Set.difference emoji_set (new Set<string>(constants.reserved))
let emoji_identifier_start = Set.difference emoji_set (new Set<string>(constants.reserved @ constants.digits))

let pidentifier_start = choice (List.map pstring <| Seq.toList emoji_identifier_start)
let pidentifier_suffix = choice (List.map pstring <| Seq.toList emoji_identifier)
let pidentifier_raw = pidentifier_start .>>. many pidentifier_suffix |>> (fun x -> (fst x + (Seq.fold (+) "" (snd x))))
let pidentifier = pidentifier_raw |>>  CalcIdentifier 
// wip code
let opers = ["➕"; "➖"; "❌"; "➗"]






let max_emoji_codepoint_length = 10 

let pedigit = choice (List.map pstring constants.digits)

let pesbracket = pstring constants.sbracket

let pestring = pesbracket >>. manyCharsTill anyChar pesbracket |>> CalcString

let pedot = pstring constants.dot |>> fun x -> "."

let edigits_to_text = Seq.map ((fun x -> Seq.findIndex ((=) x) constants.digits) >> string) >> (Seq.fold (+) "")

let peint = many1 pedigit 
                 |>> (edigits_to_text >> int)

let pefloat = many1 pedigit .>> pedot .>>. many1 pedigit
                 |>> fun x -> float (edigits_to_text (fst x ) + "." + edigits_to_text (snd x))



let pcalcnode, pcalcnoderef = createParserForwardedToRef<CalcNode, unit>()

let pcalcint = peint |>> CalcInt |>> CalcNum
let pcalcfloat = pefloat |>> CalcFloat |>> CalcNum
let pcalcoperator = (pstring constants.lbracket >>. pidentifier_raw .>>. many1 (pstring constants.comma >>. pcalcnode) .>> pstring constants.rbracket) |>> CalcExpression

do pcalcnoderef.Value <- choice (List.map attempt
    [
        pcalcfloat;
        pcalcint;
        pestring;
        pcalcoperator;
        pidentifier;
    ])
let build_ast line = 
    let r = run pcalcoperator line 
    match r with
    | Success(result, _, _)   -> result
    | Failure(errorMsg, _, _) -> failwith errorMsg
