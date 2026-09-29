using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private AudioSource doorHitSound;

    [SerializeField]
    private AudioSource runnerDieSound;

    [SerializeField]
    private AudioSource gameOverSound;

    [SerializeField]
    private AudioSource levelCompleteSound;

    [SerializeField]
    private AudioSource buttonSound;

    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerCollision.onDoorHit += PlayDoorHitSound;
        Runner.onRunnerDead += PlayRunnerDieSound;

        GameManager.OnChangeGameState += GameStateChangedCallBack;
    }

    private void OnDisable()
    {
        PlayerCollision.onDoorHit -= PlayDoorHitSound;
        Runner.onRunnerDead -= PlayRunnerDieSound;

        GameManager.OnChangeGameState -= GameStateChangedCallBack;
    }

    // Lắng nghe thay đổi trạng thái game để phát âm thanh kết thúc tương ứng
    private void GameStateChangedCallBack(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
            PlayGameOverSound();
        else if (state == GameManager.GameState.LevelComplete)
            PlayLevelCompleteSound();
    }

    // Phát âm thanh khi người chơi đi qua cửa
    public void PlayDoorHitSound()
    {
        doorHitSound.Play();
    }

    // Phát âm thanh khi một runner bị tiêu diệt
    public void PlayRunnerDieSound()
    {
        runnerDieSound.Play();
    }

    // Phát âm thanh khi thua trận
    public void PlayGameOverSound()
    {
        gameOverSound.Play();
    }

    // Phát âm thanh khi hoàn thành màn chơi
    public void PlayLevelCompleteSound()
    {
        levelCompleteSound.Play();
    }

    // Phát âm thanh khi hoàn thành màn chơi
    public void PlayButtonSound()
    {
        buttonSound.Play();
    }
}
