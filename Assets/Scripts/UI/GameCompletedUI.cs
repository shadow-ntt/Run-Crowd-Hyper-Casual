using UnityEngine;

public class GameCompletedUI : Menu
{
    void Start() { }

    public void HandlePressNext()
    {
        int currentLevel = SaveLoadManager.LoadInt("level", 1);
        SaveLoadManager.SaveInt("level", ++currentLevel);
        GameManager.Instance.ReloadScene();
        GameManager.Instance.ChangeGameState(GameManager.GameState.Game);
    }
}
