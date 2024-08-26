namespace Lang3.Builtins;

static class Types {
    public class BaseValue(object value) {
        public object value = value;
    }

    public class Num {
        public static readonly string type = "num";
        public double value;

        public Num(double value) {
            this.value = value;
        }

        public Num(int value) {
            this.value = value;
        }
    }

    public class Bool(bool value)
    {
        public static readonly string type = "bool";
        public bool value = value;
    }

    public class Var<T>(string name, T value)
    {
        public static readonly string type = "var";
        public string name = name;
        public T value = value;
    }
}