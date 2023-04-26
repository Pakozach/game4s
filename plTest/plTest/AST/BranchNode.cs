using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plTest.AST
{
    internal class BranchNode : ExpressionNode
    {
        public ExpressionNode compare;
        public ExpressionNode ifBody;
        public ExpressionNode elseBody;

        public BranchNode (ExpressionNode compare, ExpressionNode ifBody, ExpressionNode elseBody)
        {
            this.compare = compare;
            this.ifBody = ifBody;
            this.elseBody = elseBody;
        }
    }
}
