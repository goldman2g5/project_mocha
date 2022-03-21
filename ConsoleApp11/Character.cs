﻿namespace Cosoleapp3;

public class Character
{
    public string Name;
    public int Hp;
    public int MaxHp;
    public int Dmg;
    public int MaxDmg;
    public int Acc;
    public int MaxAcc;
    public int Dodge;
    public int MaxDodge;
    public int Crit;
    public int MaxCrit;
    public int Armor;
    public int MaxArmor;
    public int Initiative;
    public int MaxInitiative;
    public bool Stunned = false;
    public bool Dead = false;
    public bool IsAi;
    public List<Skill> Skills;
    public List<Status> StatusList = new List<Status>();

    public Character(string name, int hp, int dmg, int acc, int dodge, int armor, int crit, int initiative, List<Skill> skills)
    {
        Hp = hp;
        MaxHp = hp;
        Dmg = dmg;
        MaxDmg = dmg;
        Acc = acc;
        MaxAcc = acc;
        Dodge = dodge;
        MaxDodge = dodge;
        Initiative = initiative;
        MaxInitiative = initiative;
        Crit = crit;
        MaxCrit = crit;
        Armor = armor;
        MaxArmor = armor;
        Skills = skills;
        Name = name;
    }
    public Skill GetSkill()
    {
        if (!IsAi)
        {
            // Console.WriteLine($"Select a skill:\n{Skill.GetNames(this)}\n{Skill.GetInfo(Skills)}");
            Console.WriteLine($"Select a skill:\n{Skill.GetNames(this)}");
            return Skills[Misc.VerfiedInput(Skills.Count)];
        }
        return Skills[new Random().Next(Skills.Count)];
    }
    
    public void ProcessStatuses()
    {
        List<Status> temp = new List<Status>(StatusList);
        foreach (var i in StatusList)
        {
            if (!i.IsInstant)
            {
                i.Fn(this);
                Console.WriteLine($"Turns remaining: {i.Duration - 1}");
            }
            i.Duration -= 1;
            Thread.Sleep(3000);
            if (i.Duration <= 0) { temp.Remove(i); }
        }
        StatusList = temp;
        Dmg = MaxDmg;
        Acc = MaxAcc;
        Dodge = MaxDodge;
        Initiative = MaxInitiative;
        Crit = MaxCrit;
        Armor = MaxArmor;
        foreach (var i in StatusList.Where(i => i.IsInstant))
        {
            i.Fn(this);
        } 
    }

    public void TakeDamage(int dmg)
    {
        Hp = Hp - dmg <= 0 ? 0 : Hp - dmg;
        Dead = Hp == 0;
        if (!Dead) return;
        Console.WriteLine($"{Name} is dead");
        Thread.Sleep(3000);
    }
    
    public void Heal(int dmg)
    {
        Hp = Hp + dmg > MaxHp ? MaxHp : Hp + dmg;
        Dead = Hp == 0;
        if (!Dead) return;
        Console.WriteLine($"{Name} is dead");
        Thread.Sleep(3000);
    }
    
    public string GetStatuses()
    {
        return StatusList.Aggregate("", (current, i) => current + (i.Name + ", "));
    }
}