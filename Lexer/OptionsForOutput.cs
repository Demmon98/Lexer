using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    class OptionsForOutput
    {
        public static void SequenceOfTokens(List<Token> tokenList)
        {
            Console.WriteLine("------------ SEQUENCE OF TOKENS ------------");

            foreach(Token token in tokenList)
            {
                if (token.KeyValueToken.Key != TokenType.WHITE_SPACE)
                    Console.WriteLine("TokenName: " + token.KeyValueToken.Key + "\n\tvalue: " + token.KeyValueToken.Value + "\n");
            }
        }

        public static void SourceCodeHighlighting(List<Token> tokenList)
        {
            Console.WriteLine("------------ SEQUENCE CODE HIGHLIGHTING ------------");

            foreach(Token token in tokenList)
            {
                switch (token.KeyValueToken.Key)
                {
                    case TokenType.ERROR:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(token.KeyValueToken.Value);
                        break;
                    case TokenType.WHITE_SPACE:
                        Console.WriteLine(token.KeyValueToken.Value);
                        break;
                    case TokenType.NUMERIC:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(token.KeyValueToken.Value);
                        break;
                    case TokenType.LITERAL:
                    case TokenType.SYMBOLIC:
                    case TokenType.BOOLEAN_LITERAL:
                    case TokenType.NULL_LITERAL:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(token.KeyValueToken.Value);
                        break;
                    case TokenType.SINGLE_LINE_COMMENT:
                    case TokenType.MULTI_LINE_COMMENT:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(token.KeyValueToken.Value);
                        break;
                    case TokenType.KEYWORD:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(token.KeyValueToken.Value);
                        break;
                    case TokenType.IDENTIFIER:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine(token.KeyValueToken.Value);
                        break;
                    case TokenType.OPERATOR:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(token.KeyValueToken.Value);
                        break;
                    case TokenType.PUNCTUATION:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(token.KeyValueToken.Value);
                        break;
                }

            }

            Console.ResetColor();
        }

        public static void TokensSortedByType(List<Token> tokenList)
        {
            Console.WriteLine("------------ TOKENS SORTED BY TYPE ------------");

            List<Token> sortedTokensList = new List<Token>(tokenList);
            sortedTokensList.Sort();

            int countTokens = sortedTokensList.Count;
            int index = 0;

            TokenType currentTokenType = sortedTokensList[0].KeyValueToken.Key;

            if (currentTokenType != TokenType.WHITE_SPACE)
                Console.WriteLine("TokenName: " + currentTokenType);

            while (index < countTokens)
            {
                if (sortedTokensList[index].KeyValueToken.Key == TokenType.WHITE_SPACE)
                {
                    index++;
                    continue;
                }

                if (currentTokenType != sortedTokensList[index].KeyValueToken.Key)
                {
                    currentTokenType = sortedTokensList[index].KeyValueToken.Key;
                    Console.WriteLine("\nTokenName: " + currentTokenType);
                }

                Console.WriteLine("\tvalue: " + sortedTokensList[index].KeyValueToken.Value);

                index++;
            }
        }
    }
}
