using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;

namespace CSharpToJavaTranslator
{
    public struct Token
    {
        public string value;
        public Constants.TokenType type;
        public int numberLine;
        public int numberColumn;

        public Token(Constants.TokenType type, string value)
        {
            this.value = value;
            this.type = type;
            this.numberLine = 0;
            this.numberColumn = 0;
        }

        public Token(string text, Constants.TokenType type, int numberLine, int numberColumn)
        {
            this.value = text;
            this.type = type;
            this.numberLine = numberLine;
            this.numberColumn = numberColumn;
        }

        public void Print()
        {
            Console.WriteLine(value + ' ' + type + ' ' + numberLine + ' ' + numberColumn);
        }

        public string GetToken()
        {
            string st = this.value + ' ' + type + ' ' + 
                        this.numberLine.ToString() + ' ' + this.numberColumn.ToString();
            return st;
        }
    }

    internal class LexicalAnalyzer
    {
        private char[] separators = { ';', '{', '}', '(', ')', 
                                      '[', ']', ':', ',', '+', 
                                      '-', '*', '/', ' ', '=', '"', '\''};

        //private char[] ecro = {'\'', '"', };

        private List<Token> tokens = new List<Token>();

        private string[] contextKeywords = { "add", "and", "alias", "ascending", 
                                             "args", "async", "await", "by",
                                             "descending", "dynamic", "equals", "from", 
                                             "get", "global", "group", "init", 
                                             "into", "join", "let", "managed", "nameof",
                                             "not", "notnull", "on", 
                                             "orderby", "partial", "record", "remove", 
                                             "select", "set", "unmanaged", "value", 
                                             "var", "when", "where", "with", "yield" };

        CustomRichTextBox console;
        TranslationResultBus translatorOutputBus;
        public LexicalAnalyzer(CustomRichTextBox console,
                               TranslationResultBus translatorOutputBus)
        {
            this.console = console;
            this.translatorOutputBus = translatorOutputBus;
        }

        public void PrintToken()
        {
            foreach (Token token in tokens)
                token.Print();
        }

        public void PrintTokenInFile(string filename)
        {
            //System.IO.StreamWriter fs = new StreamWriter(filename);
            //foreach (Token token in tokens)
            //    fs.WriteLine(token.GetToken());
            //fs.Close();
        }

        public List<Token> parse(CustomRichTextBox inputTextBox)
        {
            string[] str = inputTextBox.getInnerTextBox().Lines;
            int numberLine = 1;

            foreach (string line in str)
            {
                checkLine(ref tokens, line, numberLine);
                numberLine++;
            }

            return tokens;
        }

