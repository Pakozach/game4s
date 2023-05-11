using Microsoft.VisualBasic;
using plTest.AST;
using System;
using System.Xml.Linq;

namespace plTest
{
    internal class Parser
    {
        public List<Token> tokens = new List<Token>();
        int pos = 0;
        
        // динамический список вида "ИмяПеременной, значение"
        public List<VarScope> scope = new List<VarScope>();

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        
        Token match(string expectedType)
        {
            string[] aExpType = expectedType.Split("|");
            
            if (this.pos < this.tokens.Count)
            {
                Token currentToken = this.tokens[this.pos];
                if (Array.Find(aExpType, element=> element==currentToken.type.name)!=null)
                {
                    this.pos += 1;
                    return currentToken;
                }
            }
            return null;
        }
        
        
        Token require(string expectedType)
        {
            Token currentToken = this.tokens[this.pos];
            Token token = this.match(expectedType);
            if (token is null)
            {
                Console.WriteLine($"Error:на позиции {this.pos} ожидается '{expectedType}', а там {currentToken.type}");
                System.Environment.Exit(0);
            }
            return token;
        }

        ExpressionNode parseVariableOrNumber()
        {
            Token number = this.match("NUMBER");
            if (number != null)
            {
                return new IntConstantNode(number);
            }
            Token variable = this.match("VARIABLE");
            if (variable != null)
            {
                return new VariableNode(variable);
            }
            Console.WriteLine($"Error: Ожидается переменная или число на {this.pos} позиции");
            System.Environment.Exit(0);
            return null;
        }

        ExpressionNode parseUnOp()
        {
            Token operatorUn = this.match("LOG|ATK|POW_ATK|DEF|DODGE");
            if (operatorUn != null)
            {
                return new UnOpNode(operatorUn, this.parseFormula());
            }
            Console.WriteLine($"Error: Ожидается унарный оператор на {this.pos} позиции");
            System.Environment.Exit(0);
            return null;
        }


        ExpressionNode parseBrackets()
        {
            if (this.match("(") != null)
            {
                ExpressionNode node = this.parseFormula();
                this.require(")");
                return node;
            }
            else
            {
                return this.parseVariableOrNumber();
            }
        }


        ExpressionNode parseFormula()
        {
            ExpressionNode leftNode = this.parseBrackets();
            Token oper = this.match("ADD|SUB|MUL|<|>|<=|>=|==");
            while (oper != null)
            {
                ExpressionNode rightNode = this.parseBrackets();
                leftNode = new BinOpNode(oper, leftNode, rightNode);
                oper = this.match("ADD|SUB|MUL|<|>|<=|>=|==");
            }
            return leftNode;
        }

        ExpressionNode parseExpression()
        {
            // анализируем обычную строчку кода

            // вдруг в ней закрывающий блок? тогда вернем NULL
            if (this.match("}") != null)
            {
                return null;
            }

            // может это переменная с присвоением?
            // немного странный вариант.
            if (this.match("VARIABLE") == null)
            {
                // нет, значит унарная операция ?            
                ExpressionNode unOpNode = this.parseUnOp();
                return unOpNode;
            }
            this.pos -= 1;
            

            // Значит, переменная
            ExpressionNode variableNode = this.parseVariableOrNumber();            

            // присвоение?
            Token assignOperator = this.match("EQ");
            if (assignOperator != null)
            {
                ExpressionNode rightFormulaNode = this.parseFormula();
                ExpressionNode binaryNode = new BinOpNode(assignOperator, variableNode, rightFormulaNode);
                return binaryNode;
            }
            Console.WriteLine($"Error: После переменной ожидается оператор присвоения на позиции { this.pos }");
            System.Environment.Exit(0);
            return null;
        }


