using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CSharpToJavaTranslator
{
    public class TranslationResultBus
    {
        private CustomRichTextBox console;
        List<Token> errors, warnings;

        public TranslationResultBus(CustomRichTextBox console)
        {
            this.console = console;
            this.errors = null;
            this.warnings = null;
        }

        public int getErrorCount()
        {
            return errors == null ? 0 : errors.Count;
        }

        public void registerUnexpectedTokenError(string[] expectedValues, Token token)
        {
            string message = "[SYNTAX][ERROR] : ожидалось ";

            foreach (string s in expectedValues)
            {
                message += "\"" + s + "\", ";
            }

            message += "встречено: \"" + token.value + "\".";

            registerError(message, token);
        }

        public void registerError(string message, Token token)
        {
            message += " Строка: " + token.numberLine +
                       ", столбец: " + (token.numberColumn + 1) + ".\n";
            console.appendText(message, Color.Red);

            if (errors == null)
            {
                errors = new List<Token>();
            }
            errors.Add(token);
        }

        public void registerWarning(string message, Token token)
        {
            string message2 = message;
            message2 += " Строка: " + token.numberLine + ", столбец: " + (token.numberColumn + 1) + ".\n";

            console.appendText(message2, Color.Orange);

            if (warnings == null)
            {
                warnings = new List<Token>();
            }
            warnings.Add(token);
        }

        public void summarizeTranslation()
        {
            int errorsCount = errors == null ? 0 : errors.Count;
            int warningsCount = warnings == null ? 0 : warnings.Count;

            console.appendText("[INFO] : ошибок: " + errorsCount + ", предупреждений: " + warningsCount + ".\n", Color.Black);
            if(errorsCount > 0)
            {
                console.appendText("[INFO] : " + System.DateTime.Now + " - трансляция завершилась с ошибками.\n", Color.Black);
            }
            else
            {
                console.appendText("[INFO] : " + System.DateTime.Now + " - трансляция завершилась успешно.\n", Color.Black);
            }
            console.appendText("=======================================================\n", Color.Black);
        }

        public void highlight(CustomRichTextBox input)
        {
            if (warnings != null)
            {
                foreach (Token token in warnings)
                {
                    int position = 0;
                    for (int i = 0; i < token.numberLine - 1; i++)
                    {
                        position += input.getInnerTextBox().Lines[i].Length + 1;
                    }

                    input.highlightText(token.numberColumn + position, token.value.Length, Color.Orange);
                }
            }
            if (errors != null)
            {
                foreach(Token token in errors)
                {
                    int position = 0;
                    for(int i = 0; i < token.numberLine - 1; i++)
                    {
                        position += input.getInnerTextBox().Lines[i].Length + 1;
                    }

                    input.highlightText(token.numberColumn + position, token.value.Length, Color.Red);
                }
            }
        }
    }
}
