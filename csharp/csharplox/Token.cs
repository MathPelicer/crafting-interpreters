using System;
using System.Globalization;

namespace csharplox;

public class Token
{
    public readonly TokenType Type;
    public readonly string Lexeme;
    public readonly Object Literal;
    public readonly int Line;

    public Token(TokenType tokenType, string lexeme, Object literal, int line)
    {
        Type = tokenType;
        Lexeme = lexeme;
        Literal = literal;
        Line = line;   
    }

    public override string ToString()
    {
        return $"{Type} {Lexeme} {Literal}"; 
    }
}
