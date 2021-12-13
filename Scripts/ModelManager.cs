using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModelManager : MonoBehaviour
{
    public ProfileManager profileManager;

    GameObject handModel;//active model
    GameObject letter;

    public List<ModelAlphabet> alphabets;//list of models

    public List<GameObject> letters;

    public int activeAlphabet = 1;//left or right || 0 = left, 1 = right

    public TextMeshPro signName;

    public void SetAlphabet()//set left or right hand active for model
    {
        if(profileManager.isLeft)//if left
        {
            activeAlphabet = 0;//set left
        }
        else{activeAlphabet = 1;}//not left, set right
    }

    public void UpdateModel()//update model
    {
        SetAlphabet();//check if left or right

        Destroy(handModel);//destroy current model
        handModel = Instantiate(alphabets[activeAlphabet].alphabet[profileManager.signIndex]);//instantiate correct model depending on left or right and current sign
        
        Destroy(letter);
        letter = Instantiate(letters[profileManager.signIndex]);//instantiate

        signName.text = profileManager.activeSign.name;//set model name display
    }

    public void DestroyModels()
    {
        Destroy(handModel);
        Destroy(letter);
    }
}