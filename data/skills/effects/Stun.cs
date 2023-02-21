using Entities;
using CombatModule;
namespace Spells.Effects.DoT;

public class Stun : Spells.Effect {
    public Stun(string name, int turns) : base(name)
    {
        Turns = turns;
    }

    public override void Logic() {
    }
}