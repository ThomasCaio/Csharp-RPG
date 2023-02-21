using Spells;
using Entities;
using CombatModule;

public class Firebolt : Spell {
    public Firebolt() : base("Firebolt") {
        BaseDamage = 5;
    }

    public override int CalculateDamage(Creature? source) {
        if (source == null) {
            return BaseDamage;
        }
        return (int)Math.Round(source.MaxHealth * 0.1) + BaseDamage;
    }

    public override string SpellDescription() {
        return $"You throw a fireball to an enemy, dealing {Damage()} damage.";
    }

    public override void Action(Creature source, Creature target, DamageSet damage) {
        int dmg = BaseDamage + CalculateDamage(source);
        damage.SetElement(dmg, Element.Magical);
        AttackSystem.Hit(source, target, damage);
    }
}
