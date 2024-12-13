using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] public PokemonBase _base;
    [SerializeField] public int level;
    [SerializeField] public bool isPlayerUnit;

    public Pokemon pokemon { get; set; }

    public void Setup()
    {
        pokemon = new Pokemon(_base, level);

        if (isPlayerUnit)
            GetComponent<Image>().sprite = pokemon.Base.backSprite;
        else
            GetComponent<Image>().sprite = pokemon.Base.frontSprite;
    }
}
