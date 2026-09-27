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

    public void ChangeGameState(GameState gameState)
    {
        Debug.Log("State Game: " + gameState);

        if (currentState != gameState)
        {
            this.currentState = gameState;
            OnChangeGameState?.Invoke(gameState);
        }
    }

    public void ReloadScene()
    {
        ++gamePlayed;
        if (gamePlayed % 3 == 0)
        {
            AdsManager.Instance.interstitialAds.ShowAd();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsGameState() => currentState == GameState.Game;

    public bool IsGameOverState() => currentState == GameState.GameOver;
}
