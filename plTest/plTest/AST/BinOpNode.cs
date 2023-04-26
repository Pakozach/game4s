namespace plTest.AST
{
    public class BinOpNode : ExpressionNode
    {
        public Token oper;
        public ExpressionNode leftNode;
        public ExpressionNode rightNode;

        public BinOpNode(Token oper, ExpressionNode leftNode, ExpressionNode rightNode)
        {
            this.oper = oper;
            this.leftNode = leftNode;
            this.rightNode = rightNode;
        }
    }
}
