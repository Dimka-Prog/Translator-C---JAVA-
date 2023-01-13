using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpToJavaTranslator
{
    public class SemanticAnalyzer
    {
        private readonly SyntaxTree syntTree;
        private SyntaxTree current;

        public SemanticAnalyzer(SyntaxTree syntTree)
        {
            this.syntTree = syntTree;
        }

        public void semanticAnalysis()
        {
            if (syntTree != null)
            {

            }
        }
    }
}
