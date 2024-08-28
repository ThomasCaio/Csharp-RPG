using SpellModule;
using SpellModule.Effects.DoT;
using EntityModule;
using CombatModule;

public class Burn : Passive {
    public int Turns;
    public Burn(int chance=20, int baseDamage=3, int turns=3) : base("Burn", PassiveType.OnHit) {
        Chance = chance;
        BaseDamage = baseDamage;
        Turns = turns;
    }

    public override string SpellDescription()
    {
        return $"Your attack has {Chance}% chance to inflict a burn that deals {BaseDamage} over {Turns} turns.";
    }

    public override void Action(Creature source, Creature target, DamageSet damage) {
        DamageOverTime burn = new DamageOverTime("Burn", BaseDamage, Turns);
        burn.Cast(source, target);
    }
}