module Program
open parser
open interpreter
//printfn "%A" (parser.build_ast "▶️➕⏸️1️⃣⏸️▶️✖⏸️3️⃣ⓕ3️⃣◀️⏸️🧵This is a test.🧵◀️")
//printfn "%A" (parser.build_ast "▶️➕⏸️1️⃣⏸️▶️➕⏸️3️⃣ⓕ3️⃣◀️⏸️🧵This is ➕⏸️ daw3️⃣◀️ test.🧵◀️")
//printfn "%A" (parser.build_ast "▶️➕⏸️🅱️⏸️▶️✖⏸️3️⃣ⓕ3️⃣◀️⏸️🧵This is ➕⏸️ a ⓕ3️⃣◀️ test.🧵◀️")
//printfn "%A" (parser.build_ast "▶️➕⏸️1️⃣⏸️▶️❌⏸️3️⃣ⓕ3️⃣◀️⏸️🧵This is ➕⏸️ fe test.🧵◀️")

let test1 = "🧽⬅️▶️➕⏸️🧵Hello 🧵⏸️🧵world!🧵◀️⏹️"
let test2 = "🧽⬅️3️⃣⏹️"
let test3 = "🕳️⬅️▶️🖨️⏸️🧵Hello world!🧵◀️⏹️"

let test4 = "🕳️⬅️▶️🖨️⏸️3️⃣◀️⏹️"
let test5 = "Void result of expression -->🕳️⬅️Start expression -->▶️🖨️<-- Print argument⏸️🧵Hello world!🧵<--String literal◀️⏹️<--End of program"
let test7 = "🧽⬅️🆕❤️⏸️➡️▶️❤️⏸️◀️🔚⏹️"
let fibonacci = @"
💠⬅️ fibonacci function (recursive)
    🆕 function definition
        💠⏸️
        🛑⏸️ stopper
        🅰️⏸️ prev
        🅱️ curr
    ➡️ function body
        ❓ if
            ▶️⚖️⏸️🛑⏸️0️⃣◀️  stopper is 0:
        ⏸️ then
            🅰️ prev
        ⏸️ else:
            ▶️💠⏸️💠 recursively call with:
             
            ⏸️
                ▶️➖⏸️🛑⏸️1️⃣◀️ stopper-1
            ⏸️ 
                🅱️ curr
            ⏸️ 
                ▶️➕⏸️🅰️⏸️🅱️◀️  prev+curr
            ◀️
        🔚 if end
    🔚 function end
⏹️
let's wrap it up nicely into an easy to use function
❗⬅️🆕🅰️➡️▶️💠⏸️💠⏸️🅰️⏸️0️⃣⏸️1️⃣◀️🔚⏹️
and define a 'print factorial' function while we're at it
🖨️❗⬅️🆕🅰️➡️▶️🖨️⏸️▶️❗⏸️🅰️◀️◀️🔚⏹️

let's now print a factorial of, say 6!
🕳️⬅️▶️🖨️❗⏸️6️⃣◀️⏹️
"
//let parseline = evaluate_line builtins (parser.build_ast test5)
//printf "%A" (evaluate_line builtins (parser.build_ast fib_line1))
//let tmp = evaluate_line builtins.builtins (parser.build_ast fib_line1)
//let tmp2 = evaluate_line tmp (parser.build_ast fib_line2)

ignore <| run_program (fibonacci)
