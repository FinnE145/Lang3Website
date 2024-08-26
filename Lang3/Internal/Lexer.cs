using Lang3.Utils.Errors;

namespace Lang3;

class Lexer(string code, string fp) {
    public class Token(string type, string value, int line, int start, int end) {
        public string type = type;
        public string value = value;
        public int line = line;
        public int start = start;
        public int end = end;

        public override string ToString() {
            return $"Token({type}, {value}, {line}, {start}, {end})";
        }
    }

    // private readonly string[] keywords = new string[] {"if", "else", "while", "for", "return", "?"};
    private readonly Dictionary<string, string> keywords = new() {
        {"if", "if"},
        {"else", "else"},
        {"while", "while"},
        {"for", "for"},
        {"return", "return"},
        {"?", "blank"}
    };

    private readonly Dictionary<string, string> opers = new() {
        {"+", "add"},
        {"-", "sub"},
        {"*", "mul"},
        {"/", "div"},
        {"**", "exp"},
        {"/^", "root"},
        {"//", "iDiv"},
        {"%", "mod"},
        {">", "gt"},
        {"<", "lt"},
        {">=", "gte"},
        {"<=", "lte"},
        {"==", "eq"},
        {"!=", "neq"},
        {"&&", "and"},
        {"||", "or"},
        {"&", "bAnd"},
        {"|", "bOr"},
        {"^", "xor"},
        {"<<", "lShift"},
        {">>", "rShift"},
        {"=", "assign"},
        {"+=", "addAssign"},
        {"-=", "subAssign"},
        {"*=", "mulAssign"},
        {"/=", "divAssign"},
        {"**=", "expAssign"},
        {"/^=", "rootAssign"},
        {"//=", "iDivAssign"},
        {"%=", "modAssign"},
        {"&&=", "andAssign"},
        {"||=", "orAssign"},
        {"&=", "bAndAssign"},
        {"|=", "bOrAssign"},
        {"^=", "xorAssign"},
        {"<<=", "lShiftAssign"},
        {">>=", "rShiftAssign"},
        {"++", "inc"},
        {"--", "dec"},
        {"!", "not"}
    };

    private readonly Dictionary<string, string> brackets = new() {
        {"(", "lParen"},
        {")", "rParen"},
        {"[", "lBracket"},
        {"]", "rBracket"},
        {"{", "lBrace"},
        {"}", "rBrace"},
        {":", "lCode"},
        {";", "rCode"}
    };

    private readonly string code = code.Replace("\r", "");
    private readonly string fp = fp;
    private int line = 0;
    private int ld = 0;
    internal static readonly char[] opStarts = ['+', '-', '*', '/', '%', '>', '<', '=', '&', '|', '^', '!'];

    private Token BucketNum(ref int i) {
        int start = i++;
        string t = "int";
        while (i-- < code.Length && (char.IsDigit(code[++i]) || code[i] == '.')) {
            if (code[i++] == '.') {
                if (t == "dec") {
                    new MalformedTokenError(this.fp, code, "Multiple decimal points in number", start, i).Raise();
                }
                t = "dec";
            }
        }
        if (code[--i] == '.') {
            new MalformedTokenError(this.fp, code, "Number cannot end with a decimal point", start, i).Raise();
        }
        return new Token(t, code[start..(i+1)], line, start-ld, i-ld+1);
    }

    private Token BucketOp(ref int i) {
        int start = i++;
        while (i < code.Length && opers.ContainsKey(code[start..(i+1)])) {
            i++;
        }
        string sample = code[start..(--i+1)];
        return new Token(sample == "++" || sample == "--" || sample == "!" ? "uOperator" : "operator", opers[sample], line, start-ld, i-ld+1);
    }

    private Token BucketBracket(ref int i) {
        return new Token(brackets[code[i].ToString()], code[i].ToString(), line, i-ld, i-ld+1);
    }

    private Token BucketWord(ref int i) {
        int start = i++;
        while (i < code.Length && (char.IsLetter(code[i]) || char.IsDigit(code[i]) || code[i] == '_')) {
            i++;
        }
        string s = code[start..i--];
        return new Token(keywords.TryGetValue(s, out string? value) ? value : "var", s, line, start-ld, i-ld+1);
    }

    public List<Token> Lex() {
        List<Token> tokens = [];

        for (int i = 0; i < code.Length; i++) {
            char c = code[i];

            if (c == ' ' || c == '\t') {
                continue;
            }

            if (c == '\n') {
                line++;
                ld = i+1;
                continue;
            }

            if (char.IsDigit(c)) {
                tokens.Add(BucketNum(ref i));
                continue;
            }

            if (opStarts.Contains(c)) {
                tokens.Add(BucketOp(ref i));
                continue;
            }

            if (brackets.ContainsKey(c.ToString())) {
                tokens.Add(BucketBracket(ref i));
                continue;
            }

            if (char.IsLetter(c) || c == '_') {
                tokens.Add(BucketWord(ref i));
                continue;
            }

            string v = c != '\n' ? c.ToString() : "\\n";
            new MalformedTokenError(this.fp, code, "Invalid character", i, i+1).Raise();
        }

        tokens.Add(new Token("EOF", "", line, code.Length, code.Length));

        return tokens;
    }
}