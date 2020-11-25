using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{ 
    PLAYING,
    GAMEOVER,
    WAITING
}
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public GameState currentGameState;

    public CookingManager cookingManager;
    public OrderManager orderManager;
    public ScoreManager scoreManager;
    public MenuManager menuManager;
    public MouseLook playerController;
    public ColourManager colourManager;
    public SoundManager soundManager;
    public MonitorManager monitorManager;
    

    public float gameTime = 5;
    float timer = 0;


    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<GameManager>();
        }

        return instance;
    }
    // Start is called before the first frame update

    void Start()
    {
        currentGameState = GameState.WAITING;
        
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //ColourManager.InitColours();
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameTime >= 1)
        //{ 
        //    timer += Time.deltaTime;
        //}

        if (currentGameState == GameState.PLAYING)
        { 
            Countdown();
        }
        if (gameTime <= 0)
        {
            menuManager.DisplayGameOver();
            currentGameState = GameState.GAMEOVER;
        }
    }

    void Countdown()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            gameTime -= 1;
            timer = 0;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("ModularToolKit");
    }

    public void StartGame()
    {
        Debug.Log("Game started.");
        currentGameState = GameState.PLAYING;
        playerController.customPlayerAnimator.StartAnimation();
    }
}
