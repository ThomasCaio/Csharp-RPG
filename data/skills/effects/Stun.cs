using EntityModule;
using CombatModule;
namespace SpellModule.Effects.DoT;

public class Stun : SpellModule.Effect {
    public Stun(string name, int turns) : base(name)
    {
        Turns = turns;
    }

    public override void Logic() {
    }
}