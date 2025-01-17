namespace csharplox;

public class Scanner(string source)
{
    private readonly string _source = source;
    private readonly List<Token> _tokens = [];
    private static readonly Dictionary<string, TokenType> _keywords = new(){
        {"and", TokenType.AND},
        {"class", TokenType.CLASS},
        {"else", TokenType.ELSE},
        {"false", TokenType.FALSE},
        {"for", TokenType.FOR},
        {"fun", TokenType.FUN},
        {"if", TokenType.IF},
        {"nil", TokenType.NIL},
        {"or", TokenType.OR},
        {"print", TokenType.PRINT},
        {"return", TokenType.RETURN},
        {"super", TokenType.SUPER},
        {"this", TokenType.THIS},
        {"true", TokenType.TRUE},
        {"var", TokenType.VAR},
        {"while", TokenType.WHILE},
    };
    private int start = 0;
    private int current = 0;
    private int line = 1;

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            start = current;
            ScanToken();
        }

        _tokens.Add(new Token(TokenType.EOF, "", null, line));

        return _tokens;
    }

    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;
            case '!':
                AddToken(IsMatch('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;
            case '=':
                AddToken(IsMatch('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '<':
                AddToken(IsMatch('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '>':
                AddToken(IsMatch('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
            case '/':
                if (IsMatch('/'))
                {
                    while (Peek() != '\n' && !IsAtEnd()) Advance();
                }
                else
                {
                    AddToken(TokenType.SLASH);
                }
                break;
            case ' ':
            case '\r':
            case '\t':
                break;
            case '\n':
                line++;
                break;
            case '"':
                String();
                break;
            default:
                if (IsDigit(c))
                {
                    Number();
                }
                else if (IsAlpha(c))
                {
                    Identifier();
                }
                else
                {
                    Lox.Error(line, "Unexpected character.");
                }
                break;
        }
    }

    private char Advance()
    {
        current++;
        return _source.ElementAt(current - 1);
    }

    private void AddToken(TokenType tokenType)
    {
        AddToken(tokenType, null);
    }

    private void AddToken(TokenType tokenType, Object literal)
    {
        string text = _source[start..current];
        _tokens.Add(new Token(tokenType, text, literal, line));
    }

    private bool IsAtEnd()
    {
        return current >= _source.Length;
    }

    private bool IsMatch(char expected)
    {
        if (IsAtEnd())
        {
            return false;
        }

        if (_source.ElementAt(current) != expected)
        {
            return false;
        }

        current++;
        return true;
    }

    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return _source.ElementAt(current);
    }

    private char PeekNext()
    {
        if (current + 1 >= _source.Length)
        {
            return '\0';
        }

        return _source.ElementAt(current + 1);
    }

    private void String()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n')
            {
                line++;
            }
            Advance();
        }

        if (IsAtEnd())
        {
            Lox.Error(line, "Unterminated string.");
            return;
        }

        Advance();
        var beginningString = start+1;
        var endString = current-1;
        var value = _source[beginningString..endString];
        AddToken(TokenType.STRING, value);
    }

    private static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private void Number()
    {
        while (IsDigit(Peek()))
        {
            Advance();
        }

        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            Advance();

            while (IsDigit(Peek()))
            {
                Advance();
            }
        }

        AddToken(TokenType.NUMBER, double.Parse(_source[start..current]));
    }

    private void Identifier()
    {
        while (IsAlphaNumeric(Peek()))
        {
            Advance();
        }

        var text = _source[start..current];
        if (!_keywords.TryGetValue(text, out TokenType type))
        {
            type = TokenType.IDENTIFIER;
        }

        AddToken(type);
    }

    private static bool IsAlpha(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
    }

    private static bool IsAlphaNumeric(char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }
}
