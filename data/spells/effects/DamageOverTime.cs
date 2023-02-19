using Entities;
using CombatModule;
namespace Spells.Effects.DoT;

public class DamageOverTime : Spells.Effect {
    public double Damage;

    public DamageOverTime(string name, double damage, int turns) : base(name) {
        Damage = damage;
        Turns = turns;
        Value = (int)DamageByTurn();
    }

    public override void Logic() {
        if (Source != null && Target != null){
            DamageSet dmg = new DamageSet(Value, Element.Magical);
            AttackSystem.Hit(Source, Target, dmg, false, false);
            RPG.Game.Log.Add($"{Name} hits {Target.Name} in {dmg.TotalDamage()} damage.");
        } else {
            DateTime now = DateTime.Now;
            Directory.CreateDirectory("./logs");
            File.AppendAllText("./logs/dot.log", $"{now} Class(DamageOverTime).Logic\n spell:{Name} S:{Source!.Name} T:{Target!.Name}");
        }
    }

    public double DamageByTurn() {
        return Damage / Turns;
    }
}