        public ExpressionNode parseCode()
        {
            CommandNode root = new CommandNode();
            if (this.match("{") == null)
            {
                this.pos -= 1;
                return null;
            }


            while (this.pos < this.tokens.Count)
            {

                if (this.match("IF") != null)
                {
                    ExpressionNode comparegNode = this.parseFormula();

                    ExpressionNode ifNode = this.parseCode();
                    
                    ExpressionNode elseNode = null;
                    if (this.match("ELSE") != null)
                    {
                        elseNode = this.parseCode();
                    }
                    
                    ExpressionNode BranchNode = new BranchNode(comparegNode, ifNode, elseNode);
                    root.addNode(BranchNode);
                }

                if (this.match("WHILE") != null)
                {
                    ExpressionNode comparegNode = this.parseFormula();
                    ExpressionNode bodyNode = this.parseCode();
                                        
                    ExpressionNode whileNode = new WhileLoopNode(comparegNode, bodyNode);
                    root.addNode(whileNode);
                }

                ExpressionNode codeStringNode = this.parseExpression();
                if(codeStringNode!= null)
                { 
                    this.require(";|}");
                    root.addNode(codeStringNode);
                }
                if (this.match("}") != null) 
                { 
                      return root; 
                }                

            }
            return root;
        }


        // -----------------------------------------------------------------------------

