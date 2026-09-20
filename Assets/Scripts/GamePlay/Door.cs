using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum TypeDoor {Plus, Multiply, Division, Subtrack};
    [SerializeField] private TMP_Text tMP_Text;
    [SerializeField] private Color IncreaseColor;
    [SerializeField] private Color DecreaseColor;

    [SerializeField] private int value;
    [SerializeField] private TypeDoor typeDoor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Generate(typeDoor, value);
    }

    // Update is called once per frame
    void Update()
    {      
    }

    public void Generate(TypeDoor typeDoor, int value){
        switch (typeDoor)
        {
            case TypeDoor.Subtrack:
                GetComponent<SpriteRenderer>().color =DecreaseColor;
                tMP_Text.text="-"+value;
                break;
            case TypeDoor.Division:
                GetComponent<SpriteRenderer>().color =DecreaseColor;
                tMP_Text.text="/"+value;
                break;
            case TypeDoor.Multiply:
                GetComponent<SpriteRenderer>().color =IncreaseColor;
                tMP_Text.text="*"+value;
                break;
            case TypeDoor.Plus:
            default:
                GetComponent<SpriteRenderer>().color =IncreaseColor;
                tMP_Text.text="+"+value;
                break;
        }
    }
    public int Active(int n)
    {
        switch (typeDoor)
        {
            case TypeDoor.Subtrack:
                if(n<value) return 1;
                return n-value;
            case TypeDoor.Division:
                return n/value;
            case TypeDoor.Multiply:
                return n*value;
            case TypeDoor.Plus:
            default:
                return n+value;
        }
    }
}
