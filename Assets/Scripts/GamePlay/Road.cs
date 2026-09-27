using System;
using UnityEngine;

public class Road : Singleton<Road>
{
    [SerializeField]
    private LevelSO[] levels;

    [SerializeField]
    private Chunk EndLine;

    private string LEVELGAME = "levelGame";
    private int levelGame = 1;
    public float EndLineZ { get; private set; }
    public static Action<int> onUpLevel;
    public int CurrentLevel => levelGame;

    void OnEnable() => GameManager.OnChangeGameState += ChangeGameStateCallBack;
    void OnDisable() => GameManager.OnChangeGameState -= ChangeGameStateCallBack;

    private void ChangeGameStateCallBack(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.LevelComplete)
            LevelUp();
    }

    protected override void Awake()
    {
        base.Awake();
        int saved = Mathf.Max(1, SaveLoadManager.LoadInt(LEVELGAME, 1));
        levelGame = (saved - 1) % GetMaxLevel() + 1;
    }

    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        int levelIndex = levelGame - 1;
        float currentZ = 0f;

        foreach (var chunk in levels[levelIndex].chunks)
        {
            float length = chunk.GetLength();
            float centerZ = currentZ + (length / 2f);

            Chunk newChunk = Instantiate(chunk, transform);
            newChunk.transform.localPosition = new Vector3(0f, 0f, centerZ);
            currentZ += length;
        }

        if (EndLine != null)
        {
            float endLength = EndLine.GetLength();
            float centerZ = currentZ + (endLength / 2f);

            Chunk endLine = Instantiate(EndLine, transform);
            endLine.transform.localPosition = new Vector3(0f, 0f, centerZ);
            EndLineZ = centerZ;
        }
    }

    public int GetMaxLevel() => levels.Length;

    private void LevelUp()
    {
        levelGame = (levelGame % GetMaxLevel()) + 1;
        SaveLoadManager.SaveInt(LEVELGAME, levelGame);
        onUpLevel?.Invoke(levelGame);
    }
}
