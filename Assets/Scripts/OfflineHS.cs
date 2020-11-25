using System;
using System.Collections;
using System.Collections.Generic;
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

        
        AddHighscoreEntry(1000, "Jorb");
        AddHighscoreEntry(900, "Jim");
        AddHighscoreEntry(1020, "Fred");
        AddHighscoreEntry(1300, "Margret");
        AddHighscoreEntry(660, "Heath");
        AddHighscoreEntry(2000, "Dean");
        

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        //highScoreContainer = JsonUtility.FromJson<Highscore>(jsonString);

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
        foreach(HighscoreEntry highscoreEntry in highScoreContainer.highscoreEntries)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoresEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
        entryTransform.GetChild(0).GetComponent<Text>().text = rankString;

        int score = highscoresEntry.score;
        entryTransform.GetChild(1).GetComponent<Text>().text = score.ToString();

        string name = highscoresEntry.name;
        entryTransform.GetChild(2).GetComponent<Text>().text = name;
        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name)
    {
        //create highscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry(score, name);

        //Load saved Highscores
        //string jsonString = PlayerPrefs.GetString("highscoreTable");
        //Highscore highscore = JsonUtility.FromJson<Highscore>(jsonString);

        //Add new entry to Highscores
        highScoreContainer.highscoreEntries.Add(highscoreEntry);

        //save updated Highscores
        //string json = JsonUtility.ToJson(highscore);
        //PlayerPrefs.SetString("highscoreTable", json);
        //PlayerPrefs.Save();
    }
}
