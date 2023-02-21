using Spells;
using Entities;
using CombatModule;

public class BoltShot : Passive {
    public double DamageFactor;
    public BoltShot(int chance=30, int baseDamage=8) : base("Bolt Shot", PassiveType.OnHit) {
        Chance = chance;
        BaseDamage = baseDamage;
        
    }

    public override string SpellDescription()
    {
        return $"You have a chance to shot a bolt dealing {DamageFactor*100}% damage on hit enemies.";
    }

    public override void Action(Creature source, Creature target, DamageSet damage) {
        var dmg = new DamageSet(BaseDamage, Element.Physical);
        AttackSystem.Hit(source, target, dmg, false, true);
    }
}