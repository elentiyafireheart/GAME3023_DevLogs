using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] public PokemonBase _base;
    [SerializeField] public int level;
    [SerializeField] public bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    public void Setup()
    {
        Pokemon = new Pokemon(_base, level);

        if (isPlayerUnit)
            GetComponent<Image>().sprite = Pokemon.Base.BackSprite;
        else
            GetComponent<Image>().sprite = Pokemon.Base.FrontSprite;
    }
}
