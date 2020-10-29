using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
  
    public Timer timer;
    public Canvas hud;

    public string menuSceneName = "Menu Scene";

    // Start is called before the first frame update
    void Start()
    {
        if(timer.time == 0)
        {
            hud.gameObject.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && timer.time <= 0)
        {
            SceneManager.LoadScene("Dan's Test Scene");
        }
    }
    

}
