using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfflineHS : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    Highscore highScoreContainer;

    private void Start()
    {

        highScoreContainer = new Highscore();
        highScoreContainer.Start();

        //entryContainer = transform.Find("highscoreEntryContainer");
        //entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        //entryTemplate.gameObject.SetActive(true);


        //AddHighscoreEntry(0, "GOA");
        //AddHighscoreEntry(1, "DEB");
        //AddHighscoreEntry(2, "SAN");
        //AddHighscoreEntry(5, "WCS");
        //AddHighscoreEntry(6, "DAN");




        string jsonString = PlayerPrefs.GetString("highscoreTable");
        highScoreContainer = JsonUtility.FromJson<Highscore>(jsonString);

        // Sort entry list by score
        for (int i = 0; i < highScoreContainer.highscoreEntries.Count; i++)
        {
            for(int j = i + 1; j < highScoreContainer.highscoreEntries.Count; j++)
            {
                if(highScoreContainer.highscoreEntries[j].score > highScoreContainer.highscoreEntries[i].score)
                {
                    //swap
                    HighscoreEntry tmp = highScoreContainer.highscoreEntries[i];
                    highScoreContainer.highscoreEntries[i] = highScoreContainer.highscoreEntries[j];
                    highScoreContainer.highscoreEntries[j] = tmp;
                }
            }
        }
        highscoreEntryTransformList = new List<Transform>();
        //foreach(HighscoreEntry highscoreEntry in highScoreContainer.highscoreEntries)
        //{
        //    CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        //}
        for (int i = 0; i <= 2; i++)
        {
            HighscoreEntry highscoreEntry = highScoreContainer.highscoreEntries[i];
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }


        
        //to add to the an empty string use this line of code
        //Highscore highscores = new Highscore { highscoreEntries = highScoreContainer.highscoreEntries };
        //string json = JsonUtility.ToJson(highscores);
        //PlayerPrefs.SetString("highscoreTable", json);
        //PlayerPrefs.Save();
        //Debug.Log(PlayerPrefs.GetString("highscoreTable"));

    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoresEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 0.2f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                //rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
        //entryTransform.GetChild(0).GetComponent<Text>().text = rankString;
        var firstChild = entryTransform.GetChild(0);
        var textComponent = firstChild.GetComponent<TextMeshProUGUI>();
        textComponent.text = rankString;

        int score = highscoresEntry.score;
        entryTransform.GetChild(1).GetComponent<TextMeshProUGUI>().text = score.ToString();

        string name = highscoresEntry.name;
        entryTransform.GetChild(2).GetComponent<TextMeshProUGUI>().text = name;
        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name)
    {
        //create highscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry(score, name);

        //Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscore highscore = JsonUtility.FromJson<Highscore>(jsonString);

        //Add new entry to Highscores
        //highScoreContainer.highscoreEntries.Add(highscoreEntry);
        highscore.highscoreEntries.Add(highscoreEntry);

        //save updated Highscores
        string json = JsonUtility.ToJson(highscore);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();

       

       
    }
}
