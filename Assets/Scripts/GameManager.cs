using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState { FirstTouch,Pause,Run,Win,Lose}

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerControl _playerControl;

    public static GameState gameState = new GameState();

    private void Awake()
    {
        gameState = GameState.FirstTouch;
        _playerControl = GameObject.FindGameObjectWithTag("PlayerControl").GetComponent<PlayerControl>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState  == GameState.FirstTouch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                gameState = GameState.Run;
            }
        }

        Debug.Log(gameState);
    }
}
