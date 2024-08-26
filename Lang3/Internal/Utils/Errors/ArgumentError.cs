namespace Lang3.Utils.Errors;

public class ArgumentError : Error {
    public override string Message {get; set;} = "Incorrect Syntax";
    public override int ErrorCode {get; set;} = 2;
    public ArgumentError(string file, string s, string message, int start, int end) : base(file, s, message, start, end) {}
    public ArgumentError(string file, string s, int start, int end) : base(file, s, start, end) {}
    public ArgumentError(string file, string message) : base(file, message) {}
    public ArgumentError(string file) : base(file) {}
    public ArgumentError() : base() {}
}