using System;
using System.Collections.Generic;
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

    [Header("GameObject")]
    [SerializeField]
    private GameObject playerGroup;
    private Collider[] colliders = new Collider[10];
    private List<Enemy> listEnemies = new List<Enemy>();

    public static Action StartCombat;
    public static Action EndCombat;

    private bool isCombat = false;
    private bool combatEnded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if (combatEnded)
            return;

        if (!isCombat)
        {
            ScanPlayer();
        }
        else
        {
            // Clean up destroyed enemies from list
            listEnemies.RemoveAll(enemy => enemy == null);

            if (listEnemies.Count == 0)
            {
                isCombat = false;
                combatEnded = true;
                EndCombat?.Invoke();
                return;
            }

            SetTargetRunner();
        }
    }

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
            listEnemies.Add(enemy);
        }
    }

    private void SetTargetRunner()
    {
        listEnemies.RemoveAll(enemy => enemy == null);

        if (listEnemies.Count == 0 || playerGroup == null)
            return;

        int runnerCount = playerGroup.transform.childCount;

        for (int i = 0; i < listEnemies.Count; i++)
        {
            if (listEnemies[i] == null)
                continue;

            if (runnerCount > 0)
            {
                Transform targetRunner = playerGroup.transform.GetChild(i % runnerCount);
                if (targetRunner != null)
                {
                    listEnemies[i].MoveToRunner(targetRunner, true);
                }
            }
            else
            {
                // If there are no runners left, move towards playerGroup without destroying it
                listEnemies[i].MoveToRunner(playerGroup.transform, false);
            }
        }
    }

    private void ScanPlayer()
    {
        int num = Physics.OverlapSphereNonAlloc(
            transform.position,
            5.0f,
            colliders,
            LayerMask.GetMask("Player")
        );
        if (num > 0)
        {
            isCombat = true;
            StartCombat?.Invoke();
            SetTargetRunner();
        }
    }
}
