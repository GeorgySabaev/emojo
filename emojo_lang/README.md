# EMOJO

why am i doing this

## Syntax

### Basics

Emojo is an Emoji-only language.

This means that only emoji can be used when writing in EMOJO. Anything that is not an emoji is considered a comment and will be ignored by the parser.

The only exception to this rule are string literals, as described below in the Data Types section. This feature goes against the principle of EMOJO, but unfortunately has to be supported for the sake of convenience.

### Data types
EMOJO is a dynamically-typed language, and supports the following data types:

- INTEGER: A signed 32 bit integer number.  
The literal for an integer is a sequence of keycap digit emoji (`0️⃣`, `1️⃣`, `2️⃣`, `3️⃣`, `4️⃣`, `5️⃣`, `6️⃣`, `7️⃣`, `8️⃣` and `9️⃣`). 
(ex. `4️⃣2️⃣` evaluates to `42`)
- FLOAT: A signed 32 bit floating point number.  
The literal for an integer is a sequence of keycap digit emoji for the whole part, followed by an `ⓕ` symbol **\[NOTE: NOT A PROPER EMOJI, SUBJECT TO CHANGE\]** and a second sequence of keycap digits for the fractional part.  
(ex. `4️⃣2️⃣0️⃣ⓕ6️⃣9️⃣` evaluates to `420.69`)
- STRING: A string of characters.  
The literal for a string of characters is the string itself, wrapped around with two string emoji (`🧵`).  
(ex. `🧵Making this language was a mistake.🧵` evaluates to `"Making this language was a mistake."`)  
Note: this is the only case where non-emoji text will not be ignored by the interpreter.
- NONE: A data type with only one value (None).  
The literal for the none type value is the Unicode character 💩, availible in the Unicode Standard since version 6.0 under the name `U+1F4A9 PILE OF POO`. 
- FUNCTION: **\[TBD\]**

### Expressions
Function calls in EMOJO are represented with expressions. An expression consists of an operand (either a built-in one, or a user-defined identifier) and optionally any number of arguments. The operand and each argument must be separated by pause symbol emoji (`⏸️`), and the whole expression must be wrapped with ▶️ and ◀️, aka the emoji versions (with `U+FE0F VARIATION SELECTOR-16`) of `U+25B6 BLACK RIGHT-POINTING TRIANGLE` and `U+25C0 BLACK LEFT-POINTING TRIANGLE` respectively.

For example:

```▶️➕⏸️1️⃣⏸️▶️🅱️⏸️3️⃣ⓕ3️⃣◀️⏸️😋👌1️⃣◀️```

evaluates to:

```1 + 🅱️(3.3) + 😋👌1️⃣```

where:
- `🅱️(3.3)` is function `🅱️` being called with 3.3 as an argument
- `😋👌1️⃣` is a user-defined variable

There is no lazy evaluation, every argument WILL be evaluated recursively before the function is called, even if the arguments are not valid.

### Identifiers
All functions and variables, built-in or defined by the user, are assigned an identifier. An identifier is any sequence of emoji, none of which are reserved by the language. An identifier cannot start with a keycap number emoji, but any subsequent emoji may be a keycap number.

Examples of valid identifiers:
- `🧽`
- `🔫😂👌`
- `🅰️🅱️🅾️🅱️🅰️`
- `🌿4️⃣2️⃣0️⃣`

Examples of invalid identifiers:
- `6️⃣9️⃣❤️6️⃣9️⃣` (Starts with a number)
- `🎵▶️🎶` (Contains a reserved character ("`▶️`"))

### Statements
Statements are the fundamental units of code in EMOJO. They consist of a single expression, which is evaluated recursively and either assigned an unoccupiied identifier or discarded.

A statement always takes the form:

`[TARGET]⬅️[ANY VALID EXPRESSION]⏹️`

Where `[TARGET]` is either an identifier, in which the contents of the line are associated with it for the rest of the interpreter's runtime (unless the identifier is occupied), or `🕳️`, in which case the result of evaluation is discarded.

Here are some example statements:
- `🕳️⬅️▶️🖨️⏸️🧵Hello world!🧵◀️⏹️`  
An obligatory "Hello world!"
- `Void result of expression -->🕳️⬅️Start expression -->▶️🖨️<-- Print argument⏸️🧵Hello world!🧵<--String literal◀️⏹️<--End of program`  
Any non-emoji text is ignored (unless it's in a string literal). Comment anywhere you like!
- `🔫Even😂inside👌identifiers❤️!🅱️⬅️▶️➖⏸️2️⃣2️⃣2️⃣2️⃣◀️⏹️`
- `🔫😂👌❤️🅱️⬅️▶️➖⏸️2️⃣Or2️⃣number2️⃣literals2️⃣!2️⃣◀️⏹️`


### Built-in functions
At the time of the writing the following built-in functions are availible in the language:
#### Arithmetic
- `➕`: Performs number addition or string concatenation.  
**Accepts:**  
  -  Any number of numeric arguments (at least 2)
  Returns their sum (INT if all arguments are INT, otherwise FLOAT)  
  **or**
  -  Any number of string arguments (at least 2)  
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

#### Input/Output
- `🖨️`: Outputs a value to the console.  
**Accepts:**  
  -  Any 1 numeric/string/NONE argument  
  Prints the argument to stdout (`"None"` for a NONE value).  
  Returns NONE.


# TO DO LIST:
- lambda function literal <- IMPORTANT
- built in input function (📖) <- IMPORTANT
- list literal
- built in list functions