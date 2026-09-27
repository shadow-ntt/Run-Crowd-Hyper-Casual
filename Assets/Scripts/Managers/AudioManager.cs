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

    private void GameStateChangedCallBack(GameManager.GameState state)
    {
        if (state == GameManager.GameState.GameOver)
            PlayGameOverSound();
        else if (state == GameManager.GameState.LevelComplete)
            PlayLevelCompleteSound();
    }

    public void PlayDoorHitSound()
    {
        doorHitSound.Play();
    }

    public void PlayRunnerDieSound()
    {
        runnerDieSound.Play();
    }

    public void PlayGameOverSound()
    {
        gameOverSound.Play();
    }

    public void PlayLevelCompleteSound()
    {
        Debug.Log("?");
        levelCompleteSound.Play();
    }
}
