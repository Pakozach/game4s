using plTest;
using plTest.AST;
using System;
using static System.Formats.Asn1.AsnWriter;

void print_hp(Character guy) 
{
    int[] hp = guy.get_hp();
    int[] hp_enemy = guy.get_hp_enemy();
    Console.WriteLine("YOURS hp_head: " + hp[0] + " --- hp_body: " + hp[1] + " ---- " + "ENEMY hp_head: " + hp_enemy[0] + "-- - hp_body: " + hp_enemy[1]);
}

int battle(Character guy, Lexer lexer)
{
    int c = 0;

    while (c < 100)
    {
        Parser parser = new Parser(lexer.tokenList);
        int[] hp = guy.get_hp();
        int[] hp_enemy = guy.get_hp_enemy();
        parser.scope.Add(new VarScope("enemyheadhp", hp_enemy[0]));
        parser.scope.Add(new VarScope("enemybodyhp", hp_enemy[1]));
        parser.scope.Add(new VarScope("yourheadhp", hp[0]));
        parser.scope.Add(new VarScope("yourbodyhp", hp[1]));
        ExpressionNode rootNode = parser.parseCode();

        print_hp(guy);
        parser.run(rootNode, guy);
        guy.enemy_damage();
        c++;
        if (guy.is_dead() == 1)
        {
            Console.WriteLine("YOU DIED!");
            return 0;
        }
        else
        {
            if (guy.enemy_dead() == 1)
            {
                Console.WriteLine("You defeated the enemy!");
                read_file("Victory_scenario.txt");
                return 1;
            }
            Console.WriteLine("------------------------------------------------------------------------------");
        }
        guy.end_round();
    }
    return 0;
}

void read_file(string name)
{
    String line;
    try
    {
        //Pass the file path and file name to the StreamReader constructor
        StreamReader sr = new StreamReader(name);
        //Read the first line of text
        line = sr.ReadLine();
        //Continue to read until you reach end of file
        while (line != null)
        {
            //write the line to console window
            Console.WriteLine(line);
            //Read the next line
            line = sr.ReadLine();
        }
        //close the file
        sr.Close();
        Console.ReadLine();
    }
    catch (Exception e)
    {
        Console.WriteLine("Exception: " + e.Message);
    }
    finally
    {
        //Console.WriteLine("Executing finally block.");
    }
}

read_file("Hello_Dialog.txt");
Console.WriteLine("START!");

const string code =
@" {
    IF(enemyheadhp > 0)
    {
        POW_ATK(0);
    } ELSE {
        POW_ATK(1);
    }
    DEF(1);
    DODGE(0);
    }
"
;

Lexer lexer = new Lexer(code);
lexer.LexAnalizator();

int inc_damage = 1;
int lvl = 1;

for (int i = 0; i < 100; i++)
{
    Character guy = new Character(50, 100, 50 * lvl, 100 * lvl, 5 * inc_damage);
    if (battle(guy, lexer) == 0)
    {
        read_file("Defeat_scenario.txt");
        Console.WriteLine("You stopped at " + lvl + " lvl");
        break;
    }
    inc_damage++;
    lvl++;
}

/* @" {   
      sum := 39 - 29;
sumtwo := (((10+(2+2)) + 6) * 2) +5;

LOG sum;
LOG sumtwo;

IF(sum < 9)
{
xx:= 100;
ATK(xx * 20);
}
ELSE
{
xx:= 20;
DEF(xx * 5);
}
sum:=0;
WHILE(sum<=10)
{
LOG sum;
sum:=sum+1;
}
suma := sum+10;
LOG suma;
    }  
" */