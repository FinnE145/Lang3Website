namespace Lang3.Builtins;

 class Operators {
    static Types.Num Add(Types.Num a, Types.Num b) {
        return new Types.Num(a.value + b.value);
    }

    static Types.Num Sub(Types.Num a, Types.Num b) {
        return new Types.Num(a.value - b.value);
    }

    static Types.Num Mul(Types.Num a, Types.Num b) {
        return new Types.Num(a.value * b.value);
    }

    static Types.Num Div(Types.Num a, Types.Num b) {
        return new Types.Num(a.value / b.value);
    }

    static Types.Num Exp(Types.Num a, Types.Num b) {
        return new Types.Num(Math.Pow(a.value, b.value));
    }

    static Types.Num Root(Types.Num a, Types.Num b) {
        return new Types.Num(Math.Pow(a.value, 1 / b.value));
    }

    static Types.Num IDiv(Types.Num a, Types.Num b) {
        return new Types.Num(Math.Floor(a.value / b.value));
    }

    static Types.Num Mod(Types.Num a, Types.Num b) {
        return new Types.Num(a.value % b.value);
    }

    static Types.Bool Gt(Types.Num a, Types.Num b) {
        return new Types.Bool(a.value > b.value);
    }

    static Types.Bool Lt(Types.Num a, Types.Num b) {
        return new Types.Bool(a.value < b.value);
    }

    static Types.Bool Gte(Types.Num a, Types.Num b) {
        return new Types.Bool(a.value >= b.value);
    }

    static Types.Bool Lte(Types.Num a, Types.Num b) {
        return new Types.Bool(a.value <= b.value);
    }

    static Types.Bool Eq(Types.Num a, Types.Num b) {
        return new Types.Bool(a.value == b.value);
    }

    static Types.Bool Neq(Types.Num a, Types.Num b) {
        return new Types.Bool(a.value != b.value);
    }

    static Types.Bool And(Types.Bool a, Types.Bool b) {
        return new Types.Bool(a.value && b.value);
    }

    static Types.Bool Or(Types.Bool a, Types.Bool b) {
        return new Types.Bool(a.value || b.value);
    }

    static Types.Var<T> Assign<T>(Types.Var<T> a, Types.BaseValue b) {
        a.value = (T)b.value;
        return a;
    }

    static Types.Var<Types.Num> AddAssign(Types.Var<Types.Num> a, Types.Num b) {
        a.value = Add(a.value, b);
        return a;
    }

    static Types.Var<Types.Num> SubAssign(Types.Var<Types.Num> a, Types.Num b) {
        a.value = Sub(a.value, b);
        return a;
    }

    static Types.Var<Types.Num> MulAssign(Types.Var<Types.Num> a, Types.Num b) {
        a.value = Mul(a.value, b);
        return a;
    }

    static Types.Var<Types.Num> DivAssign(Types.Var<Types.Num> a, Types.Num b) {
        a.value = Div(a.value, b);
        return a;
    }

    static Types.Var<Types.Num> ExpAssign(Types.Var<Types.Num> a, Types.Num b) {
        a.value = Exp(a.value, b);
        return a;
    }

    static Types.Var<Types.Num> RootAssign(Types.Var<Types.Num> a, Types.Num b) {
        a.value = Root(a.value, b);
        return a;
    }

    static Types.Var<Types.Num> IDivAssign(Types.Var<Types.Num> a, Types.Num b) {
        a.value = IDiv(a.value, b);
        return a;
    }

    static Types.Var<Types.Num> ModAssign(Types.Var<Types.Num> a, Types.Num b) {
        a.value = Mod(a.value, b);
        return a;
    }

    static Types.Var<Types.Bool> AndAssign(Types.Var<Types.Bool> a, Types.Bool b) {
        a.value = And(a.value, b);
        return a;
    }

    static Types.Var<Types.Bool> OrAssign(Types.Var<Types.Bool> a, Types.Bool b) {
        a.value = Or(a.value, b);
        return a;
    }

    static Types.Num Inc(Types.Var<Types.Num> a) {
        AddAssign(a, new Types.Num(1));
        return a.value;
    }

    static Types.Num Dec(Types.Var<Types.Num> a) {
        SubAssign(a, new Types.Num(1));
        return a.value;
    }

    static Types.Bool Not(Types.Bool a) {
        return new Types.Bool(!a.value);
    }
}