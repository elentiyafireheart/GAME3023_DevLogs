using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] public Text nameText;
    [SerializeField] public Text levelText;
    [SerializeField] public HPBar hpBar;

    private Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl. " + pokemon.Level;
        hpBar.SetHP((float)pokemon.HP/pokemon.MaxHP);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHP);
    }
}
