using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum TypeDoor { Plus, Multiply, Division, Subtrack };

    [SerializeField] private TMP_Text tMP_Text;
    [SerializeField] private Color IncreaseColor;
    [SerializeField] private Color DecreaseColor;

    [SerializeField] private int value;
    [SerializeField] private TypeDoor typeDoor;

    public TypeDoor DoorType => typeDoor;
    public int Value => value;

    void Start()
    {
        Generate(typeDoor, value);
    }

    // Khởi tạo màu sắc và văn bản hiển thị dựa trên loại cửa và giá trị
    public void Generate(TypeDoor typeDoor, int value)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        switch (typeDoor)
        {
            case TypeDoor.Subtrack:
                sr.color = DecreaseColor;
                tMP_Text.text = "-" + value;
                break;
            case TypeDoor.Division:
                sr.color = DecreaseColor;
                tMP_Text.text = "/" + value;
                break;
            case TypeDoor.Multiply:
                sr.color = IncreaseColor;
                tMP_Text.text = "x" + value;
                break;
            case TypeDoor.Plus:
            default:
                sr.color = IncreaseColor;
                tMP_Text.text = "+" + value;
                break;
        }
    }
}
