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
            identifiersClass();
            identifiersEnum();
        }

        private void typeCompatibility(SyntaxTreeNode node)
        {

        }

        private void identifiersClass()
        {
            string identifier = "";
            string dataType = "";

            SyntaxTreeNode current = root;
            do
                current = current.childNodes[0];
            while (current.type != Constants.TreeNodeType.CLASS);

            SyntaxTreeNode mainClass = current;
            for (int numChild = 0; numChild < mainClass.childNodes.Count; numChild++)
            {
                current = mainClass;
                if (current.childNodes[numChild].type == Constants.TreeNodeType.MEMBER)
                {
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
                                        console.appendText($"[SEMANT][ERROR] : Тип '{mainClass.tokens[0].value}' уже содержит определение для '{identifier}'. Строка: {current.tokens[0].numberLine}, столбец: {current.tokens[0].numberColumn}.\n", Color.Red);
                                        break;
                                    }
                                }
                            }
                            else
                                identClass.Add(identifier, dataType);

                            if (current.childNodes != null && current.childNodes[0].type == Constants.TreeNodeType.EXPRESSION && !existingIdent)
                            {
                                current = current.childNodes[0];

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
                                            if (key.Equals(current.tokens[0].value))
                                            {
                                                unknownIdent = false;
                                                console.appendText($"[SEMANT][ERROR] : инициализатор поля не может обращаться к нестатичному полю, методу или свойству '{mainClass.tokens[0].value}.{current.tokens[0].value}'. Строка: {current.tokens[0].numberLine}, столбец: {current.tokens[0].numberColumn}.\n", Color.Red);
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
                                    console.appendText($"[SEMANT][ERROR] : использование неизвестного идентификатора '{current.tokens[0].value}'. Строка: {current.tokens[0].numberLine}, столбец: {current.tokens[0].numberColumn}.\n", Color.Red);
                                else
                                    typeCompatibility(current);
                            }
                        }
                    }
                }
            }
        }

        private void identifiersEnum()
        {

        }
    }
}
