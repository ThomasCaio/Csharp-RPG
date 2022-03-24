using Spells;
using Spells.Effects.DoT;
using Entities;
using CombatModule;

public class Poison : Passive {
    public double BaseDamage = 5;
    public int Turns;
    public Poison(int chance=20, double baseDamage=5, int turns=5) : base("Poison", PassiveType.OnHit) {
        Chance = chance;
        BaseDamage = baseDamage;
        Turns = turns;
        Description = $"Your attack has {Chance}% chance to inflict a poison that deals {BaseDamage} over {Turns} turns.";
    }

    public override void Action(Creature source, Creature target, DamageSet damage) {
        DamageOverTime poison = new DamageOverTime("Poison", BaseDamage, Turns);
        poison.Cast(source, target);
    }
}