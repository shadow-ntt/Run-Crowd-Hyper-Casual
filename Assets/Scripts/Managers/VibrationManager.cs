using System;
using System.Collections;
using System.Collections.Generic;
using Solo.MOST_IN_ONE;
using UnityEngine;
using UnityEngine.UI;
using static Solo.MOST_IN_ONE.MOST_HapticFeedback;

public class VibrationManager : MonoBehaviour
{
    void OnEnable()
    {
        PlayerCollision.onDoorHit += Vibrate;
        Runner.onRunnerDead += Vibrate;

        GameManager.OnChangeGameState += GameStateChangedCallback;
    }

    private void OnDisable()
    {
        PlayerCollision.onDoorHit -= Vibrate;
        Runner.onRunnerDead -= Vibrate;

        GameManager.OnChangeGameState -= GameStateChangedCallback;
    }

    private void Vibrate()
    {
        MOST_HapticFeedback.Generate(MOST_HapticFeedback.HapticTypes.LightImpact);
    }

    private void GameStateChangedCallback(GameManager.GameState state)
    {
        if (state == GameManager.GameState.LevelComplete)
            Vibrate();
        else if (state == GameManager.GameState.GameOver)
            Vibrate();
    }

    public void EnableVibration()
    {
        MOST_HapticFeedback.HapticsEnabled = true;
    }

    public void DisableVibration()
    {
        MOST_HapticFeedback.HapticsEnabled = false;
    }
}
