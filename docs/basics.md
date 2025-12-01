# Basics

The `Basics` section describes basic usage and syntax of many commonly used aspects of Lang3, and is more like a tutorial. For more advanced information, like properties and descriptions of some symbols and features, look for the appropriate section later in the docs. Here is a helpful, but not necessarily complete, guide to which Docs section expands upon each Basics section:



| Basics Section (`/Docs/Basics/*`) | Docs Section (`/Docs/*`)  |
|----------------------------------|---------------------------|
| `Comments`                       | No Equivalent             |
| `Output`                         | `Modules > IO > Out`      |
| `Variables`                      | `Objects`                 |
| `Code Blocks and Functions`      | `Data Types > Code Block` |

## Comments

Comments are sections of code that do not run and have no functional effects.

They are used for documentation, notes, and sometimes temporarily removing code from being run.



In Lang3, line comments, or comments that continue to the end of the line, start with a single hashtag `#`.

Block comments, or comments that can be multiple lines long and must be explicitly ended, start and end with multiple hashtags.



Examples:

```lang3
io.out("This is some valid code that will run")

# This is a comment, it does nothing

io.out("Note that the comment above ends at the end of the line")

## This is a block comment

It can be multiple lines long

It must be ended with two or more hashes ##

io.out(1 + 2 ## Note that block comments can be put in the middle of functional code ##+ 3)
```

Lang3 comments have various stylistic options that do nothing, but can make code clearer and are recommended for use. They include:

- `>` after a comment hash denotes that the comment is expected output, up until a semicolon or another hash

- Colons `:;` in a comment denote code, in the same way that backticks ` `` ` denote code in markdown

- An exclamation mark `!` in a comment denotes an error, and a colon somewhere in the text following an exclamation mark will be ignored, up until a semicolon, newline, or hash



Like many symbols in Lang3, special symbols in comments can be escaped with a backslash `\` to ignore their significant meaning.



Examples:

```lang3
1 + 2       #> 3; Until the semicolon, this comment showed the output of this line

int x = 1   # This line demonstrates code in comments by assigning the value :1; to the variable :x;, with type :int;

"abc" + 1   #! TypeError: cannot add objects of type 'str' and 'int'; Before the semicolon, this comment showed an error and error message

#\! This is not an error, just a random exclamation mark in a comment, due to the backslash before the exclamation mark
```

## Output (Printing to Console)

Values can be shown in the console with the `io.out` function. This means that `out` is a method of the `io` module (learn more about modules at the top of `Modules` and learn about the `io` module under `Modules > IO`).

```lang3
io.out("Hello world")   # Print :"Hello world"; to the console

io.out(1)               # Print :1; to the console
```

By default, outputting multiple objects will show them separated by spaces. You can change this by explicitly passing a string to the `sep` parameter.

```lang3
io.out(1, 2, 3)             #> 1, 2, 3

io.out(1, 2, 3, sep="|")    #> 1|2|3
```

By default, outputting ends with a new line. You can change this by explicitly passing a string to the `end` parameter

```lang3
io.out(1, 2, 3)

io.out(4, 5, 6)

##>

1 2 3

4 5 6

##



io.out(1, 2, 3, end="|")

io.out(4, 5, 6)

##>

1 2 3|4 5 6

##
```

For more information on outputting and the `io.out` function, look at `Modules > IO > Methods > out`

## Variables

A variable is an identifier given to a particular value. In Lang3, variables can also have properties and act as larger objects (For more information, see `Objects`).



Declaring a variable consists of specifying a data type, name, and an optional value.

Lang3 may be able to infer the type if the variable is assigned in the same line as it is declared.



Examples:

```lang3
int w = 1       # A variable is declared and assigned with a type, name, and value



int x =         # This variable has been declared, but by not specifying a value it is not assigned

int x = ?       # Declaration without assignment can also be done with the "blank" symbol. You can find out more about it in the `Symbols` section

