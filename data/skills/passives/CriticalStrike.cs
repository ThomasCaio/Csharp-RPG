using SpellModule;
using EntityModule;
using CombatModule;

public class CriticalStrike : Passive {
    public double DamageFactor;
    public CriticalStrike(int chance=10, double damageFactor=1.5) : base("Critical Strike", PassiveType.OnHit) {
        Chance = chance;
        DamageFactor = damageFactor;
        
    }

    public override string SpellDescription()
    {
        return $"You have a chance to deal {DamageFactor*100}% damage on hit enemies.";
    }

    public override void Action(Creature source, Creature target, DamageSet damage) {
        double dmg = damage.GetElement(Element.Physical);
        damage.SetElement(dmg*DamageFactor, Element.Physical);
    }
}