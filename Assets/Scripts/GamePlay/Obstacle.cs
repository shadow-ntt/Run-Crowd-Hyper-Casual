using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private Vector3 size;
    private Collider[] colliders = new Collider[10];

    private bool isRegisted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (size == Vector3.zero)
        {
            size = transform.lossyScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleCollision();
    }

    // Xử lý va chạm quét các runner va phải chướng ngại vật để tiêu diệt
    void HandleCollision()
    {
        int num = Physics.OverlapBoxNonAlloc(
            transform.position,
            size / 2,
            colliders,
            transform.rotation,
            LayerMask.GetMask("Runner")
        );

        bool isColliding = num > 0;

        if (isColliding && !isRegisted)
        {
            Player.Instance.RegisterObstacle(this);
            isRegisted = true;
        }
        else if (!isColliding && isRegisted)
        {
            StartCoroutine(UnregisterAfterDelay(0.2f));
            isRegisted = false;
        }

        for (int i = 0; i < num; i++)
        {
            Destroy(colliders[i].gameObject);
        }
    }

    // Coroutine tạo độ trễ xử lý sau va chạm (backward compatibility)
    private IEnumerator UnregisterAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Player.Instance.UnRegisterObstacle(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
