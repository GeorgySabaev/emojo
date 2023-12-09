# EMOJO

This is the documentation for EMOJO, possibly the most Gen Z language to date.
EMOJO is a functional language and is somewhat reminiscent of LISP but with several noticeable (and entirely uncalled for) differences.

## Usage
Build the project and run the executable in the console with a path to a file with EMOJO source code as an argument. A file with any extension will be accepted, but it is strongly recommended to use the language's source code extension, which is `filename.✨`.

## Syntax

### Basics

Emojo is an Emoji-only language.

This means that only emoji can be used when writing in EMOJO. Anything that is not an emoji is considered a comment and will be ignored by the parser.

The only exception to this rule are string literals, as described below in the Data Types section. This feature goes against the spirit of EMOJO so I was hesitant to add it, but unfortunately it has to be supported for the sake of user convenience.

### Nodes
Anything that can be evaluated into a value of a certain data type in EMOJO is considered a node. This includes data type literals, identifiers, function calls and other similar constructs. 

### Data types
EMOJO is a dynamically-typed language, and supports the following data types:

- INTEGER: A signed 32 bit integer number.  
The literal for an integer is a sequence of keycap digit emoji (`0️⃣`, `1️⃣`, `2️⃣`, `3️⃣`, `4️⃣`, `5️⃣`, `6️⃣`, `7️⃣`, `8️⃣` and `9️⃣`). 
(ex. `4️⃣2️⃣` evaluates to `42`)
- FLOAT: A signed 32 bit floating point number.  
The literal for an integer is a sequence of keycap digit emoji for the whole part, followed by a `⏺` symbol (which represents the floating point) and a second sequence of keycap digits for the fractional part.  
(ex. `4️⃣2️⃣0️⃣⏺6️⃣9️⃣` evaluates to `420.69`)  
**Note: use the `➖` function (see** [Built-in functions](#Builtins)**) to represent negative numbers.**
- STRING: A string of characters.  
The literal for a string of characters is the string itself, wrapped around with two string emoji (`🧵`).  
(ex. `🧵Making this language was a mistake.🧵` evaluates to `"Making this language was a mistake."`)  
Note: this is the only case where non-emoji text will not be ignored by the interpreter.
- NONE: A data type with only one value (None).  
The literal for the none type value is the Unicode character 💩, availible in the Unicode Standard since version 6.0 under the name `U+1F4A9 PILE OF POO`. 
- FUNCTION: A lambda function data type.  
The literal a lambda function starts with `🆕`, followed by several [identifiers](#Identifiers) separated with `⏸️`, then `➡️` followed by a single node (the function body) and ends with an `🔚`.

### <a name="Expressions"></a> Expressions
Function calls in EMOJO are represented with expressions. An expression consists of an operator (either a built-in/user-defined identifier or a lambda function literal) and optionally any number of argument nodes. The operator and each argument must be separated by pause symbol emoji (`⏸️`), and the whole expression must be wrapped with ▶️ and ◀️, aka the emoji versions (with `U+FE0F VARIATION SELECTOR-16`) of `U+25B6 BLACK RIGHT-POINTING TRIANGLE` and `U+25C0 BLACK LEFT-POINTING TRIANGLE` respectively.

For example:

```▶️➕⏸️1️⃣⏸️▶️🅱️⏸️3️⃣⏺3️⃣◀️⏸️😋👌1️⃣◀️```

evaluates to:

```1 + 🅱️(3.3) + 😋👌1️⃣```

where:
- `🅱️(3.3)` is function `🅱️` being called with 3.3 as an argument
- `😋👌1️⃣` is a user-defined variable

There is no lazy evaluation or short-circuiting, every argument node **will** be evaluated recursively before the function is called, even if the argument count or types are not valid.

For user-defined lambda functions, the number of arguments provided to the function is checked at runtime, and if it matches the number of identifiers in its definition the body will be evaluated with the parameter identifiers assigned to the argument nodes of the function call. Otherwise, an error is raised.
All the outer scope variables are availible, unless they're shadowed by function parameters with the same identifier.

### <a name="Branching"></a> Branching
Since all the arguments in functions are evaluated before the function is ever called, a separate syntax construct exists for branching in the control flow.
A branching statement takes the form:

`❓[NODE_C]⏸️[NODE_T]⏸️[NODE_F]🔚`,

and returns `[NODE_F]` if `[NODE_C]` is equal to 0 or is a None value, and `[NODE_T]` otherwise. The discarded node is **not** evaluated.

### <a name="Identifiers"></a> Identifiers
All variables in the interpreter's context, whether built-in or defined by the user, are assigned an identifier. An identifier is any sequence of emoji, none of which are reserved by the language. An identifier cannot start with a keycap number emoji, but any subsequent emoji may be a keycap number.

Examples of valid identifiers:
- `🧽`
- `🔫😂👌`
- `🅰️🅱️🅾️🅱️🅰️`
- `🌿4️⃣2️⃣0️⃣`

Examples of invalid identifiers:
- `6️⃣9️⃣❤️6️⃣9️⃣` (Starts with a number)
- `🎵▶️🎶` (Contains a reserved character ("`▶️`"))

### <a name="Statements"></a> Statements
Statements are the an equivalent to lines of code in EMOJO, and are **not** nodes. They contain a single node, which is evaluated recursively and either assigned an unoccupiied identifier or discarded.

A statement always takes the form of:

`[TARGET]⬅️[NODE]⏹️`

Where `[TARGET]` is either an identifier, in which case the node is evaluated and associated with it for the rest of the interpreter's runtime (unless the identifier is occupied, in which case an error is thrown), or `🕳️`, in which case the result of evaluation is discarded.

Any program is a sequence of statements, which will be executed one after another. The variables defined by the previous statements will continue to be defined for the following ones. 

Note that newline characters are not emoji, and are therefore entirely optional even between statements.

Here are some example statements:
- `🕳️⬅️▶️🖨️⏸️🧵Hello world!🧵◀️⏹️`  
An obligatory "Hello world!"
- `Void result of expression -->🕳️⬅️Start expression -->▶️🖨️<-- Print argument⏸️🧵Hello world!🧵<--String literal◀️⏹️<--End of program`  
Any non-emoji text is ignored (unless it's in a string literal). Comment anywhere you like!
- `🔫Even😂inside👌identifiers❤️!🅱️⬅️▶️➖⏸️2️⃣2️⃣2️⃣2️⃣◀️⏹️`
- `🔫😂👌❤️🅱️⬅️▶️➖⏸️2️⃣Or2️⃣number2️⃣literals2️⃣!2️⃣◀️⏹️`

### <a name="Builtins"></a> Built-in functions
At the time of the writing the following built-in functions are availible in the language:
#### Arithmetic
- `➕`: Performs number addition or string concatenation.  
**Accepts:**  
  -  Any number of numeric arguments (at least 2)
  Returns their sum (INT if all arguments are INT, otherwise FLOAT)  
  **or**
  -  Any number of STRING arguments (at least 2)  
  Returns the result of concatenating all the strings, in order.
- ➖: Minus sign, negates a single number or subtracts one number from another.  
**Accepts:** 
  -  Any 1 numeric argument
  Returns the same number multiplied bu -1 (INT if the argument is INT, otherwise FLOAT)  
  **or**
  -  Any 2 numeric arguments  
  For any 2 numeric arguments `🅰️` and `🅱️`, `▶️➖⏸️🅰️⏸️🅱️◀️` is equivalent to `▶️➕⏸️🅰️⏸️▶️➖⏸️🅱️◀️◀️`
- `❌`: Multiplies together all the arguments, provided they are all numeric types.
**Accepts:**  
  -  Any number of numeric arguments (at least 2)
  Returns their product (INT if all arguments are INT, otherwise FLOAT) 
- `➖`: Minus sign, negates a single number or subtracts one number from another.  
**Accepts:** 
  -  Any 1 numeric argument
  Returns the same number multiplied bu -1 (INT if the argument is INT, otherwise FLOAT)  
  **or**
  -  Any 2 numeric arguments  
  For any 2 numeric arguments `🅰️` and `🅱️`, `▶️➖⏸️🅰️⏸️🅱️◀️` is equivalent to `▶️➕⏸️🅰️⏸️▶️➖⏸️🅱️◀️◀️`
- `➗`: Divides 2 numeric arguments.
**Accepts:** 
  -  Any 2 numeric arguments
  Returns the first argument divided by the second (FLOAT regardless of the argument types)  
#### Comparison
- `⚖️`: Checks the arguments for equality.  
**Accepts:**  
  -  At least 2 arguments of any type.  
  Returns 1 if all the arguments are equal, otherwise returns 0.  
  2 objects are considered equal in the following cases (exhaustive list):
     - They're both equal numbers.
     - They're both equal strings.
     - They're both NONE.
#### Input/Output
- `🖨️`: Outputs a value to the console.  
**Accepts:**  
  -  Any 1 numeric/STRING/NONE argument  
  Prints the argument to stdout (`"None"` for a NONE value).  
  Returns NONE.
- `📖🅰️`: Reads a line from the console.  
**Accepts:**  
  -  1 STRING/NONE argument  
  Prints the argument to stdout (`""` for a NONE value) and requests user input.  
  Returns the next line of console input.
- `📖1️⃣`: Reads an INT from the console.  
**Accepts:**  
  -  1 STRING/NONE argument  
  Prints the argument to stdout (`""` for a NONE value) and requests user input.  
  Returns the next line of console input converted to an INT, or NONE if such a conversion is impossible.
- `📖⚪️`: Reads a FLOAT from the console.  
**Accepts:**  
  -  1 STRING/NONE argument  
  Prints the argument to stdout (`""` for a NONE value) and requests user input.  
  Returns the next line of console input converted to a FLOAT, or NONE if such a conversion is impossible.

## But how do I even use this?
Here are some snippets of code, all of them are availible in the `code_snippets` directory: 

#### "Hello world":
```emojo
🕳️⬅️▶️🖨️⏸️🧵Hello world!🧵◀️⏹️
```
> Output: `Hello world!`

#### Printing the Nth Fibonacci number:
```emojo
💠⬅️🆕💠⏸️🛑⏸️🅰️⏸️🅱️➡️❓▶️⚖️⏸️🛑⏸️0️⃣◀️⏸️🅰️⏸️▶️💠⏸️💠⏸️▶️➖⏸️🛑⏸️1️⃣◀️⏸️🅱️⏸️▶️➕⏸️🅰️⏸️🅱️◀️◀️️️🔚🔚⏹️🌀⬅️🆕🅰️➡️▶️💠⏸️💠⏸️🅰️⏸️0️⃣⏸️1️⃣◀️🔚⏹️🖨️🌀⬅️🆕🅰️➡️▶️🖨️⏸️▶️🌀⏸️🅰️◀️◀️🔚⏹️🕳️⬅️▶️🖨🌀⏸️▶️📖1️⃣⏸️🧵Input n: 🧵◀️◀️⏹️
```
> Input: `6`  
Output: `8`

...okay this one might need some explaining.
#### Printing the Nth Fibonacci number but actually readable by a human being:
```emojo
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
🌀⬅️🆕🅰️➡️▶️💠⏸️💠⏸️🅰️⏸️0️⃣⏸️1️⃣◀️🔚⏹️
and define a 'print fibonacci' function while we're at it
🖨️🌀⬅️🆕🅰️➡️▶️🖨️⏸️▶️🌀⏸️🅰️◀️◀️🔚⏹️

let's now print a Nth (user-defined) fibonacci number!

🕳️⬅️▶️🖨️🌀⏸️▶️📖1️⃣⏸️🧵Input n: 🧵◀️◀️⏹️
```
> Input: `6`  
Output: `8`

#### And, finally, the factorial code:

```
💠⬅️ factorial function (recursive)
    🆕 function definition
        💠⏸️
        🛑⏸️ stopper
        🅰️⏸️ aggregator
        ♻️ curr
    ➡️ function body
        ❓ if
            ▶️⚖️⏸️▶️➕⏸️🛑⏸️1️⃣◀️⏸️♻️◀️  curr=stopper+1:
        ⏸️ then
            🅰️ return aggregator
        ⏸️ else:
            ▶️💠⏸️💠 recursively call with:
             
            ⏸️
                🛑 stopper-
            ⏸️ 
                ▶️❌⏸️🅰️⏸️♻️◀️ aggregator*curr
            ⏸️ 
                ▶️➕⏸️♻️⏸️1️⃣◀️  curr+1
            ◀️
        🔚 if end
    🔚 function end
⏹️
let's wrap it up nicely into an easy to use function
❗⬅️🆕🅰️➡️▶️💠⏸️💠⏸️🅰️⏸️1️⃣⏸️1️⃣◀️🔚⏹️
and define a 'print factorial' function while we're at it
🖨️❗⬅️🆕🅰️➡️▶️🖨️⏸️▶️❗⏸️🅰️◀️◀️🔚⏹️

let's now print a factorial of the user input!

🕳️⬅️▶️🖨️❗⏸️▶️📖1️⃣⏸️🧵Input n: 🧵◀️◀️⏹️
```
> Input: `5`  
Output: `120`