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
        | CalcNone

module constants = 
    let digits = ["0️⃣"; "1️⃣"; "2️⃣"; "3️⃣"; "4️⃣"; "5️⃣"; "6️⃣"; "7️⃣"; "8️⃣"; "9️⃣"]
    let lbracket = "▶️"
    let rbracket = "◀️"
    let dot = "ⓕ"
    let sbracket = "🧵"
    let comma = "⏸️"
    let ignore = "🕳️"
    let none = "💩"
    let assignment = "⬅️"
    let linebreak = "⏹️"
    let reserved = [lbracket; rbracket; dot; sbracket; comma; ignore; none; assignment; linebreak]

let emoji = List.map string (Seq.toList Emoji.All)
//let emoji = Set.union (Set (List.map string (Seq.toList Emoji.All))) (Set constants.reserved)
//let emoji = (Set constants.reserved)
//printf "%A" emoji
let emoji_set = new Set<string>(emoji)

let emoji_identifier = Set.difference emoji_set (new Set<string>(constants.reserved))
let emoji_identifier_start = Set.difference emoji_set (new Set<string>(constants.reserved @ constants.digits))

let pemoji : Parser<string,unit> = choice (List.map pstring <| Seq.toList emoji)
let pcomment = manyTill (anyChar |>> ignore) (lookAhead pemoji |>> ignore <|> eof) |>> fun x -> ()
let psearchfor p = pcomment >>. p
let pidentifier_start = choice (List.map pstring <| Seq.toList emoji_identifier_start)
let pidentifier_suffix = choice (List.map pstring <| Seq.toList emoji_identifier)
let pidentifier_raw = pidentifier_start .>>. many (attempt <| psearchfor pidentifier_suffix) |>> (fun x -> (fst x + (Seq.fold (+) "" (snd x))))
let pidentifier = pidentifier_raw |>>  CalcIdentifier 


let penone = pstring constants.none |>> fun x -> CalcNone

let pedigit = choice (List.map pstring constants.digits)

let pesbracket = pstring constants.sbracket

let pestring = pesbracket >>. manyCharsTill anyChar pesbracket |>> CalcString

let pedot = pstring constants.dot |>> fun x -> "."

let edigits_to_text = Seq.map ((fun x -> Seq.findIndex ((=) x) constants.digits) >> string) >> (Seq.fold (+) "")

let peint = many1 <| (attempt <| psearchfor pedigit) 
                 |>> (edigits_to_text >> int)

let pefloat = many1 (attempt <| psearchfor pedigit) .>> pedot .>>. many1 (attempt <| psearchfor pedigit)
                 |>> fun x -> float (edigits_to_text (fst x ) + "." + edigits_to_text (snd x))



let pcalcnode, pcalcnoderef = createParserForwardedToRef<CalcNode, unit>()

let pcalcint = peint |>> CalcInt |>> CalcNum
let pcalcfloat = pefloat |>> CalcFloat |>> CalcNum
let pcalcoperator = pstring constants.lbracket >>. (psearchfor pidentifier_raw) .>>. many1 (attempt <| ((psearchfor(pstring constants.comma)) >>. (psearchfor pcalcnode))) .>> (psearchfor (pstring constants.rbracket)) |>> CalcExpression

let passign = pstring constants.assignment 
let pstore = pidentifier 
let pignore = pstring constants.ignore  |>> fun x -> CalcNone

let pcalcprefix_raw = choice (List.map (attempt)
    [
        pstore;
        pignore;
    ])

let pcalcprefix = pcalcprefix_raw .>> psearchfor passign

do pcalcnoderef.Value <- choice (List.map attempt
    [
        penone;
        pcalcfloat;
        pcalcint;
        pestring;
        pidentifier;
        pcalcoperator;
    ])

let pcalcline = (psearchfor pcalcprefix) .>>. (psearchfor pcalcnode) .>> psearchfor (pstring constants.linebreak) .>> (pcomment <|> eof)
let build_ast line = 
    let r = run pcalcline line 
    match r with
    | Success(result, _, _)   -> result
    | Failure(errorMsg, _, _) -> failwith errorMsg