        private void checkLine(ref List<Token> tokens, string s, int numberLine)
        {
            int lexemBegin = -1;
            int lexemEnd = -1;
            int numberColumn = 1;
            bool inString = false;
            bool inChar = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (lexemBegin != -1 && lexemEnd == -1)
                {
                    if (inString)
                    {
                        if ((s[i - 1] != '\\' && s[i] == '"') || s.Length - 1 == i)
                        {
                            if (i - lexemBegin - 1 > 0)
                            {
                                tokens.Add(new Token(s.Substring(lexemBegin + 1, i - lexemBegin - 1), Constants.TokenType.STRING, numberLine, lexemBegin+1));
                                numberColumn++;
                            }

                            tokens.Add(new Token(s[i].ToString(), Constants.TokenType.DOUBLE_QUOTATION_MARK, numberLine, i));
                            numberColumn++;
                            lexemEnd = i;
                            inString = false;
                        }
                    }
                    else if (inChar)
                    {
                        if (s[i] == '\'' || i == s.Length - 1)
                        {
                            if (!(s[i - 2] != '\\' && (s[i - 1] == '\\')))
                            {
                                int charLen = 0;
                                if (s[i] == '\'')
                                {
                                    charLen = i - lexemBegin - 1;
                                }
                                else
                                {
                                    charLen = i - lexemBegin;
                                }
                                if (charLen == 1 || (charLen == 2 && s[lexemBegin+1] == '\\'))
                                {
                                    tokens.Add(new Token(s.Substring(lexemBegin + 1, i - lexemBegin - 1), Constants.TokenType.CHAR, numberLine, lexemBegin + 1));
                                    if (s[i] == '\'')
                                        tokens.Add(new Token(s[i].ToString(), Constants.TokenType.QUOTATION_MARK, numberLine, i));
                                }
                                else
                                {
                                    if (charLen == 0)
                                    {
                                        //Console.WriteLine("Предупреждение, char пустой: " + "Строка:" + numberLine + "  Столбец:" + numberColumn);

                                        console.appendText("[LEX][WARNING] : пустая символьная лексема. Строка: " + numberLine + ", столбец: " + numberColumn + ".\n", Color.Red);
                                    }
                                    else
                                    {
                                        if (s[i] == '\'')
                                        {
                                            //Console.WriteLine("Ошибка, избыток символов в char: " + s.Substring(lexemBegin + 1, i - lexemBegin - 1) + ". Символы были удалены. Строка:" + numberLine + "  Столбец:" + numberColumn);

                                            console.appendText("[LEX][ERROR] : избыток символов в символьной лексеме: " + s.Substring(lexemBegin + 1, i - lexemBegin - 1) + ". Символы были удалены. Строка: " + numberLine + ", столбец: " + numberColumn + ".\n", Color.Red);
                                        }
                                        else
                                        {
                                            //Console.WriteLine("Ошибка, избыток символов в char: " + s.Substring(lexemBegin + 1, i - lexemBegin) + ". Символы были удалены. Строка:" + numberLine + "  Столбец:" + numberColumn);
                                            console.appendText("[LEX][ERROR] : избыток символов в символьной лексеме: " + s.Substring(lexemBegin + 1, i - lexemBegin) + ". Символы были удалены. Строка: " + numberLine + ", столбец:" + numberColumn + ".\n", Color.Red);
                                        }
                                            
                                    }
                                }

                                lexemEnd = i;
                                inChar = false;
                            }
                        }
                    }
                    else if (i < s.Length - 1 && separators.Contains(s[i + 1]) || i == s.Length - 1)
                    {
                        //TODO: убрать эту переменную или как-то переименовать
                        char zad = s[i];

                        string text = s.Substring(lexemBegin, i - lexemBegin + 1);
                        string[] str = { text };
                        str = text.Split('.');
                        int colum = lexemBegin;
                        for (int j = 0; j < str.Length; j++)
                        {
                            sepIndifity(ref tokens, str[j], numberLine, ref colum);
                            colum = lexemBegin + str[j].Length;
                            if (str.Length > 1 && j < str.Length - 1)
                            {
                                tokens.Add(new Token(".", Constants.TokenType.DOT, numberLine, lexemBegin));
                                numberColumn++;
                                colum++;
                            }
                        }

                        lexemEnd = i;
                    }
                }

                if (lexemBegin == -1)
                {
                    switch (s[i])
                    {
                        case ' ':
                            break;
                        case '+':
                            if (i < s.Length - 1 && s[i + 1] == '+')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.INCREMENT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.PLUS_ASSIGNMENT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.PLUS, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case '-':
                            if (i < s.Length - 1 && s[i + 1] == '-')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.DECREMENT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.MINUS_ASSIGNMENT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.MINUS, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case '&':
                            if (i < s.Length - 1 && s[i + 1] == '&')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.AND, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.AND_ASSIGNMENT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.BIT_AND, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case '|':
                            if (i < s.Length - 1 && s[i + 1] == '|')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.OR, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.OR_ASSIGNMENT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.BIT_OR, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case '=':
                            if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.EQUAL, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.ASSIGNMENT, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case '*':
                            if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.MULTIPLICATION_ASSIGNMENT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.MULTIPLICATION, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case '!':
                            if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.NOT_EQUAL, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.NOT, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case '>':
                            if (i < s.Length - 3 && s[i + 1] == '>' && s[i + 2] == '>' && s[i + 3] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 4), Constants.TokenType.SHIFT_TO_RIGHT_ASSIGNMENT, numberLine, i));
                                i += 4;
                                numberColumn++;
                            }
                            else if (i < s.Length - 2 && s[i + 1] == '>' && s[i + 2] == '>')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.BIT_SHIFT_TO_RIGHT, numberLine, i));
                                i += 2;
                                numberColumn++;
                            }
                            else if (i < s.Length - 2 && s[i + 1] == '>' && s[i + 2] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.ARITHMETIC_SHIFT_TO_RIGHT_ASSIGNMENT, numberLine, i));
                                i += 2;
                                numberColumn++;
                            }
                            else if (i < s.Length - 1 && s[i + 1] == '>')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.BIT_ARITHMETIC_SHIFT_TO_RIGHT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.GREATER_OR_EQUAL, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.GREATER, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case '<':
                            if (i < s.Length - 2 && s[i + 1] == '<' && s[i + 2] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.ARITHMETIC_SHIFT_TO_LEFT_ASSIGNMENT, numberLine, i));
                                i++;
                                i++;
                                numberColumn++;
                            }
                            else
                            if (i < s.Length - 1 && s[i + 1] == '<')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.BIT_ARITHMETIC_SHIFT_TO_LEFT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.LESS_OR_EQUAL, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.LESS, numberLine, i));
                                numberColumn++;
                            }
                            break;

                        case '~':
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.BIT_TILDA, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case '^':
                            if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.EXCLUSIVE_OR_ASSIGNMENT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.BIT_EXCLUSIVE_OR, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case '/':
                            if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.DIVISION_ASSIGNMENT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.DIVISION, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case '%':
                            if (i < s.Length - 1 && s[i + 1] == '=')
                            {
                                tokens.Add(new Token(s.Substring(i, 2), Constants.TokenType.MODULO_ASSIGNMENT, numberLine, i));
                                i++;
                                numberColumn++;
                            }
                            else
                            {
                                tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.MODULO, numberLine, i));
                                numberColumn++;
                            }
                            break;
                        case ';':
                            tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.SEMICOLON, numberLine, i));
                            numberColumn++;
                            break;
                        case '{':
                            tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.OPENING_CURLY_BRACKET, numberLine, i));
                            numberColumn++;
                            break;
                        case '}':
                            tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.CLOSING_CURLY_BRACKET, numberLine, i));
                            numberColumn++;
                            break;
                        case '(':
                            tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.OPENING_BRACKET, numberLine, i));
                            numberColumn++;
                            break;
                        case ')':
                            tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.CLOSING_BRACKET, numberLine, i));
                            numberColumn++;
                            break;
                        case ',':
                            tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.COMMA, numberLine, i));
                            numberColumn++;
                            break;
                        case '[':
                            tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.OPENING_SQUARE_BRACKET, numberLine, i));
                            numberColumn++;
                            break;
                        case ']':
                            tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.CLOSING_SQUARE_BRACKET, numberLine, i));
                            numberColumn++;
                            break;
                        case '.':
                            tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.DOT, numberLine, i));
                            numberColumn++;
                            break;
                        case ':':
                            tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.COLON, numberLine, i));
                            numberColumn++;
                            break;
                        case '?':
                            tokens.Add(new Token(s.Substring(i, 1), Constants.TokenType.QUESTION_MARK, numberLine, i));
                            numberColumn++;
                            break;
                        case '"':
                            tokens.Add(new Token(s[i].ToString(), Constants.TokenType.DOUBLE_QUOTATION_MARK, numberLine, i));
                            inString = true;
                            lexemBegin = i;
                            numberColumn++;
                            break;
                        case '\'':
                            tokens.Add(new Token(s[i].ToString(), Constants.TokenType.QUOTATION_MARK, numberLine, i));
                            inChar = true;
                            lexemBegin = i;
                            numberColumn++;
                            break;
                        default:
                            lexemBegin = i;
                            i--;
                            break;
                    }
                }
                if (lexemBegin != -1 && lexemEnd != -1)
                {
                    lexemBegin = -1;
                    lexemEnd = -1;
                }
            }
        }

        private void sepIndifity(ref List<Token> tokens, string s, int numberLine, ref int numberColumn)
        {
            if (Regex.IsMatch(s, @"^\d+$"))
            {
                tokens.Add(new Token(s, Constants.TokenType.INT_NUMBER, numberLine, numberColumn));
            }
            else if (Regex.IsMatch(s, @"^\d+.\d+$"))
            {
                tokens.Add(new Token(s, Constants.TokenType.REAL_NUMBER, numberLine, numberColumn));
            }
            else if (s == "null")
            {
                tokens.Add(new Token(s, Constants.TokenType.NULL, numberLine, numberColumn));
            }
            else if (s == "switch")
            {
                tokens.Add(new Token(s, Constants.TokenType.SWITCH, numberLine, numberColumn));
            }
            else if (s == "case")
            {
                tokens.Add(new Token(s, Constants.TokenType.CASE, numberLine, numberColumn));
            }
            else if (s == "break")
            {
                tokens.Add(new Token(s, Constants.TokenType.BREAK, numberLine, numberColumn));
            }
            else if (s == "continue")
            {
                tokens.Add(new Token(s, Constants.TokenType.CONTINUE, numberLine, numberColumn));
            }
            else if (s == "default")
            {
                tokens.Add(new Token(s, Constants.TokenType.DEFAULT, numberLine, numberColumn));
            }
            else if (s == "new")
            {
                tokens.Add(new Token(s, Constants.TokenType.NEW, numberLine, numberColumn));
            }
            else if (s == "return")
            {
                tokens.Add(new Token(s, Constants.TokenType.RETURN, numberLine, numberColumn));
            }
            else if (s == "using")
            {
                tokens.Add(new Token(s, Constants.TokenType.USING, numberLine, numberColumn));
            }
            else if (s == "namespace")
            {
                tokens.Add(new Token(s, Constants.TokenType.NAMESPACE, numberLine, numberColumn));
            }
            else if (s == "class")
            {
                tokens.Add(new Token(s, Constants.TokenType.CLASS, numberLine, numberColumn));
            }
            else if (s == "struct")
            {
                tokens.Add(new Token(s, Constants.TokenType.STRUCT, numberLine, numberColumn));
            }
            else if (s == "enum")
            {
                tokens.Add(new Token(s, Constants.TokenType.ENUM, numberLine, numberColumn));
            }
            else if (s == "if")
            {
                tokens.Add(new Token(s, Constants.TokenType.IF, numberLine, numberColumn));
            }
            else if (s == "else")
            {
                tokens.Add(new Token(s, Constants.TokenType.ELSE, numberLine, numberColumn));
            }
            else if (s == "while")
            {
                tokens.Add(new Token(s, Constants.TokenType.WHILE, numberLine, numberColumn));
            }
            else if (s == "do")
            {
                tokens.Add(new Token(s, Constants.TokenType.DO, numberLine, numberColumn));
            }
            else if (s == "for")
            {
                tokens.Add(new Token(s, Constants.TokenType.FOR, numberLine, numberColumn));
            }
            else if (s == "foreach")
            {
                tokens.Add(new Token(s, Constants.TokenType.FOREACH, numberLine, numberColumn));
            }
            else if (s == "public")
            {
                tokens.Add(new Token(s, Constants.TokenType.PUBLIC, numberLine, numberColumn));
            }
            else if (s == "protected")
            {
                tokens.Add(new Token(s, Constants.TokenType.PROTECTED, numberLine, numberColumn));
            }
            else if (s == "private")
            {
                tokens.Add(new Token(s, Constants.TokenType.PRIVATE, numberLine, numberColumn));
            }
            else if (s == "internal")
            {
                tokens.Add(new Token(s, Constants.TokenType.INTERNAL, numberLine, numberColumn));
            }
            else if (s == "const")
            {
                tokens.Add(new Token(s, Constants.TokenType.CONST, numberLine, numberColumn));
            }
            else if (s == "static")
            {
                tokens.Add(new Token(s, Constants.TokenType.STATIC, numberLine, numberColumn));
            }
            else if (s == "this")
            {
                tokens.Add(new Token(s, Constants.TokenType.THIS, numberLine, numberColumn));
            }
            else if (s == "true")
            {
                tokens.Add(new Token(s, Constants.TokenType.TRUE, numberLine, numberColumn));
            }
            else if (s == "false")
            {
                tokens.Add(new Token(s, Constants.TokenType.FALSE, numberLine, numberColumn));
            }
            else if (s == "const")
            {
                tokens.Add(new Token(s, Constants.TokenType.CONST, numberLine, numberColumn));
            }
            else if (s == "in")
            {
                tokens.Add(new Token(s, Constants.TokenType.IN, numberLine, numberColumn));
            }

            //Небольшая доработка.
            else if (s == "abstract" || s == "base" || s == "interface" || 
                     s == "override" || s == "sealed" || s == "virtual")
            {
                console.appendText("[LEX][ERROR] : наследование не поддерживается данной грамматикой. Строка: " 
                                  + numberLine + ", столбец: " + numberColumn + ".\n", Color.Red);
            }
            else if (s == "try" || s == "throw" || s == "catch" || s == "finally")
            {
                console.appendText("[LEX][ERROR] : исключения не поддерживаются данной грамматикой. Строка: "
                                  + numberLine + ", столбец: " + numberColumn + ".\n", Color.Red);
            }
            else if (s == "operator")
            {
                console.appendText("[LEX][ERROR] : перегрузка операторов не поддерживается данной грамматикой. Строка: "
                                  + numberLine + ", столбец: " + numberColumn + ".\n", Color.Red);
            }
            else if (s == "extern")
            {
                console.appendText("[LEX][ERROR] : межфайловые объекты и функции не поддерживаются данной грамматикой. Строка: "
                                  + numberLine + ", столбец: " + numberColumn + ".\n", Color.Red);
            }
            else if (s == "out" || s == "ref")
            {
                console.appendText("[LEX][ERROR] : ссылки не поддерживаются данной грамматикой. Строка: "
                                  + numberLine + ", столбец: " + numberColumn + ".\n", Color.Red);
            }
            else if (s == "params")
            {
                console.appendText("[LEX][ERROR] : вариационные функции не поддерживаются данной грамматикой. Строка: "
                                  + numberLine + ", столбец: " + numberColumn + ".\n", Color.Red);
            }
            else if (s == "as" || s == "is" || s == "checked" || 
                     s == "delegate" || s == "event" || s == "explicit" || s == "fixed" ||
                     s == "goto" || s == "implicit" || s == "lock" || s == "readonly" ||
                     s == "sizeof" || s == "stackalloc" || s == "typeof" || s == "unchecked" ||
                     s == "unsafe" || s == "volatile")
            {
                console.appendText("[LEX][ERROR] : ключевое слово \"" + s + "\" не поддерживается данной грамматикой. Строка: "
                                  + numberLine + ", столбец: " + numberColumn + ".\n", Color.Red);
            }
            else if (this.contextKeywords.Contains(s))
            {
                tokens.Add(new Token(s, Constants.TokenType.IDENTIFIER, numberLine, numberColumn));
                translatorOutputBus.registerWarning("[LEX][WARNING] : лексема \"" + s + "\" является контекстным ключевым словом" +
                                                    " в языке C#, но далее будет рассматриваться как идентификатор.", 
                                                    tokens.Last());
            }
            else if (s == "nuint" || s == "nint" || s == "uint")
            {
                tokens.Add(new Token(s, Constants.TokenType.IDENTIFIER, numberLine, numberColumn));
                translatorOutputBus.registerWarning("[LEX][WARNING] : лексема \"" + s + "\" будет заменена на \"int\".", 
                                                    tokens.Last());
            }
            else if (s == "ulong")
            {
                tokens.Add(new Token(s, Constants.TokenType.IDENTIFIER, numberLine, numberColumn));
                translatorOutputBus.registerWarning("[LEX][WARNING] : лексема \"" + s + "\" заменена на \"long\".",
                                                    tokens.Last());
            }

            else if (Regex.IsMatch(s, @"^[_a-zA-Z]\S*$"))
            {
                tokens.Add(new Token(s, Constants.TokenType.IDENTIFIER, numberLine, numberColumn));
            }
            else
            {
                console.appendText("[LEX][ERROR] : обнаружена неизвестная лексема \"" + s + "\", лексема удалена из выходного потока. Строка: " 
                                  + numberLine + ", столбец: " + numberColumn + ".\n", Color.Red);
            }
            numberColumn++;
        }
    }
}