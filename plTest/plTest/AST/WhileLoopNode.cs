using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plTest.AST
{
    internal class WhileLoopNode : ExpressionNode
    {
        public ExpressionNode compare;
        public ExpressionNode body;
        

        public WhileLoopNode(ExpressionNode compare, ExpressionNode body)
        {
            this.compare = compare;
            this.body = body;            
        }
    }
}
