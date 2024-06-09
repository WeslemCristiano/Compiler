using System.Text;

namespace Compiler
{
    public class Lexer
    {
        public static char EOF = '\0';

        public string Input { get; set; }
        private int _ptr;
        public SymbolTable Table { get; private set; }

        public Lexer(string input, SymbolTable table)
        {
            Input = input;
            _ptr = 0;
            Table = table;
        }

        private char Scan()
        {
            if (_ptr >= Input.Length)
                return EOF;
            return Input[_ptr++];
        }

        private int ParseInt(char c)
        {
            return c - '0';
        }

        private void SkipWhitespace()
        {
            while (_ptr < Input.Length && (Input[_ptr] == ' ' || Input[_ptr] == '\t'))
            {
                _ptr++;
            }
        }

        public Token NextToken()
        {
            SkipWhitespace();
            char c = Scan();

            if (c == EOF)
                return new Token { Type = TokenType.EOF };

            switch (c)
            {
                case '+': return new Token { Type = TokenType.SUM };
                case '-': return new Token { Type = TokenType.SUB };
                case '*': return new Token { Type = TokenType.MUL };
                case '/': return new Token { Type = TokenType.DIV };
                case '(': return new Token { Type = TokenType.OPEN };
                case ')': return new Token { Type = TokenType.CLOSE };
                case '=': return new Token { Type = TokenType.EQ };
            }

            if (char.IsDigit(c))
            {
                int x = ParseInt(c);
                while (char.IsDigit((c = Scan())))
                {
                    x = x * 10 + ParseInt(c);
                }
                _ptr--; // Retrocede o ponteiro para não consumir o próximo caractere não numérico
                return new Token { Type = TokenType.NUM, Value = x };
            }

            if (char.IsUpper(c))
            {
                if (c == 'P')
                {
                    string token = "P";
                    while (char.IsLetter(c = Scan()))
                    {
                        token += c;
                    }
                    _ptr--; // Retrocede o ponteiro para não consumir o próximo caractere não alfabético

                    if (token == "PRINT")
                    {
                        return new Token { Type = TokenType.PRINT };
                    }
                }
                return new Token { Type = TokenType.UNK };
            }

            if (char.IsLower(c))
            {
                var sb = new StringBuilder();
                sb.Append(c);
                while (char.IsLower(c = Scan()))
                {
                    sb.Append(c);
                }
                _ptr--; // Retrocede o ponteiro para não consumir o próximo caractere não alfabético
                string varName = sb.ToString();
                return new Token { Type = TokenType.VAR, Value = Table.AddSymbol(varName) }; // Implementar tabela de símbolos
            }

            return new Token { Type = TokenType.UNK };
        }
    }

    public struct Token
    {
        public TokenType Type;
        public int Value;
    }

    public enum TokenType
    {
        VAR, NUM, EQ, SUB, SUM, MUL, DIV, OPEN, CLOSE, PRINT, EOF, UNK
    }
}
