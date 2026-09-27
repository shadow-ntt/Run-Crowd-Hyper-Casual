using UnityEngine;

public class GameOverUI : Menu
{
    public void HandlePressRetry()
    {
        GameManager.Instance.ReloadScene();
    }
}
