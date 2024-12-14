using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerAction,
    PlayerMove,
    EnemyMove,
    Busy
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] public BattleUnit playerUnit;
    [SerializeField] public BattleHUD playerHUD;

    [SerializeField] public BattleUnit enemyUnit;
    [SerializeField] public BattleHUD enemyHUD;

    [SerializeField] public BattleDialog dialogBox;

    private BattleState state;
    private int currentAction;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        playerHUD.SetData(playerUnit.Pokemon);

        enemyUnit.Setup();
        enemyHUD.SetData(enemyUnit.Pokemon);

        dialogBox.SetMoveName(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} APPEARED!");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelecton(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelecton(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelecton(true);
    }

    public IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var move = playerUnit.Pokemon.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Pokemon.Base.Name}");
        yield return new WaitForSeconds(1f);

        bool isFainted = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHUD.UpdateHP();

        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} Fainted.");
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    public IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        var move = enemyUnit.Pokemon.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(1f);

        bool isFainted = playerUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return playerHUD.UpdateHP();

        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} Fainted.");
        }
        else
        {
            PlayerAction();
        }
    }

    private void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction <1)
                ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
                --currentAction;
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                // Fight
                PlayerMove();
            }
            else if (currentAction == 1)
            {
                // Run
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 1)
                currentMove++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
                currentMove--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 2)
                currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove >1)
                currentMove -= 2;
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelection(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }
}
