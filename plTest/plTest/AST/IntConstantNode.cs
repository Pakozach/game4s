namespace plTest.AST
{
    public class IntConstantNode : ExpressionNode
    {
        public Token intConstant;

        public IntConstantNode(Token intConstant)
        {
            this.intConstant = intConstant;
        }
    }
}
