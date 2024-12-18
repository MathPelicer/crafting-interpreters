using System;
using System.Collections;
using System.Diagnostics;
using System.Dynamic;

namespace csharplox;

public class Scanner(string source)
{
    private readonly string _source = source;
    private readonly List<Token> _tokens = [];
    private int start = 0;
    private int current = 0;
    private int line = 1;

    public List<Token> ScanTokens()
    {
        while(!IsAtEnd())
        {
            start = current;
            ScanTokens();
        }

        _tokens.Add(new Token(TokenType.EOF, "", null, line));

        return _tokens;
    }

    private bool IsAtEnd() 
    {
        return current <= _source.Length;
    }
}