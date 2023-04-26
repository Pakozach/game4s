namespace plTest
{

    public enum LANGTYPES
    {
        tCONST,
        tVAR,
        tDELIM,
        tFUNC,
        tBINOP,
        tSINGLEOP,
        tTRASH,
        tBRANCH,
        tLOOP
    }
    public class TokenType
    {
        public string name;
        public LANGTYPES langtype;
        public string regex;


        public TokenType(string name, LANGTYPES langtype, string regex)
        {
            this.name = name;
            this.langtype = langtype;
            this.regex = regex;
        }

        static public TokenType[] tokenTypeLilst = {
                new TokenType("NUMBER", LANGTYPES.tCONST,  "[0-9]+"),
                new TokenType("VARIABLE", LANGTYPES.tVAR, "[a-z]+"),
                new TokenType("FILLERS", LANGTYPES.tTRASH,"[ \\n\\t\\r]"),
                new TokenType(";", LANGTYPES.tDELIM,";"),
                new TokenType("EQ", LANGTYPES.tBINOP,":="),
                new TokenType("LOG", LANGTYPES.tSINGLEOP,"LOG"),
                new TokenType("ATK", LANGTYPES.tSINGLEOP,"ATK"),
                new TokenType("POW_ATK", LANGTYPES.tSINGLEOP,"POW_ATK"),
                new TokenType("DEF", LANGTYPES.tSINGLEOP,"DEF"),
                new TokenType("DODGE", LANGTYPES.tSINGLEOP,"DODGE"),
                new TokenType("ADD", LANGTYPES.tBINOP, "\\+"),
                new TokenType("MUL", LANGTYPES.tBINOP, "\\*"),
                new TokenType("SUB", LANGTYPES.tBINOP, "\\-"),
                new TokenType("IF", LANGTYPES.tBRANCH, "IF"),
                new TokenType("ELSE", LANGTYPES.tBRANCH, "ELSE"),
                new TokenType("WHILE", LANGTYPES.tLOOP, "WHILE"),
                new TokenType("<=", LANGTYPES.tBRANCH, "\\<="),
                new TokenType("<", LANGTYPES.tBRANCH, "\\=="),
                new TokenType("<", LANGTYPES.tBRANCH, "\\<="),
                new TokenType(">", LANGTYPES.tBRANCH, "\\>="),
                new TokenType("<", LANGTYPES.tBRANCH, "\\<"),                
                new TokenType(">", LANGTYPES.tBRANCH, "\\>"),
                new TokenType("(", LANGTYPES.tDELIM, "\\("),
                new TokenType(")", LANGTYPES.tDELIM, "\\)"),
                new TokenType("{", LANGTYPES.tDELIM, "\\{"),
                new TokenType("}", LANGTYPES.tDELIM, "\\}"),
                new TokenType("COMMENT", LANGTYPES.tTRASH, "#")
        };

    }

}