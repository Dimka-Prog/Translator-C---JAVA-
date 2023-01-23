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

        private void typeCompatibility(SyntaxTreeNode node, string identifierLocation)
        {

        }

        private void checkExpressionClass(SyntaxTreeNode current, string identifierLocation)
        {
            bool error = false;
            string expression;

            for (int numToken = 0; numToken < current.tokens.Count; numToken++)
            {
                expression = current.tokens[numToken].value;

                if (expression == "this")
                {
                    error = true;
                    resultBus.registerError($"[SEMANT][ERROR] : ключевое слово '{expression}' не пременимо в текущем контексте.", current.tokens[numToken]);
                    numToken += 2;
                }
                else if (Regex.IsMatch(expression, @"^[a-zA-Z0-9_]+$") && expression != "new" && current.tokens[numToken].type != Constants.TokenType.STRING &&
                    expression != "int" && expression != "double" && expression != "string" && expression != "char") // Только буквы, цифры и подчеркивание
                {
                    double number;
                    if (!double.TryParse(expression, out number) && expression != "false" && expression != "true")
                    {
                        bool unknownIdent = true;
                        foreach (string key in identClass.Keys)
                        {
                            if (key.Equals(expression))
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
                            resultBus.registerError($"[SEMANT][ERROR] : использование неизвестного идентификатора '{expression}'.", current.tokens[numToken]);
                        } 
                    }
                    else if (expression == "false" || expression == "true")
                    {
                        string tokenBefore = "";
                        string tokenAfter = "";

                        if ((numToken - 1) >= 0)
                            tokenBefore = current.tokens[numToken - 1].value;

                        if ((numToken + 1) < current.tokens.Count)
                            tokenAfter = current.tokens[numToken + 1].value;

                        if (tokenBefore != "" && !Regex.IsMatch(tokenBefore, @"^[a-zA-Z0-9_]+$"))
                        {
                            error = true;
                            resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenBefore}' невозможно применить к операнду типа 'bool'", current.tokens[numToken]);
                        }
                        else if (tokenAfter != "" && !Regex.IsMatch(tokenAfter, @"^[a-zA-Z0-9_]+$"))
                        {
                            error = true;
                            resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenAfter}' невозможно применить к операнду типа 'bool'", current.tokens[numToken]);
                        }
                    }
                }
                else if (current.tokens[numToken].type == Constants.TokenType.STRING)
                {
                    string tokenBefore = "";
                    string tokenAfter = "";

                    if ((numToken - 2) >= 0)
                        tokenBefore = current.tokens[numToken - 2].value;

                    if ((numToken + 2) < current.tokens.Count)
                        tokenAfter = current.tokens[numToken + 2].value;

                    if (tokenBefore != "" && tokenBefore != "+")
                    {
                        error = true;
                        resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenBefore}' невозможно применить к операнду типа 'string'", current.tokens[numToken]);
                    }
                    if (tokenAfter != "" && tokenAfter != "+")
                    {
                        error = true;
                        resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenAfter}' невозможно применить к операнду типа 'string'", current.tokens[numToken]);
                    }
                }
            }

            if (!error)
                typeCompatibility(current, identifierLocation);
        }

        private bool checkExpressionMethod(string expression)
        {
            bool unknownIdent = true;
            double number;
            if (!double.TryParse(expression, out number) && expression != "false" && expression != "true")
            {
                foreach (string key in identMethod.Keys)
                {
                    if (key.Equals(expression))
                    {
                        unknownIdent = false;
                        break;
                    }
                }

                if (unknownIdent)
                {
                    foreach (string key in identClass.Keys)
                    {
                        if (key.Equals(expression))
                        {
                            unknownIdent = false;
                            break;
                        }
                    }
                }
            }
            else
                unknownIdent = false;

            return unknownIdent;
        }

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
                        checkExpressionClass(current, "class");
                    }

                }
            }
        }

        private void identifiersEnum(SyntaxTreeNode nodeClass)
        {
            string nameEnum = "";

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

                                int lastToken = current.tokens.Count - 1;
                                bool unknownIdent = true;
                                string expression = current.tokens[lastToken].value;

                                if (current.tokens.Count <= 2)
                                {
                                    double number;
                                    if (!double.TryParse(expression, out number) && expression != "false" && expression != "true")
                                    {
                                        foreach (string key in identEnum.Keys)
                                        {
                                            if (key.Equals(expression))
                                            {
                                                unknownIdent = false;
                                                break;
                                            }
                                        }

                                        var lastIdent = identEnum.Keys.Last();
                                        if (lastIdent.Equals(expression))
                                        {
                                            unknownIdent = false;
                                            resultBus.registerError($"[SEMANT][ERROR] : при оценке постоянного значения для '{nodeClass.tokens[0].value}.{nameEnum}.{lastIdent}' ипользуется циклическое определение.", current.tokens[lastToken]);
                                        }
                                    }
                                    else
                                        unknownIdent = false;
                                }
                                else
                                    unknownIdent = false;

                                if (unknownIdent)
                                    resultBus.registerError($"[SEMANT][ERROR] : использование неизвестного идентификатора '{expression}'.", current.tokens[lastToken]);
                                else
                                    typeCompatibility(current, "enum");
                            }
                        }
                    }
                }
            }
        }

        private void identifiersMethod(SyntaxTreeNode nodeClass)
        {
            string identifier = "";
            string dataTypeIdentifier = "";
            string dataTypeMethod = "";

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
                                    checkExpressionClass(current, "paramMethod");
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
                                {
                                    current = current.childNodes[0];

                                    int lastToken = current.tokens.Count - 1;
                                    bool unknownIdent = true;
                                    string expression = current.tokens[lastToken].value;

                                    if (current.tokens.Count <= 2)
                                    {
                                            unknownIdent = checkExpressionMethod(expression);
                                    }
                                    else if (current.tokens.Count == 3 && current.tokens[0].value == "this")
                                    {
                                        double number;
                                        if (!double.TryParse(expression, out number))
                                        {
                                            foreach (string key in identClass.Keys)
                                            {
                                                if (key.Equals(expression))
                                                {
                                                    unknownIdent = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                        unknownIdent = false;

                                    if (unknownIdent)
                                        resultBus.registerError($"[SEMANT][ERROR] : использование неизвестного идентификатора '{expression}'.", current.tokens[lastToken]);
                                    else
                                        typeCompatibility(current, "method");
                                }
                            }
                            else if (current.type == Constants.TreeNodeType.EXPRESSION)
                            {
                                string assignment = current.childNodes[0].tokens[current.childNodes[0].tokens.Count - 1].value;
                                bool unknownIdent = false;

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
                                                unknownIdent = true;
                                                break;
                                            }
                                        }

                                        if (!unknownIdent)
                                            resultBus.registerError($"[SEMANT][ERROR] : использование неизвестного идентификатора '{identifier}'.", current.tokens[numToken]);
                                        else
                                            unknownIdent = false;
                                    }
                                    else if (Regex.IsMatch(identifier, @"^[a-zA-Z0-9_]+$") && identifier != "new" && current.tokens[numToken].type != Constants.TokenType.STRING &&
                                             identifier != "int" && identifier != "double" && identifier != "string" && identifier != "char")
                                    {
                                        if (checkExpressionMethod(identifier))
                                        {
                                            unknownIdent = true;
                                            resultBus.registerError($"[SEMANT][ERROR] : использование неизвестного идентификатора '{identifier}'.", current.tokens[numToken]);
                                        }
                                        else if (identifier == "false" || identifier == "true")
                                        {
                                            string tokenBefore = "";
                                            string tokenAfter = "";

                                            if ((numToken - 1) >= 0)
                                                tokenBefore = current.tokens[numToken - 1].value;

                                            if ((numToken + 1) < current.tokens.Count)
                                                tokenAfter = current.tokens[numToken + 1].value;

                                            if (tokenBefore != "" && !Regex.IsMatch(tokenBefore, @"^[a-zA-Z0-9_]+$"))
                                            {
                                                unknownIdent = true;
                                                resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenBefore}' невозможно применить к операнду типа 'bool'", current.tokens[numToken]);
                                            }
                                            else if (tokenAfter != "" && !Regex.IsMatch(tokenAfter, @"^[a-zA-Z0-9_]+$"))
                                            {
                                                unknownIdent = true;
                                                resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenAfter}' невозможно применить к операнду типа 'bool'", current.tokens[numToken]);
                                            }
                                            else if (assignment != "=")
                                            {
                                                unknownIdent = true;
                                                resultBus.registerError($"[SEMANT][ERROR] : оператор '{assignment}' невозможно применить к операнду типа 'bool'", current.tokens[numToken]);
                                            }
                                        }
                                    }
                                    else if (current.tokens[numToken].type == Constants.TokenType.STRING)
                                    {
                                        string tokenBefore = "";
                                        string tokenAfter = "";

                                        if ((numToken - 2) >= 0)
                                            tokenBefore = current.tokens[numToken - 2].value;

                                        if ((numToken + 2) < current.tokens.Count)
                                            tokenAfter = current.tokens[numToken + 2].value;

                                        if (tokenBefore != "" && tokenBefore != "+")
                                        {
                                            unknownIdent = true;
                                            resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenBefore}' невозможно применить к операнду типа 'string'", current.tokens[numToken]);
                                        }
                                        if (tokenAfter != "" && tokenAfter != "+")
                                        {
                                            unknownIdent = true;
                                            resultBus.registerError($"[SEMANT][ERROR] : оператор '{tokenAfter}' невозможно применить к операнду типа 'string'", current.tokens[numToken]);
                                        }
                                    }
                                }

                                if (!unknownIdent)
                                    typeCompatibility(current, "method");
                            }
                        }
                    }
                }
                identMethod.Clear();
            }
        }
    }
}
