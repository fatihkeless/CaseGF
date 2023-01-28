using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    [SerializeField] GameObject tocuhCanvas;
    [SerializeField] GameObject runCanvas;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject winCanvas;
    [SerializeField] GameObject loseCanvas;

    // Start is called before the first frame update
    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (GameManager.gameState)
        {
            case GameState.FirstTouch:
                tocuhCanvas.SetActive(true);
                runCanvas.SetActive(false);
                pauseCanvas.SetActive(false);
                winCanvas.SetActive(false);
                loseCanvas.SetActive(false);

                break;
            case GameState.Run:

                tocuhCanvas.SetActive(false);
                runCanvas.SetActive(true);
                pauseCanvas.SetActive(false);
                winCanvas.SetActive(false);
                loseCanvas.SetActive(false);

                break;
            case GameState.Pause:

                tocuhCanvas.SetActive(false);
                runCanvas.SetActive(false);
                pauseCanvas.SetActive(true);
                winCanvas.SetActive(false);
                loseCanvas.SetActive(false);

                break;

            case GameState.Win:

                tocuhCanvas.SetActive(false);
                runCanvas.SetActive(false);
                pauseCanvas.SetActive(false);
                winCanvas.SetActive(true);
                loseCanvas.SetActive(false);

                break;

            case GameState.Lose:

                tocuhCanvas.SetActive(false);
                runCanvas.SetActive(false);
                pauseCanvas.SetActive(false);
                winCanvas.SetActive(false);
                loseCanvas.SetActive(true);

                break;
        }
    }






    public void replay()
    {

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadSceneAsync(currentScene,LoadSceneMode.Single);


    }
    public void nextLevel()
    {
        int nextSceneIndex = activeSceneIndex() + 1;
        int sceneIndex = SceneManager.sceneCountInBuildSettings - 1;


        if (nextSceneIndex <= sceneIndex)
        {
            SceneManager.LoadSceneAsync(nextSceneIndex);
        }

        if (nextSceneIndex > sceneIndex)
        {
            SceneManager.LoadSceneAsync(0);
        }

    }


    public void Pause()
    {
        GameManager.gameState = GameState.Pause;
    }


    public void Resume()
    {
        GameManager.gameState = GameState.Run;
    }


    private int activeSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
