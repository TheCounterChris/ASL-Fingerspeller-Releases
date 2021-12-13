using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public AudioManager audioManager;

    public List<Level> levels;//list of levels

    public int levelIndex = 0;//current level number
    int wordIndex = 0;//designates word to spell
    int letterIndex = 0;//designates letter to produce

    public string toSpell;//word to spell
    public string spelled;//what player has spelled so far

    Level level;//current level
    List<string> dictionary;//dictionary of current level   

    public bool spellCheck;
    public bool levelActive = false;
    public int levelLength;

    public float wordTimer = 0f;
    public float levelTimer = 0f;

    public int clp = 0;
    public int ilp = 0;

    public Vector2[] highScores = new Vector2[5];

    public UnityEvent OnLevelComplete;

    void Start()
    {
        for(int i = 0; i < levels.Count; i++)
        {
            ShuffleDictionary(i);
            highScores[i].x = 200;
            highScores[i].y = 500;
        }

        level = levels[levelIndex];//set current level
        dictionary = level.dictionary;//set dictionary
        toSpell = dictionary[wordIndex];//set word to spell
    }

    public void CheckLetter(string letter)//check if letter just produced is the right letter
    {
        if(spellCheck)
        {
            if(letter[0].Equals(toSpell[letterIndex]))//if first char of string associated with produced sign matches expected letter
            {
                AddLetter(letter);//add the letter to the spelled word
            }
            else
            {
                ilp++;
            }
        }        
    }

    void AddLetter(string letter)//check if word is fully spelled
    {
        audioManager.Play("Correct Letter");
        spelled += letter;//add the letter to the spelled word
        letterIndex++;//go to the next letter

        clp++;

        if(spelled.Equals(toSpell))//if spelled word matches word to spell
        {
            NextWord();//go to next word
        }        
    }

    public void SkipLetter()
    {
        spelled += toSpell[letterIndex];//add the letter to the spelled word
        letterIndex++;//go to the next letter

        if(spelled.Equals(toSpell))//if spelled word matches word to spell
        {
            NextWord();//go to next word
        }
    }

    public void NextWord()//go to next word
    {
        audioManager.Play("Next Word");

        wordTimer = 0;

        spelled = null;//reset word to spell
        letterIndex = 0;//reset letter, go to start of word

        wordIndex++;//go to next word

        if(wordIndex == dictionary.Count)//if that was the last word
        {            
            OnLevelComplete.Invoke();//invoke end of level event
            audioManager.Play("Level Complete");//play the end level sound
            levelActive = false;//turn off level active

            if(ilp < highScores[levelIndex].x)//if fewer incorrect letters detected
            {
                highScores[levelIndex].x = ilp;
            }
            if(levelTimer < highScores[levelIndex].y)//if less time taken
            {
                highScores[levelIndex].y = levelTimer;
            }
        }

        ToSpellUpdate();//update word to spell
    }

    void NextLevel()//go to next level //never used in current game version
    {
        wordIndex = 0;//go to first word
        levelIndex++;//increment level count
        
        if(levelIndex == levels.Count)//if no more levels left
        {
            levelIndex = 0;//replace with function for game over //go back to first level
            level = levels[levelIndex];//set active level
        }
        else//if levels left to play
        {
            level = levels[levelIndex];//set level
        }

        DictionaryUpdate();//update dictionary
        ToSpellUpdate();//update word to spell
    }

    void RestartLevel()//restart level //never used in current game version
    {
        wordIndex = 0;
        letterIndex = 0;
        spelled = null;

        ToSpellUpdate();
    }

    void RestartWord()//start word over //never used in current game version
    {
        spelled = null;//reset word
        letterIndex = 0;//go to start of word
    }

    void ToSpellUpdate()//reset word to spell
    {
        toSpell = level.dictionary[wordIndex];//set active word
    }

    void DictionaryUpdate()
    {
        dictionary = level.dictionary;//set active dictionary
    }

    public void LoadLevel(int levelNo)
    {
        ShuffleDictionary(levelNo);
        levelIndex = levelNo;
        wordIndex = 0;
        letterIndex = 0;
        level = levels[levelIndex];
        spelled = null;
        wordTimer = 0;
        levelTimer = 0;

        clp = 0;
        ilp = 0;

        DictionaryUpdate();
        ToSpellUpdate();
        SetLevelActive();
        SpellCheckOn();
    }

    void ShuffleDictionary(int levelNo)
    {
        levelLength = 0;

        for(int j = 0; j < levels[levelNo].dictionary.Count; j++)
        {
            int rand = Random.Range(0, levels[levelNo].collection.Count - 1);
            levels[levelNo].dictionary[j] = levels[levelNo].collection[rand];
            for(int k = j; k >= 0; k--)
            {
                if(levels[levelNo].dictionary[k].Equals(levels[levelNo].dictionary[j]))
                {
                    rand = Random.Range(0, levels[levelNo].collection.Count - 1);
                    levels[levelNo].dictionary[j] = levels[levelNo].collection[rand];
                }
            }
            levelLength += levels[levelNo].dictionary[j].Length;            
        }
    }

    void SetLevelActive()
    {
        levelActive = true;
    }

    public void SetLevelInactive()
    {
        levelActive = false;
    }

    void SpellCheckOn()
    {
        spellCheck = true;
    }

    public void SpellCheckOff()
    {
        spellCheck = false;
    }

    void Update()
    {
        if(levelActive)
        {
            wordTimer += Time.deltaTime;
            levelTimer += Time.deltaTime;
        }
    }
    
}//class