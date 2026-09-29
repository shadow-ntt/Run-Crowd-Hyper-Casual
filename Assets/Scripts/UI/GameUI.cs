using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    [SerializeField]
    private Slider progressLevel;

    [SerializeField]
    private TMP_Text textLevel;

    [SerializeField]
    private string levelPrefix = "Level: ";

    void OnEnable()
    {
        GameManager.OnChangeGameState += ChangeGameStateCallBack;
        Road.onUpLevel += HandleUpLevel;
    }

    void OnDisable()
    {
        GameManager.OnChangeGameState -= ChangeGameStateCallBack;
        Road.onUpLevel -= HandleUpLevel;
    }

    // Cập nhật text hiển thị số màn chơi khi người chơi lên cấp
    private void HandleUpLevel(int level)
    {
        textLevel.text = $"{levelPrefix}{level}";
    }

    // Xử lý giao diện khi trạng thái game thay đổi
    private void ChangeGameStateCallBack(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Menu) { }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        HandleUpLevel(Road.Instance.CurrentLevel);
    }

    void Update() { }

    // Cập nhật giá trị thanh trượt thể hiện tiến độ hoàn thành màn chơi
    public void setProgressLevel(float value)
    {
        progressLevel.value = value;
    }
}
