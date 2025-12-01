# Operators

## Computational Operators

## Comparison Operators

All like comparison operators are chainable (ex. `1 < 2 < 3 < 4` is valid).

This is accomplished through a property on the objects outputted from comparison operators. The `bool`s outputted are technically of type `compBool`, a subtype of `bool`, and differ from the standard `bool` type in that they have a properties `.lhs` and `.rhs` that return the left and right values passed to the operator. If the next comparison operator receives a `true` `compBool` on the left, it will compare the `.rhs` property of it's left argument with its right argument. If it receives a `false` value, it returns `false`.



Example:

```lang3
1 < 2 < 3 < 4 < 5       #: true ; This is equivalent to :(1 < 2) && (2 < 3) && (3 < 4) && (4 < 5);, and is implemented as :(((1 < 2).rhs < 3).rhs < 4).rhs < 5;

c = 1 < 2               #: true ;

type(c)                 #: compBool ; The result of a comparison is a :compBool; object

c.rhs                   #: 2 ;
```

## Other Operators

### Expansion/Condensation
