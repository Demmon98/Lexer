using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    public class Token : IComparable<Token>
    {
        public KeyValuePair<TokenType, string> KeyValueToken { get; set; }

        public Token(TokenType tokenType, string value)
        {
            KeyValueToken = new KeyValuePair<TokenType, string>(tokenType, value);
        }

        public int CompareTo(Token token)
        {
            return this.KeyValueToken.Key.CompareTo(token.KeyValueToken.Key);
        }
    }
}
