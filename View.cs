namespace Views;

public abstract class View {
    public RPG.Game Parent;
    public View(RPG.Game parent) {
        Parent = parent;
    }

    public abstract void Render();
}