using Lang3.Utils.Errors;
using System.Diagnostics;

namespace Lang3;

class Lang3Runner {
    public static void Main(string[] args) {
        string fp;
        if (args.Length == 0) {
            fp = "C:\\Users\\finne\\OneDrive\\Documents\\0coding\\Lang3\\Lang3\\test.l3";
        } else {
            fp = args[0];
        }

        string code = "";
        try {
            code = File.ReadAllText(fp) + "\n";
        } catch (FileNotFoundException) {
            new IoResourceError("<Lang3 Runner>", $"{fp} not found").Raise();
        }

        Lexer lexer = new(code, fp);

        Stopwatch sw = new();
        sw.Start();
        List<Lexer.Token> tokens = lexer.Lex();
        sw.Stop();

        //Console.WriteLine($"Lexing completed in {sw.ElapsedMilliseconds}ms");
        foreach (Lexer.Token token in tokens) {
            //Console.WriteLine(token.ToString());
        }
        //Console.WriteLine();

        sw.Reset();

        Parser parser = new(tokens, code, fp);
        
        try {
            sw.Start();
            Parser.Node root = parser.Parse();
            sw.Stop();

            //Console.WriteLine($"Parsing completed in {sw.ElapsedMilliseconds}ms");
            Console.WriteLine(root.ToString());
        } catch (StackOverflowException) {
            Console.WriteLine("Stack Overflow");
        }

        Interpreter
    }
}