# Data Types

Every data type has a singular and sequence form. The singular form contains one object of that data type, while sequences contain multiple objects of that data type. Sequences of a particular type may have methods and properties that differ from the singular data type, and may have different behaviors when used in an operator.

### Sequences



> Lang3 `sequence`s are similar to what some other languages call `Array`s



`sequence`s are special data types that can be created from any other data type. Sequence data types are specified with a singular type followed by square brackets `[]`. Inside the brackets is an optional number literal specifying the maximum number of objects in that sequence.

```lang3
num[]   # An unlimited-length sequence of :num; objects (for more information on :num;, see below)

num[3]  # A sequence of up to 3 :num; objects
```

A sequence literal consists of multiple values separated by commas. If enclosed in square brackets, it becomes a `list`, a subtype of `seq`.



This means that arguments passed to a function are actually a sequence in parentheses! More on this can be found in `Data Types > Functions`

```lang3
num[] a = 1, 2, 3
```

## Builtin Data Types

### Number (`num`)

The `num` datatype represents both integer/whole numbers and decimals. It can be inferred from literals like `1`, `12.34` and `0.1`.



#### Methods

**`round` - Rounds to the nearest multiple of a number**



> <u>Params</u>:

> - `num n`: Round to the nearest multiple of this number

> - `?str mode` [*OPTIONAL*]: The rounding mode to use. Possible values are:

>   - `"nearest"|"n"` [*DEFAULT*]: When the number is halfway between two multiples, round up if it's even and down if it's odd

>   - `"nearest-up"|"nu"`: If the number is exactly halfway between two multiples, round up

>   - `"nearest-down"|"nd"`: If the number is exactly halfway between two multiples, round down

>   - `"up"|"u"`: Always round up

>   - `"down"|"d"`: Always round down

>

> <u>Returns</u>:

> - `num`: The rounded number



Example usage:

```lang3
(34).round(3)           #: 33 ; The closest multiple of 3 to 34 is 33

(35).round(3)           #: 36 ; The closest multiple of 3 to 35 is 36

(35).round(3, "d")      #: 33 ; Round down to 33 because of the rounding mode argument



(1.2345).round(0.01)    #: 1.23

(1.2345).round(0.05)    #: 1.2
```

**`roundToPlaces` - Rounds to a certain number of decimal places**



> <u>Params</u>:

> - `num p`: Round to this many decimal places

> - `?str mode` [*OPTIONAL*]: The rounding mode to use. Same possible values as `round`.

>

> <u>Returns</u>:

> - `num`: The rounded number



Example Usage:

```lang3
(1.234).roundToPlaces(2)                    #: 1.23 ;

(1.5).roundToPlaces(0)                      #: 1.0 ; Round down because .5 is halfway, and 1.5 is odd

(1.55).roundToPlaces(1, "nu")               #: 1.6 ; Round up even though 1.55 is odd, due to the rounding mode argument
```
