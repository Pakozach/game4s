using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plTest
{
    internal class Character
    {
        private int hp_head;
        private int hp_body;
        private int damage;
        private double enemy_power;
        private int enemy_head;
        private int enemy_body;
        private int is_def_head;
        private int is_def_body;
        private int is_dodge;
        private int mana;

        public Character(int hp_head, int hp_body, int enemy_head, int enemy_body, double power)
        {
            this.hp_head = hp_head;
            this.hp_body = hp_body;
            this.enemy_head = enemy_head;
            this.enemy_body = enemy_body;
            this.enemy_power = power;
            this.damage = 10;
            this.is_def_head = 0;
            this.is_def_body = 0;
            this.is_dodge = 0;
            this.mana = 5;
        }

        public int[] get_hp()
        {
            int[] res = new int[2] { this.hp_head, this.hp_body };
            return res;
        }

        public int[] get_hp_enemy()
        {
            int[] res = new int[2] { this.enemy_head, this.enemy_body };
            return res;
        }

        public void do_damage(int target, int power) // 1 - в голову 2 - в тело
        {
            int mana = 1;
            if (power == 1)
                mana += 1;
            if (this.is_enough_mana(mana) == 1)
            {
                double proc = 1;
                if (power == 1)
                    proc = 2.5;
                if (this.hp_head <= 0)
                    proc *= 0.5;
                if (target == 0)
                    this.enemy_head -= (int)(this.damage * proc);
                else
                    this.enemy_body -= (int)(this.damage * proc);
                if (this.enemy_head < 0)
                    this.enemy_head = 0;
                if (this.enemy_body < 0)
                    this.enemy_body = 0;
            }
        }

        public int is_enough_mana(int a)
        {
            if (this.mana >= a)
            {
                this.mana -= a;
                return 1;
            } else
                return 0;
        }

        public int is_dead() 
        {
            if (this.hp_body <= 0)
                return 1;
            else return 0;
        }

        public void do_def(int target)
        {
            if (this.is_enough_mana(1) == 1)
                if (target == 0)
                    this.is_def_head = 1;
                else
                    this.is_def_body = 1;
        }

        public void do_dodge()
        {
            if (this.is_enough_mana(2) == 1)
                this.is_dodge = 1;
        }

        public int enemy_dead()
        {
            if (this.enemy_body <= 0)
                return 1;
            else return 0;
        }

        public int is_head()
        {
            if (this.hp_head <= 0)
                return 0;
            else return 1;
        }

        public void enemy_damage()
        {
            int value = 0;
            if (this.is_dodge == 1)
            {
                Random rnd = new Random();
                value = rnd.Next(0, 2);
            }
            if (value == 1)
                return;
            double def_head = 1;
            if (this.is_def_head == 1)
                def_head = 0.5;
            double def_body = 1;
            if (this.is_def_body == 1)
                def_body = 0.5;
            if (this.enemy_head <= 0)
            {
                def_body *= 0.5;
                def_head *= 0.5;
            }
            double d_head = 1 - this.enemy_power * def_head / 100;
            double d_body = 1 - this.enemy_power * def_body / 100;
            this.hp_body = (int)(this.hp_body * d_body);
            this.hp_head = (int)(this.hp_head * d_head);
            this.hp_head -= (int)(this.enemy_power * def_head);
            this.hp_body -= (int)(this.enemy_power * def_body);
            if (this.hp_head < 0)
                this.hp_head = 0;
            if (this.hp_body < 0)
                this.hp_body = 0;
        }

        public void end_round()
        {
            this.is_def_head = 0;
            this.is_def_body = 0;
            this.is_dodge = 0;
            this.mana = 5;
        }
    }
}
