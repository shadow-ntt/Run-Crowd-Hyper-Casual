using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    [SerializeField]
    private GameObject border;

    [SerializeField]
    private GameObject lockIcon;

    [SerializeField]
    [FormerlySerializedAs("icon")]
    private GameObject characterIcon;

    [SerializeField]
    private Button button;

    public SkinItemSO SkinItemSO { get; set; }

    void OnEnable()
    {
        StoreManager.onSelectedSkin += HandleSelectedSkin;
        StoreManager.onOpenSkin += HandleOpen;
    }

    void OnDisable()
    {
        StoreManager.onSelectedSkin -= HandleSelectedSkin;
        StoreManager.onOpenSkin -= HandleOpen;
    }

    void Start()
    {
        if (StoreManager.Instance.IsSkinItemUnlocked(SkinItemSO.Name))
        {
            OnUnlock();
            SetSpriteIcon(SkinItemSO.Icon);
        }
        else
        {
            OnLock();
        }

        UpdateSelectedVisual(StoreManager.Instance.GetSkinItemSelected());

        button.onClick.AddListener(Onclick);
    }

    public void OnLock()
    {
        lockIcon.SetActive(true);
        characterIcon.SetActive(false);
        button.interactable = false;
    }

    public void OnUnlock()
    {
        lockIcon.SetActive(false);
        characterIcon.SetActive(true);
        button.interactable = true;
    }

    void HandleSelectedSkin(SkinItemSO skinItemSO)
    {
        UpdateSelectedVisual(skinItemSO);
    }

    void UpdateSelectedVisual(SkinItemSO selectedSkin)
    {
        border.SetActive(SkinItemSO.Name.Equals(selectedSkin.Name));
    }

    void HandleOpen(SkinItemSO skinItemSO)
    {
        if (SkinItemSO.Name.Equals(skinItemSO.Name))
        {
            OnUnlock();
            SetSpriteIcon(SkinItemSO.Icon);
            StoreManager.Instance.SelectSkin(skinItemSO);
        }
    }

    void SetSpriteIcon(Sprite sprite)
    {
        characterIcon.GetComponent<Image>().sprite = sprite;
    }

    void Onclick()
    {
        if (StoreManager.Instance.IsSkinItemUnlocked(SkinItemSO.Name))
            StoreManager.Instance.SelectSkin(SkinItemSO);
    }
}
