using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private Door leftDoor;
    [SerializeField] private Door rightDoor;

    public Door LeftDoor => leftDoor;
    public Door RightDoor => rightDoor;

    // Lấy cửa tương ứng (trái hoặc phải) dựa theo vị trí trục X
    public Door GetDoorByX(float xPos)
    {
        if (xPos < 0)
            return leftDoor;
        else
            return rightDoor;
    }

    // Tắt các collider và hủy cụm cửa
    public void Disable()
    {
        // Tat toan bo Collider trong cum cua
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
