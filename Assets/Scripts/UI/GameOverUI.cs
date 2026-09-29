using UnityEngine;

public class GameOverUI : Menu
{
    // Xử lý sự kiện bấm nút Retry để chơi lại màn hiện tại
    public void HandlePressRetry()
    {
        GameManager.Instance.ReloadScene();
    }
}
