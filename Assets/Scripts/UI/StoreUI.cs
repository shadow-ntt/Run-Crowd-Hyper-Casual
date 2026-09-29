using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : Menu
{
    [SerializeField]
    private GameObject buttonSelectGroup;

    [SerializeField]
    private SkinButton buttonPrefab;

    [SerializeField]
    private Button buttonOpen;

    [SerializeField]
    private TMP_Text textPriceOpen;

    void OnEnable()
    {
        DataManager.onChangeCoin += UpdateButtonOpen;
        StoreManager.onOpenSkin += HandleOpenSkin;
        UpdateButtonOpen();
    }

    void OnDisable()
    {
        DataManager.onChangeCoin -= UpdateButtonOpen;
        StoreManager.onOpenSkin -= HandleOpenSkin;
    }

    // Hook khi menu mở: cập nhật trạng thái nút mở khóa
    protected override void OnOpen()
    {
        base.OnOpen();
        UpdateButtonOpen();
    }

    void Start()
    {
        Generate();
        UpdateButtonOpen();
    }

    // Khởi tạo danh sách các button skin trong shop từ danh sách SkinItems
    void Generate()
    {
        foreach (var skinItem in StoreManager.Instance.SkinItems)
        {
            SkinButton skinButton = Instantiate(buttonPrefab, buttonSelectGroup.transform);
            skinButton.SkinItemSO = skinItem;
        }
    }

    // Cập nhật trạng thái tương tác và text hiển thị giá của nút mở khóa ngẫu nhiên
    public void UpdateButtonOpen(int coin = 0)
    {
        buttonOpen.interactable = StoreManager.Instance.CanUnlockRandom();
        if (!StoreManager.Instance.HasLockedSkin())
        {
            textPriceOpen.text = "FULL";
        }
        else
        {
            textPriceOpen.text = StoreManager.Instance.PriceOpenRandomSkin.ToString();
        }
    }

    // Lắng nghe sự kiện mở khóa skin để cập nhật lại nút mở khóa
    private void HandleOpenSkin(SkinItemSO skinItemSO)
    {
        UpdateButtonOpen();
    }
}
