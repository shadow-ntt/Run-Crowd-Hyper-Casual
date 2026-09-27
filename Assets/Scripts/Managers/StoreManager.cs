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
    public static Action<SkinItemSO> onSelectedSkin;
    public static Action<SkinItemSO> onOpenSkin;

    protected override void Awake()
    {
        base.Awake();
        //Default skin first is unlocked
        SaveLoadManager.SaveInt(nameSkinSelectedDefault, 1);
    }

    void Start() { }

    void Update() { }

    public SkinItemSO GetSkinItem(string name) =>
        skinItems.FirstOrDefault(skinItem => skinItem.Name.Equals(name));

    public bool IsSkinItemUnlocked(string name)
    {
        return SaveLoadManager.LoadInt(name, 0) == 1;
    }

    public bool HasLockedSkin()
    {
        foreach (var skinItem in skinItems)
        {
            if (!IsSkinItemUnlocked(skinItem.Name))
                return true;
        }
        return false;
    }

    public bool CanUnlockRandom()
    {
        return HasLockedSkin() && DataManager.Instance.Coins >= priceOpenRandomSkin;
    }

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

    public SkinItemSO GetSkinItemSelected()
    {
        string nameItem = SaveLoadManager.LoadString(
            NAME_SKINITEM_SELECTED,
            nameSkinSelectedDefault
        );
        return skinItems.FirstOrDefault(item => item.Name.Equals(nameItem));
    }

    public void SelectSkin(SkinItemSO skinItemSO)
    {
        SaveLoadManager.SaveString(NAME_SKINITEM_SELECTED, skinItemSO.Name);
        onSelectedSkin?.Invoke(skinItemSO);
    }
}
