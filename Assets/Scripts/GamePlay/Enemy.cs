using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Idle();
    }

    // Kích hoạt animation chạy cho enemy
    public void Running()
    {
        if (animator != null)
            animator.SetBool("isMoving", true);
    }

    // Đặt animation enemy về trạng thái đứng yên
    public void Idle()
    {
        if (animator != null)
            animator.SetBool("isMoving", false);
    }

    // Di chuyển enemy về phía runner và tiêu diệt mục tiêu khi tiếp cận đủ gần
    public void MoveToRunner(Transform transformRunner, bool canDestroyTarget = true)
    {
        if (transformRunner == null)
            return;

        Running();
        transform.position = Vector3.MoveTowards(
            transform.position,
            transformRunner.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, transformRunner.position) < 0.2f)
        {
            if (canDestroyTarget && transformRunner != null)
            {
                Destroy(transformRunner.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
