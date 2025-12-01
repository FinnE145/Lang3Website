# Objects and Types

## Modifying Types

To modify an variable's type, change it's properties. This will not affect new objects of the original type.



To modify a type directly, modify the properties on the type, not the variable.

```lang3
obj v =                 # Declare a variable

type(v)                 #> obj; The variable is of type :obj;



v.prop = "abc"          # Add a property to the variable

type(v)                 #> $v; The variable is now a new type



obj v2 =                # Declare a new variable of the same original type

v2.prop                 #! UndefinedError: obj.prop has not been defined; The *base type* has not been modified to include the new property





type(v).prop            #> abc; The type *of the variable* has been modified to include the new property



type(v).prop2 = "def"   # Add another property, this time to the *type*, not the variable

type(newType)           #> $newType; The type was modified, but a new type was not created



type(v) v3 =            # Create a new variable of type :$v; (the modified type of the original variable)

v3.prop2                #> def; This new variable now has the new property, as the type was modified
```

## Creating Types/Classes

As seen above, to create a class or type (exclusively called a type in Lang3), you must create a variable and modify it's properties. You can then use it's type (as given by the `type()` function) as a type like any other.

```lang3
obj newVar =            # Declare a variable of the base :obj; type with no value

type(newVar)            #: obj ; The type of the variable is :obj;



newVar.prop = 1         # Add a property to the variable

type(newVar)            #> $newVar; The type is no longer :obj;



newType = type(newVar)  # Assign the type of the variable to a new variable



newType o =             # You can now use the new type!

io.out(o.prop)          #> 1
```

To avoid the need to assign the type of the modified variable to another variable, Lang3 has syntactic sugar that allows you to modify a type directly. Put a dollar sign `$` in front of a variable when declaring it in order to directly declare and modify it as a type:

```lang3
obj newType =           # Declare a variable, with the base type :obj; and no value

$newType.prop = 1       # Add a property directly to the variable's type



newType o =             # You can use the variable directly as a type for creating new objects/variables

o.prop                  #> 1
```

The above code is equivalent to:

```lang3
obj newType =

type(newType).prop = 1

newType = type(newType)



newType o =

o.prop
```

In many languages, if you create an object from a given class, its value will be the object itself:

```py

class A:            # Create a class/type

    prop = "abc"    # Add a property to that class/type



a = A()             # Create an object of that type

print(a)            # <__main__.A object at 0x000001A2BC487550> - Accessing the object returns the object reference

print(a.prop)       # "abc" - You can use the object to access its properties

```



However, in Lang3, every object has a root value in addition to the properties of an object:

```lang3
A =                 # Declare a variable

A.prop = "abc"      # Add a property to that variable



A a = 1             # Create an object of that type, with a root value of :1;

io.out(a)           #> 1; Accessing the object returns the value of the object

io.out(a.prop)      #> abc; You can use the object to access its properties
```

### Type Modification Behavior

Any object (called the template object when used as a type) can be used as a type, and the new object (the target object) created of that type will inherit the children of the template object:

```lang3
int target =            # Declare a new variable with an explicit type and no value

type(target)            #: int

target.prop =           # Adding a property (in this case with no value) makes it different than the original type

newType = type(target)  # Assign the new type to a variable



target t = 1            # Create an object of the new type
```

If an variable of an existing type with it's children unchanged is used as a type, the target variable uses the original existing type:

```lang3
int newType =

type($newType)      #: int      ; int is an existing type

$newType x =

type($x)            #: int      ; Since :newType; didn't modify the children of int in any way, it did not create a new type
```

If a type is changed twice identically, it still creates new types for both:

```lang3
# Modify the :int; type once

int type1 = 

$type1.prop = "abc"

type($type1)                    #: type1    ; a new type is created



# Modify the :int; type again identically

int type2 = 

$type2.prop = "abc"

type($type2)                    #: type2    ; a second new type is created



type($type1) == type($type2)    #: true     ; the types are identical

type($type1) is type($type2)    #: false    ; the types are not the same object
```

Note that values cannot have their types modified:

```lang3
loop newType = 1



type(newType)           #: int

newType.prop = "abc"    #! TypeError - The type of a variable's value cannot be modified (adding a property modifies the type)
```

### Traditional Class Behavior

##### Instantiation (as seen above):

```lang3
a1 = A(1)

a2 = A(2)
```

##### Inheritance:

```lang3
A B =           # Declare a new variable of type :A;

B.addThree =    # Add a new child to the variable, creating a new type that inherits all the properties and methods of the previous type
```

##### Abstraction (Protected children):

```lang3
A.prop1 =                       # :A.prop1; can be accessed and modified from anywhere

private A.prop2 =               # :A.prop2; can only be accessed or modified within :A; or its children

readonly A.prop3 =              # :A.prop3; cannot be modified after declaration, can be read or called from anywhere

private readonly A.prop4 =      # :A.prop4; cannot be modified after declaration, can only be read or called within :A; or its children



A =                             # :A; can be accessed outside from anywhere, and properties and methods can be added or deleted

hidden A =                      # :A; can only be accessed from itself or its children, and appears undefined otherwise

locked A =                      # :A; can be accessed from anywhere, but properties and methods cannot be added or deleted

hidden locked A =               # :A; can only be accessed from itself or its own children, and properties and methods cannot be added or deleted
```

> Note that `private` and `hidden` are similar in that they have the same effect on a variable's value or a variables's object reference respectively, and same goes for `readonly` and `locked`

##### Polymorphism:

```lang3

```
