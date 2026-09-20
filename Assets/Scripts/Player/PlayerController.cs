using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float runSpeed;

    [SerializeField]
    public float slidSpeed;
    private Vector3 clickPositionPlayer;
    private Vector3 clickPositionMouse;

    public bool IsMoving = true;

    //
    void Update()
    {
        if (IsMoving)
        {
            HandlePlayerControll();
            MoveToward();
        }
    }

    void OnDisable()
    {
        GameManager.OnChangeGameState -= OnChangeGameStateCallBack;
    }

    void OnEnable()
    {
        GameManager.OnChangeGameState += OnChangeGameStateCallBack;
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
            transform.position = position;
        }
    }

    void MoveToward()
    {
        transform.position += Vector3.forward * Time.deltaTime * runSpeed;
    }
}
