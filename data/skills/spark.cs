using Spells;
using Entities;
using CombatModule;

public class Spark : Spell {
    public Spark(int baseDamage=5) : base("Spark") {
        BaseDamage = baseDamage;
    }

    public override int CalculateDamage(Creature? source) {
        if (source == null) {
            return 0;
        }
        return (int)Math.Round(source.MaxHealth * 0.1);
    }

    public override string SpellDescription() {
        return $"You throw a spark to an enemy, dealing {BaseDamage} + {CalculateDamage(Source)} damage.";
    }

    public override void Action(Creature source, Creature target, DamageSet damage) {
        damage.SetElement(Damage(), Element.Magical);
        AttackSystem.Hit(source, target, damage);
    }
}
