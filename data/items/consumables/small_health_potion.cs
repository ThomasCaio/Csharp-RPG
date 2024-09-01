using EntityModule;
using RPG;

namespace ItemModule.All;

public class SmallHealthPotion : Potion {
    public SmallHealthPotion() : base("Small Health Potion") {
        Price = 5;
    }

    public override void Action(Character character)
    {
        character.Health += Formula(character);
    }

    public override int Formula(Character character)
    {
        return (int)(character.MaxHealth * 0.2);
    }

    public override void UseText(Character character)
    {
        Game.Log.Add($"{character.Name} drinks a {this.Name} and heals {Formula(character)} health.");
    }

    public override Dictionary<string, string> Look()
    {
        return new Dictionary<string, string>
            {
                { "Title", Name },
                { "Description", "Heals the player equals 20% of his maximum health." },
            };
    }
}

// TODO: Por algum motivo, não está aparecendo no shop.