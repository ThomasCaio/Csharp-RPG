namespace Monsters.Factory;
using Entities;

using System;
using System.Linq;
using System.Reflection;
using Monsters.All;

public static class MonsterFactory {
    public static Monster? New(string name)
    {
        Type? t = Type.GetType($"Monsters.All.{name.Replace(" ", "")}");
        if (t == null)
        {
            throw new ArgumentException($"Invalid monster name: {name}");
        }
        if (!typeof(Monster).IsAssignableFrom(t))
        {
            throw new ArgumentException($"Type {t} is not a subclass of Monster");
        }
        return (Monster)Activator.CreateInstance(t)!;
    }
}