using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    public enum TokenType
    {
        ERROR,
        WHITE_SPACE,
        NUMERIC,
        LITERAL,
        BOOLEAN_LITERAL,
        NULL_LITERAL,
        SYMBOLIC,
        SINGLE_LINE_COMMENT,
        MULTI_LINE_COMMENT,
        KEYWORD,
        IDENTIFIER,
        OPERATOR,
        PUNCTUATION
    }
}
