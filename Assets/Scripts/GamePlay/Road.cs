using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private LevelSO[] levels;
    [SerializeField] private Chunk EndLine;
    public  float EndLineZ {get;private set;}
    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance chưa được khởi tạo!");
            return;
        }

        int numLevel = SaveLoadManager.LoadInt("level",1);
        if (levels == null || levels.Length == 0)
        {
            Debug.LogError("Chưa gán danh sách Levels trong Road!");
            return;
        }

        int levelIndex = (numLevel - 1) % levels.Length;
        if (levelIndex < 0 || levelIndex >= levels.Length || levels[levelIndex] == null)
        {
            Debug.LogError($"Level {numLevel} không hợp lệ!");
            return;
        }

        float currentZ = 0f;
        foreach (var chunk in levels[levelIndex].chunks)
        {
            if (chunk == null) continue;

            float length = chunk.GetLength();
            float centerZ = currentZ + (length / 2f);

            Chunk newChunk = Instantiate(chunk, this.transform);
            newChunk.transform.localPosition = new Vector3(0f, 0f, centerZ);

            currentZ += length;
        }

        if (EndLine != null)
        {
            float endLength = EndLine.GetLength();
            float centerZ = currentZ + (endLength / 2f);

            Chunk endLine = Instantiate(EndLine, this.transform);
            endLine.transform.localPosition = new Vector3(0f, 0f, centerZ);

            currentZ += endLength;
            EndLineZ = centerZ;
        }
    }
}
