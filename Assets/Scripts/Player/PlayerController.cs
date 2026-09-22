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

    public bool IsMoving = true;
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

    private void OnStartCombat()
    {
        IsMoving = false;
    }

    private void OnEndCombat()
    {
        IsMoving = true;
    }

    private void OnChangeGameStateCallBack(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Game)
            IsMoving = true;
        else
            IsMoving = false;
    }

    //

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

    void MoveToward()
    {
        transform.position += Vector3.forward * Time.deltaTime * runSpeed;
    }
}
