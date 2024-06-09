using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Compiler
{
    public class Parser
    {
        private Lexer _lexer;
        private SymbolTable _table;
        private Token _lookahead;

        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            _lookahead = _lexer.NextToken();
            _table = lexer.Table;
        }

        public void Match(TokenType token)
        {
            if (_lookahead.Type == token)
            {
                _lookahead = _lexer.NextToken();
            }
            else
            {
                throw new Exception($"Erro de sintaxe: esperado {token}, mas encontrado {_lookahead.Type}");
            }
        }

        public string Cmd()
{
    if (_lookahead.Type == TokenType.PRINT)
    {
        Match(TokenType.PRINT);
        var value = Expr();
        return value.ToString();
    }
    else
    {
        return Expr().ToString(); // Retorne o resultado da expressão diretamente
    }
}


        public void Atrib()
        {
            string varName = _lookahead.Value.ToString();
            Match(TokenType.VAR);
            Match(TokenType.EQ);
            double value = Expr();
            _table.SetSymbol(varName, value); // Defina o valor na tabela de símbolos
        }

        public double Expr()
        {
            double value = Term();
            return Rest(value);
        }

        public double Rest(double left)
        {
            if (_lookahead.Type == TokenType.SUM)
            {
                Match(TokenType.SUM);
                double right = Expr();
                return left + right;
            }
            else if (_lookahead.Type == TokenType.SUB)
            {
                Match(TokenType.SUB);
                double right = Expr();
                return left - right;
            }
            else
            {
                return left;
            }
        }

        public double Term()
        {
            double value = Factor();
            return TermRest(value);
        }

        public double TermRest(double left)
        {
            if (_lookahead.Type == TokenType.MUL)
            {
                Match(TokenType.MUL);
                double right = Term();
                return left * right;
            }
            else if (_lookahead.Type == TokenType.DIV)
            {
                Match(TokenType.DIV);
                double right = Term();
                if (right == 0)
                {
                    throw new DivideByZeroException("Divisão por zero não é permitida.");
                }
                return left / right;
            }
            else
            {
                return left;
            }
        }

        public double Factor()
        {
            if (_lookahead.Type == TokenType.NUM)
            {
                double value = _lookahead.Value;
                Match(TokenType.NUM);
                return value;
            }
            else if (_lookahead.Type == TokenType.VAR)
            {
                string varName = _lookahead.Value.ToString();
                Match(TokenType.VAR);
                return _table.GetSymbol(varName); // Retorne o valor da variável na tabela de símbolos
            }
            else if (_lookahead.Type == TokenType.OPEN)
            {
                Match(TokenType.OPEN);
                double value = Expr();
                Match(TokenType.CLOSE);
                return value;
            }
            else
            {
                throw new Exception("Erro de sintaxe: fator inválido");
            }
        }
    }
}