io.out(x)       #! ValueError: x does not have a value; A variable declared but not assigned cannot have its value accessed



y = 2           # This variable does not have a type explicitly specified because it can be inferred from the value

type(y)         #: int



z = y           # This variable also does not have a type explicitly specified, but can infer from y's type
```

Using a variable is as easy as writing its name in an expression.



Examples:

```lang3
int a = 1

int b = 2



io.out(a + 1)   #: 2

io.out(a + b)   #: 3



int c = i + 1   #! UndefinedError: :i; has not been declared
```

## Code Blocks and Functions

### Code Blocks

Code blocks are objects that hold other code. They are enclosed by colons `:;`.



Examples:

```lang3
code c1 = :io.out("This is in a code block");



code c2 = :

    io.out("Code blocks can be multiline...")

    io.out("...and can contain multiple statements")

;



code c3 = :

    "Code blocks can also contain" + "just an expression"

;



code c4 = :

    io.out("Note that this code will not be run, it is simply being assigned to a variable")

;
```

### Functions

Functions are a concept in many languages, where code can be assigned a name and run multiple times, with different values assigned to variables in the code called parameters. In Lang3, functions consist of a code block that can be "called", optionally with arguments passed to parameters.



To call a code block/function, add parentheses `()` after the code block or a `code` type variable, with optional argument values inside.



Examples:

```lang3
: io.out("This code block is being run directly") ;()     #> This code block is being run directly

#> This code block is being run directly



code f = :

    io.out("This is a function")

    io.out("It will not be run immediately, but can be called later")

;



f()         # This calls the function

##>

This is a function

It will not be run immediately, but can be called later

##
```

#### Parameters and Arguments

To assign parameter variables to a function, declare variables (without the equal sign) inside square brackets `[]` in front of the code block or `code` type variable. Those variables can then be used inside the code block.

The desired value of parameter variables, called arguments, must be passed inside the parentheses when a function is called.



Examples:

```lang3
code printAValue = [x]:     # This parameter has no type, and will infer from what is passed

    # This code block outputs the value of x

    io.out(x)

;



printAValue(1)      #> 1



code add = [num x, num y]:  # These parameters have types, and will only accept numbers

    # This code block outputs the sum of x and y

    io.out(x + y)

;



add(1, 2)           #> 3

add("apple", "banana")      #! TypeError: :x; must be an instance of num, not str



[a, b]: a - b ;(3, 2)       #> 1; Code blocks can also be called directly with parameters and arguments



code func = :

    io.out("The value of a is: {a}")

    io.out("The value of b is: {b}")

;



[str a, str b]func("apple", "banana")   # Code blocks can have their parameters added after initial declaration



code multiply = :

    io.out("The product of a and b is: {a * b}")

;



multiply = [num a, num b]multiply   # Code blocks can be reassigned to themselves with parameters



multiply(2, 5)                      #> The product of a and b is: 10
```

If parameters are not passed before the code block is called, an `UndefinedError` will occur. If the wrong number or type of arguments are passed, an `ArgumentError` will occur.



### Creating Traditional Classes

While classes as a distinct concept do not exist in lang3, they can be created with full functionality.

##### Example in Java:

```java

class A {                   // Declare the class

    int x;                  // Declare an instance variable



    public A(int n) {       // Declare the constructor

        this.x = n;            // Initialize the instance variable with the parameter value

    }



    public int addTwo() {   // Declare a method

        return this.x + 2;     // Access the instance variable and return a value

    }

}



A a = new A(1);             // Declare and initialize an instance of the class

a.addTwo();                 // Call a method on that instance

```

##### Equivalent in Lang3:

```lang3
A = [n]:                    # Create the variable A and assign a function code block to it as its constructor

    inst $                    # Use the :inst; keyword to duplicate the current object...

                                # (previously accessible from :$;) and assign the new object's object reference to :$;

    $.x = n                   # Declare and initialize a property with the parameter value

    return $                  # Return the object

