using System;
using System.Globalization;

namespace csharplox;

public class Token
{
    readonly TokenType _type;
    readonly string _lexeme;
    readonly Object _literal;
    readonly int _line;

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
