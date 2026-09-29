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

    // Chuyển đổi animation của runner (chạy hoặc đứng yên) theo trạng thái game
    private void OnChangeGameStateCallBack(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Game)
            PlayerRun();
        else
            PlayerIdle();
    }

    // Bật cờ di chuyển để kích hoạt animation chạy cho toàn bộ runner
    public void PlayerRun()
    {
        for (int i = 0; i < RunnerCount(); i++)
        {
            RunnerGroup.GetChild(i).GetComponent<Animator>().SetBool("isMoving", true);
        }
    }

    // Tắt cờ di chuyển để chuyển toàn bộ runner về animation đứng yên
    public void PlayerIdle()
    {
        for (int i = 0; i < RunnerCount(); i++)
        {
            RunnerGroup.GetChild(i).GetComponent<Animator>().SetBool("isMoving", false);
        }
    }

    // Lấy tổng số lượng runner hiện tại trong nhóm
    public int RunnerCount()
    {
        return RunnerGroup.childCount;
    }
}
