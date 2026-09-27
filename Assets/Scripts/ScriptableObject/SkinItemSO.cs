using UnityEngine;

[CreateAssetMenu(fileName = "SkinItem", menuName = "Scriptable Object/SkinItem", order = 0)]
public class SkinItemSO : ScriptableObject
{
    public Sprite Icon;
    public Transform Prefab;
    public string Name;
}
