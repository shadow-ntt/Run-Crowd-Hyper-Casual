using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private LevelSO[] levels;

    [SerializeField]
    private int gamePlayed = 0;
    private GameState currentState;

    public enum GameState
    {
        Menu,
        Game,
        LevelComplete,
        GameOver,
    }

    public static Action<GameState> OnChangeGameState;

    public static GameManager Instance { get; private set; }

    // Ads

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }

    void Start()
    {
        currentState = GameState.Game;
        ChangeGameState(GameState.Menu);
        AdsManager.Instance.bannerAds.ShowBannerAd();
    }

    // Thay đổi trạng thái hiện tại của game và kích hoạt sự kiện thông báo
    public void ChangeGameState(GameState gameState)
    {
        Debug.Log("State Game: " + gameState);

        if (currentState != gameState)
        {
            this.currentState = gameState;
            OnChangeGameState?.Invoke(gameState);
        }
    }

    // Tải lại màn chơi hiện tại và hiển thị quảng cáo sau mỗi 3 lượt chơi
    public void ReloadScene()
    {
        ++gamePlayed;
        if (gamePlayed % 3 == 0)
        {
            AdsManager.Instance.interstitialAds.ShowAd();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Kiểm tra xem game có đang ở trạng thái Gameplay hay không
    public bool IsGameState() => currentState == GameState.Game;

    // Kiểm tra xem game có đang ở trạng thái GameOver hay không
    public bool IsGameOverState() => currentState == GameState.GameOver;
}
