using System.Runtime.CompilerServices;

namespace plTest.AST
{
    public class CommandNode : ExpressionNode
    {
        public List<ExpressionNode> codeStrs = new List<ExpressionNode>();

        public void addNode(ExpressionNode node)
        {
            this.codeStrs.Add(node);            
        }              

    }
}
