namespace Lang3.Utils.Errors;

public class IoResourceError : Error {
    public override string Message {get; set;} = "There was an error interacting with the requested resource";
    public override int ErrorCode {get; set;} = 3;
    public override bool Fatal {get; set;} = true;
    public IoResourceError(string file, string s, string message, int start, int end) : base(file, s, message, start, end) {}
    public IoResourceError(string file, string s, int start, int end) : base(file, s, start, end) {}
    public IoResourceError(string file, string message) : base(file, message) {}
    public IoResourceError(string file) : base(file) {}
    public IoResourceError() : base() {}
}