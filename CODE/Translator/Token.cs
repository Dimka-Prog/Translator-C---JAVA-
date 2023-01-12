using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
