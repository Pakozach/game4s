using System.Text.RegularExpressions;


namespace plTest
{
    public class Lexer
    {
        string code;
        int pos = 0;
        public List<Token> tokenList = new List<Token>();

        public Lexer(string code)
        {
            this.code = code;
        }

        public List<Token> LexAnalizator()
        {
            while (this.NextToken()) { }
            foreach (Token ftoken in this.tokenList)
            {
                Console.WriteLine($"Token: {ftoken.text}  type: {ftoken.type.name}  pos: {ftoken.pos.ToString()}");
            }
            return tokenList;
        }

        bool NextToken()
        {

            if (this.pos >= this.code.Length)
            {
                return false;
            }

            for (int i = 0; i < TokenType.tokenTypeLilst.Length; i++)
            {
                TokenType tokenType = TokenType.tokenTypeLilst[i];
                Regex regex = new Regex("^" + tokenType.regex);
                Match m = regex.Match(this.code.Substring(this.pos));
                if (m.Success)
                {
                    if (tokenType.name != "FILLERS")
                    {
                        Token token = new Token(tokenType, m.Value, this.pos);
                        this.tokenList.Add(token);
                    }
                    this.pos += m.Length;

                    return true;
                }
            }
            Console.WriteLine($"На позиции { this.pos } обнаружена ошибка");
            return false;
        }
    }
}

