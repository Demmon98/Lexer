using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    class Lexer
    {

        private string wholeFile;
        private int currentIndex;

        private State state = State.START;

        private StringBuilder buffer = new StringBuilder();
        private List<Token> tokens = new List<Token>();

        public Lexer(string filePath)
        {
            ReadWholeFile(filePath);
            Analyser();
            OptionsForOutput.SequenceOfTokens(tokens);
            OptionsForOutput.SourceCodeHighlighting(tokens);
            OptionsForOutput.TokensSortedByType(tokens);
        }

        private void ReadWholeFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                wholeFile = reader.ReadToEnd();
            }

            wholeFile = wholeFile.Replace("\r", "");
            wholeFile += "\n";
        }

        private void Analyser()
        {
            for (currentIndex = 0; currentIndex < wholeFile.Length; currentIndex++)
            {
                char c = wholeFile[currentIndex];
                switch (state)
                {
                    case State.START:
                        StartState(c);
                        break;
                    case State.SINGLE_SLASH:
                        SingleSlashState(c);
                        break;
                    case State.SINGLE_LINE_COMMENT:
                        SingleLineCommentState(c);
                        break;
                    case State.MULTI_LINE_COMMENT:
                        MultiLineCommentState(c);
                        break;
                    case State.MULTI_LINE_COMMENT_AND_STAR:
                        MultiLineCommentAndStarState(c);
                        break;
                    case State.SINGLE_PLUS:
                        SinglePlusState(c);
                        break;
                    case State.SINGLE_MINUS:
                        SingleMinusState(c);
                        break;
                    case State.OPERATOR_MAYBE_BEFORE_EQUAL:
                        OperatorMaybeBeforeEqualState(c);
                        break;
                    case State.SINGLE_LESS_THAN:
                        SingleLessThanState(c);
                        break;
                    case State.SINGLE_GREATER_THAN:
                        SingleGreaterThanState(c);
                        break;
                    case State.SINGLE_COLON:
                        SingleColonState(c);
                        break;
                    case State.SINGLE_AMPERSAND:
                        SingleAmpersandState(c);
                        break;
                    case State.SINGLE_VERTICAL_BAR:
                        SingleVerticalBarState(c);
                        break;
                    case State.SINGLE_DOT:
                        SingleDotState(c);
                        break;
                    case State.IDENTIFIER:
                        IdentifierState(c);
                        break;
                    case State.SINGLE_DOLLAR:
                        SingleDollarState(c);
                        break;
                    case State.SINGLE_AT:
                        SingleAtState(c);
                        break;
                    case State.DECIMAL_NUMBER:
                        DecimalNumberState(c);
                        break;
                    case State.OCTAL_NUMBER:
                        OctalNumberState(c);
                        break;
                    case State.HEX_NUMBER:
                        HexNumberState(c);
                        break;
                    case State.FLOATING_POINT_NUMBER:
                        FloatingPointNumberState(c);
                        break;
                    case State.SINGLE_ZERO:
                        SingleZeroState(c);
                        break;
                    case State.BINARY_NUMBER:
                        BinaryNumberState(c);
                        break;
                    case State.SYMBOLIC_CONSTANT:
                        SymbolicConstantState(c);
                        break;
                    case State.BACKSLASH_IN_SYMBOLIC_CONSTANT:
                        BackslashInSymbolicConstantState(c);
                        break;
                    case State.ONE_OCTAL_DIGIT_AFTER_BACKSLASH_IN_SYMBOLIC_CONSTANT:
                        OneOctalDigitAfterBackslashInSymbolicConstantState(c);
                        break;
                    case State.TWO_OCTAL_DIGIT_AFTER_BACKSLASH_IN_SYMBOLIC_CONSTANT:
                        TwoOctalDigitAfterBackslashInSymbolicConstantState(c);
                        break;
                    case State.END_OF_SYMBOLIC_CONSTANT:
                        EndOfSymbolicConstantState(c);
                        break;
                    case State.ERROR_READ_SYMBOLIC_CONSTANT:
                        ErrorReadSymbolicConstantState(c);
                        break;
                    case State.BACKSLASH_INSIDE_ERROR_READ_SYMBOLIC_CONSTANT:
                        BackslashInsideErrorReadSymbolicConstantState(c);
                        break;
                    case State.LITERAL_CONSTANT:
                        LiteralConstantState(c);
                        break;
                    case State.BACKSLASH_IN_LITERAL_CONSTANT:
                        BackslashInLiteralConstantState(c);
                        break;
                    case State.ERROR_READ_LITERAL_CONSTANT:
                        ErrorReadLiteralConstantState(c);
                        break;
                }
            }

            if (buffer.Length != 0)
            {
                AddToken(TokenType.ERROR);
            }
        }


        private void AddcharacterToBuffer(char c)
        {
            buffer.Append(c);
        }

        private void AddcharacterToBuffer(char c, State state)
        {
            buffer.Append(c);
            this.state = state;
        }

        private void AddToken(TokenType tokenType, string value)
        {
            tokens.Add(new Token(tokenType, value));
            //Console.WriteLine("Add token with type: " + tokenType.tostring() + " and value: " + value);
        }

        private void AddToken(TokenType tokenType)
        {
            AddToken(tokenType, buffer.ToString());
            buffer = new StringBuilder();
        }

        private void AddToken(TokenType tokenType, char value)
        {
            tokens.Add(new Token(tokenType, value.ToString()));
            //Console.WriteLine("Add token with type: " + tokenType.tostring() + " and byte value: " + (byte) value);
        }

        //buffer is empty
        private void StartState(char c)
        {
            if (char.IsWhiteSpace(c))
            {
                AddToken(TokenType.WHITE_SPACE, c);
            }
            else if (c == '/')
            {
                AddcharacterToBuffer(c, State.SINGLE_SLASH);
            }
            else if (c == '+')
            {
                AddcharacterToBuffer(c, State.SINGLE_PLUS);
            }
            else if (c == '-')
            {
                AddcharacterToBuffer(c, State.SINGLE_MINUS);
            }
            else if (c == '=' || c == '^' || c == '%' || c == '!' || c == '*')
            {
                //^, %, !, =, *
                AddcharacterToBuffer(c, State.OPERATOR_MAYBE_BEFORE_EQUAL);
            }
            else if (c == '<')
            {
                AddcharacterToBuffer(c, State.SINGLE_LESS_THAN);
            }
            else if (c == '>')
            {
                AddcharacterToBuffer(c, State.SINGLE_GREATER_THAN);
            }
            else if (c == '?' || c == '~')
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.OPERATOR);
            }
            else if (c == ':')
            {
                AddcharacterToBuffer(c, State.SINGLE_COLON);
            }
            else if (c == '&')
            {
                AddcharacterToBuffer(c, State.SINGLE_AMPERSAND);
            }
            else if (c == '|')
            {
                AddcharacterToBuffer(c, State.SINGLE_VERTICAL_BAR);
            }
            else if (c == '(' || c == ')' || c == '{' || c == '}' || c == '[' || c == ']' || c == ';' || c == ',')
            { 
                //( ) { } [ ] ; ,
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.PUNCTUATION);
            }
            else if (c == '@')
            {
                AddcharacterToBuffer(c, State.SINGLE_AT);
            }
            else if (c == '.')
            {
                AddcharacterToBuffer(c, State.SINGLE_DOT);
            }
            else if (char.IsLetter(c) || c == '_')
            {
                AddcharacterToBuffer(c, State.IDENTIFIER);
            }
            else if (c == '$')
            {
                AddcharacterToBuffer(c, State.SINGLE_DOLLAR);
            }
            else if (c == '0')
            {
                AddcharacterToBuffer(c, State.SINGLE_ZERO);
            }
            else if (c >= '1' && c <= '9')
            {
                AddcharacterToBuffer(c, State.DECIMAL_NUMBER);
            }
            else if (c == '\'')
            {
                AddcharacterToBuffer(c, State.SYMBOLIC_CONSTANT);
            }
            else if (c == '\"')
            {
                AddcharacterToBuffer(c, State.LITERAL_CONSTANT);
            }
            else
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.ERROR);
            }

        }

        //possible states: //, /*, /=
        private void SingleSlashState(char c)
        {
            if (c == '/')
            {
                AddcharacterToBuffer(c, State.SINGLE_LINE_COMMENT);
            }
            else if (c == '*')
            {
                AddcharacterToBuffer(c, State.MULTI_LINE_COMMENT);
            }
            else if (c == '=')
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.OPERATOR);
            }
            else
            {
                AddToken(TokenType.OPERATOR);
                currentIndex--;
                state = State.START;
            }
        }

        private void SingleLineCommentState(char c)
        {
            if (c == '\n')
            {
                AddToken(TokenType.SINGLE_LINE_COMMENT);
                AddToken(TokenType.WHITE_SPACE, c);
                state = State.START;
            }
            else
            {
                AddcharacterToBuffer(c);
            }

        }

        private void MultiLineCommentState(char c)
        {
            if (c == '*')
            {
                AddcharacterToBuffer(c, State.MULTI_LINE_COMMENT_AND_STAR);
            }
            else
            {
                AddcharacterToBuffer(c);
            }
        }

        private void MultiLineCommentAndStarState(char c)
        {
            if (c == '/')
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.MULTI_LINE_COMMENT);
            }
            else if (c == '*')
            {
                AddcharacterToBuffer(c);
            }
            else
            {
                AddcharacterToBuffer(c, State.MULTI_LINE_COMMENT);
            }
        }

        private void SinglePlusState(char c)
        {
            if (c == '+' || c == '=')
            {
                // ++ +=
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.OPERATOR);
            }
            else
            {
                AddToken(TokenType.OPERATOR);
                currentIndex--;
                state = State.START;
            }
        }

        private void SingleMinusState(char c)
        {
            if (c == '-' || c == '=')
            {
                //-- -=
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.OPERATOR);
            }
            else
            {
                //-
                AddToken(TokenType.OPERATOR);
                currentIndex--;
                state = State.START;
            }
        }

        private void OperatorMaybeBeforeEqualState(char c)
        {
            if (c == '=' || c == '>')
            {
                //==  =>
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.OPERATOR);
            }
            else
            {
                //=
                AddToken(TokenType.OPERATOR);
                currentIndex--;
                state = State.START;
            }
        }

        private void SingleLessThanState(char c)
        {
            if (c == '<')
            {
                //<<
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.OPERATOR);
            }
            else if (c == '=')
            {
                //<=
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.OPERATOR);
            }
            else
            {
                //<
                AddToken(TokenType.OPERATOR);
                currentIndex--;
                state = State.START;
            }
        }

        private void SingleGreaterThanState(char c)
        {
            if (c == '>')
            {
                //>>
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.OPERATOR);
            }
            else if (c == '=')
            {
                //>=
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.OPERATOR);
            }
            else
            {
                //>
                AddToken(TokenType.OPERATOR);
                currentIndex--;
                state = State.START;
            }
        }

        private void SingleColonState(char c)
        {
            if (c == ':')
            {
                // ::
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.PUNCTUATION);
            }
            else
            {
                AddToken(TokenType.OPERATOR);
                currentIndex--;
                state = State.START;
            }
        }

        private void SingleAmpersandState(char c)
        {
            if (c == '&' || c == '=')
            {
                // && &=
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.OPERATOR);
            }
            else
            {
                AddToken(TokenType.OPERATOR);
                currentIndex--;
                state = State.START;
            }
        }

        private void SingleVerticalBarState(char c)
        {
            if (c == '|' || c == '=')
            {
                // || |=
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.OPERATOR);
            }
            else
            {
                AddToken(TokenType.OPERATOR);
                currentIndex--;
                state = State.START;
            }
        }

        private void SingleDotState(char c)
        {
            if (c == '.')
            {
                //..
                AddToken(TokenType.ERROR);
                currentIndex--;
                state = State.START;
            }
            else if (char.IsDigit(c))
            {
                AddcharacterToBuffer(c, State.FLOATING_POINT_NUMBER);
            }
            else
            {
                //.
                AddToken(TokenType.PUNCTUATION);
                currentIndex--;
                state = State.START;
            }
        }

        private void IdentifierState(char c)
        {
            if (char.IsDigit(c) || char.IsLetter(c) || c == '_' || c == '$')
            {
                AddcharacterToBuffer(c);
            }
            else if (SymbolType.IsKeyword(buffer.ToString()))
            {
                AddToken(TokenType.KEYWORD);
                currentIndex--;
                state = State.START;
            }
            else if (buffer.ToString() == "true" || buffer.ToString() == "false")
            {
                AddToken(TokenType.BOOLEAN_LITERAL);
                currentIndex--;
                state = State.START;
            }
            else if (buffer.ToString() == "null")
            {
                AddToken(TokenType.NULL_LITERAL);
                currentIndex--;
                state = State.START;
            }
            else
            {
                AddToken(TokenType.IDENTIFIER);
                currentIndex--;
                state = State.START;
            }
        }

        private void SingleDollarState(char c)
        {
            //$"
            if (c == '"')
            {
                AddToken(TokenType.OPERATOR);
                currentIndex--;
                state = State.START;
            }
            //$@
            else if (c == '@')
            {
                AddToken(TokenType.OPERATOR, c);
                state = State.SINGLE_DOLLAR;
            }
            else
            {
                AddcharacterToBuffer(c, State.IDENTIFIER);
            }
        }

        private void SingleAtState(char c)
        {
            //@"
            if (c == '"')
            {
                AddToken(TokenType.OPERATOR);
                currentIndex--;
                state = State.START;
            }
            //@$
            else if (c == '$')
            {
                AddToken(TokenType.OPERATOR, c);
                state = State.SINGLE_AT;
            }
            else
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.PUNCTUATION);
            }
        }

        private void DecimalNumberState(char c)
        {
            if (char.IsDigit(c) || c == '_' || c == 'e' || c == 'E')
            {
                AddcharacterToBuffer(c);
            }
            else if (c == '.')
            {
                AddcharacterToBuffer(c, State.FLOATING_POINT_NUMBER);
            }
            else if (c == 'l' || c == 'L' || c == 'd' || c == 'D' || c == 'f' || c == 'F')
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.NUMERIC);
            }
            else
            {
                AddToken(TokenType.NUMERIC);
                currentIndex--;
                state = State.START;
            }
        }

        private void SingleZeroState(char c)
        {
            if (c == '.')
            {
                AddcharacterToBuffer(c, State.FLOATING_POINT_NUMBER);
            }
            else if (c == 'x' || c == 'X')
            {
                AddcharacterToBuffer(c, State.HEX_NUMBER);
            }
            else if (c >= '0' && c <= '7' || c == '_')
            {
                AddcharacterToBuffer(c, State.OCTAL_NUMBER);
            }
            else if (c == 'b' || c == 'B')
            {
                AddcharacterToBuffer(c, State.BINARY_NUMBER);
            }
            else
            {//0
                AddToken(TokenType.NUMERIC);
                currentIndex--;
                state = State.START;
            }
        }

        private void OctalNumberState(char c)
        {
            if (c >= '0' && c <= '7' || c == '_')
            {
                AddcharacterToBuffer(c);
            }
            else if (c == 'l' || c == 'L')
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.NUMERIC);
            }
            else
            {
                AddToken(TokenType.NUMERIC);
                currentIndex--;
                state = State.START;
            }
        }

        private void HexNumberState(char c)
        {
            if (char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || c == '_')
            {
                AddcharacterToBuffer(c);
            }
            else
            {
                AddToken(TokenType.NUMERIC);
                currentIndex--;
                state = State.START;
            }
        }

        private void FloatingPointNumberState(char c)
        {
            if (char.IsDigit(c) || c == '_' || c == 'e' || c == 'E')
            {
                AddcharacterToBuffer(c);
            }
            else if (c == 'd' || c == 'D' || c == 'f' || c == 'F' || c == 'm' || c == 'M')
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.NUMERIC);
            }
            else
            {
                AddToken(TokenType.NUMERIC);
                currentIndex--;
                state = State.START;
            }
        }

        private void BinaryNumberState(char c)
        {
            if (c == '0' || c == '1' || c == '_')
            {
                AddcharacterToBuffer(c);
            }
            else
            {
                AddToken(TokenType.NUMERIC);
                currentIndex--;
                state = State.START;
            }
        }

        //' in buffer
        private void SymbolicConstantState(char c)
        {
            if (c == '\\')
            {//'\
                AddcharacterToBuffer(c, State.BACKSLASH_IN_SYMBOLIC_CONSTANT);
            }
            else if (c == '\n')
            {
                AddToken(TokenType.ERROR);
                AddToken(TokenType.WHITE_SPACE, c);
                state = State.START;
            }
            else
            {
                AddcharacterToBuffer(c, State.END_OF_SYMBOLIC_CONSTANT);
            }
        }

        //'\ in buffer
        private void BackslashInSymbolicConstantState(char c)
        {
            if (c >= '0' && c <= '7')
            {
                AddcharacterToBuffer(c, State.ONE_OCTAL_DIGIT_AFTER_BACKSLASH_IN_SYMBOLIC_CONSTANT);
            }
            else if (SymbolType.IsEscapeSequence(c))
            {
                AddcharacterToBuffer(c, State.END_OF_SYMBOLIC_CONSTANT);
            }
            else
            {
                AddToken(TokenType.ERROR);
                currentIndex--;
                state = State.START;
            }
        }

        private void OneOctalDigitAfterBackslashInSymbolicConstantState(char c)
        {
            if (c >= '0' && c <= '7')
            {
                AddcharacterToBuffer(c, State.TWO_OCTAL_DIGIT_AFTER_BACKSLASH_IN_SYMBOLIC_CONSTANT);
            }
            else
            {
                currentIndex--;
                state = State.END_OF_SYMBOLIC_CONSTANT;
            }
        }

        private void TwoOctalDigitAfterBackslashInSymbolicConstantState(char c)
        {
            if (c >= '0' && c <= '7')
            {
                AddcharacterToBuffer(c, State.END_OF_SYMBOLIC_CONSTANT);
            }
            else
            {
                currentIndex--;
                state = State.END_OF_SYMBOLIC_CONSTANT;
            }
        }

        private void EndOfSymbolicConstantState(char c)
        {
            if (c == '\'')
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.SYMBOLIC);
            }
            else if (c == '\n')
            {
                AddToken(TokenType.ERROR);
                AddToken(TokenType.WHITE_SPACE, c);
                state = State.START;
            }
            else
            {
                AddcharacterToBuffer(c, State.ERROR_READ_SYMBOLIC_CONSTANT);
            }
        }

        private void ErrorReadSymbolicConstantState(char c)
        {
            if (c == '\n')
            {
                AddToken(TokenType.ERROR);
                AddToken(TokenType.WHITE_SPACE, c);
                state = State.START;
            }
            else if (c == '\'')
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.ERROR);
            }
            else if (c == '\\')
            {
                AddcharacterToBuffer(c, State.BACKSLASH_INSIDE_ERROR_READ_SYMBOLIC_CONSTANT);
            }
            else
            {
                AddcharacterToBuffer(c);
            }
        }

        private void BackslashInsideErrorReadSymbolicConstantState(char c)
        {
            AddcharacterToBuffer(c, State.ERROR_READ_SYMBOLIC_CONSTANT);
        }

        private void LiteralConstantState(char c)
        {
            if (c == '\\')
            {
                AddcharacterToBuffer(c, State.BACKSLASH_IN_LITERAL_CONSTANT);
            }
            else if (c == '\"')
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.LITERAL);
            }
            else if (c == '\n')
            {
                AddToken(TokenType.ERROR);
                AddToken(TokenType.WHITE_SPACE, c);
                state = State.START;
            }
            else
            {
                AddcharacterToBuffer(c);
            }
        }

        //"string\ in buffer
        private void BackslashInLiteralConstantState(char c)
        {
            if (SymbolType.IsEscapeSequence(c) || (c >= '0' && c <= '7'))
            {
                AddcharacterToBuffer(c, State.LITERAL_CONSTANT);
            }
            else
            {
                AddcharacterToBuffer(c, State.ERROR_READ_LITERAL_CONSTANT);
            }
        }

        private void ErrorReadLiteralConstantState(char c)
        {
            if (c == '\n')
            {
                AddToken(TokenType.ERROR);
                AddToken(TokenType.WHITE_SPACE, c);
                state = State.START;
            }
            else if (c == '\"')
            {
                AddcharacterToBuffer(c, State.START);
                AddToken(TokenType.ERROR);
            }
            else
            {
                AddcharacterToBuffer(c);
            }
        }
    }
}
