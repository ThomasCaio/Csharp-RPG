using Spells;
using Entities;
using CombatModule;

public class Firebolt : Spell {
    public Firebolt() : base("Firebolt") {
    }

    public override string SpellDescription() {
        int damage = (Source != null) ? CalculateDamage(Source) + BaseDamage : BaseDamage;
        return $"You throw a fireball to an enemy, dealing {damage} damage.";
    }

    public override void Action(Creature source, Creature target, DamageSet damage) {
        int dmg = BaseDamage + CalculateDamage(source);
        damage.SetElement(dmg, Element.Magical);
        AttackSystem.Hit(source, target, damage);
    }
}