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

    // Hook khi menu mở: làm mới toàn bộ giao diện nâng cấp
    protected override void OnOpen()
    {
        base.OnOpen();
        UpdateUI();
    }

    void Start()
    {
        UpdateUI();
    }

    // Cập nhật text level, giá nâng cấp và trạng thái các nút bấm trên UI
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

    // Cập nhật trạng thái cho phép bấm của các nút nâng cấp theo số coin hiện có
    public void UpdateButtonsState()
    {
        buttonUnit.interactable = DataManager.Instance.IsEnoughBuyUpLevelRunner();
        buttonIncome.interactable = DataManager.Instance.IsEnoughBuyUpLevelIncome();
    }

    // Lắng nghe sự kiện thay đổi coin để cập nhật trạng thái các nút bấm
    private void HandleChangeCoin(int coins)
    {
        UpdateButtonsState();
    }

    // Lắng nghe sự kiện nâng cấp runner để làm mới giao diện
    private void HandleUpLevelUnit(int level)
    {
        UpdateUI();
    }

    // Lắng nghe sự kiện nâng cấp income để làm mới giao diện
    private void HandleUpLevelIncome(int level)
    {
        UpdateUI();
    }

    // Bắt đầu màn chơi bằng cách chuyển trạng thái game sang Gameplay
    public void StartGame()
    {
        GameManager.Instance.ChangeGameState(GameManager.GameState.Game);
    }
}