        public dynamic run (ExpressionNode node, Character player)
        {
            
            if (node is IntConstantNode) {
                return int.Parse((node as IntConstantNode).intConstant.text);
            }

            if (node is UnOpNode)
            {
                UnOpNode unode = (node as UnOpNode);
                switch (unode.oper.type.name) 
                {
                    case "LOG":
                        Console.WriteLine(this.run((node as UnOpNode).operand, player));
                        return null;
                    case "DEF":
                        if (this.run((node as UnOpNode).operand, player) == 0)
                            player.is_def_head = 1;
                        else
                            player.is_def_body = 1;
                        //Console.WriteLine($"Доджим {this.run((node as UnOpNode).operand, player)}");
                        return null;
                    case "DODGE":
                        player.is_dodge = 1;
                        //Console.WriteLine($"Доджим {this.run((node as UnOpNode).operand, player)}");
                        return null;
                    case "ATK":
                        player.do_damage(this.run((node as UnOpNode).operand, player), 0);
                        //Console.WriteLine($"Атакуем на {this.run((node as UnOpNode).operand, player)}");
                        return null;
                    case "POW_ATK":
                        player.do_damage(this.run((node as UnOpNode).operand, player), 1);
                        return null;
                }
            }

            if (node is BinOpNode)
            {
                BinOpNode bnode = (node as BinOpNode);
                switch (bnode.oper.type.name) 
                {                        
                        case "ADD":
                            return this.run(bnode.leftNode, player) + this.run(bnode.rightNode, player);
                        case "SUB":
                            return this.run(bnode.leftNode, player) - this.run(bnode.rightNode, player);
                        case "MUL":
                            return this.run(bnode.leftNode, player) * this.run(bnode.rightNode, player);
                        case "EQ":
                            dynamic result = this.run(bnode.rightNode, player);
                            VariableNode variableNode = (bnode.leftNode as VariableNode);                            
                            VarScope varF = scope.Find(element => element.name==variableNode.variable.text);
                            if (varF is not null)
                            {
                                varF.value = result;
                            }
                            else
                            {
                                scope.Add(new VarScope(variableNode.variable.text, result));
                            }
                                                       
                            return result;                        
                }
            }

            if (node is VariableNode)
            {
                VariableNode bnode = (node as VariableNode);
                                
                VarScope varF = scope.Find(element => element.name == bnode.variable.text);
                if (varF is not null)
                {
                    return varF.value;
                }
                else
                {
                    Console.WriteLine($"Error: Переменная с названием {bnode.variable.text} не обнаружена");
                    System.Environment.Exit(0);
                    return null;
                }
            }

            if (node is CommandNode) {
                (node as CommandNode).codeStrs.ForEach(codeStrs => { this.run(codeStrs, player); });
                return null;
            }

            if (node is BranchNode)
            {
                // если это блок условий, разбираем его на составляющие
                BranchNode bRnode = (node as BranchNode);
                BinOpNode bnode = (bRnode.compare as BinOpNode);
                CommandNode ifBody = (bRnode.ifBody as CommandNode);
                CommandNode elseBody = (bRnode.elseBody as CommandNode);
                bool result = false;
                switch (bnode.oper.type.name)
                {
                    case ">":
                        if (this.run(bnode.leftNode, player) > this.run(bnode.rightNode, player)) {
                            result =true;
                        }
                        break;
                    case "<":                        
                        if (this.run(bnode.leftNode, player) < this.run(bnode.rightNode, player)) {
                            result = true;
                        }
                        break;
                    case ">=":
                        if (this.run(bnode.leftNode, player) >= this.run(bnode.rightNode, player))
                        {
                            result = true;
                        }
                        break;
                    case "<=":
                        if (this.run(bnode.leftNode, player) <= this.run(bnode.rightNode, player))
                        {
                            result = true;
                        }
                        break;
                    case "==":
                        if (this.run(bnode.leftNode, player) == this.run(bnode.rightNode, player))
                        {
                            result = true;
                        }
                        break;
                }
                if (result)
                {
                    ifBody.codeStrs.ForEach(codeStrs => { this.run(codeStrs, player); });
                }
                else 
                {
                    if (elseBody!=null)
                    {
                        elseBody.codeStrs.ForEach(codeStrs => { this.run(codeStrs, player); });
                    }
                    
                }
                return null;
            }

            if (node is WhileLoopNode)
            {
                // если это блок циклв WHILE
                WhileLoopNode wlnode = (node as WhileLoopNode);
                BinOpNode bnode = (wlnode.compare as BinOpNode);
                CommandNode wbody = (wlnode.body as CommandNode);
                bool result = false;

                switch (bnode.oper.type.name)
                {
                    case ">":
                        if (this.run(bnode.leftNode, player) > this.run(bnode.rightNode, player))
                        {
                            result = true;
                        }
                        break;
                    case "<":
                        if (this.run(bnode.leftNode, player) < this.run(bnode.rightNode, player))
                        {
                            result = true;
                        }
                        break;
                    case ">=":
                        if (this.run(bnode.leftNode, player) >= this.run(bnode.rightNode, player))
                        {
                            result = true;
                        }
                        break;
                    case "<=":
                        if (this.run(bnode.leftNode, player) <= this.run(bnode.rightNode, player))
                        {
                            result = true;
                        }
                        break;
                    case "==":
                        if (this.run(bnode.leftNode, player) == this.run(bnode.rightNode, player))
                        {
                            result = true;
                        }
                        break;
                }
                while (result)
                {
                    wbody.codeStrs.ForEach(codeStrs => { this.run(codeStrs, player); });
                    result = false;
                    switch (bnode.oper.type.name)
                    {
                        case ">":
                            if (this.run(bnode.leftNode, player) > this.run(bnode.rightNode, player))
                            {
                                result = true;
                            }
                            break;
                        case "<":
                            if (this.run(bnode.leftNode, player) < this.run(bnode.rightNode, player))
                            {
                                result = true;
                            }
                            break;
                        case ">=":
                            if (this.run(bnode.leftNode, player) >= this.run(bnode.rightNode, player))
                            {
                                result = true;
                            }
                            break;
                        case "<=":
                            if (this.run(bnode.leftNode, player) <= this.run(bnode.rightNode, player))
                            {
                                result = true;
                            }
                            break;
                        case "==":
                            if (this.run(bnode.leftNode, player) == this.run(bnode.rightNode, player))
                            {
                                result = true;
                            }
                            break;
                    }
                }
                
                return null;
            }

            Console.WriteLine($"Error: Неизвестный блок кода!");
            System.Environment.Exit(0);
            return null;
            
         }
        
    }
}
