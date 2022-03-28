namespace Spells;
using Entities;
using CombatModule;

public abstract class Spell {
    public string Name;
    public string Description {get => SpellDescription();}

    public Creature? Source = null;
    public Creature? Target;
    public TargetType Area = TargetType.Single;
    public int BaseDamage = 0;

    public Spell(string name) {
        Name = name;
    }

    public void Cast(Creature source, Creature target, DamageSet damage) {
        Source = source;
        Target = target;
        PreAction(source, target, damage);
        Action(source, target, damage);
        PostAction(source, target, damage);
    }

    public virtual int CalculateDamage(Creature source) {
        return 0;
    }


    public virtual string SpellDescription() {
        return "";
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

public enum TargetType {
    Single,
    TargetedArea,
    AreaOfEffect,
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