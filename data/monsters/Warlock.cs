using EntityModule;
namespace Monsters.Warlock;
public class Warlock : Monster {
    public Warlock() : base("Warlock") {
        BaseDamage = 5;
        Passives.Add(new CriticalStrike(25));
    }
}