using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelSO[] levels;
    private GameState currentState;
    public int numLevel=1;

    public enum GameState { Menu, Game, LevelComplete, GameOver }
    public static GameManager Instance;
    public static Action<GameState> OnChangeGameState; 
    void Awake()
    {
        if (Instance == null)
        {
            Instance =this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        currentState=GameState.Game;
    }
    public void ChangeGameState(GameState gameState)
    {
        if (currentState != gameState)
        {
            this.currentState=gameState;
            OnChangeGameState?.Invoke(gameState);
            Debug.Log("State change: "+gameState);
        }
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}