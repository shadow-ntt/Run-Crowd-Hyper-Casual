using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGameUI : Menu
{
    [Header("Level Texts")]
    [SerializeField]
    private TMP_Text textLevelUnit;

    [SerializeField]
    private TMP_Text textLevelIncome;

    [Header("Price Texts")]
    [SerializeField]
    private TMP_Text textPriceUnit;

    [SerializeField]
    private TMP_Text textPriceIncome;

    [Header("Upgrade Buttons")]
    [SerializeField]
    private Button buttonUnit;

    [SerializeField]
    private Button buttonIncome;

    void OnEnable()
    {
        DataManager.onUpLevelIncome += HandleUpLevelIncome;
        DataManager.onUpLevelRunner += HandleUpLevelUnit;
        DataManager.onChangeCoin += HandleChangeCoin;
    }

    void OnDisable()
    {
        DataManager.onUpLevelIncome -= HandleUpLevelIncome;
        DataManager.onUpLevelRunner -= HandleUpLevelUnit;
        DataManager.onChangeCoin -= HandleChangeCoin;
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        UpdateUI();
    }

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // 1. Cập nhật Level
        textLevelUnit.text = $"{DataManager.Instance.LevelUnit()}\n Level";
        textLevelIncome.text = $"{DataManager.Instance.LevelIncome}\n Level";

        // 2. Cập nhật Giá nâng cấp thực tế
        textPriceUnit.text = DataManager.Instance.PriceUpgradeAmountStartRunner.ToString();
        textPriceIncome.text = DataManager.Instance.GetCoinIncome().ToString();

        // 3. Cập nhật trạng thái khả dụng của nút bấm
        UpdateButtonsState();
    }

    public void UpdateButtonsState()
    {
        buttonUnit.interactable = DataManager.Instance.IsEnoughBuyUpLevelRunner();
        buttonIncome.interactable = DataManager.Instance.IsEnoughBuyUpLevelIncome();
    }

    private void HandleChangeCoin(int coins)
    {
        UpdateButtonsState();
    }

    private void HandleUpLevelUnit(int level)
    {
        UpdateUI();
    }

    private void HandleUpLevelIncome(int level)
    {
        UpdateUI();
    }

    public void StartGame()
    {
        GameManager.Instance.ChangeGameState(GameManager.GameState.Game);
    }
}
