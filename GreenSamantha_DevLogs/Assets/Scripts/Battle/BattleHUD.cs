using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] public Text nameText;
    [SerializeField] public Text levelText;
    [SerializeField] public HPBar hpBar;

    public void SetData(Pokemon pokemon)
    {
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl. " + pokemon.Level;
        hpBar.SetHP((float)pokemon.HP/pokemon.MaxHP);
    }
}
