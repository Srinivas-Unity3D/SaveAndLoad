using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour, ISaveable
{
    public static int score;
    Text text;
    
    void Awake()
    {
        text = GetComponent<Text>();
        ResetScore();
    }

    void Update()
    {
        text.text = "Score: " + score;
    }

    public void ResetScore()
    {
        score = 0;
        if (text != null)
        {
            text.text = "Score: " + score;
        }
    }

    public void Save(SaveData saveData)
    {
        saveData.score = score;
    }

    public void Load(SaveData saveData)
    {
        score = saveData.score;
        if (text != null)
        {
            text.text = "Score: " + score;
        }
    }
}