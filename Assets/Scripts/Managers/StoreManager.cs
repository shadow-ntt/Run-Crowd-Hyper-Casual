using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreManager : Singleton<StoreManager>
{
    [SerializeField]
    private SkinItemSO[] skinItems;

    [SerializeField]
    private string nameSkinSelectedDefault = "skin1";

    [SerializeField]
    private int priceOpenRandomSkin = 1000;

    //0: locked, 1: unlocked
    private string NAME_SKINITEM_SELECTED = "NAME_SKINITEM_SELECTED";

    //
    public SkinItemSO[] SkinItems => skinItems;
    public int PriceOpenRandomSkin => priceOpenRandomSkin;

    //
    public static event Action<SkinItemSO> onSelectedSkin;
    public static event Action<SkinItemSO> onOpenSkin;

    protected override void Awake()
    {
        base.Awake();
        //Default skin first is unlocked
        SaveLoadManager.SaveInt(nameSkinSelectedDefault, 1);
    }

    void Start() { }

    void Update() { }

    // Tìm và lấy SkinItemSO theo tên
    public SkinItemSO GetSkinItem(string name) =>
        skinItems.FirstOrDefault(skinItem => skinItem.Name.Equals(name));

    // Kiểm tra xem skin tương ứng đã được mở khóa hay chưa
    public bool IsSkinItemUnlocked(string name)
    {
        return SaveLoadManager.LoadInt(name, 0) == 1;
    }

    // Kiểm tra xem còn skin nào chưa được mở khóa hay không
    public bool HasLockedSkin()
    {
        foreach (var skinItem in skinItems)
        {
            if (!IsSkinItemUnlocked(skinItem.Name))
                return true;
        }
        return false;
    }

    // Kiểm tra điều kiện có thể mở khóa ngẫu nhiên skin (còn skin khóa và đủ coin)
    public bool CanUnlockRandom()
    {
        return HasLockedSkin() && DataManager.Instance.Coins >= priceOpenRandomSkin;
    }

    // Mở khóa ngẫu nhiên một skin chưa sở hữu bằng coin
    public void UnlockRandom()
    {
        if (!CanUnlockRandom())
            return;

        List<SkinItemSO> skinItemSOLocks = new List<SkinItemSO>();
        foreach (var skinItem in skinItems)
        {
            if (!IsSkinItemUnlocked(skinItem.Name))
            {
                skinItemSOLocks.Add(skinItem);
            }
        }

        if (skinItemSOLocks.Count == 0)
            return;

        DataManager.Instance.WithDrawCoins(priceOpenRandomSkin);
        int random = UnityEngine.Random.Range(0, skinItemSOLocks.Count);
        SkinItemSO unlockedSkin = skinItemSOLocks[random];
        SaveLoadManager.SaveInt(unlockedSkin.Name, 1);
        SelectSkin(unlockedSkin);
        onOpenSkin?.Invoke(unlockedSkin);
    }

    // Lấy thông tin skin hiện đang được người chơi lựa chọn
    public SkinItemSO GetSkinItemSelected()
    {
        string nameItem = SaveLoadManager.LoadString(
            NAME_SKINITEM_SELECTED,
            nameSkinSelectedDefault
        );
        return skinItems.FirstOrDefault(item => item.Name.Equals(nameItem));
    }

    // Chọn và trang bị skin, đồng thời lưu lại thiết lập
    public void SelectSkin(SkinItemSO skinItemSO)
    {
        SaveLoadManager.SaveString(NAME_SKINITEM_SELECTED, skinItemSO.Name);
        onSelectedSkin?.Invoke(skinItemSO);
    }
}
