namespace Views;
using Spectre.Console;
using RPG;

public abstract class View {
    public Game Parent;
    public View(Game parent) {
        Parent = parent;
    }

    public abstract void Render();
}