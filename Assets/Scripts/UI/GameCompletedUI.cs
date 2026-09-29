using TMPro;
using UnityEngine;

public class GameCompletedUI : Menu
{
    [SerializeField]
    private TMP_Text coinReward;

    // Hook khi menu mở: cập nhật phần thưởng hiển thị
    protected override void OnOpen()
    {
        base.OnOpen();
        UpdateRewardUI();
    }

    // Hiển thị số lượng coin thưởng tính toán được lên giao diện
    public void UpdateRewardUI()
    {
        coinReward.text = $"+{Player.Instance.CaculateReward()}";
    }

    // Xử lý sự kiện khi bấm nút Next để sang màn chơi kế tiếp
    public void HandlePressNext()
    {
        GameManager.Instance.ReloadScene();
    }
}
