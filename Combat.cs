namespace CombatModule;
using EntityModule;
using SpellModule;

public class DamageSet {
    public Dictionary<Element, double> _damages = new Dictionary<Element, double>();

    public DamageSet(double value) : this(value, Element.Physical) {}
    public DamageSet(double value, Element element) {
        SetElement(value, element);
    }
    public int TotalDamage() {
        int Dmg = 0;
        foreach (int d in _damages.Values) {
            if (d <= 0) {
                continue;
            }
            Dmg += d;
        }
        return Dmg;
    }

    public double GetElement(Element element) {
        if (_damages.TryGetValue(element, out double value)) {
            return value;
        } else {return 0;}
    }

    public void SetElement(double value, Element element) {
        if (_damages.ContainsKey(element)) {
            _damages[element] = value;
        } else {Add(value, element);}
    }

    public void Add(double value, Element element) {
        _damages.Add(element, value);
    }
}

public enum Element {
    Physical,
    Magical,
}

public class Damage {
    public double Value;
    public Element Type;

    public Damage(double value){
        Value = value;
        Type = Element.Physical;
    }
    public Damage(double value, Element element) {
        Value = value;
        Type = element;
    }
}


public static class Combat {
    public static Random RNG = new();

    public static void Attack(Creature source, Creature target) {
        DamageSet damage;
        if (source.Type == CreatureType.Character) {

            Character s = (Character)source;
            damage = new DamageSet(s.TotalDamage);
        } else {
            damage = new DamageSet(source.TotalDamage);
            }
        int chance = RNG.Next(1, 100);
        if (chance < 75) {AttackSystem.Hit(source, target, damage, true);} 
        else {AttackSystem.Miss(source, target, damage);}
    }

    public static void Cast(Creature source, Creature target, Spell spell) {
        spell.Cast(source, target, new DamageSet(0, Element.Magical));
    }
}

public static class AttackSystem {
    public static void Hit(Creature source, Creature target, DamageSet damage, bool castPassives=false, bool showLog=true) {
        if (castPassives) {
            if (source.Passives.Count > 0) {
                foreach (Passive p in source.OnHitPassives) {
                    if (p.Chance > Combat.RNG.Next(1, 100)) {
                        if (showLog) {
                            RPG.Game.Log.Add($"{source.Name} casts {p.Name}!");
                        }
                        p.Cast(source, target, damage);
                    }
                }
            }
        }
        Resistance(source, target, damage, showLog);
    }

    public static void Miss(Creature source, Creature target, DamageSet damage) {
        RPG.Game.Log.Add($"{source.Name} missed!");
    }
    public static void Resistance(Creature source, Creature target, DamageSet damage, bool showLog=true) {
        var physicalDamage = damage.GetElement(Element.Physical);
        var magicalDamage = damage.GetElement(Element.Magical);

        if (target is Character character)
        {
            double physical_resistance = character.PhysicalResistance;
            damage.SetElement(physicalDamage, Element.Physical);

            if (RPG.Game.Debug) 
            {
                if (character.PhysicalResistance > 0)
                {
                    RPG.Game.Log.Add($"DEBUG: {target.Name}'s physical resistance reduces the damage by {physical_resistance:F1}%.");
                }
            }
        }
        Block(source, target, damage, showLog);
    }
    public static void Block(Creature source, Creature target, DamageSet damage, bool showLog=true) {
        var physicalDamage = damage.GetElement(Element.Physical);

        if (target is Character character)
        {
            double armor = character.Armor;
            physicalDamage -= armor;
            damage.SetElement(physicalDamage, Element.Physical);
            
            if (RPG.Game.Debug) 
            {
                if (character.Armor > 0)
                {
                    RPG.Game.Log.Add($"DEBUG: {target.Name}'s armor reduces the damage by {armor}.");
                }
            }
        }
        Damage(source, target, damage, showLog);
    }
    public static void Damage(Creature source, Creature target, DamageSet damage, bool showLog=true) {
        var dmg = damage.TotalDamage();
        if (dmg > 0)
        {
            int health = target.Health - damage.TotalDamage();
            if (showLog) {
                RPG.Game.Log.Add($"{source.Name} hits {target.Name} in {damage.TotalDamage()} damage.");
            }
            
            if (health <= 0) {
                target.Health = health;
                Death(source, target, damage);
                return;
            }
            target.Health = health;
        }

        else 
        {
            if (showLog) {
                RPG.Game.Log.Add($"{target.Name} blocked all {source.Name} damage.");
            }
        }
        
    }

    public static void Death(Creature source, Creature target, DamageSet damage) {
        RPG.Game.Log.Add($"{target.Name} dies.");
        if ((source is Character character) && (target is Monster monster)){

        if (monster.GetGoldDrop() > 0) {
            var gold = monster.GetGoldDrop();
            character.Gold += gold;
            RPG.Game.Log.Add($"{monster.Name} dropped {gold} gold.");
        }
        if (monster.DropExp > 0) {
            character.Experience += monster.DropExp;
            RPG.Game.Log.Add($"You gain {monster.DropExp} experience.");
        }
        return;
        }
        
    }

}