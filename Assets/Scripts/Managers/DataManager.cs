using System;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private string COINS = "coins";
    private string LEVELINCOME = "levelIncome";
    private string AMOUNT_START_RUNNER = "amout_start_runner";

    //
    [SerializeField]
    private int beginCoins = 100;

    [SerializeField]
    private int coinsReward = 200;

    [SerializeField]
    private int priceUpgradeAmountStartRunner = 2000;

    //
    private int amoutStartRunnerDefault = 1;

    //
    public int LevelIncome { get; private set; }
    public int Coins { get; private set; }
    public int AmoutStartRunner { get; private set; }

    //
    public static Action<int> onChangeCoin;
    public static Action<int> onUpLevelIncome;
    public static Action<int> onUpLevelRunner;

    protected override void Awake()
    {
        base.Awake();
        Coins = SaveLoadManager.LoadInt(COINS, beginCoins);
        LevelIncome = SaveLoadManager.LoadInt(LEVELINCOME, 1);
        AmoutStartRunner = SaveLoadManager.LoadInt(AMOUNT_START_RUNNER, amoutStartRunnerDefault);
    }

    void Start() { }

    public void AddCoins(int coin)
    {
        Coins += coin;
        SaveLoadManager.SaveInt(COINS, Coins);
        onChangeCoin?.Invoke(Coins);
    }

    public void WithDrawCoins(int coin)
    {
        if (coin > Coins)
            return;
        Coins -= coin;
        SaveLoadManager.SaveInt(COINS, Coins);
        onChangeCoin?.Invoke(Coins);
    }

    public void HandleUpLevelIncome()
    {
        if (IsEnoughBuyUpLevelIncome())
        {
            WithDrawCoins(GetCoinIncome());
            SaveLoadManager.SaveInt(LEVELINCOME, ++LevelIncome);
            onUpLevelIncome?.Invoke(LevelIncome);
        }
    }

    public void AddPlayRunner()
    {
        if (IsEnoughBuyUpLevelRunner())
        {
            WithDrawCoins(priceUpgradeAmountStartRunner);
            AmoutStartRunner++;
            SaveLoadManager.SaveInt(AMOUNT_START_RUNNER, AmoutStartRunner);
            onUpLevelRunner?.Invoke(AmoutStartRunner);
        }
    }

    // reward ads
    public void RewardCoin()
    {
        AddCoins(coinsReward);
    }

    public int GetCoinIncome() => LevelIncome * 100;

    public int PriceUpgradeAmountStartRunner => priceUpgradeAmountStartRunner;

    public int LevelUnit() => AmoutStartRunner - amoutStartRunnerDefault;

    public bool IsEnoughBuyUpLevelIncome() => Coins >= GetCoinIncome();

    public bool IsEnoughBuyUpLevelRunner() => Coins >= priceUpgradeAmountStartRunner;
}
