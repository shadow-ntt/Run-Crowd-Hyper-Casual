using TMPro;
using UnityEngine;

public class GameCompletedUI : Menu
{
    [SerializeField]
    private TMP_Text coinReward;

    protected override void OnOpen()
    {
        base.OnOpen();
        UpdateRewardUI();
    }

    public void UpdateRewardUI()
    {
        coinReward.text = $"+{Player.Instance.CaculateReward()}";
    }

    public void HandlePressNext()
    {
        GameManager.Instance.ReloadScene();
    }
}
