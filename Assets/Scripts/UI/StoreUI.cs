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

    void Generate()
    {
        foreach (var skinItem in StoreManager.Instance.SkinItems)
        {
            SkinButton skinButton = Instantiate(buttonPrefab, buttonSelectGroup.transform);
            skinButton.SkinItemSO = skinItem;
        }
    }

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

    private void HandleOpenSkin(SkinItemSO skinItemSO)
    {
        UpdateButtonOpen();
    }
}
