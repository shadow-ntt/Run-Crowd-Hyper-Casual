using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private Vector3 size;
    private Collider[] colliders = new Collider[10];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        HandleCollision();
    }

    void HandleCollision()
    {
        int num = Physics.OverlapBoxNonAlloc(
            transform.position,
            size / 2,
            colliders,
            transform.rotation,
            LayerMask.GetMask("Runner")
        );
        if (num > 0)
        {
            Player.Instance.RegisterObstacle(this);
            for (int i = 0; i < num; i++)
            {
                Destroy(colliders[i].gameObject);
            }
            StartCoroutine(A());
            Player.Instance.UnRegisterObstacle(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, size + new Vector3(0f, 1f, 0f));
    }

    private IEnumerator A()
    {
        yield return new WaitForSeconds(0.2f);
    }
}
