
using System.Text;

namespace csharplox;

public class AstPrinter : Expr.IVisitor<string>
{
    string Print(Expr expr)
    {
        return expr.Accept(this);
    }

    string Expr.IVisitor<string>.VisitBinaryExpr(Expr.Binary expr)
    {
        return Parenthesize(expr.op._lexeme, expr.left, expr.right);
    }

    string Expr.IVisitor<string>.VisitGroupingExpr(Expr.Grouping expr)
    {
        return Parenthesize("group", expr.expression);
    }

    string Expr.IVisitor<string>.VisitLiteralExpr(Expr.Literal expr)
    {
        if (expr.value == null)
        {
            return "nil";
        }
        return expr.value.ToString();
    }

    string Expr.IVisitor<string>.VisitUnaryExpr(Expr.Unary expr)
    {
        return Parenthesize(expr.op._lexeme, expr.right);
    }

    private string Parenthesize(string lexeme, params Expr[] exprs)
    {
        StringBuilder sb = new();

        sb.Append('(').Append(lexeme);
        foreach(var expr in exprs)
        {
            sb.Append(' ');
            sb.Append(expr.Accept(this));
        }
        sb.Append(')');

        return sb.ToString();
    }

    public static void Main(string[] args)
    {
        Expr expression = new Expr.Binary(
            new Expr.Unary(
                new Token(TokenType.MINUS, "-", null, 1),
                new Expr.Literal(123)
            ),
            new Token(TokenType.STAR, "*", null, 1),
            new Expr.Grouping(
                new Expr.Literal(45.67)
            ));

        Console.WriteLine(new AstPrinter().Print(expression));
    }
}
