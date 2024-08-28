using SpellModule;
using EntityModule;
using CombatModule;

public class BoltShot : Passive {
    public double DamageFactor;
    public BoltShot(int chance=30, int baseDamage=10) : base("Bolt Shot", PassiveType.OnHit) {
        Chance = chance;
        BaseDamage = baseDamage;
        
    }

    public override string SpellDescription()
    {
        return $"You have a chance to shot a bolt dealing {BaseDamage} damage on enemy.";
    }

    public override void Action(Creature source, Creature target, DamageSet damage) {
        var dmg = new DamageSet(BaseDamage, Element.Physical);
        AttackSystem.Hit(source, target, dmg, false, true);
    }
}