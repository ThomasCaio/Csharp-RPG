using Entities;
namespace Monsters.All;
public class Dwarf : Monster {
    public Dwarf() : base("Dwarf") {
        MaxHealth = 50;
        BaseDamage = 6;
        BaseDefense = 3;
        Passives.Add(new Bash());
        DropExp = 5;
        DropGold = new Tuple<int, int>(3, 6);
    }
}