using System;
using TMPro;
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

    [SerializeField]
    private float speed;

    [SerializeField]
    private TextMeshPro textCount;

    public bool isLerp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (isLerp)
            PlaceRunnersMoveEase();
        else
            PlaceRunners();
        GameUI.Instance.setProgressLevel(ProgressEndLine());
        textCount.text = RunnerGroup.childCount.ToString();
    }

    //
    public float GetRadiusGroup()
    {
        int count = RunnerGroup.childCount;

        if (count == 0)
            return 0f;

        return radius * Mathf.Sqrt(count - 1);
    }

    private Vector3 GetRunnerLocalPositions(int index)
    {
        float r = radius * Mathf.Sqrt(index);

        float x = r * Mathf.Cos(angle * index * Mathf.Deg2Rad);
        float z = r * Mathf.Sin(angle * index * Mathf.Deg2Rad);

        return new Vector3(x, 0, z);
    }

    public void PlaceRunners()
    {
        for (int i = 0; i < RunnerCount(); i++)
        {
            RunnerGroup.GetChild(i).localPosition = GetRunnerLocalPositions(i);
        }
    }

    public void PlaceRunnersMoveEase()
    {
        for (int i = 0; i < RunnerCount(); i++)
        {
            RunnerGroup.GetChild(i).localPosition = Vector3.Lerp(
                transform.position,
                GetRunnerLocalPositions(i),
                speed * Time.deltaTime
            );
        }
        for (int i = 0; i < RunnerCount(); i++)
        {
            if (
                Vector3.Distance(RunnerGroup.GetChild(i).position, GetRunnerLocalPositions(i))
                > 0.1f
            )
                return;
        }
        isLerp = false;
    }

    public int RunnerCount()
    {
        return RunnerGroup.childCount;
    }

    public void ApplyAmount(Door.TypeDoor doorType, int amount)
    {
        switch (doorType)
        {
            case Door.TypeDoor.Plus:
                SetSerialRuner(RunnerCount() + amount);
                break;
            case Door.TypeDoor.Subtrack:
                SetSerialRuner(Mathf.Max(1, RunnerCount() - amount));
                break;
            case Door.TypeDoor.Multiply:
                SetSerialRuner(RunnerCount() * amount);
                break;
            case Door.TypeDoor.Division:
                if (amount <= 0)
                    return;
                SetSerialRuner(Mathf.Max(1, RunnerCount() / amount));
                break;
        }
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
