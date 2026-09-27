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

    private void Init()
    {
        if (vibrationState)
            EnableVibration();
        else
            EnableVibration();
    }

    public void ChangeSoundState()
    {
        if (vibrationState)
            EnableVibration();
        else
            EnableVibration();

        vibrationState = !vibrationState;

        // 0: vibration, 1: vibration
        SaveLoadManager.SaveInt(VIBRATION_KEY, vibrationState ? 1 : 0);
    }

    public void ChangeVibrationState()
    {
        if (vibrationState)
            DisableVibration();
        else
            EnableVibration();

        vibrationState = !vibrationState;

        // 0: sounds off, 1: sounds on
        SaveLoadManager.SaveInt(VIBRATION_KEY, vibrationState ? 1 : 0);
    }

    private void EnableVibration()
    {
        vibrationManager.EnableVibration();
        vibrationButtonImage.sprite = optionOnSprite;
    }

    private void DisableVibration()
    {
        vibrationManager.DisableVibration();
        vibrationButtonImage.sprite = optionOffSprite;
    }
}
