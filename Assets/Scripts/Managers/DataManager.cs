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
    public static event Action<int> onChangeCoin;
    public static event Action<int> onUpLevelIncome;
    public static event Action<int> onUpLevelRunner;

    protected override void Awake()
    {
        base.Awake();
        Coins = SaveLoadManager.LoadInt(COINS, beginCoins);
        LevelIncome = SaveLoadManager.LoadInt(LEVELINCOME, 1);
        AmoutStartRunner = SaveLoadManager.LoadInt(AMOUNT_START_RUNNER, amoutStartRunnerDefault);
    }

    void Start() { }

    // Cộng thêm coin và lưu lại dữ liệu
    public void AddCoins(int coin)
    {
        Coins += coin;
        SaveLoadManager.SaveInt(COINS, Coins);
        onChangeCoin?.Invoke(Coins);
    }

    // Trừ bớt coin nếu đủ số dư và lưu lại dữ liệu
    public void WithDrawCoins(int coin)
    {
        if (coin > Coins)
            return;
        Coins -= coin;
        SaveLoadManager.SaveInt(COINS, Coins);
        onChangeCoin?.Invoke(Coins);
    }

    // Nâng cấp level thu nhập (income) khi người chơi đủ coin
    public void HandleUpLevelIncome()
    {
        if (IsEnoughBuyUpLevelIncome())
        {
            WithDrawCoins(GetCoinIncome());
            SaveLoadManager.SaveInt(LEVELINCOME, ++LevelIncome);
            onUpLevelIncome?.Invoke(LevelIncome);
        }
    }

    // Nâng cấp số lượng runner ban đầu khi người chơi đủ coin
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

    // Thưởng coin cho người chơi sau khi xem quảng cáo
    public void RewardCoin()
    {
        AddCoins(coinsReward);
    }

    // Lấy lượng coin nhận được theo LevelIncome hiện tại
    public int GetCoinIncome() => LevelIncome * 100;

    public int PriceUpgradeAmountStartRunner => priceUpgradeAmountStartRunner;

    // Tính cấp độ nâng cấp runner hiện tại
    public int LevelUnit() => AmoutStartRunner - amoutStartRunnerDefault;

    // Kiểm tra xem có đủ coin để nâng cấp thu nhập hay không
    public bool IsEnoughBuyUpLevelIncome() => Coins >= GetCoinIncome();

    // Kiểm tra xem có đủ coin để nâng cấp số lượng runner hay không
    public bool IsEnoughBuyUpLevelRunner() => Coins >= priceUpgradeAmountStartRunner;
}
