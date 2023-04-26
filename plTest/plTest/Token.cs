namespace plTest
{
    public class Token
    {
        public TokenType type;
        public string text;
        public int pos;

        public Token(TokenType type, string text, int pos)
        {
            this.type = type;
            this.text = text;
            this.pos = pos;
        }
    }
}
