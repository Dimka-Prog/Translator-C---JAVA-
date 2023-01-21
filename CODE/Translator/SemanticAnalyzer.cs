using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

            identifiersClass(nodeClass);
            identifiersEnum(nodeClass);
            identifiersMethod(nodeClass);
        }

        private void typeCompatibility(SyntaxTreeNode node, string identifierLocation)
        {

        }

        private void checkExpression(SyntaxTreeNode current, string messageError)
        {
            int numberToken = current.tokens.Count - 1;
            bool unknownIdent = true;
            string expression = "";

            if (current.tokens.Count <= 2)
            {
                for (int numToken = 0; numToken < current.tokens.Count; numToken++)
                    expression += current.tokens[numToken].value;

                double number;
                if (!double.TryParse(expression, out number) && expression != "false" && expression != "true")
                {
                    foreach (string key in identClass.Keys)
                    {
                        if (key.Equals(current.tokens[numberToken].value))
                        {
                            unknownIdent = false;
                            resultBus.registerError(messageError, current.tokens[numberToken]);
                            break;
                        }
                    }
                }
                else
                    unknownIdent = false;
            }
            else
                unknownIdent = false;

            if (unknownIdent)
                resultBus.registerError($"[SEMANT][ERROR] : использование неизвестного идентификатора '{current.tokens[numberToken].value}'.", current.tokens[numberToken]);
            else
                typeCompatibility(current, "class");
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

                    if (current.childNodes[0].type == Constants.TreeNodeType.PARAMETER)
                    {
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
                                    resultBus.registerError($"[SEMANT][ERROR] : тип '{nodeClass.tokens[0].value}' уже содержит определение для '{identifier}'.", current.tokens[0]);
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
                            checkExpression(current, $"[SEMANT][ERROR] : инициализатор поля не может обращаться к нестатичному полю, методу или свойству '{nodeClass.tokens[0].value}.{current.tokens[current.tokens.Count - 1].value}'.");
                        }
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

                                int numberToken = current.tokens.Count - 1;
                                bool unknownIdent = true;
                                string identifier = ""; 

                                if (current.tokens.Count <= 2)
                                {
                                    for (int numToken = 0; numToken < current.tokens.Count; numToken++)
                                        identifier += current.tokens[numToken].value;

                                    double number;
                                    if (!double.TryParse(identifier, out number) && identifier != "false" && identifier != "true")
                                    {
                                        foreach (string key in identEnum.Keys)
                                        {
                                            if (key.Equals(current.tokens[numberToken].value))
                                            {
                                                unknownIdent = false;
                                                break;
                                            }
                                        }

                                        var lastIdent = identEnum.Keys.Last();
                                        if (lastIdent.Equals(current.tokens[numberToken].value))
                                        {
                                            unknownIdent = false;
                                            resultBus.registerError($"[SEMANT][ERROR] : при оценке постоянного значения для '{nodeClass.tokens[0].value}.{nameEnum}.{lastIdent}' ипользуется циклическое определение.", current.tokens[numberToken]);
                                        }
                                    }
                                    else
                                        unknownIdent = false;
                                }
                                else
                                    unknownIdent = false;

                                if (unknownIdent)
                                    resultBus.registerError($"[SEMANT][ERROR] : использование неизвестного идентификатора '{current.tokens[numberToken].value}'.", current.tokens[numberToken]);
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
            Dictionary<string, string> identMethod = new Dictionary<string, string>();

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
                                    checkExpression(current, $"[SEMANT][ERROR] : значение параметра по умолчанию для '{identifier}' должно быть константой.");
                                }
                            }
                        }
                    }
                }
                identMethod.Clear();
            }
        }
    }
}
