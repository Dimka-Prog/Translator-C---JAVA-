using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpToJavaTranslator
{
    public class CodeGenerator
    {
        private SyntaxTree syntaxTree;
        private List<string> code;

        public CodeGenerator(SyntaxTree syntaxTree)
        {
            this.syntaxTree = syntaxTree;
        }

        public List<string> generateCode()
        {
            //Переход в узел namespace
            syntaxTree.goToRoot();
            syntaxTree.goToChild(syntaxTree.getChildrenCount() - 1);

            code = new List<string>();
            code.Add("//Program " + syntaxTree.getToken(0).value);

            code.Add("");
            syntaxTree.goToChild(0);
            traverseMember(0, true);

            print();

            return code;
        }

        private void traverseMember(int depth, bool mainClass)
        {
            if(mainClass)
            {
                for (int i = 0; i < syntaxTree.getTokensCount(); i++)
                {
                    if (syntaxTree.getToken(i).type == Constants.TokenType.PUBLIC)
                    {
                        code.Add("public ");
                    }
                }
            }
            else
            {
                addWhiteSpaces(depth);
                for (int i = 0; i < syntaxTree.getTokensCount(); i++)
                {
                    code[code.Count - 1] += syntaxTree.getToken(i).value + " ";
                }
            }

            for (int i = 0; i < syntaxTree.getChildrenCount(); i++)
            {
                syntaxTree.goToChild(i);
                if(syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.CLASS ||
                   syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.STRUCT)
                {
                    traverseClassOrStruct(depth);
                }
                else if(syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.ENUM)
                {
                    traverseEnum(depth);
                }
                syntaxTree.goToParent();
            }
        }

        private void traverseClassOrStruct(int depth)
        {
            code[code.Count - 1] += "class " + syntaxTree.getToken(0).value + " ";
            addWhiteSpaces(depth);
            code[code.Count - 1] += "{";

            for (int i = 0; i < syntaxTree.getChildrenCount(); i++)
            {
                syntaxTree.goToChild(i);
                if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.MEMBER)
                {
                    traverseMember(depth + 1, false);
                }
                syntaxTree.goToParent();
            }

            addWhiteSpaces(depth);
            code[code.Count - 1] += "}";
        }

        private void traverseEnum(int depth)
        {
            code[code.Count - 1] += "enum " + syntaxTree.getToken(0).value + " ";
            addWhiteSpaces(depth);
            code[code.Count - 1] += "{ ";
            addWhiteSpaces(depth + 1);

            for (int i = 0; i < syntaxTree.getChildrenCount(); i++)
            {
                syntaxTree.goToChild(i);
                code[code.Count - 1] += syntaxTree.getToken(0).value;
                if(syntaxTree.getChildrenCount() != 0)
                {
                    code[code.Count - 1] += " = ";
                    syntaxTree.goToChild(0);
                    traverseExpression();
                    syntaxTree.goToParent();
                }
                syntaxTree.goToParent();

                if (i != syntaxTree.getChildrenCount() - 1)
                {
                    code[code.Count - 1] += ", ";
                }
            }

            addWhiteSpaces(depth);
            code[code.Count - 1] += "}";
        }

        private void traverseExpression()
        {
            for (int i = 0; i < syntaxTree.getTokensCount(); i++)
            {
                code[code.Count - 1] += syntaxTree.getToken(i).value + " ";
            }
        }

        private void traverseMethod(int depth)
        {
            addWhiteSpaces(depth);
            for (int i = 0; i < syntaxTree.getTokensCount(); i++)
            {
                code[code.Count - 1] += syntaxTree.getToken(i) + " ";
            }
            addWhiteSpaces(depth);
            code.Add("{");
        }

        private void addWhiteSpaces(int depth)
        {
            code.Add("");
            for (int i = 0; i < depth; i++)
            {
                code[code.Count - 1] += "    ";
            }
        }

        private void print()
        {
            for (int i = 0; i < code.Count; i++)
            {
                Console.WriteLine(code[i]);
            }
        }
    }
}
