using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Player player;
    private Collider[] hitColliders = new Collider[10];
    public static event Action onDoorHit;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        HandleDoorCollision();
    }

    // Quét phát hiện và xử lý va chạm với cửa để áp dụng hiệu ứng và kích hoạt âm thanh
    private void HandleDoorCollision()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, 1f, hitColliders);
        for (int i = 0; i < count; i++)
        {
            Collider col = hitColliders[i];
            if (col == null)
                continue;

            if (col.TryGetComponent(out Doors doors))
            {
                Door chosenDoor = doors.GetDoorByX(transform.position.x);
                if (chosenDoor != null)
                {
                    player.ApplyAmount(chosenDoor.DoorType, chosenDoor.Value);
                    onDoorHit?.Invoke();
                }
                doors.Disable();
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Collision EndLine-Finish
        if (other.CompareTag("EndLine"))
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.LevelComplete);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
