using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float runSpeed;

    [SerializeField]
    private float slidSpeed;

    [SerializeField]
    private float maxX = 5;
    private Vector3 clickPositionPlayer;
    private Vector3 clickPositionMouse;

    private bool IsMoving = false;
    private Player player;

    //
    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (IsMoving)
        {
            HandlePlayerControll();
            MoveToward();
        }
    }

    void OnEnable()
    {
        GameManager.OnChangeGameState += OnChangeGameStateCallBack;
        EnemyGroup.StartCombat += OnStartCombat;
        EnemyGroup.EndCombat += OnEndCombat;
    }

    void OnDisable()
    {
        GameManager.OnChangeGameState -= OnChangeGameStateCallBack;
        EnemyGroup.StartCombat -= OnStartCombat;
        EnemyGroup.EndCombat -= OnEndCombat;
    }

    // Tạm dừng di chuyển khi bắt đầu giao chiến với nhóm kẻ địch
    private void OnStartCombat()
    {
        IsMoving = false;
    }

    // Tiếp tục di chuyển sau khi kết thúc trận chiến nếu chưa thua
    private void OnEndCombat()
    {
        if (!GameManager.Instance.IsGameOverState())
            IsMoving = true;
    }

    // Bật hoặc tắt trạng thái di chuyển dựa theo trạng thái game
    private void OnChangeGameStateCallBack(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Game)
            IsMoving = true;
        else
            IsMoving = false;
    }

    //
    // Xử lý thao tác kéo/vuốt ngang của người chơi và kẹp trong giới hạn đường chạy
    void HandlePlayerControll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPositionMouse = Input.mousePosition;
            clickPositionPlayer = transform.position;
        }
        else if (Input.GetMouseButton(0))
        {
            //clickPositionMouse = Input.mousePosition;
            float xDifferent = (Input.mousePosition.x - clickPositionMouse.x) / Screen.width;
            Vector3 position = transform.position;
            position.x = xDifferent * slidSpeed + clickPositionPlayer.x;
            float radiusContain = maxX - player.GetRadiusGroup();
            if (position.x > 0 && position.x > radiusContain)
                position.x = radiusContain;
            if (position.x < 0 && position.x < -radiusContain)
                position.x = -radiusContain;
            transform.position = position;
        }
    }

    // Tự động di chuyển cả nhóm tiến về phía trước theo tốc độ chạy
    void MoveToward()
    {
        transform.position += Vector3.forward * Time.deltaTime * runSpeed;
    }
}
