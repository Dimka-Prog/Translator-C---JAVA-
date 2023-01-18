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
        private TranslationResultBus translationResultBus;
        private List<string> code;

        public CodeGenerator(SyntaxTree syntaxTree, TranslationResultBus translationResultBus)
        {
            this.syntaxTree = syntaxTree;
            this.translationResultBus = translationResultBus;
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
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.METHOD)
                {
                    traverseMethod(depth);
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.FIELD)
                {
                    traverseFieldOrDeclaration();
                    code[code.Count - 1] += ";";
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
            addWhiteSpaces(depth);
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
                    syntaxTree.goToChild(0);
                    translationResultBus.registerWarning("[GEN][WARNING] : синтаксис языка Java не " +
                                                         "поддерживает присваивание в перечислениях. Выражение " +
                                                         "будет пропущено, что может привести к некорректной " +
                                                         "работе транслированной программы.", 
                                                         syntaxTree.getToken(0));
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
            addWhiteSpaces(depth);
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
            for (int i = 0; i < syntaxTree.getTokensCount(); i++)
            {
                code[code.Count - 1] += syntaxTree.getToken(i).value + " ";
            }

            code[code.Count - 1] += "(";

            int tempPos = 0;
            for (int i = 0; i < syntaxTree.getChildrenCount(); i++)
            {
                syntaxTree.goToChild(i);
                if(syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.PARAMETER)
                {
                    if (i != 0)
                    {
                        code[code.Count - 1] += ", ";
                    }

                    for (int j = 0; j < syntaxTree.getTokensCount(); j++)
                    {
                        code[code.Count - 1] += syntaxTree.getToken(j).value + " ";
                    }
                }
                else
                {
                    syntaxTree.goToParent();
                    tempPos = i;
                    break;
                }
                syntaxTree.goToParent();
                tempPos = i;
            }
            code[code.Count - 1] += ")";

            traverseCodeBlock(depth, tempPos, syntaxTree.getChildrenCount() - 1);

            addWhiteSpaces(depth);
        }

        private void traverseFieldOrDeclaration()
        {
            for (int i = 0; i < syntaxTree.getTokensCount(); i++)
            {
                code[code.Count - 1] += syntaxTree.getToken(i).value + " ";
            }

            for (int i = 0; i < syntaxTree.getChildrenCount(); i++)
            {
                syntaxTree.goToChild(i);
                code[code.Count - 1] += syntaxTree.getToken(0).value;

                if (syntaxTree.getChildrenCount() != 0)
                {
                    code[code.Count - 1] += " = ";
                    syntaxTree.goToChild(0);
                    traverseExpression();
                    syntaxTree.goToParent();
                    syntaxTree.goToParent();
                }
                else
                {
                    syntaxTree.goToParent();
                }

                if (i != syntaxTree.getChildrenCount() - 1)
                {
                    code[code.Count - 1] += ", ";
                    
                }
            }
        }

        private void traverseCodeBlock(int depth, int begin, int end)
        {
            addWhiteSpaces(depth);
            code[code.Count - 1] += "{";

            for (int i = begin; i <= end; i++)
            {
                syntaxTree.goToChild(i);
                Console.WriteLine("ВЫПОЛНЕН ПЕРЕХОД В УЗЕЛ " + syntaxTree.getCurrentNodeType());
                if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.DECLARATION)
                {
                    addWhiteSpaces(depth + 1);
                    traverseFieldOrDeclaration();
                    code[code.Count - 1] += ";";
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.FOR)
                {
                    addWhiteSpaces(depth + 1);
                    traverseFor(depth + 1);
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.WHILE)
                {
                    addWhiteSpaces(depth + 1);
                    traverseWhile(depth + 1);
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.DO)
                {
                    addWhiteSpaces(depth + 1);
                    traverseDoWhile(depth + 1);
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.IF)
                {
                    addWhiteSpaces(depth + 1);
                    traverseIf(depth + 1);
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.SWITCH)
                {
                    addWhiteSpaces(depth + 1);
                    traverseSwitch(depth + 1);
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.EXPRESSION)
                {
                    addWhiteSpaces(depth + 1);
                    traverseExpression();
                    code[code.Count - 1] += ";";
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.BREAK)
                {
                    addWhiteSpaces(depth + 1);
                    code[code.Count - 1] += "break;";
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.CONTINUE)
                {
                    addWhiteSpaces(depth + 1);
                    code[code.Count - 1] += "continue;";
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.CASE)
                {
                    addWhiteSpaces(depth + 1);
                    code[code.Count - 1] += "case ";
                    syntaxTree.goToChild(0);
                    traverseExpression();
                    code[code.Count - 1] += ":";
                    syntaxTree.goToParent();
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.DEFAULT)
                {
                    addWhiteSpaces(depth + 1);
                    code[code.Count - 1] += "default:";
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.RETURN)
                {
                    addWhiteSpaces(depth + 1);
                    code[code.Count - 1] += "return";
                    if(syntaxTree.getChildrenCount() != 0)
                    {
                        syntaxTree.goToChild(0);
                        code[code.Count - 1] += " ";
                        traverseExpression();
                        syntaxTree.goToParent();
                    }
                    code[code.Count - 1] += ";";
                }
                syntaxTree.goToParent();
            }

            addWhiteSpaces(depth);
            code[code.Count - 1] += "}";
        }

        private void traverseFor(int depth)
        {
            code[code.Count - 1] += "for (";

            int tempPos = 0;
            for (int i = 0; i < syntaxTree.getChildrenCount(); i++)
            {
                syntaxTree.goToChild(i);
                if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.DECLARATION)
                {
                    traverseFieldOrDeclaration();
                }
                else if (syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.PARAMETER)
                {
                    if (i != 0)
                    {
                        code[code.Count - 1] += "; ";
                    }

                    syntaxTree.goToChild(0);
                    traverseExpression();
                    syntaxTree.goToParent();
                }
                else
                {
                    syntaxTree.goToParent();
                    tempPos = i;
                    break;
                }
                syntaxTree.goToParent();
                tempPos = i;
            }
            code[code.Count - 1] += ")";

            traverseCodeBlock(depth, tempPos, syntaxTree.getChildrenCount() - 1);

            addWhiteSpaces(depth);
        }

        private void traverseWhile(int depth)
        {
            code[code.Count - 1] += "while (";

            syntaxTree.goToChild(0);
            traverseExpression();
            syntaxTree.goToParent();

            code[code.Count - 1] += ")";

            traverseCodeBlock(depth, 1, syntaxTree.getChildrenCount() - 1);

            addWhiteSpaces(depth);
        }

        private void traverseDoWhile(int depth)
        {
            code[code.Count - 1] += "do";

            traverseCodeBlock(depth, 0, syntaxTree.getChildrenCount() - 2);

            code[code.Count - 1] += "while (";
            syntaxTree.goToChild(syntaxTree.getChildrenCount() - 1);
            traverseExpression();
            code[code.Count - 1] += ");";
            syntaxTree.goToParent();

            addWhiteSpaces(depth);
        }

        private void traverseIf (int depth)
        {
            code[code.Count - 1] += "if (";
            syntaxTree.goToChild(0);
            traverseExpression();
            code[code.Count - 1] += ")";
            syntaxTree.goToParent();

            syntaxTree.goToChild(syntaxTree.getChildrenCount() - 1);
            if(syntaxTree.getCurrentNodeType() == Constants.TreeNodeType.ELSE)
            {
                syntaxTree.goToParent();
                traverseCodeBlock(depth, 1, syntaxTree.getChildrenCount() - 2);
                addWhiteSpaces(depth);
                syntaxTree.goToChild(syntaxTree.getChildrenCount() - 1);
                traverseElse(depth);
                syntaxTree.goToParent();
            }
            else
            {
                syntaxTree.goToParent();
                traverseCodeBlock(depth, 1, syntaxTree.getChildrenCount() - 1);
            }

            addWhiteSpaces(depth);
        }

        private void traverseElse(int depth)
        {
            code[code.Count - 1] += "else";
            traverseCodeBlock(depth, 0, syntaxTree.getChildrenCount() - 1);
        }

        private void traverseSwitch(int depth)
        {
            code[code.Count - 1] += "switch (";
            syntaxTree.goToChild(0);
            traverseExpression();
            code[code.Count - 1] += ")";
            syntaxTree.goToParent();

            traverseCodeBlock(depth, 1, syntaxTree.getChildrenCount() - 1);
            addWhiteSpaces(depth);
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
