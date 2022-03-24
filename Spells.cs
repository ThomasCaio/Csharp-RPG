namespace Spells;
using Entities;
using CombatModule;

public abstract class Spell {
    public string Name;
    public string Description;

    public Spell(string name) {
        Name = name;
        Description = "Description";
    }

    public void Cast(Creature source, Creature target, DamageSet damage) {
        PreAction(source, target, damage);
        Action(source, target, damage);
        PostAction(source, target, damage);
    }
    public virtual void PreAction(Creature source, Creature target, DamageSet damage) {
    }
    public abstract void Action(Creature source, Creature target, DamageSet damage);
    public virtual void PostAction(Creature source, Creature target, DamageSet damage) {}
}

public enum PassiveType {
    OnHit,
    OnKill,
    OnCast,
    WhenHit,
    WhenDie,
}

public abstract class Passive : Spell {
    public PassiveType Type;
    public int Chance = 0;

    public Passive(string name, PassiveType type) : base(name) {
        Type = type;
    }

}

public enum EffectType {
    Armor,
    Block,
    Damage,
    Health,
}

public abstract class Effect {
    public string Name;
    public int Value;

    public Creature? Source;
    public Creature? Target;

    public EffectType Type;
    public int Turns;

    public Effect(string name) {
        Name = name;
    }

    public override string ToString()
    {
        return $"{Name}*{Turns}";
    }

    public void Cast(Creature source, Creature target) {
        Source = source;
        Target = target;
        target.Effects.Add(this);
    }

    public void Action() {
        Logic();
        PostLogic();
    }

    public abstract void Logic();
    public virtual void PostLogic(){
        Turns--;
    }
}