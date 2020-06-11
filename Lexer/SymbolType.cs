using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    public class SymbolType
    {

        public static readonly HashSet<string> keywords = new HashSet<string>() {"abstract", "as", "base", "bool",
                "break", "byte", "case", "catch",
                "char", "checked", "class", "const",
                "continue", "decimal", "default", "delegate",
                "do", "double", "else", "enum",
                "event", "explicit", "extern", "false",
                "finally", "fixed", "float", "for",
                "foreach", "goto", "if", "implicit",
                "in", "int", "interface",
                "internal", "is", "lock", "long",
                "namespace", "new", "null", "object",
                "operator", "out", "override",
                "params", "private", "protected", "public",
                "readonly", "ref", "return", "sbyte",
                "sealed", "short", "sizeof", "stackalloc",
                "static", "string", "struct", "switch",
                "this", "throw", "true", "try",
                "typeof", "uint", "ulong", "unchecked",
                "unsafe", "ushort", "using", "virtual",
                "void", "volatile", "while", "var"};

        public static readonly HashSet<char> escapeSequences = new HashSet<char>() { 'b', 't', 'n', '\\', '\'', '"', 'r', 'f' };

        public static bool IsKeyword(String word)
        {
            return keywords.Contains(word);
        }

        public static bool IsEscapeSequence(char character)
        {
            return escapeSequences.Contains(character);
        }
    }
}
