using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace CSharpToJavaTranslator
{
    public class SemanticAnalyzer
    {
        private readonly SyntaxTreeNode root;
        private TranslationResultBus resultBus;

        private Dictionary<string, string> identClass = new Dictionary<string, string>();
        private Dictionary<string, string> identEnum = new Dictionary<string, string>();
        private Dictionary<string, string> identMethod = new Dictionary<string, string>();

        private string nameClass;

        public SemanticAnalyzer(SyntaxTree syntTree, TranslationResultBus resultBus)
        {
            root = syntTree.root;
            this.resultBus = resultBus;
        }

        public void semanticAnalysis()
        {
            usingUndeclaredIdentifier();
        }

        private void usingUndeclaredIdentifier()
        {
            SyntaxTreeNode nodeClass = root;
            do
            {
                nodeClass = nodeClass.childNodes[0];
                if (nodeClass.childNodes == null)
                    return;
            }
            while (nodeClass.type != Constants.TreeNodeType.CLASS);
            nameClass = nodeClass.tokens[0].value;

            identifiersClass(nodeClass);
            identifiersEnum(nodeClass);
            identifiersMethod(nodeClass);
        }

        /// <summary>
        /// Выполняет соответсвие типов данных между идентифкатором и присваивыемым ему выражением
        /// </summary>
        /// <param name="node"></param>
        /// <param name="identifierLocation"></param>
        private void typeCompatibility(SyntaxTreeNode node, string identifierLocation)
        {

        }

        /// <summary>
        /// Проверяет присваиваемое выражение идентификатору класса или в параметре метода
        /// </summary>
        /// <param name="current"></param>
        /// <param name="identifierLocation"></param>
        private void expressionClass(SyntaxTreeNode current, string identifierLocation)
        {
            bool error = false;
            string identifier;

            for (int numToken = 0; numToken < current.tokens.Count; numToken++)
            {
                identifier = current.tokens[numToken].value;

                if (identifier == "this")
                {
                    error = true;
                    resultBus.registerError($"[SEMANT][ERROR] : ключевое слово '{identifier}' не применимо в текущем контексте.", current.tokens[numToken]);
                    numToken += 2;
                }
                else if (Regex.IsMatch(identifier, @"^[a-zA-Z0-9_]+$") && current.tokens[numToken].type != Constants.TokenType.STRING &&
                         identifier != "new" && identifier != "int" && identifier != "double" && identifier != "string" && identifier != "char") // Только буквы, цифры и подчеркивание
                {
                    double number;
                    if (!double.TryParse(identifier, out number) && identifier != "false" && identifier != "true")
                    {
                        bool unknownIdent = true;
                        foreach (string key in identClass.Keys)
                        {
                            if (key.Equals(identifier))
                            {
                                unknownIdent = false;
                                error = true;

                                if (identifierLocation == "class")
                                    resultBus.registerError($"[SEMANT][ERROR] : инициализатор поля не может обращаться к нестатичному полю, методу или свойству '{nameClass}.{current.tokens[numToken].value}'.", current.tokens[numToken]);
                                else if (identifierLocation == "paramMethod")
                                    resultBus.registerError($"[SEMANT][ERROR] : значение параметра по умолчанию для '{current.parentNode.tokens[current.parentNode.tokens.Count - 1].value}' должно быть константой.", current.tokens[numToken]);

                                break;
                            }
                        }

                        if (unknownIdent)
                        {
                            error = true;
                            resultBus.registerError($"[SEMANT][ERROR] : использование неизвестного идентификатора '{identifier}'.", current.tokens[numToken]);
                        } 
                    }
                    else if (identifier == "false" || identifier == "true")
                        error = checkBool(current, numToken);
                }
                else if (current.tokens[numToken].type == Constants.TokenType.STRING)
                    error = checkString(current, numToken);
            }

            if (!error)
                typeCompatibility(current, identifierLocation);
        }

        /// <summary>
        /// Проверяет присваиваемое выражение идентификатору в теле метода
        /// </summary>
        /// <param name="current"></param>
        private void expressionMethod(SyntaxTreeNode current)
        {
            bool unknownIdent = true;
            bool error = false;
            string identifier;
            
            for (int numToken = 0; numToken < current.tokens.Count; numToken++)
            {
                identifier = current.tokens[numToken].value;
                if (identifier == "this")
                {
                    numToken += 2;
                    identifier = current.tokens[numToken].value;

                    foreach (string key in identClass.Keys)
                    {
                        if (key.Equals(identifier))
                        {
                            unknownIdent = false;
                            break;
                        }
                    }

                    if (unknownIdent)
                    {
                        error = true;
                        resultBus.registerError($"[SEMANT][ERROR] : использование неизвестного идентификатора '{identifier}'.", current.tokens[numToken]);
                    }
                    else
                        unknownIdent = true;
                }
                else if (Regex.IsMatch(identifier, @"^[a-zA-Z0-9_]+$") && current.tokens[numToken].type != Constants.TokenType.STRING &&
                         identifier != "new" && identifier != "int" && identifier != "double" && identifier != "string" && identifier != "char")
                {
                    double number;
                    if (!double.TryParse(identifier, out number) && identifier != "false" && identifier != "true")
                    {
                        foreach (string key in identMethod.Keys)
                        {
                            if (key.Equals(identifier))
                            {
                                unknownIdent = false;
                                break;
                            }
                        }

                        if (unknownIdent)
                        {
                            foreach (string key in identClass.Keys)
                            {
                                if (key.Equals(identifier))
                                {
                                    unknownIdent = false;
                                    break;
                                }
                            }
                        }

                        if (unknownIdent)
                        {
                            error = true;
                            resultBus.registerError($"[SEMANT][ERROR] : использование неизвестного идентификатора '{identifier}'.", current.tokens[numToken]);
                        }
                        else
                            unknownIdent = true;
                    }
                    else if (identifier == "false" || identifier == "true")
                        error = checkBool(current, numToken);
                }
                else if (current.tokens[numToken].type == Constants.TokenType.STRING)
                    error = checkString(current, numToken);
            }

            if (!error)
                typeCompatibility(current, "method");
        }

        /// <summary>
        /// Проверяет конкатенацию строк в присваиваемом выражении
        /// </summary>
        /// <param name="current"></param>
        /// <param name="numToken"></param>
        /// <returns></returns>
        private bool checkString(SyntaxTreeNode current, int numToken)
        {
            string tokenBefore = "";
            string tokenAfter = "";

            if ((numToken - 2) >= 0)
                tokenBefore = current.tokens[numToken - 2].value;

            if ((numToken + 2) < current.tokens.Count)
                tokenAfter = current.tokens[numToken + 2].value;

            if (tokenBefore != "" && tokenBefore != "+")
            {
                resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenBefore}' невозможно применить к операнду типа 'string'", current.tokens[numToken]);
                return true;
            }
            else if (tokenAfter != "" && tokenAfter != "+")
            {
                resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenAfter}' невозможно применить к операнду типа 'string'", current.tokens[numToken]);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверяет логический идентификатор в присваиваемом выражении на арифметические операции
        /// </summary>
        /// <param name="current"></param>
        /// <param name="numToken"></param>
        /// <returns></returns>
        private bool checkBool(SyntaxTreeNode current, int numToken)
        {
            string tokenBefore = "";
            string tokenAfter = "";

            if ((numToken - 1) >= 0)
                tokenBefore = current.tokens[numToken - 1].value;

            if ((numToken + 1) < current.tokens.Count)
                tokenAfter = current.tokens[numToken + 1].value;

            if (tokenBefore != "" && !Regex.IsMatch(tokenBefore, @"^[a-zA-Z0-9_]+$") && tokenBefore != "=")
            {
                resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenBefore}' невозможно применить к операнду типа 'bool'", current.tokens[numToken]);
                return true;
            }
            else if (tokenAfter != "" && !Regex.IsMatch(tokenAfter, @"^[a-zA-Z0-9_]+$"))
            {
                resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenAfter}' невозможно применить к операнду типа 'bool'", current.tokens[numToken]);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверяет сущестование глобальных идентифкаторов класса
        /// </summary>
        /// <param name="nodeClass"></param>
        private void identifiersClass(SyntaxTreeNode nodeClass)
        {
            string identifier = "";
            string dataType = "";

            SyntaxTreeNode current;

            for (int numChild = 0; numChild < nodeClass.childNodes.Count; numChild++)
            {
                current = nodeClass;
                current = current.childNodes[numChild];

                if (current.childNodes[0].type == Constants.TreeNodeType.FIELD)
                {
                    current = current.childNodes[0];

                    if (current.tokens.Count == 1)
                        dataType = current.tokens[0].value;
                    else
                        dataType = $"{current.tokens[0].value} array";

                    current = current.childNodes[0];
                    identifier = current.tokens[0].value;

                    bool existingIdent = false;
                    if (identClass.Count != 0)
                    {
                        foreach (string key in identClass.Keys)
                        {
                            if (key.Equals(identifier))
                            {
                                existingIdent = true;
                                resultBus.registerError($"[SEMANT][ERROR] : тип '{nameClass}' уже содержит определение для '{identifier}'.", current.tokens[0]);
                                break;
                            }
                        }

                        if (!existingIdent)
                            identClass.Add(identifier, dataType);
                    }
                    else
                        identClass.Add(identifier, dataType);

                    if (current.childNodes != null && current.childNodes[0].type == Constants.TreeNodeType.EXPRESSION && !existingIdent)
                    {
                        current = current.childNodes[0];
                        expressionClass(current, "class");
                    }

                }
            }
        }

        /// <summary>
        /// Проверяет сущестование идентифкаторов enum
        /// </summary>
        /// <param name="nodeClass"></param>
        private void identifiersEnum(SyntaxTreeNode nodeClass)
        {
            string nameEnum;

            SyntaxTreeNode current;
            SyntaxTreeNode nodeEnum;


            for (int numChild = 0; numChild < nodeClass.childNodes.Count; numChild++)
            {
                current = nodeClass;
                current = current.childNodes[numChild];

                if (current.childNodes[0].type == Constants.TreeNodeType.ENUM)
                {
                    nodeEnum = current.childNodes[0];
                    nameEnum = nodeEnum.tokens[0].value;

                    if (nodeEnum.childNodes != null)
                    {
                        for (int num = 0; num < nodeEnum.childNodes.Count; num++)
                        {
                            current = nodeEnum;
                            current = current.childNodes[num];

                            bool existingIdent = false;
                            if (identEnum.Count != 0)
                            {
                                foreach (string key in identEnum.Keys)
                                {
                                    if (key.Equals(current.tokens[0].value))
                                    {
                                        existingIdent = true;
                                        resultBus.registerError($"[SEMANT][ERROR] : тип '{nodeClass.tokens[0].value}.{nameEnum}' уже содержит определение для '{current.tokens[0].value}'.", current.tokens[0]);
                                        break;
                                    }
                                }

                                if (!existingIdent)
                                    identEnum.Add(current.tokens[0].value, nameEnum);
                            }
                            else
                                identEnum.Add(current.tokens[0].value, nameEnum);

                            if (current.childNodes != null && current.childNodes[0].type == Constants.TreeNodeType.EXPRESSION && !existingIdent)
                            {
                                current = current.childNodes[0];

                                bool error = false;
                                bool unknownIdent = true;
                                string identifier;

                                for (int numToken = 0; numToken < current.tokens.Count; numToken++)
                                {
                                    identifier = current.tokens[numToken].value;

                                    if (identifier == "this")
                                    {
                                        error = true;
                                        resultBus.registerError($"[SEMANT][ERROR] : ключевое слово '{identifier}' не применимо в текущем контексте.", current.tokens[numToken]);
                                        numToken += 2;
                                    }
                                    else if (Regex.IsMatch(identifier, @"^[a-zA-Z0-9_]+$") && current.tokens[numToken].type != Constants.TokenType.STRING &&
                                             identifier != "new" && identifier != "int" && identifier != "double" && identifier != "string" && identifier != "char")
                                    {
                                        double number;
                                        if (!double.TryParse(identifier, out number) && identifier != "false" && identifier != "true")
                                        {
                                            foreach (string key in identEnum.Keys)
                                            {
                                                if (key.Equals(identifier))
                                                {
                                                    unknownIdent = false;
                                                    break;
                                                }
                                            }

                                            var lastIdent = identEnum.Keys.Last();
                                            if (lastIdent.Equals(identifier))
                                            {
                                                unknownIdent = false;
                                                error = true;
                                                resultBus.registerError($"[SEMANT][ERROR] : при оценке постоянного значения для '{nodeClass.tokens[0].value}.{nameEnum}.{lastIdent}' ипользуется циклическое определение.", current.tokens[numToken]);
                                            }

                                            if (unknownIdent)
                                            {
                                                error = true;
                                                resultBus.registerError($"[SEMANT][ERROR] : использование неизвестного идентификатора '{identifier}'.", current.tokens[numToken]);
                                            }
                                        }
                                        else if (identifier == "false" || identifier == "true")
                                            error = checkBool(current, numToken);
                                    }
                                    else if (current.tokens[numToken].type == Constants.TokenType.STRING)
                                        error = checkString(current, numToken);
                                }

                                if (!error)
                                    typeCompatibility(current, "enum");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Проверяет сущестование идентифкаторов в теле метода
        /// </summary>
        /// <param name="nodeClass"></param>
        private void identifiersMethod(SyntaxTreeNode nodeClass)
        {
            string identifier;
            string dataTypeIdentifier;
            string dataTypeMethod;

            SyntaxTreeNode current;
            SyntaxTreeNode nodeMethod;

            for (int numChild = 0; numChild < nodeClass.childNodes.Count; numChild++)
            {
                current = nodeClass;
                current = current.childNodes[numChild];

                if (current.childNodes[0].type == Constants.TreeNodeType.METHOD)
                {
                    nodeMethod = current.childNodes[0];

                    if (nodeMethod.tokens.Count == 2)
                        dataTypeMethod = nodeMethod.tokens[0].value;
                    else
                        dataTypeMethod = $"{nodeMethod.tokens[0].value} array";

                    if (nodeMethod.childNodes != null)
                    {
                        for (int num = 0; num < nodeMethod.childNodes.Count; num++)
                        {
                            current = nodeMethod;
                            current = current.childNodes[num];

                            if (current.type == Constants.TreeNodeType.PARAMETER)
                            {
                                if (current.tokens.Count == 2)
                                    dataTypeIdentifier = current.tokens[0].value;
                                else
                                    dataTypeIdentifier = $"{current.tokens[0].value} array";

                                identifier = current.tokens[current.tokens.Count - 1].value;

                                bool existingIdent = false;
                                if (identMethod.Count != 0)
                                {
                                    foreach (string key in identMethod.Keys)
                                    {
                                        if (key.Equals(identifier))
                                        {
                                            existingIdent = true;
                                            resultBus.registerError($"[SEMANT][ERROR] : повторяющееся имя параметра '{identifier}'", current.tokens[current.tokens.Count - 1]);
                                            break;
                                        }
                                    }

                                    if (!existingIdent)
                                        identMethod.Add(identifier, dataTypeIdentifier);
                                }
                                else
                                    identMethod.Add(identifier, dataTypeIdentifier);

                                if (current.childNodes != null && current.childNodes[0].type == Constants.TreeNodeType.EXPRESSION && !existingIdent)
                                {
                                    current = current.childNodes[0];
                                    expressionClass(current, "paramMethod");
                                }
                            }
                            else if (current.type == Constants.TreeNodeType.DECLARATION)
                            {
                                if (current.tokens.Count == 2)
                                    dataTypeIdentifier = current.tokens[0].value;
                                else
                                    dataTypeIdentifier = $"{current.tokens[0].value} array";

                                current = current.childNodes[0];
                                identifier = current.tokens[0].value;

                                bool existingIdent = false;
                                if (identMethod.Count != 0)
                                {
                                    foreach (string key in identMethod.Keys)
                                    {
                                        if (key.Equals(identifier))
                                        {
                                            existingIdent = true;
                                            resultBus.registerError($"[SEMANT][ERROR] : объявление уже существующего локального идентификатора '{identifier}' в методе '{nodeMethod.tokens[nodeMethod.tokens.Count - 1].value}'.", current.tokens[0]);
                                            break;
                                        }
                                    }

                                    if (!existingIdent)
                                        identMethod.Add(identifier, dataTypeIdentifier);
                                }
                                else
                                    identMethod.Add(identifier, dataTypeIdentifier);

                                if (current.childNodes != null && current.childNodes[0].type == Constants.TreeNodeType.EXPRESSION && !existingIdent)
                                    expressionMethod(current.childNodes[0]);
                            }
                            else if (current.type == Constants.TreeNodeType.EXPRESSION)
                                expressionMethod(current);
                        }
                    }
                }
                identMethod.Clear();
            }
        }
    }
}
