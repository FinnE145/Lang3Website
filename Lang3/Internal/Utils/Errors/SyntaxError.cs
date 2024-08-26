namespace Lang3.Utils.Errors;

public class SyntaxError : Error {
    public override string Message {get; set;} = "Incorrect Syntax";
    public override int ErrorCode {get; set;} = 2;
    public SyntaxError(string file, string s, string message, int start, int end) : base(file, s, message, start, end) {}
    public SyntaxError(string file, string s, int start, int end) : base(file, s, start, end) {}
    public SyntaxError(string file, string message) : base(file, message) {}
    public SyntaxError(string file) : base(file) {}
    public SyntaxError() : base() {}
}