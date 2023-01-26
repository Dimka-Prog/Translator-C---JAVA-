using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpToJavaTranslator
{
    public class SyntaxAnalyzer
    {
        /// <summary>
        /// Этот метод выполняет парсинг области
        /// подключения пространств имён. Согласно грамматике C#,
        /// эти конструкции могут находиться только в начале текста
        /// программы.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseHeadOfProgram(ref Token[] tokens)
        {
            state = Constants.State.EXPECTING_USING_OR_NAMESPACE;

            while (position < tokens.Length)
            {
                if (state == Constants.State.EXPECTING_USING_OR_NAMESPACE)
                {
                    if (tokens[position].type == Constants.TokenType.USING)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"using\", ожидается имя пространства имён...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.USING);
                        state = Constants.State.EXPECTING_IDENTIFIER;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.NAMESPACE)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"namespace\", ожидается имя пространства имён...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.NAMESPACE);
                        state = Constants.State.EXPECTING_IDENTIFIER;
                        Console.WriteLine("[SYNTAX][INFO] : парсинг области подключения пространств имён завершён.");
                        position++;
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[2] { "using", "namespace" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_IDENTIFIER)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено имя пространства имён, ожидается \".\" или \";\"...");
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_DOT_OR_SEMICOLON;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "имя пространства имён" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_DOT_OR_SEMICOLON)
                {
                    if (tokens[position].type == Constants.TokenType.DOT)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \".\", ожидается имя пространства имён...");
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_IDENTIFIER;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.SEMICOLON)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \";\", ожидается \"using\" или \"namespace\"...");
                        syntaxTree.goToParent();
                        state = Constants.State.EXPECTING_USING_OR_NAMESPACE;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { ";" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция подключения пространств имён.", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг главного пространства имён.
        /// Используется упрощённая грамматика, поэтому внутри пространства имён
        /// может быть только один главный класс программы.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseNamespace(ref Token[] tokens)
        {
            string currentNamespaceName = "";

            while (position < tokens.Length)
            {
                if (state == Constants.State.EXPECTING_IDENTIFIER)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        currentNamespaceName = tokens[position].value;
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено имя пространства имён \"" + tokens[position].value + "\", ожидается \"{\"...");
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_OPENING_CURLY_BRACKET;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "имя пространства имён" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_OPENING_CURLY_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"{\", переход к парсингу содержимого пространства имён...");
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "{" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.PUBLIC ||
                        tokens[position].type == Constants.TokenType.PROTECTED ||
                        tokens[position].type == Constants.TokenType.PRIVATE ||
                        tokens[position].type == Constants.TokenType.INTERNAL)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"" + tokens[position].value
                                          + "\", ожидаются \"static\" или \"class\"...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.MEMBER);
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_STATIC_OR_CLASS;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.STATIC)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"static\", ожидается \"class\"...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.MEMBER);
                        syntaxTree.appendToken(new Token(Constants.TokenType.PRIVATE, "private"));
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_CLASS;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.CLASS)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"class\", ожидается имя класса...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.MEMBER);
                        syntaxTree.appendToken(new Token(Constants.TokenType.PRIVATE, "private"));
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.CLASS);
                        position++;
                        parseClassOrStruct(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                    }
                    else if (tokens[position].type == Constants.TokenType.CLOSING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"}\", парсинг пространства имён \"" + currentNamespaceName + "\" завершён.");
                        position++;
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[7] { "public", "protected", "private", "internal", "static", "class", "}" },
                                                     tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_STATIC_OR_CLASS)
                {
                    if (tokens[position].type == Constants.TokenType.STATIC)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"static\", ожидается \"class\"...");
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_CLASS;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.CLASS)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"class\", ожидается имя класса...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.CLASS);
                        position++;
                        parseClassOrStruct(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        syntaxTree.goToParent();
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[2] { "static", "class" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CLASS)
                {
                    if (tokens[position].type == Constants.TokenType.CLASS)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"class\", ожидается имя класса...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.CLASS);
                        position++;
                        parseClassOrStruct(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        syntaxTree.goToParent();
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "class" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция пространства имён \"" + currentNamespaceName + "\".", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг класса или структуры.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseClassOrStruct(ref Token[] tokens)
        {
            string currentClassOrStructName = "";
            state = Constants.State.EXPECTING_IDENTIFIER;

            while (position < tokens.Length)
            {
                if (state == Constants.State.EXPECTING_IDENTIFIER)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        currentClassOrStructName = tokens[position].value;
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено имя класса: " + tokens[position].value + ", ожидается \";\"...");
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_OPENING_CURLY_BRACKET;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "имя класса" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_OPENING_CURLY_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"{\", переход к парсингу содержимого класса...");
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "{" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET)
                {
                    //Внутри класса можно встретить классы, структуры, перечисления и методы.
                    if (tokens[position].type == Constants.TokenType.PUBLIC ||
                        tokens[position].type == Constants.TokenType.PROTECTED ||
                        tokens[position].type == Constants.TokenType.PRIVATE ||
                        tokens[position].type == Constants.TokenType.INTERNAL)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"" + tokens[position].value + "\", ожидаются \"static\", \"const\", \"class\", \"struct\", \"enum\" или тип данных...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.MEMBER);
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_STATIC_OR_CONST_OR_CLASS_OR_STRUCT_OR_ENUM_OR_DATA_TYPE;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.STATIC)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"static\", ожидается \"class\" или тип данных...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.MEMBER);
                        syntaxTree.appendToken(new Token(Constants.TokenType.PRIVATE, "private"));
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_CLASS_OR_DATA_TYPE;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.CONST)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"const\", переход к парсингу константы...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.MEMBER);
                        syntaxTree.appendToken(new Token(Constants.TokenType.PRIVATE, "private"));
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_DATA_TYPE;
                        position++;
                        parseFieldOrDeclaration(ref tokens, false); position++;
                        syntaxTree.goToParent();
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                    }
                    else if (tokens[position].type == Constants.TokenType.CLASS)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"class\", переход к парсингу класса...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.MEMBER);
                        syntaxTree.appendToken(new Token(Constants.TokenType.PRIVATE, "private"));
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.CLASS);
                        position++;
                        parseClassOrStruct(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                    }
                    else if (tokens[position].type == Constants.TokenType.STRUCT)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"struct\", переход к парсингу структуры...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.MEMBER);
                        syntaxTree.appendToken(new Token(Constants.TokenType.PRIVATE, "private"));
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.STRUCT);
                        position++;
                        parseClassOrStruct(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                    }
                    else if (tokens[position].type == Constants.TokenType.ENUM)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"enum\", переход к парсингу перечисления...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.MEMBER);
                        syntaxTree.appendToken(new Token(Constants.TokenType.PRIVATE, "private"));
                        position++;
                        parseEnum(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                    }
                    else if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружен тип данных \"" + tokens[position].value + "\", переход к парсингу члена структуры/класса...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.MEMBER);
                        syntaxTree.appendToken(new Token(Constants.TokenType.PRIVATE, "private"));
                        parseFieldOrMethod(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        //Костыль.
                        if (syntaxTree.getCurrentNodeType() != Constants.TreeNodeType.CLASS &&
                            syntaxTree.getCurrentNodeType() != Constants.TreeNodeType.STRUCT)
                        {
                            syntaxTree.goToParent();
                        }
                    }
                    else if (tokens[position].type == Constants.TokenType.CLOSING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"}\", парсинг класса/структуры \"" + currentClassOrStructName + "\" завершён.");
                        syntaxTree.goToParent();
                        syntaxTree.goToParent();
                        position++;
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[11] { "public", "protected", "private",
                                                                          "internal", "static", "const", "class",
                                                                          "struct", "enum", "идентификатор", "}" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_STATIC_OR_CONST_OR_CLASS_OR_STRUCT_OR_ENUM_OR_DATA_TYPE)
                {
                    //Ожидается идентификатор поля.
                    if (tokens[position].type == Constants.TokenType.STATIC)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"static\", ожидается \"class\" или тип данных...");
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_CLASS_OR_DATA_TYPE;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.CONST)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"const\", переход к парсингу константы...");
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_DATA_TYPE;
                        position++;
                        parseFieldOrDeclaration(ref tokens, false); position++;
                        syntaxTree.goToParent();
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        syntaxTree.goToParent();
                    }
                    else if (tokens[position].type == Constants.TokenType.CLASS)
                    {
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.CLASS);
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"class\", переход к парсингу класса...");
                        position++;
                        parseClassOrStruct(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                    }
                    else if (tokens[position].type == Constants.TokenType.STRUCT)
                    {
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.STRUCT);
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"struct\", переход к парсингу структуры...");
                        position++;
                        parseClassOrStruct(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                    }
                    else if (tokens[position].type == Constants.TokenType.ENUM)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"enum\", переход к парсингу перечисления...");
                        position++;
                        parseEnum(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                    }
                    else if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружен тип данных \"" + tokens[position].value + "\", переход к парсингу члена структуры/класса...");
                        parseFieldOrMethod(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        syntaxTree.goToParent();
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[6] { "static", "const", "class",
                                                                          "struct", "enum", "идентификатор" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CLASS_OR_DATA_TYPE)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружен тип данных \"" + tokens[position].value + "\", переход к парсингу члена структуры/класса...");
                        parseFieldOrMethod(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        syntaxTree.goToParent();
                    }
                    else if (tokens[position].type == Constants.TokenType.CLASS)
                    {
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.CLASS);
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"class\", переход к парсингу класса...");
                        position++;
                        parseClassOrStruct(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[2] { "тип данных", "class" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_DATA_TYPE)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружен тип данных \"" + tokens[position].value + "\", переход к парсингу члена класса/структуры...");
                        parseFieldOrMethod(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        syntaxTree.goToParent();
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "тип данных" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция класса/структуры \"" + currentClassOrStructName + "\".", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг перечисления.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseEnum(ref Token[] tokens)
        {
            string currentEnumName = "";
            state = Constants.State.EXPECTING_NAME_OF_SELF;

            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.ENUM);

            while (position < tokens.Length)
            {
                if (state == Constants.State.EXPECTING_NAME_OF_SELF)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        currentEnumName = tokens[position].value;
                        syntaxTree.appendToken(tokens[position]);
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено имя перечисления \"" + tokens[position].value + "\", ожидается \"{\"...");
                        state = Constants.State.EXPECTING_OPENING_CURLY_BRACKET;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "имя перечисления" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_OPENING_CURLY_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"{\", переход к парсингу содержимого перечисления...");
                        state = Constants.State.EXPECTING_ENUM_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "{" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_ENUM_CONTENT_OR_CLOSING_CURLY_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено имя константы \"" + tokens[position].value + "\" в перечислении, ожидается \"=\", \",\" или \"}\"...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.FIELD);
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_COMMA_OR_ASSIGNMENT_OR_CLOSING_CURLY_BRACKET;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.CLOSING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"}\", парсинг перечисления \"" + currentEnumName + "\" завершён.");
                        position++;
                        syntaxTree.goToParent();
                        syntaxTree.goToParent();
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[2] { "имя константы", "}" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_COMMA_OR_ASSIGNMENT_OR_CLOSING_CURLY_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.ASSIGNMENT)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено \"=\", переход к парсингу выражения...");
                        position++;
                        parseExpression(ref tokens);
                        syntaxTree.goToParent();
                        //Метод parseExpression не знает, где закончится выражение.
                        //Он идёт вперёд до тех пор, пока не посчитает, что встреченная
                        //лексема означает конец выражения. Эту лексему он не обрабатывает,
                        //а просто возвращает управление сюда. Соответственно, эту лексему
                        //нужно обработать здесь. Поэтому position не увеличивается.
                        if (tokens[position].type == Constants.TokenType.CLOSING_CURLY_BRACKET)
                        {
                            position++;
                            syntaxTree.goToParent();
                            syntaxTree.goToParent();
                            return;
                        }

                        position++;
                        state = Constants.State.EXPECTING_IDENTIFIER;
                    }
                    else if (tokens[position].type == Constants.TokenType.COMMA)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \",\", ожидается имя константы...");
                        state = Constants.State.EXPECTING_IDENTIFIER;
                        position++;
                        syntaxTree.goToParent();
                    }
                    else if (tokens[position].type == Constants.TokenType.CLOSING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"}\", парсинг перечисления \"" + currentEnumName + "\" завершён.");
                        position++;
                        syntaxTree.goToParent();
                        syntaxTree.goToParent();
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[3] { "=", ",", "}" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_IDENTIFIER)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено имя константы \"" + tokens[position].value + "\", ожидается \"=\", \",\" или \"}\"...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.FIELD);
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_COMMA_OR_ASSIGNMENT_OR_CLOSING_CURLY_BRACKET;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "имя константы" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_COMMA_OR_CLOSING_CURLY_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.COMMA)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \",\", ожидается имя константы...");
                        state = Constants.State.EXPECTING_IDENTIFIER;
                        position++;
                        syntaxTree.goToParent();
                    }
                    else if (tokens[position].type == Constants.TokenType.CLOSING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"}\", парсинг перечисления \"" + currentEnumName + "\" завершён.");
                        position++;
                        syntaxTree.goToParent();
                        syntaxTree.goToParent();
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[2] { ",", "}" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция перечисления \"" + currentEnumName + "\".", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг членов классов/структур, не являющихся
        /// константами.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseFieldOrMethod(ref Token[] tokens)
        {
            bool isField = false;
            int tempPosition = position;
            while(tempPosition < tokens.Length)
            {
                if(tokens[tempPosition].type == Constants.TokenType.OPENING_CURLY_BRACKET)
                {
                    break;
                }
                else if(tokens[tempPosition].type == Constants.TokenType.SEMICOLON)
                {
                    isField = true;
                    break;
                }
                tempPosition++;
            }

            if(isField)
            {
                parseFieldOrDeclaration(ref tokens, true);
                if (syntaxTree.getCurrentNodeType() != Constants.TreeNodeType.CLASS &&
                    syntaxTree.getCurrentNodeType() != Constants.TreeNodeType.STRUCT)
                {
                    syntaxTree.goToParent();
                }

                while (position < tokens.Length)
                {
                    if(tokens[position].type == Constants.TokenType.SEMICOLON)
                    {
                        position++;
                        break;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "," }, tokens[position]);
                    }
                    position++;
                }
                return;
            }
            else
            {
                syntaxTree.appendAndGoToChild(Constants.TreeNodeType.METHOD);
                syntaxTree.appendToken(tokens[position]);
                position++;

                state = Constants.State.EXPECTING_OPENING_SQUARE_BRACKET_OR_IDENTIFIER;

                while (position < tokens.Length)
                {
                    if (state == Constants.State.EXPECTING_OPENING_SQUARE_BRACKET_OR_IDENTIFIER)
                    {
                        if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                        {
                            Console.WriteLine("[SYNTAX][INFO] : обнаружено имя метода \"" + tokens[position].value + "\", ожидается \"(\"...");
                            syntaxTree.appendToken(tokens[position]);
                            state = Constants.State.EXPECTING_OPENING_BRACKET;
                            position++;
                        }
                        else if (tokens[position].type == Constants.TokenType.OPENING_SQUARE_BRACKET)
                        {
                            Console.WriteLine("[SYNTAX][INFO] : обнаружена \"[\", ожидается \"]\"...");
                            syntaxTree.appendToken(tokens[position]);
                            state = Constants.State.EXPECTING_CLOSING_SQUARE_BRACKET;
                            position++;
                        }
                        else
                        {
                            translationResultBus.registerUnexpectedTokenError(new string[2] { "]", "имя метода" }, tokens[position]);
                            position++;
                        }
                    }
                    else if (state == Constants.State.EXPECTING_CLOSING_SQUARE_BRACKET)
                    {
                        if (tokens[position].type == Constants.TokenType.CLOSING_SQUARE_BRACKET)
                        {
                            Console.WriteLine("[SYNTAX][INFO] : обнаружена \"]\", ожидается имя метода...");
                            syntaxTree.appendToken(tokens[position]);
                            position++;
                            state = Constants.State.EXPECTING_IDENTIFIER;
                        }
                        else
                        {
                            translationResultBus.registerUnexpectedTokenError(new string[1] { "]" }, tokens[position]);
                            position++;
                        }
                    }
                    if (state == Constants.State.EXPECTING_IDENTIFIER)
                    {
                        if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                        {
                            Console.WriteLine("[SYNTAX][INFO] : обнаружено имя метода \"" + tokens[position].value + "\", ожидается \"(\"...");
                            syntaxTree.appendToken(tokens[position]);
                            state = Constants.State.EXPECTING_OPENING_BRACKET;
                            position++;
                        }
                        else
                        {
                            translationResultBus.registerUnexpectedTokenError(new string[1] { "имя метода" }, tokens[position]);
                            position++;
                        }
                    }
                    else if (state == Constants.State.EXPECTING_OPENING_BRACKET)
                    {
                        if (tokens[position].type == Constants.TokenType.OPENING_BRACKET)
                        {
                            Console.WriteLine("[SYNTAX][INFO] : обнаружена \"(\", переход к парсингу параметров и тела метода...");
                            position++;
                            state = Constants.State.EXPECTING_DATA_TYPE;
                            parseFunctionParametersList(ref tokens);
                            state = Constants.State.EXPECTING_OPENING_CURLY_BRACKET;
                            parseCodeBlock(ref tokens);
                            Console.WriteLine("[SYNTAX][INFO] : парсинг метода завершён.");
                            syntaxTree.goToParent();
                            return;
                        }
                        else
                        {
                            translationResultBus.registerUnexpectedTokenError(new string[1] { "(" }, tokens[position]);
                            position++;
                        }
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция члена класса/структуры.", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг списка параметров методов.
        /// Парсинг переходит сюда из метода parseFieldOrMethod, если находит
        /// "(". Когда управление передаётся сюда, "(" пропускается, и парсинг
        /// начинается со следующего символа после неё.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseFunctionParametersList(ref Token[] tokens)
        {
            int parametersCount = 0;

            while(position < tokens.Length)
            {
                if(state == Constants.State.EXPECTING_DATA_TYPE)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружен тип данных параметра \"" + tokens[position].value + "\", ожидается \"[\" " +
                                          "или имя параметра...");
                        parametersCount++;
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.PARAMETER);
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_OPENING_SQUARE_BRACKET_OR_IDENTIFIER;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.CLOSING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : парсинг параметров метода завершён.");
                        if (parametersCount > 0)
                        {
                            syntaxTree.goToParent();
                        }
                        position++;
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "тип данных" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_IDENTIFIER)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        syntaxTree.appendToken(tokens[position]);
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено имя параметра \"" + tokens[position].value + "\", ожидается \"=\", \",\" или \")\"...");
                        state = Constants.State.EXPECTING_COMMA_OR_ASSIGNMENT_OR_CLOSING_BRACKET;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "имя параметра" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_OPENING_SQUARE_BRACKET_OR_IDENTIFIER)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        syntaxTree.appendToken(tokens[position]);
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено имя параметра \"" + tokens[position].value + "\", ожидается \"=\", \",\" или \")\"...");
                        state = Constants.State.EXPECTING_COMMA_OR_ASSIGNMENT_OR_CLOSING_BRACKET;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.OPENING_SQUARE_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"[\", ожидается \"]\"...");
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_CLOSING_SQUARE_BRACKET;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[2] { "имя параметра", "[" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_COMMA_OR_ASSIGNMENT_OR_CLOSING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.COMMA)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \",\", ожидается тип параметра...");
                        syntaxTree.goToParent();
                        state = Constants.State.EXPECTING_DATA_TYPE;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.ASSIGNMENT)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено \"=\", переход к парсингу выражения...");
                        position++;
                        parseExpression(ref tokens);
                        state = Constants.State.EXPECTING_COMMA_OR_CLOSING_BRACKET;
                    }
                    else if (tokens[position].type == Constants.TokenType.CLOSING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : парсинг параметров метода завершён.");
                        if(parametersCount > 0)
                        {
                            syntaxTree.goToParent();
                        }
                        position++;
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[3] { ",", "=", ")" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CLOSING_SQUARE_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.CLOSING_SQUARE_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"]\", ожидается имя параметра...");
                        syntaxTree.appendToken(tokens[position]);
                        state = Constants.State.EXPECTING_IDENTIFIER;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "]" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_COMMA_OR_CLOSING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.COMMA)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \",\", ожидается тип параметра...");
                        syntaxTree.goToParent();
                        state = Constants.State.EXPECTING_DATA_TYPE;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.CLOSING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : парсинг параметров метода завершён.");
                        if (parametersCount > 0)
                        {
                            syntaxTree.goToParent();
                        }
                        position++;
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[2] { ",", ")" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция параметров метода.", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг блоков кода, ограниченных фигурными
        /// скобками (тела методов), или состоящих
        /// из одной строки (условных операторов и циклов).
        /// Допустимыми конструкциями в блоке являются:
        /// вызов функции, объявление переменной, new, ++, --,
        /// if, else, for, foreach, do, while, 
        /// break, continue, switch, case, return.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseCodeBlock(ref Token[] tokens)
        {
            bool multiline = false;
            
            while (position < tokens.Length)
            {
                if (state == Constants.State.EXPECTING_OPENING_CURLY_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"{\", переход к парсингу содержимого блока кода...");
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        position++;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "{" }, tokens[position]);
                        position++;
                    }
                    multiline = true;
                }
                else if (state == Constants.State.EXPECTING_CONTENT_OR_OPENING_CURLY_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"{\", переход к парсингу содержимого блока кода...");
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        position++;
                        multiline = true;
                    }
                    else
                    {
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                    }
                }
                else if (state == Constants.State.EXPECTING_SEMICOLON)
                {
                    if (tokens[position].type == Constants.TokenType.SEMICOLON)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \";\"...");
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        position++;
                        if (!multiline) return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { ";" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.CLOSING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"}\".");
                        position++;
                        return;
                    }
                    else if (tokens[position].type == Constants.TokenType.IDENTIFIER ||
                             tokens[position].type == Constants.TokenType.THIS)
                    {
                        if (isDeclarationAhead(ref tokens))
                        {
                            Console.WriteLine("[SYNTAX][INFO] : переход к парсингу объявления...");
                            parseFieldOrDeclaration(ref tokens, false);
                            //Костыль.
                            if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.DECLARATION)
                            {
                                syntaxTree.goToParent();
                            } 
                            state = Constants.State.EXPECTING_SEMICOLON;
                            Console.WriteLine("[SYNTAX][INFO] : парсинг объявления завершён.");
                        }
                        else
                        {
                            Console.WriteLine("[SYNTAX][INFO] : переход к парсингу выражения...");
                            parseExpression(ref tokens);
                            this.state = Constants.State.EXPECTING_SEMICOLON;
                            Console.WriteLine("[SYNTAX][INFO] : парсинг выражения завершён.");
                        }
                    }
                    else if (tokens[position].type == Constants.TokenType.IF)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"if\", переход к парсингу оператора...");
                        position++;
                        parseIfElse(ref tokens);
                        Console.WriteLine("[SYNTAX][INFO] : парсинг оператора \"if\" завершён.");
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        if (!multiline) return;
                    }
                    else if (tokens[position].type == Constants.TokenType.ELSE)
                    {
                        translationResultBus.registerError("[SYNTAX][ERROR] : обнаружен оператор \"else\" без соответствующего оператора \"if\".", tokens[position]);
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.WHILE)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"while\"," +
                                          " переход к парсингу оператора...");
                        position++;
                        parseWhile(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        if (!multiline) return;
                    }
                    else if (tokens[position].type == Constants.TokenType.DO)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"do\"," +
                                          " переход к парсингу оператора...");
                        position++;
                        parseDoWhile(ref tokens);
                        state = Constants.State.EXPECTING_SEMICOLON;
                    }
                    else if (tokens[position].type == Constants.TokenType.FOR)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"for\"," +
                                          " переход к парсингу оператора...");
                        position++;
                        parseFor(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        if (!multiline) return;
                    }
                    else if (tokens[position].type == Constants.TokenType.FOREACH)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"foreach\"," +
                                          " переход к парсингу оператора...");
                        position++;
                        parseForeach(ref tokens);
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        if (!multiline) return;
                    }
                    else if (tokens[position].type == Constants.TokenType.BREAK)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"break\".");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.BREAK);
                        syntaxTree.goToParent();
                        state = Constants.State.EXPECTING_SEMICOLON;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.CONTINUE)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"continue\".");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.CONTINUE);
                        syntaxTree.goToParent();
                        state = Constants.State.EXPECTING_SEMICOLON;
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.SWITCH)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"switch\", переход к парсингу оператора...");
                        position++;
                        state = Constants.State.EXPECTING_OPENING_BRACKET;
                        parseSwitch(ref tokens);
                        Console.WriteLine("[SYNTAX][INFO] : парсинг оператора \"switch\" завершён.");
                        state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        if (!multiline) return;
                    }
                    else if (tokens[position].type == Constants.TokenType.CASE)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"case\", переход к парсингу метки...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.CASE);
                        position++;
                        parseExpression(ref tokens);
                        if(tokens[position].type == Constants.TokenType.COLON)
                        {
                            Console.WriteLine("[SYNTAX][INFO] : обнаружено \":\", парсинг метки завершён.");
                            syntaxTree.goToParent();
                            position++;
                            state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        }
                        else
                        {
                            translationResultBus.registerUnexpectedTokenError(new string[1] { ":" }, tokens[position]);
                        }
                    }
                    else if (tokens[position].type == Constants.TokenType.DEFAULT)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"default\", переход к парсингу метки...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.DEFAULT);
                        position++;
                        if (tokens[position].type == Constants.TokenType.COLON)
                        {
                            Console.WriteLine("[SYNTAX][INFO] : обнаружено \":\", парсинг метки завершён.");
                            syntaxTree.goToParent();
                            position++;
                            state = Constants.State.EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET;
                        }
                        else
                        {
                            translationResultBus.registerUnexpectedTokenError(new string[1] { ":" }, tokens[position]);
                        }
                    }
                    else if (tokens[position].type == Constants.TokenType.INCREMENT ||
                             tokens[position].type == Constants.TokenType.DECREMENT)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружен \"" + 
                                          tokens[position].value + "\", переход к парсингу выражения...");
                        parseExpression(ref tokens);
                        state = Constants.State.EXPECTING_SEMICOLON;
                    }
                    else if (tokens[position].type == Constants.TokenType.RETURN)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"return\", переход к парсингу оператора...");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.RETURN);
                        position++;
                        if (position < tokens.Length - 1)
                        {
                            if (tokens[position].type != Constants.TokenType.SEMICOLON)
                            {
                                parseExpression(ref tokens);
                            }
                            syntaxTree.goToParent();
                        }
                        state = Constants.State.EXPECTING_SEMICOLON;
                        Console.WriteLine("[SYNTAX][INFO] : парсинг оператора \"return\" завершён.");
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[18] { "объявление", "присваивание", 
                                                                      "вызов метода", "if", "else", 
                                                                      "for", "foreach", "do", "while", 
                                                                      "break", "continue", "switch", 
                                                                      "case", "break", "continue", 
                                                                      "++", "--", "return"}, 
                                                     tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция.", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг полей классов и объявлений переменных
        /// в методах.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseFieldOrDeclaration(ref Token[] tokens, bool isField)
        {
            if(isField)
            {
                syntaxTree.appendAndGoToChild(Constants.TreeNodeType.FIELD);
            }
            else
            {
                syntaxTree.appendAndGoToChild(Constants.TreeNodeType.DECLARATION);
            }
            
            state = Constants.State.EXPECTING_DATA_TYPE;

            while (position < tokens.Length)
            {
                if (state == Constants.State.EXPECTING_DATA_TYPE)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружен тип данных, ожидается \".\" или имя типа данных...");
                        syntaxTree.appendToken(tokens[position]);
                        position++;
                        state = Constants.State.EXPECTING_DOT_OR_OPENING_SQUARE_BRACKET_OR_IDENTIFIER;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "тип данных" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_DOT_OR_OPENING_SQUARE_BRACKET_OR_IDENTIFIER)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_SQUARE_BRACKET)
                    {
                        syntaxTree.appendToken(tokens[position]);
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"[\", ожидается \"]\"...");
                        position++;
                        state = Constants.State.EXPECTING_CLOSING_SQUARE_BRACKET;
                    }
                    else if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружен тип данных...");
        
                        if (tokens[position - 1].type == Constants.TokenType.IDENTIFIER)
                        {
                            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.PARAMETER);
                            syntaxTree.appendToken(tokens[position]);
                            state = Constants.State.EXPECTING_ASSIGNMENT_OR_COMMA_OR_SEMICOLON_OR_IN;
                        }
                        else
                        {
                            syntaxTree.appendToken(tokens[position]);
                            state = Constants.State.EXPECTING_DOT_OR_OPENING_SQUARE_BRACKET_OR_IDENTIFIER;
                        }
                        position++;
                    }
                    else if (tokens[position].type == Constants.TokenType.DOT)
                    {
                        syntaxTree.appendToken(tokens[position]);
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \".\", ожидается имя типа данных...");
                        position++;
                        state = Constants.State.EXPECTING_DATA_TYPE;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[3] { "[", ".", "имя типа данных" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CLOSING_SQUARE_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.CLOSING_SQUARE_BRACKET)
                    {
                        syntaxTree.appendToken(tokens[position]);
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"]\", ожидается имя переменной...");
                        position++;
                        state = Constants.State.EXPECTING_IDENTIFIER;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "]" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_IDENTIFIER)
                {
                    if (tokens[position].type == Constants.TokenType.IDENTIFIER)
                    {
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.PARAMETER);
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено имя переменной...");
                        syntaxTree.appendToken(tokens[position]);
                        position++;
                        state = Constants.State.EXPECTING_ASSIGNMENT_OR_COMMA_OR_SEMICOLON_OR_IN;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "имя переменной" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_ASSIGNMENT_OR_COMMA_OR_SEMICOLON_OR_IN)
                {
                    if (tokens[position].type == Constants.TokenType.ASSIGNMENT)
                    {
                        position++;
                        parseExpression(ref tokens);
                        if(tokens[position].type == Constants.TokenType.COMMA)
                        {
                            syntaxTree.goToParent();
                        }
                        
                        state = Constants.State.EXPECTING_COMMA_OR_SEMICOLON;
                    }
                    else if (tokens[position].type == Constants.TokenType.COMMA)
                    {
                        syntaxTree.goToParent();
                        position++;
                        state = Constants.State.EXPECTING_IDENTIFIER;
                    }
                    else if (tokens[position].type == Constants.TokenType.SEMICOLON ||
                             tokens[position].type == Constants.TokenType.IN)
                    {
                        syntaxTree.goToParent();
                        Console.WriteLine("[SYNTAX][INFO] : парсинг объявления завершён, потому что встречено \"" + tokens[position].value + "\".");
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[4] { ",", ";", "in", "присваивание" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_COMMA_OR_SEMICOLON)
                {
                    if (tokens[position].type == Constants.TokenType.COMMA)
                    {
                        position++;
                        state = Constants.State.EXPECTING_IDENTIFIER;
                    }
                    else if (tokens[position].type == Constants.TokenType.SEMICOLON)
                    {
                        syntaxTree.goToParent();
                        //position++;
                        Console.WriteLine("[SYNTAX][INFO] : парсинг объявления завершён.");
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[2] { ",", ";" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция объявления.", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг оператора if, включая все 
        /// присоединённые к нему операторы else.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseIfElse(ref Token[] tokens)
        {
            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.IF);
            state = Constants.State.EXPECTING_OPENING_BRACKET;

            while (position < tokens.Length)
            {
                if(state == Constants.State.EXPECTING_OPENING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"(\", переход к парсингу выражения...");
                        position++;
                        parseExpression(ref tokens);
                        state = Constants.State.EXPECTING_CLOSING_BRACKET;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "(" }, tokens[position]);
                        position++;
                    }
                }
                if (state == Constants.State.EXPECTING_CLOSING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.CLOSING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \")\", переход к парсингу тела оператора...");
                        position++;
                        state = Constants.State.EXPECTING_CONTENT_OR_OPENING_CURLY_BRACKET;
                        parseCodeBlock(ref tokens);
                        
                        if(tokens[position].type == Constants.TokenType.ELSE)
                        {
                            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.ELSE);
                            Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"else\", переход к парсингу тела оператора...");
                            position++;
                            state = Constants.State.EXPECTING_CONTENT_OR_OPENING_CURLY_BRACKET;
                            parseCodeBlock(ref tokens);
                            syntaxTree.goToParent();
                        }

                        syntaxTree.goToParent();
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { ")" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция оператора \"if\".", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг оператора for.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseFor(ref Token[] tokens)
        {
            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.FOR);
            state = Constants.State.EXPECTING_OPENING_BRACKET;
            int semicolonCount = 0;
            
            while (position < tokens.Length)
            {
                if (state == Constants.State.EXPECTING_OPENING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"(\"...");
                        position++;

                        if(isDeclarationAhead(ref tokens))
                        {
                            parseFieldOrDeclaration(ref tokens, false);
                            state = Constants.State.EXPECTING_SEMICOLON;
                            //Костыль.
                            if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.DECLARATION)
                            {
                                syntaxTree.goToParent();
                            }
                        }
                        else
                        {
                            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.PARAMETER);
                            parseExpression(ref tokens);
                            state = Constants.State.EXPECTING_COMMA_OR_SEMICOLON;
                        }
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "(" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_SEMICOLON)
                {
                    if (tokens[position].type == Constants.TokenType.SEMICOLON)
                    {
                        semicolonCount++;
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \";\".");
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.PARAMETER);
                        position++;
                        parseExpression(ref tokens);
                        state = Constants.State.EXPECTING_COMMA_OR_SEMICOLON;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { ";" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_COMMA_OR_SEMICOLON)
                {
                    if (tokens[position].type == Constants.TokenType.COMMA)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \",\", переход к парсингу выражения...");
                        position++;
                        parseExpression(ref tokens);
                        state = Constants.State.EXPECTING_COMMA_OR_SEMICOLON;
                    }
                    else if (tokens[position].type == Constants.TokenType.SEMICOLON)
                    {
                        semicolonCount++;
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \";\".");
                        syntaxTree.goToParent();
                        syntaxTree.appendAndGoToChild(Constants.TreeNodeType.PARAMETER);
                        position++;
                        parseExpression(ref tokens);
                        if (semicolonCount == 2)
                        {
                            state = Constants.State.EXPECTING_CLOSING_BRACKET_OR_COMMA;
                        }
                        else
                        {
                            state = Constants.State.EXPECTING_COMMA_OR_SEMICOLON;
                        }
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[2] { ",", ";" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CLOSING_BRACKET_OR_COMMA)
                {
                    if (tokens[position].type == Constants.TokenType.CLOSING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \")\", переход к парсингу тела оператора...");
                        position++;
                        state = Constants.State.EXPECTING_CONTENT_OR_OPENING_CURLY_BRACKET;
                        syntaxTree.goToParent();
                        parseCodeBlock(ref tokens);
                        Console.WriteLine("[SYNTAX][INFO] : парсинг оператора \"for\" завершён.");
                        syntaxTree.goToParent();
                        return;
                    }
                    else if (tokens[position].type == Constants.TokenType.COMMA)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \",\", переход к парсингу выражения...");
                        position++;
                        parseExpression(ref tokens);
                        state = Constants.State.EXPECTING_CLOSING_BRACKET_OR_COMMA;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { ")" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция оператора \"for\".", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг оператора foreach.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseForeach(ref Token[] tokens)
        {
            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.FOREACH);
            state = Constants.State.EXPECTING_OPENING_BRACKET;
            
            while (position < tokens.Length)
            {
                if (state == Constants.State.EXPECTING_OPENING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"(\", переход к парсингу объявления...");
                        position++;
                        parseFieldOrDeclaration(ref tokens, false);
                        state = Constants.State.EXPECTING_IN;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "(" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_IN)
                {
                    if (tokens[position].type == Constants.TokenType.IN)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружено ключевое слово \"in\", переход к парсингу выражения...");
                        syntaxTree.goToParent();
                        position++;
                        parseExpression(ref tokens);
                        state = Constants.State.EXPECTING_CLOSING_BRACKET;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "in" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CLOSING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.CLOSING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \")\", переход к парсингу тела оператора...");
                        position++;
                        state = Constants.State.EXPECTING_CONTENT_OR_OPENING_CURLY_BRACKET;
                        parseCodeBlock(ref tokens);
                        syntaxTree.goToParent();
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { ")" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция оператора \"foreach\".", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг оператора while.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseWhile(ref Token[] tokens)
        {
            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.WHILE);
            state = Constants.State.EXPECTING_OPENING_BRACKET;

            while (position < tokens.Length)
            {
                if (state == Constants.State.EXPECTING_OPENING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"(\", переход к парсингу выражения...");
                        position++;
                        parseExpression(ref tokens);
                        state = Constants.State.EXPECTING_CLOSING_BRACKET;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "(" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CLOSING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.CLOSING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \")\", переход к парсингу тела оператора...");
                        position++;
                        state = Constants.State.EXPECTING_CONTENT_OR_OPENING_CURLY_BRACKET;
                        parseCodeBlock(ref tokens);
                        Console.WriteLine("[SYNTAX][INFO] : парсинг оператора \"while\" завершён.");
                        syntaxTree.goToParent();
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { ")" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция оператора \"while\".", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг оператора do-while.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseDoWhile(ref Token[] tokens)
        {
            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.DO);
            state = Constants.State.EXPECTING_OPENING_CURLY_BRACKET;

            while (position < tokens.Length)
            {
                if(state == Constants.State.EXPECTING_OPENING_CURLY_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_CURLY_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"{\", переход к парсингу тела оператора...");
                        parseCodeBlock(ref tokens);
                        state = Constants.State.EXPECTING_WHILE;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "(" }, tokens[position]);
                        position++;
                    }
                }
                else if( state == Constants.State.EXPECTING_WHILE)
                {
                    if (tokens[position].type == Constants.TokenType.WHILE)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружен \"while\", переход к парсингу выражения...");
                        position++;
                        state = Constants.State.EXPECTING_OPENING_BRACKET;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "while" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_OPENING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"(\", переход к парсингу выражения...");
                        position++;
                        parseExpression(ref tokens);
                        state = Constants.State.EXPECTING_CLOSING_BRACKET;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "(" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CLOSING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.CLOSING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \")\", парсинг оператора \"do-while\" завершён.");
                        position++;
                        syntaxTree.goToParent();
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { ")" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция оператора \"do-while\".", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг оператора switch.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseSwitch(ref Token[] tokens)
        {
            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.SWITCH);

            while (position < tokens.Length)
            {
                if (state == Constants.State.EXPECTING_OPENING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.OPENING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \"(\", переход к парсингу выражения...");
                        position++;
                        parseExpression(ref tokens);
                        state = Constants.State.EXPECTING_CLOSING_BRACKET;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { "(" }, tokens[position]);
                        position++;
                    }
                }
                else if (state == Constants.State.EXPECTING_CLOSING_BRACKET)
                {
                    if (tokens[position].type == Constants.TokenType.CLOSING_BRACKET)
                    {
                        Console.WriteLine("[SYNTAX][INFO] : обнаружена \")\", переход к парсингу тела оператора...");
                        position++;
                        state = Constants.State.EXPECTING_OPENING_CURLY_BRACKET;
                        parseCodeBlock(ref tokens);
                        syntaxTree.goToParent();
                        return;
                    }
                    else
                    {
                        translationResultBus.registerUnexpectedTokenError(new string[1] { ")" }, tokens[position]);
                        position++;
                    }
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённая конструкция оператора \"switch\".", tokens[tokens.Length - 1]);
        }

        /// <summary>
        /// Этот метод выполняет парсинг арифметических и логических
        /// выражений. При анализе выполняется преобразование из инфиксной формы
        /// в постфиксную (обратная польская запись) с помощью алгоритма
        /// сортировочной станции (shunting yard). Это делается для упрощения
        /// задачи семантическому анализатору, которому нужно будет проверить
        /// соответствие типов и числа аргументов.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        private void parseExpression(ref Token[] tokens)
        {
            Stack<Token> stack = new Stack<Token>();

            //Запятая может быть разделителем параметров функции:
            //function(paramType param, paramType param2, ...)
            //И может быть концом выражения:
            //varType var1 = value1, var2 = value2 ...
            //Конструкции вида
            //var1 = var2, var2 = var3 ...
            //не поддерживаются языком C#.
            //Чтобы отличить запятую в вызове функции и
            //запятую в конце выражения, в этой переменной
            //хранится глубина вложенности вызова функции.
            //Т.е. если был встречен идентификатор функции,
            //то глубина увеличивается. Если встречена запятая и
            //глубина не равна 0, то эта запятая является
            //разделителем аргументов в вызове функции.
            int functionDepth = 0;

            //Нужно для правильной обработки операторов ++ и --.
            int expressionStartPosition = position;

            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.EXPRESSION);
            syntaxTree.appendAndGoToChild(Constants.TreeNodeType.EXPRESSION_RPN);
            syntaxTree.goToParent();

            while(position < tokens.Length)
            {
                if (tokens[position].type == Constants.TokenType.SEMICOLON ||
                    tokens[position].type == Constants.TokenType.IN)
                {
                    //Если встречена точка с запятой, это
                    //значит, что встречен конец строки с выражением.
                    while (stack.Count() != 0)
                    {
                        if(stack.First().type == Constants.TokenType.OPENING_BRACKET)
                        {
                            translationResultBus.registerError("[SYNTAX][ERROR] : в выражении пропущена \")\".", stack.First());
                        }
                        else if (stack.First().type == Constants.TokenType.OPENING_SQUARE_BRACKET)
                        {
                            translationResultBus.registerError("[SYNTAX][ERROR] : в выражении пропущена \"]\".", stack.First());
                        }
                        else
                        {
                            syntaxTree.goToChild(0);
                            syntaxTree.appendToken(stack.First());
                            syntaxTree.goToParent();

                            Console.Write(" " + stack.First().value);
                        }
                        stack.Pop();
                    }

                    Console.WriteLine();
                    Console.WriteLine("[SYNTAX][INFO] : парсинг выражения завершён.");
                    syntaxTree.goToParent();
                    
                    return;
                }
                else if (tokens[position].type == Constants.TokenType.COMMA)
                {
                    //Запятая является разделителем аргументов функции.
                    if(functionDepth != 0)
                    {
                        syntaxTree.appendToken(tokens[position]);

                        while(stack.Count() != 0 && 
                              stack.First().type != Constants.TokenType.OPENING_BRACKET)
                        {
                            syntaxTree.goToChild(0);
                            syntaxTree.appendToken(stack.First());
                            syntaxTree.goToParent();

                            Console.Write(" " + stack.First().value);

                            stack.Pop();
                        }

                        if(stack.Count() == 0)
                        {
                            Console.WriteLine();
                            translationResultBus.registerError("[SYNTAX][ERROR] : в выражении пропущена \"(\" или \",\", разделяющая аргументы функции.", tokens[position]);
                        }

                        position++;
                    }
                    else
                    {
                        while(stack.Count() != 0)
                        {
                            if (stack.First().type == Constants.TokenType.OPENING_SQUARE_BRACKET)
                            {
                                Console.WriteLine();
                                translationResultBus.registerError("[SYNTAX][ERROR] : в выражении пропущена \"]\".", stack.First());
                            }
                            if (stack.First().type == Constants.TokenType.OPENING_BRACKET)
                            {
                                Console.WriteLine();
                                translationResultBus.registerError("[SYNTAX][ERROR] : в выражении пропущена \")\".", stack.First());
                            }

                            syntaxTree.goToChild(0);
                            syntaxTree.appendToken(stack.First());
                            syntaxTree.goToParent();

                            Console.Write(" " + stack.First().value);
                            stack.Pop();
                        }

                        Console.WriteLine();
                        Console.WriteLine("[SYNTAX][INFO] : парсинг выражения завершён.");
                        syntaxTree.goToParent();
                        return;
                    }
                }
                else if (tokens[position].type == Constants.TokenType.INT_NUMBER ||
                         tokens[position].type == Constants.TokenType.REAL_NUMBER ||
                         tokens[position].type == Constants.TokenType.TRUE ||
                         tokens[position].type == Constants.TokenType.FALSE ||
                         tokens[position].type == Constants.TokenType.NULL)
                {
                    if (isOperand(tokens[position - 1], true))
                    {
                        translationResultBus.registerError("[SYNTAX][ERROR] : требуется оператор.", tokens[position]);
                    }

                    Console.Write(" " + tokens[position].value);
                    syntaxTree.appendToken(tokens[position]);
                    syntaxTree.goToChild(0);
                    syntaxTree.appendToken(tokens[position]);
                    syntaxTree.goToParent();

                    position++;
                }
                else if (tokens[position].type == Constants.TokenType.IDENTIFIER ||
                         tokens[position].type == Constants.TokenType.THIS)
                {
                    LinkedList<Token> tempStack = new LinkedList<Token>();

                    syntaxTree.appendToken(tokens[position]);
                    tempStack.AddFirst(tokens[position]);
                    position++;
                    state = Constants.State.EXPECTING_DOT;

                    while (position < tokens.Length && 
                           (tokens[position].type == Constants.TokenType.IDENTIFIER ||
                           tokens[position].type == Constants.TokenType.DOT))
                    {
                        if(tokens[position].type == Constants.TokenType.IDENTIFIER)
                        {
                            tempStack.AddFirst(tokens[position]);
                            tempStack.AddFirst(tokens[position - 1]);
                            syntaxTree.appendToken(tokens[position]);
                            state = Constants.State.EXPECTING_DOT;
                        }
                        else if(tokens[position].type == Constants.TokenType.DOT)
                        {
                            if (state == Constants.State.EXPECTING_IDENTIFIER)
                            {
                                translationResultBus.registerError("[SYNTAX][ERROR] : требуется операнд.", tokens[position]);
                            }

                            syntaxTree.appendToken(tokens[position]);
                            state = Constants.State.EXPECTING_IDENTIFIER;
                        }

                        position++;
                    }

                    if (state == Constants.State.EXPECTING_IDENTIFIER)
                    {
                        translationResultBus.registerError("[SYNTAX][ERROR] : требуется операнд.", tokens[position - 1]);
                    }

                    //Встречена открывающая скобка,
                    //при этом образуется цепочка вида
                    //<идентификатор>.<идентификатор>...<идентификатор>(
                    if (position < tokens.Length &&
                       tokens[position].type == Constants.TokenType.OPENING_BRACKET)
                    {
                        while (tempStack.Count > 0)
                        {
                            stack.Push(tempStack.First());
                            tempStack.RemoveFirst();
                        }

                        functionDepth++;
                    }
                    else
                    {
                        while (tempStack.Count > 0)
                        {
                            syntaxTree.goToChild(0);
                            syntaxTree.appendToken(tempStack.Last());
                            syntaxTree.goToParent();
                            Console.Write(" " + tempStack.Last().value);
                            tempStack.RemoveLast();
                        }
                    }
                }
                else if (tokens[position].type >= Constants.TokenType.PLUS &&
                         tokens[position].type <= Constants.TokenType.BIT_SHIFT_TO_RIGHT ||
                         tokens[position].type == Constants.TokenType.DOT ||
                         tokens[position].type == Constants.TokenType.QUESTION_MARK ||
                         tokens[position].type >= Constants.TokenType.ASSIGNMENT &&
                         tokens[position].type <= Constants.TokenType.SHIFT_TO_RIGHT_ASSIGNMENT ||
                         tokens[position].type == Constants.TokenType.NEW)
                {
                    syntaxTree.appendToken(tokens[position]);

                    //Проверка соответсвия количества операндов и операторов.
                    if (getArity(tokens[position]) == 1)
                    {
                        //Эти операторы могут стоять как до, так и после операнда.
                        //Но ситуаций, в которых эти операторы стояли бы между двумя
                        //операндами, быть не может.
                        if(tokens[position].type == Constants.TokenType.INCREMENT ||
                           tokens[position].type == Constants.TokenType.DECREMENT)
                        {
                            if (position > expressionStartPosition)
                            {
                                if (!((isOperand(tokens[position - 1], true) &&
                                    !isOperand(tokens[position + 1], false)) ||
                                    (!isOperand(tokens[position - 1], true) &&
                                    isOperand(tokens[position + 1], false))))
                                {
                                    translationResultBus.registerError("[SYNTAX][ERROR] : требуется операнд.", tokens[position]);
                                }
                            }
                            else
                            {
                                if (!isOperand(tokens[position + 1], true))
                                {
                                    translationResultBus.registerError("[SYNTAX][ERROR] : требуется операнд.", tokens[position]);
                                }
                            }
                        }
                        else
                        {
                            if (!isOperand(tokens[position + 1], true))
                            {
                                translationResultBus.registerError("[SYNTAX][ERROR] : требуется операнд.", tokens[position]);
                            }
                        }
                    }
                    else if (getArity(tokens[position]) == 2)
                    {
                        if (!((getArity(tokens[position - 1]) == 1 ||
                               isOperand(tokens[position - 1], true)) &&
                               position < tokens.Length - 1 &&
                               (getArity(tokens[position + 1]) == 1 ||
                                isOperand(tokens[position + 1], false))))
                        {
                            translationResultBus.registerError("[SYNTAX][ERROR] : оператор требует 2 аргумента.", tokens[position]);
                        }
                    }

                    while (stack.Count() != 0)
                    {
                        int pr1 = getPriority(stack.First());
                        int pr2 = getPriority(tokens[position]);
                        if (pr1 > pr2)
                        {
                            syntaxTree.goToChild(0);
                            syntaxTree.appendToken(stack.First());
                            syntaxTree.goToParent();

                            Console.Write(" " + stack.First().value);
                            stack.Pop();
                        }
                        else if(pr1 == pr2)
                        {
                            if(isLeftAssociative(tokens[position]))
                            {
                                syntaxTree.goToChild(0);
                                syntaxTree.appendToken(stack.First());
                                syntaxTree.goToParent();

                                Console.Write(" " + stack.First().value);
                                stack.Pop();
                            }
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }

                    stack.Push(tokens[position]);
                    position++;
                }
                else if (tokens[position].type == Constants.TokenType.COLON)
                {
                    //Обнаружена часть тернарного оператора ":"
                    while (stack.Count() != 0 &&
                           stack.First().type != Constants.TokenType.QUESTION_MARK)
                    {
                        if (stack.First().type == Constants.TokenType.OPENING_SQUARE_BRACKET)
                        {
                            Console.WriteLine();
                            translationResultBus.registerError("[SYNTAX][ERROR] : в выражении пропущена \"]\".", stack.First());
                        }
                        if (stack.First().type == Constants.TokenType.OPENING_BRACKET)
                        {
                            Console.WriteLine();
                            translationResultBus.registerError("[SYNTAX][ERROR] : в выражении пропущена \")\".", stack.First());
                        }

                        syntaxTree.goToChild(0);
                        syntaxTree.appendToken(stack.First());
                        syntaxTree.goToParent();

                        Console.Write(" " + stack.First().value);
                        stack.Pop();
                    }

                    if (stack.Count() != 0)
                    {
                        if(!isOperand(tokens[position - 1], true))
                        {
                            translationResultBus.registerError("[SYNTAX][ERROR] : требуется операнд.", tokens[position]);
                        }

                        syntaxTree.appendToken(tokens[position]);

                        stack.Pop();

                        Token t = new Token();
                        t.type = Constants.TokenType.UTILITY_TERNARY_CLOSED_IF;
                        t.value = "?";
                        stack.Push(t);
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("[SYNTAX][INFO] : парсинг выражения завершён.");
                        syntaxTree.goToParent();

                        return;
                    }
                    position++;
                }
                else if (tokens[position].type == Constants.TokenType.OPENING_SQUARE_BRACKET)
                {
                    if (tokens[position - 1].type == Constants.TokenType.CLOSING_SQUARE_BRACKET)
                    {
                        translationResultBus.registerError("[SYNTAX][ERROR] : требуется операнд.", tokens[position - 1]);
                    }

                    syntaxTree.appendToken(tokens[position]);
                    stack.Push(tokens[position]);
                    position++;
                }
                else if (tokens[position].type == Constants.TokenType.CLOSING_SQUARE_BRACKET)
                {
                    syntaxTree.appendToken(tokens[position]);

                    while (stack.Count() != 0 &&
                           stack.First().type != Constants.TokenType.OPENING_SQUARE_BRACKET)
                    {
                        if (stack.First().type == Constants.TokenType.OPENING_BRACKET)
                        {
                            Console.WriteLine();
                            translationResultBus.registerError("[SYNTAX][ERROR] : в выражении пропущена \")\".", stack.First());
                        }

                        syntaxTree.goToChild(0);
                        syntaxTree.appendToken(stack.First());
                        syntaxTree.goToParent();

                        Console.Write(" " + stack.First().value);
                        stack.Pop();
                    }

                    if (stack.Count() != 0)
                    {
                        stack.Pop();
                        Console.Write(" []");

                        syntaxTree.goToChild(0);
                        syntaxTree.appendToken(new Token(Constants.TokenType.UTILITY_ARRAY_ELEMENT_ACCESS, "[]"));
                        syntaxTree.goToParent();
                    }
                    else
                    {
                        Console.WriteLine();
                        translationResultBus.registerError("[SYNTAX][ERROR] : в выражении пропущена \"[\".", tokens[position]);
                    }

                    position++;
                }
                else if (tokens[position].type == Constants.TokenType.OPENING_BRACKET)
                {
                    if (tokens[position - 1].type == Constants.TokenType.CLOSING_BRACKET ||
                        tokens[position - 1].type == Constants.TokenType.CLOSING_SQUARE_BRACKET)
                    {
                        translationResultBus.registerError("[SYNTAX][ERROR] : требуется оператор.", tokens[position - 1]);
                    }

                    syntaxTree.appendToken(tokens[position]);
                    stack.Push(tokens[position]);
                    position++;
                }
                else if (tokens[position].type == Constants.TokenType.CLOSING_BRACKET)
                {
                    //Если встречена закрывающая скобка и стек пустой, это значит,
                    //что встречена конструкция типа if(... ) или что-то подобное.
                    if (stack.Count() == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("[SYNTAX][INFO] : парсинг выражения завершён.");
                        syntaxTree.goToParent();
                        return;
                    }
                    else
                    {
                        while (stack.Count() != 0 &&
                               stack.First().type != Constants.TokenType.OPENING_BRACKET)
                        {
                            if (stack.First().type == Constants.TokenType.OPENING_SQUARE_BRACKET)
                            {
                                Console.WriteLine();
                                translationResultBus.registerError("[SYNTAX][ERROR] : в выражении пропущена \"]\".", stack.First());
                            }

                            syntaxTree.goToChild(0);
                            syntaxTree.appendToken(stack.First());
                            syntaxTree.goToParent();

                            Console.Write(" " + stack.First().value);
                            stack.Pop();
                        }

                        if (stack.Count() != 0)
                        {
                            syntaxTree.appendToken(tokens[position]);

                            stack.Pop();

                            if (stack.Count() != 0 &&
                                (stack.First().type == Constants.TokenType.IDENTIFIER ||
                                stack.First().type == Constants.TokenType.DOT))
                            {
                                functionDepth = functionDepth > 0 ? functionDepth - 1 : 0;
                            }

                            //Проверка, нет ли идентификатора функции на вершине стека.
                            while(stack.Count() != 0 && 
                                  (stack.First().type == Constants.TokenType.IDENTIFIER ||
                                  stack.First().type == Constants.TokenType.DOT))
                            {
                                syntaxTree.goToChild(0);
                                syntaxTree.appendToken(stack.First());
                                syntaxTree.goToParent();

                                Console.Write(" " + stack.First().value);
                                stack.Pop();
                            }
                        }
                        else
                        {
                            Console.WriteLine();
                            //Закрывающая скобка может быть частью операторов.
                            //Поэтому алгоритм завершает разбор выражения, когда находит
                            //непарную скобку.
                            syntaxTree.goToParent();
                            return;
                        }

                        position++;
                    }   
                }
                else if (tokens[position].type == Constants.TokenType.CLOSING_CURLY_BRACKET)
                {
                    if(functionDepth > 0)
                    {
                        translationResultBus.registerError("[SYNTAX][ERROR] : в выражении пропущена \")\" в конце перечисления аргументов метода.", tokens[position]);
                    }

                    while(stack.Count != 0)
                    {
                        syntaxTree.goToChild(0);
                        syntaxTree.appendToken(stack.First());
                        syntaxTree.goToParent();

                        Console.WriteLine(" " + stack.First().value);
                        stack.Pop();
                    }

                    Console.WriteLine("[SYNTAX][INFO] : парсинг выражения завершён.");
                    syntaxTree.goToParent();
                    return;
                }
                else if (tokens[position].type == Constants.TokenType.QUOTATION_MARK)
                {
                    //После "'" обязательно должна идти символьная лексема и ещё одна "'".
                    syntaxTree.appendToken(tokens[position]);
                   
                    if(position < tokens.Length - 2)
                    {
                        if (tokens[position + 1].type == Constants.TokenType.CHAR &&
                           tokens[position + 2].type == Constants.TokenType.QUOTATION_MARK)
                        {
                            syntaxTree.appendToken(tokens[position + 1]);
                            syntaxTree.goToChild(0);
                            syntaxTree.appendToken(tokens[position + 1]);
                            syntaxTree.goToParent();

                            Console.Write(" " + tokens[position + 1].value);

                            syntaxTree.appendToken(tokens[position + 2]);

                            Console.Write(" " + tokens[position + 2].value);

                            position += 3;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else if (tokens[position].type == Constants.TokenType.DOUBLE_QUOTATION_MARK)
                {
                    syntaxTree.appendToken(tokens[position]);

                    if (position < tokens.Length - 2)
                    {
                        if (tokens[position + 1].type == Constants.TokenType.STRING &&
                            tokens[position + 2].type == Constants.TokenType.DOUBLE_QUOTATION_MARK)
                        {
                            syntaxTree.appendToken(tokens[position + 1]);
                            syntaxTree.goToChild(0);
                            syntaxTree.appendToken(tokens[position + 1]);
                            syntaxTree.goToParent();

                            Console.Write(" " + tokens[position + 1].value);

                            syntaxTree.appendToken(tokens[position + 2]);

                            Console.Write(" " + tokens[position + 2].value);

                            position += 3;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    translationResultBus.registerUnexpectedTokenError(new string[2] { "арифметические или логические операторы", "new" }, tokens[position]);
                    position++;
                }
            }

            translationResultBus.registerError("[SYNTAX][ERROR] : незавершённое выражение.", tokens[position - 1]);
        }

        /// <summary>
        /// Этот метод возвращает приоритет 
        /// выполнения оператора в выражении.
        /// </summary>
        /// <param name="token">Токен, содержащий в себе оператор.</param>
        /// <returns></returns>
        private int getPriority(Token token)
        {
            if(token.type >= Constants.TokenType.ASSIGNMENT &&
               token.type <= Constants.TokenType.SHIFT_TO_RIGHT_ASSIGNMENT)
            {
                return -1;
            }
            else if (token.type == Constants.TokenType.OPENING_BRACKET ||
                token.type == Constants.TokenType.OPENING_SQUARE_BRACKET ||
                token.type == Constants.TokenType.QUESTION_MARK ||
                token.type == Constants.TokenType.COLON ||
                token.type == Constants.TokenType.UTILITY_TERNARY_CLOSED_IF)
            {
                return -1;
            }
            else if (token.type >= Constants.TokenType.ASSIGNMENT &&
               token.type <= Constants.TokenType.SHIFT_TO_RIGHT_ASSIGNMENT)
            {
                return 0;
            }
            else if (token.type == Constants.TokenType.OR)
            {
                return 1;
            }
            else if (token.type == Constants.TokenType.AND)
            {
                return 2;
            }
            else if (token.type == Constants.TokenType.BIT_OR)
            {
                return 3;
            }
            else if (token.type == Constants.TokenType.BIT_EXCLUSIVE_OR)
            {
                return 4;
            }
            else if (token.type == Constants.TokenType.BIT_AND)
            {
                return 5;
            }
            else if (token.type == Constants.TokenType.EQUAL ||
                     token.type == Constants.TokenType.NOT_EQUAL)
            {
                return 6;
            }
            else if (token.type == Constants.TokenType.LESS ||
                     token.type == Constants.TokenType.LESS_OR_EQUAL ||
                     token.type == Constants.TokenType.GREATER ||
                     token.type == Constants.TokenType.GREATER_OR_EQUAL)
            {
                return 7;
            }
            else if (token.type == Constants.TokenType.BIT_ARITHMETIC_SHIFT_TO_RIGHT ||
                     token.type == Constants.TokenType.BIT_ARITHMETIC_SHIFT_TO_LEFT ||
                     token.type == Constants.TokenType.BIT_SHIFT_TO_RIGHT)
            {
                return 8;
            }
            else if (token.type == Constants.TokenType.PLUS ||
                     token.type == Constants.TokenType.MINUS)
            {
                return 9;
            }
            else if (token.type == Constants.TokenType.MULTIPLICATION ||
                     token.type == Constants.TokenType.DIVISION ||
                     token.type == Constants.TokenType.MODULO)
            {
                return 10;
            }
            else if (token.type == Constants.TokenType.NOT ||
                     token.type == Constants.TokenType.UNARY_MINUS ||
                     token.type == Constants.TokenType.INCREMENT ||
                     token.type == Constants.TokenType.DECREMENT ||
                     token.type == Constants.TokenType.BIT_TILDA)
            {
                return 11;
            }
            else
            {
                return 12;
            }
        }

        /// <summary>
        /// Этот метод определяет, является ли оператор левоассоциативным.
        /// </summary>
        /// <param name="token">Токен, содержащий в себе оператор.</param>
        /// <returns></returns>
        private bool isLeftAssociative(Token token)
        {
            if(token.type == Constants.TokenType.QUESTION_MARK ||
               token.type == Constants.TokenType.UTILITY_TERNARY_CLOSED_IF ||
               (token.type >= Constants.TokenType.ASSIGNMENT &&
                token.type <= Constants.TokenType.SHIFT_TO_RIGHT_ASSIGNMENT))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Этот метод возвращает арность оператора.
        /// </summary>
        /// <param name="token">Токен, содержащий в себе оператор.</param>
        /// <returns>Арность.</returns>
        private int getArity(Token token)
        {
            if(token.type == Constants.TokenType.NOT ||
               token.type == Constants.TokenType.UNARY_MINUS ||
               token.type == Constants.TokenType.INCREMENT ||
               token.type == Constants.TokenType.DECREMENT ||
               token.type == Constants.TokenType.BIT_TILDA ||
               token.type == Constants.TokenType.NEW)
            {
                return 1;
            }
            else if(token.type == Constants.TokenType.QUESTION_MARK)
            {
                return 3;
            }

            return 2;
        }

        /// <summary>
        /// Данный метод проверяет, может ли токен быть операндом в выражениях.
        /// </summary>
        /// <param name="token">Токен, предположительно являющийся операндом.</param>
        /// <param name="previous">Флаг, показывающий, с какой стороны текущего токена
        /// проверяется данный токен.</param>
        /// <returns>Является ли токен операндом.</returns>
        private bool isOperand(Token token, bool previous)
        {
            if(previous)
            {
                return (token.type == Constants.TokenType.CLOSING_BRACKET ||
                        token.type == Constants.TokenType.CLOSING_SQUARE_BRACKET ||
                        token.type == Constants.TokenType.THIS ||
                        token.type == Constants.TokenType.NULL ||
                        token.type == Constants.TokenType.TRUE ||
                        token.type == Constants.TokenType.FALSE ||
                        token.type == Constants.TokenType.INT_NUMBER ||
                        token.type == Constants.TokenType.REAL_NUMBER ||
                        token.type == Constants.TokenType.IDENTIFIER ||
                        token.type == Constants.TokenType.QUOTATION_MARK ||
                        token.type == Constants.TokenType.DOUBLE_QUOTATION_MARK);
            }
            else
            {
                return (token.type == Constants.TokenType.OPENING_BRACKET ||
                        token.type == Constants.TokenType.THIS ||
                        token.type == Constants.TokenType.NULL ||
                        token.type == Constants.TokenType.TRUE ||
                        token.type == Constants.TokenType.FALSE ||
                        token.type == Constants.TokenType.INT_NUMBER ||
                        token.type == Constants.TokenType.REAL_NUMBER ||
                        token.type == Constants.TokenType.IDENTIFIER ||
                        token.type == Constants.TokenType.QUOTATION_MARK ||
                        token.type == Constants.TokenType.DOUBLE_QUOTATION_MARK);
            }
        }

        /// <summary>
        /// Этот метод проверяет, является ли встреченная конструкция
        /// объявлением. Нужен, потому что сразу отличить объявление от, например,
        /// вызова метода в текущей архитектуре анализатора невозможно.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        /// <returns></returns>
        private bool isDeclarationAhead(ref Token[] tokens)
        {
            int tempPosition = position;
            while (tempPosition < tokens.Length &&
                  tokens[tempPosition].type != Constants.TokenType.SEMICOLON)
            {
                if (tokens[tempPosition].type == Constants.TokenType.THIS)
                {
                    //Если встречено this, то это однозначно не объявление.
                    return false;
                }
                else if (tokens[tempPosition].type == Constants.TokenType.IDENTIFIER)
                {
                    if (tokens[tempPosition - 1].type == Constants.TokenType.IDENTIFIER ||
                       tokens[tempPosition - 1].type == Constants.TokenType.CLOSING_SQUARE_BRACKET &&
                       tokens[tempPosition - 2].type == Constants.TokenType.OPENING_SQUARE_BRACKET)
                    {
                        //Два идентификатора подряд - это признак объявления.
                        //Также в объявлении возможна ситуация
                        //вида <идентификатор>[] <идентификатор>.
                        return true;
                    }
                }

                tempPosition++;
            }

            return false;
        }

        //Этот метод используется только для дебага.
        private void printSourceCode(ref Token[] tokens)
        {
            int depthLevel = 0;

            int i = 0;
            while(i < tokens.Length)
            {
                for (int j = 0; j < depthLevel; j++)
                { Console.Write("    "); }

                while (tokens[i].value != ";" && tokens[i].value != "{" && tokens[i].value != "}")
                {
                    Console.Write(tokens[i].value + " ");
                    i++;
                }

                if(tokens[i].value == ";")
                {
                    if(i < tokens.Length - 1 && tokens[i + 1].value == "}")
                    {
                        Console.Write(";");
                    }
                    else
                    {
                        Console.WriteLine(";");
                    }
                }
                if (tokens[i].value == "{")
                {
                    Console.WriteLine();
                    for (int j = 0; j < depthLevel; j++)
                    { Console.Write("    "); }

                    if (i < tokens.Length - 1 && tokens[i + 1].value == "}")
                    {
                        Console.Write("{");
                    }
                    else
                    {
                        Console.WriteLine("{");
                    }
                }

                if (tokens[i].value == "{")
                {
                    depthLevel++; 
                }
                else if (tokens[i].value == "}")
                {
                    depthLevel--; 
                }

                if (tokens[i].value == "}")
                {
                    Console.WriteLine();
                    for (int j = 0; j < depthLevel; j++)
                    { Console.Write("    "); }

                    if(i < tokens.Length - 1 && tokens[i + 1].value == "}")
                    {
                        Console.Write(tokens[i].value);
                    }
                    else
                    {
                        Console.WriteLine(tokens[i].value);
                    }
                }

                i++;
            }
        }

        public SyntaxAnalyzer(TranslationResultBus translationResultBus)
        {
            this.translationResultBus = translationResultBus;
        }

        /// <summary>
        /// Этот метод вызывается первым для выполнения синтаксического анализа.
        /// </summary>
        /// <param name="tokens">Указатель на массив токенов, 
        /// полученных из лексического анализатора.</param>
        /// <returns>Абстрактное синтаксическое дерево, подготовленное для
        /// семантического анализатора.</returns>
        public SyntaxTree parse(ref Token[] tokens)
        {
            position = 0;
            syntaxTree = new SyntaxTree();

            parseHeadOfProgram(ref tokens);
            parseNamespace(ref tokens);

            //После парсинга всего входного потока лескем он должен быть пустым.
            //Всё, что осталось за пределами главного пространства имён - ошибка.
            while (position < tokens.Length)
            {
                translationResultBus.registerError("[SYNTAX][ERROR] : лексема за пределами пространства имён.", tokens[position]);
                position++;
            }

            Console.WriteLine("==============================");
            syntaxTree.print();

            return syntaxTree;
        }

        private int position;
        private Constants.State state;
        private SyntaxTree syntaxTree;
        private TranslationResultBus translationResultBus;
    }
}
