namespace Views;
using Spectre.Console;

public class GameLog {
    RPG.Game Parent;
    public Table Table {
        get { return TB;}
    }
    public List<string> Events{get {return RPG.Game.Log;}}
    public List<string> LogList = new List<string>();
    Table TB = new Table().Width(60).Title("Log").HideHeaders();

    public void AddEvent(string str) {
        Events.Add(str);
    }

    public void RemoveEvent(string str) {
        Events.Remove(str);
    }

    public void AddLog(string str) {
        LogList.Add(str);
    }

    public void RemoveLog(string str) {
        LogList.Remove(str);
    }

    public GameLog(RPG.Game parent) {
        Parent = parent;
        Table.AddColumn("Message");
    }

    public void LoadEvents() {
        foreach (string str in RPG.Game.Log) {
            LogList.Add(str);
            Table.AddRow(str);
        }
    }

    public void ClearLog() {
        Table.Rows.Clear();
        LogList.Clear();
    }

    public void ClearEvents() {
        RPG.Game.Log.Clear();
    }

    public void RenderWithoutClear() {
        foreach (string str in RPG.Game.Log) {
            Table.AddRow(str);
        }
        if (TB.Rows.Count > 0) {
            AnsiConsole.Write(TB);
        }
    }

    public void Render() {
        RenderWithoutClear();
        ClearLog();
        ClearEvents();
    }
}