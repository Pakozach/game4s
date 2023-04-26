namespace plTest.AST
{
    public class VariableNode : ExpressionNode
    {
        public Token variable;
        public VariableNode(Token variable)
        {
            this.variable = variable;
        }
    }
}
