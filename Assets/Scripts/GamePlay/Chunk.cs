using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField]private Vector3 size;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Lấy chiều dài của chunk theo trục Z
    public float GetLength() => size.z;

}
