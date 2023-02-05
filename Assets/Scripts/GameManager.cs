using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Fader fader;
    [SerializeField] private int mainMenuSceneIndex;
    [SerializeField] private int nextScene;

    public static GameManager Instance;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
    }


    private void Start()
    {
        fader.FadeIn();
    }
    private void OnPause(InputValue value)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        Time.timeScale = 1 - Time.timeScale;
        if (Time.timeScale <= 0)
            pauseMenu.SetActive(true);
        else
            pauseMenu.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        fader.TransitionToScene(mainMenuSceneIndex);
    }

    public void EndGame()
    {
        fader.TransitionToScene(nextScene);
    }

    public void PlayerDead()
    {
        SoundsManager.Instance.GameOver();
        fader.TransitionToScene(SceneManager.GetActiveScene().buildIndex, 2);
    }
}
