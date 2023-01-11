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
            this.root = new SyntaxTreeNode(Constants.TreeNodeType.PROGRAM_BEGINNING);
            this.root.parentNode = null;
            this.root.childNodes = null;
            this.root.tokens = null;

            this.ptr = this.root;
        }

        public void goToParent()
        {
            if (this.ptr.parentNode == null)
            {
                return;
            }
            this.ptr = this.ptr.parentNode;
        }

        public void goToChild(int number)
        {
            if (this.ptr.childNodes == null) return;
            if (number > this.ptr.childNodes.Count - 1) return;

            this.ptr = this.ptr.childNodes[number];
        }

        public void appendAndGoToChild(Constants.TreeNodeType type)
        {
            SyntaxTreeNode node = new SyntaxTreeNode(type);

            if (this.ptr.childNodes == null)
            {
                this.ptr.childNodes = new List<SyntaxTreeNode>();
            }
            this.ptr.childNodes.Add(node);
            node.parentNode = this.ptr;
            this.ptr = node;
        }

        public void appendToken(Token token)
        {
            if (this.ptr.tokens == null)
            {
                this.ptr.tokens = new List<Token>();
            }
            this.ptr.tokens.Add(token);
        }

        public void print()
        {
            this.root.print(0);
        }
    }
}