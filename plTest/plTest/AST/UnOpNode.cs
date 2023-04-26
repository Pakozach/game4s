namespace plTest.AST
{
    public class UnOpNode : ExpressionNode
    {
        public Token oper;
        public ExpressionNode operand;

        public UnOpNode(Token oper, ExpressionNode operand)
        {
            this.oper = oper;
            this.operand = operand;
        }
    }
}
