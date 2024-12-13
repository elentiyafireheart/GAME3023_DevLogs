using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName= "Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] string pokemonName;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    [SerializeField] public List<LearnableMove> learnableMoves;

    public string Name
    {
        get { return pokemonName; }
    }

    public string Description
    {
        get { return description; }
    }

    public int MaxHP
    {
        get { return maxHP; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int SpAttack
    {
        get { return spAttack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int SpDefense
    {
        get { return spDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }

}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] public MoveBase moveBase;
    [SerializeField] public int level;

    public MoveBase Base
    {
        get { return new MoveBase(); }
    }
    public int Level
    {
        get { return level; }
    }
}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Fairy
}