using Spells;
using Spells.Effects.DoT;
using Entities;
using CombatModule;

public class MonsterCast : Passive {
    public Spell MonsterSpell;
    public MonsterCast(Spell spell, int chance=20) : base("Cast", PassiveType.OnHit) {
        Chance = chance;
        MonsterSpell = spell;
        Name = spell.Name;
    }

    public override string SpellDescription()
    {
        return $"Monster attack has {Chance}% chance to cast a {MonsterSpell.Name}.";
    }

    public override void Action(Creature source, Creature target, DamageSet damage) {
        Combat.Cast(source, target, MonsterSpell);
    }
}