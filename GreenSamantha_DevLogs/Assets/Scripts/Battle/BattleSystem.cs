using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] public BattleUnit playerUnit;
    [SerializeField] public BattleHUD playerHUD;

    [SerializeField] public BattleUnit enemyUnit;
    [SerializeField] public BattleHUD enemyHUD;

    private void Start()
    {
        SetupBattle();
    }

    public void SetupBattle()
    {
        playerUnit.Setup();
        playerHUD.SetData(playerUnit.Pokemon);

        enemyUnit.Setup();
        enemyHUD.SetData(enemyUnit.Pokemon);
    }
}
