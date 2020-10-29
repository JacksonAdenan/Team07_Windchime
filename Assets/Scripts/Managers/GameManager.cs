using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{ 
    PLAYING,
    GAMEOVER
}
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public GameState currentGameState;

    public CookingManager cookingManager;
    public OrderManager orderManager;
    public ScoreManager scoreManager;
    public MenuManager menuManager;

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
        currentGameState = GameState.PLAYING;

    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTime >= 1)
        { 
            timer += Time.deltaTime;
        }

        Countdown();
        if (gameTime <= 0)
        {
            menuManager.DisplayGameOver();
            currentGameState = GameState.GAMEOVER;
        }
    }

    void Countdown()
    {
        if (timer >= 1)
        {
            gameTime -= 1;
            timer = 0;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("DansTestScene7");
    }
}
