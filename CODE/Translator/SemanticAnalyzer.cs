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
        private CustomRichTextBox console;
        
        private Dictionary<string, string> identClass = new Dictionary<string, string>();
        private Dictionary<string, string> identEnum = new Dictionary<string, string>();

        public SemanticAnalyzer(SyntaxTree syntTree, CustomRichTextBox console)
        {
            root = syntTree.root;
            this.console = console;
        }

        public void semanticAnalysis()
        {
            usingUndeclaredIdentifier();
        }

        private void usingUndeclaredIdentifier()
        {
            SyntaxTreeNode nodeClass = root;
            do
                nodeClass = nodeClass.childNodes[0];
            while (nodeClass.type != Constants.TreeNodeType.CLASS);

            identifiersClass(nodeClass);
            identifiersEnum(nodeClass);
        }

        private void typeCompatibility(SyntaxTreeNode node)
        {

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
                    dataType = current.tokens[0].value;

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
                                    console.appendText($"[SEMANT][ERROR] : Тип '{nodeClass.tokens[0].value}' уже содержит определение для '{identifier}'. Строка: {current.tokens[0].numberLine}, столбец: {current.tokens[0].numberColumn}.\n", Color.Red);
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

                            int numberToken = current.tokens.Count - 1;
                            bool unknownIdent = true;
                            identifier = "";

                            if (current.tokens.Count <= 2)
                            {
                                for (int numToken = 0; numToken < current.tokens.Count; numToken++)
                                    identifier += current.tokens[numToken].value;

                                double number;
                                if (!double.TryParse(identifier, out number))
                                {
                                    foreach (string key in identClass.Keys)
                                    {
                                        if (key.Equals(current.tokens[numberToken].value))
                                        {
                                            unknownIdent = false;
                                            console.appendText($"[SEMANT][ERROR] : инициализатор поля не может обращаться к нестатичному полю, методу или свойству '{nodeClass.tokens[0].value}.{current.tokens[numberToken].value}'. Строка: {current.tokens[numberToken].numberLine}, столбец: {current.tokens[numberToken].numberColumn}.\n", Color.Red);
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
                                console.appendText($"[SEMANT][ERROR] : использование неизвестного идентификатора '{current.tokens[numberToken].value}'. Строка: {current.tokens[numberToken].numberLine}, столбец: {current.tokens[numberToken].numberColumn}.\n", Color.Red);
                            else
                                typeCompatibility(current);
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
                                        console.appendText($"[SEMANT][ERROR] : тип '{nodeClass.tokens[0].value}.{nameEnum}' уже содержит определение для '{current.tokens[0].value}'. Строка: {current.tokens[0].numberLine}, столбец: {current.tokens[0].numberColumn}.\n", Color.Red);
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
                                    if (!double.TryParse(identifier, out number))
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
                                            console.appendText($"[SEMANT][ERROR] : при оценке постоянного значения для '{nodeClass.tokens[0].value}.{nameEnum}.{lastIdent}' ипользуется циклическое определение. Строка: {current.tokens[numberToken].numberLine}, столбец: {current.tokens[numberToken].numberColumn}.\n", Color.Red);
                                        }
                                    }
                                    else
                                        unknownIdent = false;
                                }
                                else
                                    unknownIdent = false;

                                if (unknownIdent)
                                    console.appendText($"[SEMANT][ERROR] : использование неизвестного идентификатора '{current.tokens[numberToken].value}'. Строка: {current.tokens[numberToken].numberLine}, столбец: {current.tokens[numberToken].numberColumn}.\n", Color.Red);
                                else
                                    typeCompatibility(current);
                            }
                        }
                    }
                }
            }
        }
    }
}
