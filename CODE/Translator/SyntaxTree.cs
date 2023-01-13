using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace CSharpToJavaTranslator
{
    public class SyntaxTreeNode
    {
        public List<SyntaxTreeNode> childNodes;
        public SyntaxTreeNode parentNode;
        public Constants.TreeNodeType type;
        public List<Token> tokens;

        public SyntaxTreeNode(Constants.TreeNodeType type)
        {
            this.type = type;
            this.childNodes = null;
            this.parentNode = null;
            this.tokens = null;
        }

        public void print(int depth)
        {
            for (int i = 0; i < depth - 1; i++)
            {
                Console.Write(" |  ");
            }
            if (depth >= 1) Console.Write(" |--");
            Console.WriteLine();
            for (int i = 0; i < depth - 1; i++)
            {
                Console.Write(" |  ");
            }
            if (depth >= 1) Console.Write(" |--");
            Console.WriteLine(this.type);
            for (int i = 0; i < depth - 1; i++)
            {
                Console.Write(" |  ");
            }
            if (depth >= 1) Console.Write(" |--");
            Console.Write("Параметры: ");

            if (this.tokens != null)
            {
                foreach (Token t in this.tokens)
                {
                    Console.Write(t.value + " ");
                }
            }
            Console.WriteLine();

            for (int i = 0; i < depth - 1; i++)
            {
                Console.Write(" |  ");
            }
            if (depth >= 1) Console.Write(" |--");
            if (this.childNodes != null)
            {
                Console.WriteLine("Потомки (" + this.childNodes.Count + "):");
                foreach (SyntaxTreeNode c in this.childNodes)
                {
                    c.print(depth + 1);
                }
            }
            else
            {
                Console.WriteLine("Нет потомков.");
            }
        }
    }

    /// <summary>
    /// Класс синтаксического дерева.
    /// Каждый узел содержит указатель на предка,
    /// список потомков и тип. 
    /// </summary>
    public class SyntaxTree
    {
        private SyntaxTreeNode root;
        private SyntaxTreeNode ptr;

        public SyntaxTree()
        {
            root = new SyntaxTreeNode(Constants.TreeNodeType.PROGRAM_BEGINNING);
            root.parentNode = null;
            root.childNodes = null;
            root.tokens = null;

            ptr = root;
        }

        public void goToParent()
        {
            if (ptr.parentNode == null)
            {
                return;
            }
            ptr = ptr.parentNode;
        }

        public void goToChild(int number)
        {
            if (ptr.childNodes == null) return;
            if (number > ptr.childNodes.Count - 1) return;

            ptr = ptr.childNodes[number];
        }

        public void appendAndGoToChild(Constants.TreeNodeType type)
        {
            SyntaxTreeNode node = new SyntaxTreeNode(type);

            if (ptr.childNodes == null)
            {
                ptr.childNodes = new List<SyntaxTreeNode>();
            }
            ptr.childNodes.Add(node);
            node.parentNode = ptr;
            ptr = node;
        }

        public Constants.TreeNodeType getCurrentNodeType()
        {
            return ptr.type;
        }

        public void appendToken(Token token)
        {
            if (ptr.tokens == null)
            {
                ptr.tokens = new List<Token>();
            }
            ptr.tokens.Add(token);
        }

        public void print()
        {
            root.print(0);
        }
    }
}