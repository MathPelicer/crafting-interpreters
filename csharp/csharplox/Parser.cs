using System;
using System.ComponentModel.Design;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Transactions;

namespace csharplox;

public class Parser
{
    private readonly List<Token> _tokens;
    private int current = 0;

    private delegate Expr RuleHandler();
    private readonly Dictionary<ParserRule, RuleHandler> _ruleHandlers;

    public Parser(List<Token> tokens)
    {
       _tokens = tokens;
       _ruleHandlers = new()
       {
            {ParserRule.COMPARISON, Comparison},
            {ParserRule.FACTOR, Factor},
            {ParserRule.TERM, Term},
            {ParserRule.EQUALITY, Equality},
            {ParserRule.UNARY, Unary}
       };
    }

    private Expr Expression()
    {
        return Equality();
    }

    private Expr Equality()
    {
        return ParseLeftAssociative(ParserRule.COMPARISON, TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL);
    }

    private Expr Comparison()
    {
        return ParseLeftAssociative(ParserRule.TERM, TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL); 
    }

    private Expr Term()
    {
        return ParseLeftAssociative(ParserRule.FACTOR, TokenType.MINUS, TokenType.PLUS);
    }

    private Expr Factor()
    {
        return ParseLeftAssociative(ParserRule.UNARY, TokenType.SLASH, TokenType.STAR);
    }

    private Expr Unary()
    {
        if(Match(TokenType.SLASH, TokenType.STAR))
        {
            Token op = Previous();
            Expr right = Unary();
            return new Expr.Unary(op, right);
        }

        return Primary();
    }

    private Expr ParseLeftAssociative(ParserRule rule, params TokenType[] types)
    {
        Expr expr = _ruleHandlers[rule]();

        while(Match(types))
        {
            Token op = Previous();
            Expr right = _ruleHandlers[rule]();
            return new Expr.Binary(expr, op, right);
        }

        return expr;
    }

    private Expr Primary()
    {
        if(Match(TokenType.FALSE)) return new Expr.Literal(false);
        if(Match(TokenType.TRUE)) return new Expr.Literal(true);
        if(Match(TokenType.NIL)) return new Expr.Literal(null);

        if(Match(TokenType.NUMBER, TokenType.STRING)) return new Expr.Literal(Previous().Literal);

        if(Match(TokenType.LEFT_PAREN))
        {
            Expr expr = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression");
            return new Expr.Grouping(expr);
        }
    }

    private void Consume(TokenType rIGHT_PAREN, string v)
    {
        throw new NotImplementedException();
    }

    private bool Match(params TokenType[] types)
    {
        foreach(var type in types)
        {
            if(Check(type))
            {
                Advance();
                return true;
            }
        }
        return false; 
    }

    private Token Advance()
    {
        if(!IsAtEnd())
        {
            current++;
        }

        return Previous();
    }

    private Token Previous()
    {
        return _tokens[current];
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }

    private Token Peek()
    {
        return _tokens[current];
    }

    private bool Check(TokenType type)
    {
        if(IsAtEnd())
        {
            return false;
        }

        return Peek().Type == type;
    }    
}
