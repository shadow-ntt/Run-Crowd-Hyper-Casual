using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField]
    private int amount;

    [SerializeField]
    private Enemy enemyPrefab;

    [SerializeField]
    private float radius;

    [SerializeField]
    private float angle = 137.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update() { }

    //
    private Vector3 EnemyLocalPositions(int index)
    {
        float r = radius * Mathf.Sqrt(index);

        float x = r * Mathf.Cos(angle * index * Mathf.Deg2Rad);
        float z = r * Mathf.Sin(angle * index * Mathf.Deg2Rad);

        return new Vector3(x, 0, z);
    }

    private void Generate()
    {
        for (int i = 0; i < amount; i++)
        {
            Enemy enemy = Instantiate(enemyPrefab, transform);
            enemy.transform.localPosition = EnemyLocalPositions(i);
        }
    }
}