;

# POINT 1

$A.addTwo = [n]:$.x + 2;    # Add a method to A as an object, not the code block (A's value)

# POINT 2



a = A(1)

a.addTwo()
```

> Note that in the value-object system, all classes with a constructor have their value set to a code block, meaning that they can be run as functions which return the object themselves



> Also note how the object's type changes:

>   At `POINT 1`, these are the types of A as a value and as an object

```lang3
type(A)                     #: code ; A code block is assigned to :A;

type($A)                    #: code ; The object infers its type from its value when not otherwise stated
```

>   At `POINT 2`, these are those same types:

```lang3
type(A)                     #: code ; The code block value is unchanged

type($A)                    #: _A   ; The object A is now different from the code block type, and became its own type
```

```lang3
code add = :

    io.out(x + y)

;



add(1, 2)       #! UndefinedError: :x; has not been declared

add = [num x, num y]add

add(1, 2)       #> 3



add(1)          #! ArgumentError: :add; requires 2 arguments, but 1 was given

add(1, 2, 3)    #! ArgumentError: :add; requires 2 arguments, but 3 were given
```

An unlimited number of arguments can be passed, and accessed as a sequence inside the code block. To accept them, make a sequence parameter, with an arrow `->` in front of it. This shows that the arguments passed should be "condensed" into the sequence variable. (For more on the arrow symbol and expansion/condensation, see `Operators > Other Operators > Expansion/Condensation`)



Examples:

```lang3
code add = [-> num[] args]:

    num sum = 0



    for args -> num, :  # Learn more about loops in `Control Structures > Loops`

        sum += num

    ;



    io.out(sum)

;



add(1, 2, 3)    #> 6

add(1)          #> 1

add()           #> 0; The :args; parameter is an empty sequence
```

#### Overloading

Overloading is when the same function can have different parameters and code that runs. Different overloads for a function are generally supposed to accomplish the same thing and have the same return types, but can accept different parameters.



In Lang3, functions are overloaded by creating a `sequence` of code blocks. When that sequence is called, the first code block with matching parameters is run. If no parameters match, an `ArgumentError` will be raised.



> Note that if it were **not** a sequence of functions, a `TypeError` would be called if one or more of the arguments are of the wrong type, but the right number were passed. When a function is overloaded (a sequence of functions), arguments of the wrong type that do not match any parameters will raise an `ArgumentError`.

```lang3
code[] add = [num a, num b]:

    io.out("The sum of {a} and {b} is {a + b}")

;,

[str a, str b]:

    io.out("When you concatenate {a} and {b}, you get {a + b}")

;,

[seq a, seq b]:

    io.out("When you combine {a} and {b}, you get {a + b}")

;



add(1, 2)                   #> The sum of 1 and 2 is 3

add("apple", "banana")      #> When you concatenate apple and banana, you get applebanana

add([1, 2], [3, 4])         #> When you combine [1, 2] and [3, 4], you get [1, 2, 3, 4]



add(4, 5, 6)                #! ArgumentError: no overloads of :add; match the arguments [num, num, num]

add(1, "apple")             #! ArgumentError: no overloads of :add; match the arguments [num, str]
```

Overloads should differ in type, number, or order of differently-typed arguments. If two overloads have the same parameters, the first will be called when appropriate arguments are passed.



A particular function within a code block sequence can be called via indexing, like any sequence.

```lang3
code[] add = [num a, num b]:

    io.out("The sum of {a} and {b} is {a + b}")

;,

[num a, num b, num c]:

    io.out("The sum of {a}, {b}, and {c} is {a + b + c}")

;,

[num a, num b]:

    io.out("This is another function that adds {a} and {b}")

;



add(1, 2)       #> The sum of 1 and 2 is 3

add[2](1, 2)    #> This is another function that adds 1 and 2
```
