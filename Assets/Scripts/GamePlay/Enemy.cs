using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void Running()
    {
        GetComponent<Animator>().SetBool("isMoving", true);
    }

    public void Idle()
    {
        GetComponent<Animator>().SetBool("isMoving", false);
    }

    public void MoveToRunner(Transform transformRunner)
    {
        transform.position = Vector3.Lerp(
            transform.position,
            transformRunner.position,
            speed * Time.deltaTime
        );
    }
}
