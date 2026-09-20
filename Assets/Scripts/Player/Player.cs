using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float radius;

    [SerializeField]
    private Transform RunnerGroup;

    [SerializeField]
    private GameObject RunnerPrefab;

    [SerializeField]
    private Road road;

    [SerializeField]
    private float angle = 137.5f;
    private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < RunnerCount(); i++)
        {
            RunnerGroup.GetChild(i).localPosition = PlayerRunnerLocalPositions(i);
        }
        GameUI.Instance.setProgressLevel(ProgressEndLine());
    }

    //
    private Vector3 PlayerRunnerLocalPositions(int index)
    {
        float r = radius * Mathf.Sqrt(index);

        float x = r * Mathf.Cos(angle * index * Mathf.Deg2Rad);
        float z = r * Mathf.Sin(angle * index * Mathf.Deg2Rad);

        return new Vector3(x, 0, z);
    }

    public void MoveEase()
    {
        for (int i = 0; i < RunnerCount(); i++)
        {
            RunnerGroup.GetChild(i).localPosition = Vector3.Lerp(
                transform.position,
                PlayerRunnerLocalPositions(i),
                speed * Time.deltaTime
            );
        }
    }

    public int RunnerCount()
    {
        return RunnerGroup.childCount;
    }

    public void SetSerialRuner(int n)
    {
        int runnerCount = RunnerCount();
        //add runner
        if (n > RunnerCount())
        {
            for (int i = 0; i < n - runnerCount; i++)
            {
                Instantiate(RunnerPrefab, RunnerGroup);
            }
        }
        //remove runner
        if (n < runnerCount)
        {
            for (int i = 0; i < runnerCount - n; i++)
            {
                Destroy(RunnerGroup.GetChild(i).gameObject);
            }
        }
    }

    public float ProgressEndLine()
    {
        return transform.position.z / road.EndLineZ;
    }
}
