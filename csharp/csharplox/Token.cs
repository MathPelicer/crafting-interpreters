using System;
using System.Globalization;

namespace csharplox;

public class Token
{
    public readonly TokenType _type;
    public readonly string _lexeme;
    public readonly Object _literal;
    public readonly int _line;

    public Token(TokenType tokenType, string lexeme, Object literal, int line)
    {
        _type = tokenType;
        _lexeme = lexeme;
        _literal = literal;
        _line = line;   
    }

    public override string ToString()
    {
        return $"{_type} {_lexeme} {_literal}"; 
    }
}
