using System;
using UnityEngine;

public class Runner : MonoBehaviour
{
    [SerializeField]
    private float speed;
    public static Action onRunnerDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void OnDestroy()
    {
        onRunnerDead?.Invoke();
    }
}
