using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Player player;
    void Awake()
    {
        player = GetComponent<Player>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleCollision();
    }

    //
    void HandleCollision()
    {
         Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 0.0f);
         foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.TryGetComponent<Door>(out Door door))
            {
                int count = door.Active(player.RunnerCount());
                Destroy(door.transform.parent.gameObject);
                player.SetSerialRuner(count);
                return;
            }else if (hitCollider.CompareTag("EndLine"))
            {
                GameManager.Instance.ChangeGameState(GameManager.GameState.LevelComplete);
            }
        }
    }
    
    //
}
