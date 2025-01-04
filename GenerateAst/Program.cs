internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: generate_ast <output directory>");
            Environment.Exit(64);
        }

        var outputDir = args[0];

        DefineAst(outputDir, "Expr",
        [
            "Binary: Expr left, Token op, Expr right",
            "Grouping: Expr expression",
            "Literal: Object value",
            "Unary: Token op, Expr right"
        ]);        
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types)
    {
        string path = Path.Combine(outputDir, baseName + ".cs");
        using StreamWriter sw = new(path);

        sw.WriteLine("namespace csharplox;");
        sw.WriteLine("");
        sw.WriteLine($"abstract class {baseName}" + "{");

        foreach (var type in types)
        {
            var className = type.Split(':')[0].Trim();
            var fields = type.Split(':')[1].Trim();

            DefineType(sw, baseName, className, fields);
        }

        sw.WriteLine("}");
        sw.Close();
    }

    private static void DefineType(StreamWriter sw, string baseName, string className, string fields)
    {
        sw.WriteLine($"    class {className} : {baseName}" + "{");

        foreach (var field in fields.Split(','))
        {
            var name = field.TrimStart().Split(' ')[1];
            var type = field.TrimStart().Split(' ')[0];
            sw.WriteLine($"        private readonly {type} {name};");
        }

        sw.WriteLine();
        sw.WriteLine($"        {className} ({fields})" + "{");

        foreach (var field in fields.Split(','))
        {
            var name = field.TrimStart().Split(' ')[1];
            sw.WriteLine($"            this.{name} = {name};");
        }

        sw.WriteLine("        }");
        sw.WriteLine();
        sw.WriteLine("    }");
        }
}