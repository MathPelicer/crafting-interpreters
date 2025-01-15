// https://aka.ms/new-console-template for more information
using System.Text;
using csharplox;

public class Lox
{
    public static bool hadError = false;

    public static void Main(string[] args){
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: csharplox [script]");
            Environment.Exit(64);
        }
        else if (args.Length == 1){
            RunFiles(args[0]);
        }
        else{
            RunPrompt();
        }
    }
    private static void RunFiles(string path)
    {
        byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
        Run(Encoding.Default.GetString(bytes));

        if(hadError){
            Environment.Exit(65);
        }
    }

    private static void RunPrompt()
    {
        while(true)
        {
            Console.WriteLine("> ");
            string line = Console.ReadLine();
            if (line == null)
            {
                break;
            }

            Run(line);
            hadError = false;
        }
    }

    private static void Run(string source)
    {
        var scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        Parser parser = new(tokens);
        Expr expr = parser.Parse();

        if(hadError) return;

        Console.WriteLine(new AstPrinter().Print(expr));
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error {where}: {message}");
        hadError = true;
    }

    public static void Error(Token token, string message)
    {
        if(token.Type == TokenType.EOF)
        {
            Report(token.Line, " at end", message);
        }
        else
        {
            Report(token.Line, $" at '{token.Lexeme}'", message);
        }
    }

}
