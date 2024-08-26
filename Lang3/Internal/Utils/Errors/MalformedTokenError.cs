namespace Lang3.Utils.Errors;

public class MalformedTokenError : Error {
    public override string Message {get; set;} = "An incorrectly formed token was found";
    public override int ErrorCode {get; set;} = 2;
    public MalformedTokenError(string file, string s, string message, int start, int end) : base(file, s, message, start, end) {}
    public MalformedTokenError(string file, string s, int start, int end) : base(file, s, start, end) {}
    public MalformedTokenError(string file, string message) : base(file, message) {}
    public MalformedTokenError(string file) : base(file) {}
    public MalformedTokenError() : base() {}
}