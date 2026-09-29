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

    // Đưa nút skin về trạng thái khóa (hiển thị ổ khóa, ẩn icon và tắt bấm)
    public void OnLock()
    {
        lockIcon.SetActive(true);
        characterIcon.SetActive(false);
        button.interactable = false;
    }

    // Đưa nút skin về trạng thái đã mở khóa (hiện icon skin, bật bấm)
    public void OnUnlock()
    {
        lockIcon.SetActive(false);
        characterIcon.SetActive(true);
        button.interactable = true;
    }

    // Lắng nghe sự kiện chọn skin để cập nhật hiển thị viền chọn
    void HandleSelectedSkin(SkinItemSO skinItemSO)
    {
        UpdateSelectedVisual(skinItemSO);
    }

    // Cập nhật viền highlight nếu skin này đang được chọn
    void UpdateSelectedVisual(SkinItemSO selectedSkin)
    {
        border.SetActive(SkinItemSO.Name.Equals(selectedSkin.Name));
    }

    // Xử lý khi mở khóa thành công skin này từ hộp ngẫu nhiên
    void HandleOpen(SkinItemSO skinItemSO)
    {
        if (SkinItemSO.Name.Equals(skinItemSO.Name))
        {
            OnUnlock();
            SetSpriteIcon(SkinItemSO.Icon);
            StoreManager.Instance.SelectSkin(skinItemSO);
        }
    }

    // Gán sprite hình ảnh cho icon skin
    void SetSpriteIcon(Sprite sprite)
    {
        characterIcon.GetComponent<Image>().sprite = sprite;
    }

    // Xử lý khi click vào nút để chọn trang bị skin (nếu đã mở khóa)
    void Onclick()
    {
        if (StoreManager.Instance.IsSkinItemUnlocked(SkinItemSO.Name))
            StoreManager.Instance.SelectSkin(SkinItemSO);
    }
}
