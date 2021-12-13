using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    public LevelManager levelManager;

    public TextMeshPro toSpell;
    public TextMeshPro spelled;
    public TextMeshPro timer;
    public List<GameObject> levelTitles;
    public List<GameObject> levelCompleteModels;
    public List<GameObject> highScoreModels;
    GameObject titleModel;
    GameObject levelCompleteModel;
    GameObject highScoreModel;

    public TextMeshPro clp;
    public TextMeshPro ilp;
    public TextMeshPro timeTaken;

    public TextMeshPro ilpHS;
    public TextMeshPro timeHS;

    void Update()
    {
        // if(levelManager.levelActive)
        // {
            toSpell.text = levelManager.toSpell;
            spelled.text = levelManager.spelled;

            timer.text = levelManager.wordTimer.ToString("F1");
        // }
        // else
        // {        
            clp.text = levelManager.clp.ToString() + "/" + levelManager.levelLength.ToString();
            ilp.text = levelManager.ilp.ToString();
            timeTaken.text = levelManager.levelTimer.ToString("F1");

            ilpHS.text = levelManager.highScores[levelManager.levelIndex].x.ToString();
            timeHS.text = levelManager.highScores[levelManager.levelIndex].y.ToString("F1");
        // }
    }

    public void UpdateTitles()
    {
        Destroy(titleModel);
        titleModel = Instantiate(levelTitles[levelManager.levelIndex]);
    }

    public void DestroyTitle()
    {
        Destroy(titleModel);
    }

    public void SetLevelCompleteModel()
    {
        levelCompleteModel = Instantiate(levelCompleteModels[levelManager.levelIndex]);
        highScoreModel = Instantiate(highScoreModels[levelManager.levelIndex]);
    }

    public void DestroyLevelCompleteModel()
    {
        Destroy(levelCompleteModel);
        Destroy(highScoreModel);
    }
}//class