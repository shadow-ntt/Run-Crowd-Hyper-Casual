using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private Transform RunnerGroup;

    //
    void OnDisable()
    {
        GameManager.OnChangeGameState -= OnChangeGameStateCallBack;
    }

    void OnEnable()
    {
        GameManager.OnChangeGameState += OnChangeGameStateCallBack;
    }

    private void OnChangeGameStateCallBack(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Game)
            PlayerRun();
        else
            PlayerIdle();
    }

    public void PlayerRun()
    {
        for (int i = 0; i < RunnerCount(); i++)
        {
            RunnerGroup.GetChild(i).GetComponent<Animator>().SetBool("isMoving", true);
        }
    }

    public void PlayerIdle()
    {
        for (int i = 0; i < RunnerCount(); i++)
        {
            RunnerGroup.GetChild(i).GetComponent<Animator>().SetBool("isMoving", false);
        }
    }

    public int RunnerCount()
    {
        return RunnerGroup.childCount;
    }
}
