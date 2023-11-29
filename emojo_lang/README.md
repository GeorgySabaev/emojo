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

### Expressions
Function calls in EMOJO are represented with expressions. An expression consists of an operand (either a built-in one, or a user-defined identifier) and optionally any number of arguments. The operand and each argument must be separated by pause symbol emoji (`⏸️`), and the whole expression must be wrapped with ▶️ and ◀️, aka the emoji versions (with `U+FE0F Variation Selector-16`) of `U+25B6 BLACK RIGHT-POINTING TRIANGLE` and `U+25C0 BLACK LEFT-POINTING TRIANGLE` respectively.

For example:

```▶️➕⏸️1️⃣⏸️▶️🅱️⏸️3️⃣ⓕ3️⃣◀️⏸️😋👌1️⃣◀️```

evaluates to:

```1 + 🅱️(3.3) + 😋👌1️⃣```

where:
- `🅱️(3.3)` is function `🅱️` being called with 3.3 as an argument
- `😋👌1️⃣` is a user-defined variable


# TO DO LIST:

- comments
- evaluation
- let syntax
- lambda function literal
- built in io functions (📖 and 🖨️)
- list literal
- built in list functions