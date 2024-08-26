namespace Lang3.Utils.Errors;

public class Error {
    public string file = "";
    public virtual string Message {get; set;} = "Default error message";
    public int start = -1;
    public int end = -1;
    public int line = -1;
    public int lineOffset = -1;
    public string sample = "";
    public bool raised = false;
    public virtual int ErrorCode {get; set;} = 1;
    public virtual bool Fatal {get; set;} = true;

    public static void GetLineNumber(string s, int start, out int line, out int lineOffset) {
        line = 0;
        lineOffset = 0;

        for (int cc = 0; cc < start; cc++) {
            if (s[cc] == '\n') {
                line++;
                lineOffset = cc;
            }
        }
    }

    public Error(string file, string s, string message, int start, int end) {
        this.file = file;
        this.Message = message;
        this.start = start;
        this.end = end;
        GetLineNumber(s, start, out line, out lineOffset);
        sample = s.Split('\n')[line];
    }

    public Error(string file, string s, int start, int end) {
        this.file = file;
        this.start = start;
        this.end = end;
        GetLineNumber(s, start, out line, out lineOffset);
        sample = s.Split('\n')[line];
    }

    public Error(string file, string message) {
        this.file = file;
        this.Message = message;
    }

    public Error(string file) {
        this.file = file;
    }

    public Error() {}

    private int RaiseArgs(string file, int line, int lStart, int lEnd, string message, string sample) {
        raised = true;
        line += 1;
        lStart += line == 1 ? 1 : 0;
        lEnd += line == 1 ? 1 : 0;
        string loc = "";
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(this.GetType().Name);
        Console.ResetColor();
        Console.Write(" in ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(Path.GetFileName(file));
        Console.ResetColor();
        Console.Write(": ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
        if (sample != "") {
            int ulPadding = 0;
            if (line != 0 || lStart != 0 || lEnd != 0) {
                ulPadding = lStart + line.ToString().Length + lStart.ToString().Length + lEnd.ToString().Length + 9;
                loc = $":{line}:{lStart}-{lEnd}";
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"Line {line}: {lStart}-{lEnd}> ");
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(sample[0..(lStart-1)]);
            Console.ResetColor();
            Console.Write(sample[(lStart-1)..(lEnd-1)]);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(sample[(lEnd-1)..]);
            Console.ResetColor();
            Console.WriteLine($"{new string(' ', ulPadding)}{new string('^', lEnd-lStart)}");
        }
        if (file != "" && !file.Contains('<')) {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Path: ");
            Console.ResetColor();
            Console.WriteLine(Path.GetFullPath(file) + loc);
        }
        Console.WriteLine();
        Console.ResetColor();
        if (Fatal) {
            Environment.Exit(this.ErrorCode);
        }
        return this.ErrorCode;
    }

    private int RaiseWithLocalArgs(string file, string s, int line, int start, int end, string message) {
        if (line == -1 || lineOffset == -1) {
            GetLineNumber(s, start, out line, out lineOffset);
        }
        string sample = s.Split('\n')[line];
        return RaiseArgs(file, line, start-lineOffset, end-lineOffset, message, sample);
    }

    public int Raise(string file, string s, int start, int end, string message = "") {
        line = -1;
        return RaiseWithLocalArgs(file, s, line, start, end, message);
    }

    public int Raise(string s, int start, int end, string message = "") {
        line = -1;
        return RaiseWithLocalArgs(file, s, line, start, end, message);
    }

    public int Raise(string message) {
        return RaiseArgs(file, line, start-lineOffset, end-lineOffset, message, sample);
    }

    public int Raise() {
        return RaiseArgs(file, line, start-lineOffset, end-lineOffset, Message, sample);
    }
}