using System;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public static event Action onRunnerDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void OnDestroy()
    {
        onRunnerDead?.Invoke();
    }
}
