using Lang3.Utils.Errors;

namespace Lang3;

class Parser(List<Lexer.Token> tokens, string code, string fp) {
    public class Node(string type, string value, Lexer.Token token) {
        public string type = type;
        public string value = value;
        public List<Node> children = [];
        public Lexer.Token token = token;

        private string ChildrenToString(int depth, bool showToken, bool shortChildren, int childCount = 0) {
            string s = $"[{(shortChildren ? "" : "\n")}";
            foreach (Node child in children) {
                s += $"{child.ToString(depth + 4, showToken, shortChildren, childCount)}{(shortChildren ? ", " : "\n")}";
            }
            return $"{(shortChildren ? s[..^2] : s)}{new string(' ', shortChildren ? 0 : depth)}]";
        }

        public override string ToString() {
            return ToString(0, false);
        }

        public string ToString(bool shortChildren, int? maxChildren = null) {
            return ToString(0, false, shortChildren, maxChildren);
        }

        public string ToString(int maxChildren) {
            return ToString(0, false, false, maxChildren);
        }

        public string ToString(int depth, bool showToken, bool shortChildren = false, int? maxChildren = null, int childCount = 0) {
            if (maxChildren is not null && childCount > maxChildren) {
                return $"{new string(' ', shortChildren ? 0 : depth)}...";
            }
            return $"{new string(' ', shortChildren ? 0 : depth)}Node({(shortChildren ? "" : type)}{(value.Length > 0 ? $"{(shortChildren ? "" : ", ")}{value}" : "")}{(children.Count > 0 ? $", {ChildrenToString(depth, showToken, shortChildren, ++childCount)}" : "")}{(showToken ? $", {token}" : "")})";
        }
    }

    private static readonly List<string> valueTypes = ["value", "parens", "operator"];

    // higher precedence is exavluated first
    private static readonly Dictionary<string, int> precedence = new() {
        {"add", 1},
        {"sub", 1},
        {"mul", 2},
        {"div", 2},
        {"exp", 3},
        {"root", 3},
        {"iDiv", 2},
        {"mod", 2},
        {"gt", 0},
        {"lt", 0},
        {"gte", 0},
        {"lte", 0},
        {"eq", 0},
        {"neq", 0},
        {"and", -2},
        {"or", -2},
        {"bAnd", 1},
        {"bOr", 1},
        {"xor", 1},
        {"lShift", 2},
        {"rShift", 2}
    };

    private readonly List<Lexer.Token> tokens = tokens;
    private readonly string fp = fp;
    private readonly string code = code;

    private void HangParens(Node root, ref int i) {
        Node node = new("parens", "", tokens[i]);
        root.children.Add(node);

        i++;
        if (Parse(node, ref i, "rParen")) {
            i++;
            return;
        } else {
            new SyntaxError(fp, "Unmatched parenthesis", tokens[i].start, tokens[i].end).Raise();
        }
    }

    private static bool SwitchArgs(Node op, Node lastOp, Node? root = null) {

        //Console.WriteLine("Switching " + lastOp.ToString(true) + " and " + op.ToString(true));
        bool addNode = true;

        if (lastOp.children.Count == 0 || lastOp.type != "operator") {

            //Console.WriteLine("NOT AN OP - " + lastOp.ToString(true) + (lastOp.type != "operator" ? " is a " + lastOp.type : "has no children"));

            op.children.Add(lastOp);
            root?.children.RemoveAt(root.children.Count - 1);
        } else if (precedence[op.value] > precedence[lastOp.value]) {

            //Console.WriteLine("WRONG ORDER - " + op.value + " is higher precedence than " + lastOp.value);

            if (lastOp.children[^1].children.Count != 0 && lastOp.children[^1].type == "operator" && !ReferenceEquals(lastOp.children[^1], op)) {
                SwitchArgs(op, lastOp.children[^1], lastOp);
            } else {
                op.children.Add(lastOp.children[^1]);
                lastOp.children.RemoveAt(lastOp.children.Count - 1);
                lastOp.children.Add(op);
            }

            addNode = false;
        } else {
            //Console.WriteLine("RIGHT ORDER - " + op.value + " comes after " + lastOp.value);

            op.children.Add(lastOp);
            root?.children.RemoveAt(root.children.Count - 1);
        }

        return addNode;
    }

    private void HangOps(Node root, ref int i) {
        Node node = new("operator", tokens[i].value, tokens[i]);
        if (root.children.Count == 0 || !valueTypes.Contains(root.children[0].type)) {
            new ArgumentError(fp, code, "Expected a value before the operator", tokens[i].start, tokens[i].end).Raise();
        } else {
            bool addNode = SwitchArgs(node, root.children[^1], root);
            i++;

            Node argHolder = new("argHolder", "", new("", "", 0, 0, 0));
            bool parseResult = Parse(argHolder, ref i, "EOF", 1);

            if (parseResult && argHolder.children.Count != 0 && valueTypes.Contains(argHolder.children[^1].type)) {
                node.children.Add(argHolder.children[^1]);
                if (addNode) {
                    root.children.Add(node);
                }
            } else {
                new SyntaxError(fp, code, "Expected a value after the operator", tokens[i].start, tokens[i].end).Raise();
            }
        }
    }

    public Node Parse() {
        Node root = new("root", "", new("", "", 0, 0, 0));
        int i = 0;
        Parse(root, ref i);
        return root;
    }

    private bool Parse(Node root, ref int i, string stopType = "EOF", int? max = null) {
        List<int> lastIs = [];
        int lastLength = root.children.Count;

        //Console.WriteLine("Stop type: " + stopType);
        while (i < tokens.Count) {
            max -= root.children.Count - lastLength;
            lastLength = root.children.Count;

            //Console.WriteLine("\nCurrent i: " + i);
            //Console.WriteLine("Max: " + max);
            //Console.Write("Current token: " + tokens[i].type);

            if (tokens[i].type == stopType || (max ?? 1) <= 0) {
                //Console.WriteLine("\nStopping");
                return true;
            }

            //check for var, check for brackets after var, parse inside brackets, check that whats inside is a seq of values

            if (tokens[i].type == "lParen") {
                //Console.WriteLine();
                HangParens(root, ref i);
            } else if (tokens[i].type == "rParen") {
                new SyntaxError(fp, code, "Unmatched parenthesis", tokens[i].start, tokens[i].end).Raise();
            } else if (tokens[i].type == "operator") {
                //Console.WriteLine(" " + tokens[i].value);
                HangOps(root, ref i);
            } else {
                //Console.Write(" " + tokens[i].value);
                Node node = new("value", tokens[i].value, tokens[i]);
                root.children.Add(node);
                i++;
            }
            //Console.WriteLine();

            if (lastIs.Count == 3 && lastIs[0] == lastIs[1] && lastIs[1] == lastIs[2] && lastIs[2] == i) {
                throw new Exception("Last 3 i's have been the same");
            } else if (lastIs.Count == 3) {
                lastIs.RemoveAt(0);
                lastIs.Add(i);
            } else {
                lastIs.Add(i);
            }
        }
        //Console.WriteLine("\nRan out of tokens");
        return false;
    }
}