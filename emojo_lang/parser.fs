module parser
open FParsec
open NeoSmart.Unicode

type CalcNode =
        | CalcInt of int
        | CalcFloat of float
        | CalcOperator of oper: string * args: CalcNode list
        | CalcIdentifier of string
        | CalcString of string

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
let pidentifier = pidentifier_start .>>. many1 pidentifier_suffix |>> (fun x -> CalcIdentifier (fst x + (Seq.fold (+) "" (snd x))))

// wip code
let opers = ["➕"; "➖"; "✖"; "➗"]






let max_emoji_codepoint_length = 10 

let poper = choice (List.map pstring opers)

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

let pcalcint = peint |>> CalcInt
let pcalcfloat = pefloat |>> CalcFloat
let pcalcoperator = (pstring constants.lbracket >>. poper .>>. many1 (pstring constants.comma >>. pcalcnode) .>> pstring constants.rbracket) |>> CalcOperator

do pcalcnoderef.Value <- choice (List.map attempt
    [
        pcalcfloat;
        pcalcint;
        pestring;
        pcalcoperator;
        pidentifier;
    ])
let build_ast = run pcalcoperator
