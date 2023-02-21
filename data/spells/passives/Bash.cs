using Spells;
using Spells.Effects.DoT;
using Entities;
using CombatModule;

public class Bash : Passive {
    public int Turns;
    public Bash(int chance=20, int turns=1, int baseDamage=0) : base("Stun", PassiveType.OnHit) {
        Chance = chance;
        BaseDamage = baseDamage;
        Turns = turns;
    }

    public override string SpellDescription()
    {
        return $"Your attack has {Chance}% chance to inflict a poison that deals {BaseDamage} over {Turns} turns.";
    }

    public override void Action(Creature source, Creature target, DamageSet damage) {
        Stun bash = new Stun("Stun", Turns);
        bash.Cast(source, target);
    }
}