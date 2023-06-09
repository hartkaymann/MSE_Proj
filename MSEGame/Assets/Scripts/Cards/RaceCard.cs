using UnityEngine;

public enum Race
{
    Human,
    Elf,
    Orc
}

public class RaceCard : DoorCard
{
    public readonly string ability;

    public readonly Race race;

    public RaceCard(Race race, string id, Sprite artwork) : base(race.ToString(), CardType.Race, id, artwork)
    {
        this.race = race;
    }
}
