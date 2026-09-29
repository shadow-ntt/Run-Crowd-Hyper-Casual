using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VibrationSetting : Menu
{
    [Header("References")]
    [SerializeField]
    private VibrationManager vibrationManager;

    [SerializeField]
    private Image vibrationButtonImage;

    [SerializeField]
    private Sprite optionOnSprite;

    [SerializeField]
    private Sprite optionOffSprite;

    private bool vibrationState = true;
    private const string VIBRATION_KEY = "vibration";

    protected override void Awake()
    {
        base.Awake();
        // For the first time in game
        vibrationState = SaveLoadManager.LoadInt(VIBRATION_KEY, 1) == 1;
    }

    private void Start()
    {
        Init();
    }

    // Khởi tạo trạng thái rung ban đầu từ dữ liệu đã lưu
    private void Init()
    {
        if (vibrationState)
            EnableVibration();
        else
            DisableVibration();
    }

    // Bật hoặc tắt trạng thái rung và lưu vào PlayerPrefs
    public void ChangeVibrationState()
    {
        vibrationState = !vibrationState;
        if (vibrationState)
            EnableVibration();
        else
            DisableVibration();

        // 0: sounds off, 1: sounds on
        SaveLoadManager.SaveInt(VIBRATION_KEY, vibrationState ? 1 : 0);
    }

    // Bật rung và cập nhật hình ảnh nút bật
    private void EnableVibration()
    {
        vibrationManager.EnableVibration();
        vibrationButtonImage.sprite = optionOnSprite;
    }

    // Tắt rung và cập nhật hình ảnh nút tắt
    private void DisableVibration()
    {
        vibrationManager.DisableVibration();
        vibrationButtonImage.sprite = optionOffSprite;
    }
}
