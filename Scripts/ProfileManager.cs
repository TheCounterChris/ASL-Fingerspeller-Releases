using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance = null;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public AudioManager audioManager;//audiomanager

    public Spawner spawner;//spawner for practice mode
    
    /**************************************************************************/

    //profiles
    public List<Profile> profiles;//list of profiles in the game

    public Profile activeProfile;//profile the player is using
    public List<Sign> activeList;//the active list of signs, either right hand or left hand

    //hands
    public OVRSkeleton leftHand;//left hand
    public OVRSkeleton rightHand;//right hand
    public OVRSkeleton activeHand;//hand player is using
    public bool isLeft = false;//left hand or right hand

    //counters and active data
    public int profileIndex = 0;//active profile indexer
    public int signIndex = 1;//active sign indexer
    public Sign activeSign;//active sign being manipulated

    //display stuff
    public TextMeshPro profileTag;    

    /****************Sign Detection*************************/
    private Sign previousSign;//last sign found

    [Range(0.005f, 0.1f)]
    public float tolerance = 0.02f;//bone distance tolerance

    [Range(0.05f, 1f)]
    public float angleTolerance = 0.2f;//wrist rotation tolerance

    public bool detect;//whether to be detecting or not
    /***************************************************/

    void Start()
    {
        activeProfile = profiles[profileIndex];//set active profile
        activeList = activeProfile.rightHandSigns;//set active list
        activeHand = rightHand;//set active hand
        activeSign = activeList[signIndex];//set active sign

        UpdateProfileTag();

        previousSign = new Sign();//set previous sign so not null
    }

    public void NewProfile()//function for creating new profiles
    {
        Profile newProfile = new Profile();//new blank profile
        newProfile = newProfile.DeepCopy(profiles[0]);//copy OG profile

        newProfile.name = "Profile " + (profiles.Count + 1);//set name

        activeProfile = newProfile;//set as active
        profiles.Add(newProfile);//add to list
        profileIndex = profiles.IndexOf(newProfile);//second part of setting as active so that can overwrite on the new profile
        UpdateProfileTag();//update display     
    }

    public void NextProfile()//Go to next profile
    {
        profileIndex++;//increment the profile index
        
        if(profileIndex == profiles.Count)//if higher than number of profiles created
        {
            profileIndex = 0;//go to first profile
        }

        activeProfile = profiles[profileIndex];//set active profile
        if(isLeft){SetLeft();}//set left hand if left
        else{SetRight();}//set right hand if right

        UpdateProfileTag();     
    }

    public void PreviousProfile()//go to previous profile
    {
        profileIndex--;//decrement index

        if(profileIndex < 0)//if lower than lowest value
        {
            profileIndex = profiles.Count - 1;//go to end of list
        }

        activeProfile = profiles[profileIndex];//set active profile
        if(isLeft){SetLeft();}//set left if left
        else{SetRight();}//set right if right

        UpdateProfileTag();
    }

    public void UpdateProfileTag()
    {
        profileTag.text = activeProfile.name;
    }

    public void SetLeft()//set left hand as active
    {
        activeHand = leftHand;//set left hand active
        activeList = activeProfile.leftHandSigns;//set active list as left hand list
        activeSign = activeList[signIndex];

        isLeft = true;//is left
    }

    public void SetRight()//set right hand as active
    {
        activeHand = rightHand;//set right hand active
        activeList = activeProfile.rightHandSigns;//set active list as right hand list
        activeSign = activeList[signIndex];

        isLeft = false;//is right
    }

    public void SaveSign()//function to save a new sign //may not be used in final game and number of signs should be limited
    {
        Sign sign = new Sign();//initialize new sign

        sign.name = "New Sign";//name it according to our switch case in SignManager

        List<Vector3> positionalData = new List<Vector3>();//list for positional data of bones
        foreach(var bone in activeHand.Bones)//go thorugh all th ebones in the hand
        {
            positionalData.Add(activeHand.transform.InverseTransformPoint(bone.Transform.position));//get positional data for all the bones
        }

        sign.fingerPositionalData = positionalData;//save the postitonal data
        sign.rootRotation = activeHand.Bones[1].Transform.rotation;//save the wrist rotation data

        //activeList.Add(sign);//return the new sign

        if(isLeft)
        {
            profiles[profileIndex].leftHandSigns.Add(sign);
        }
        else
        {
            profiles[profileIndex].rightHandSigns.Add(sign);
        }
    }

    public void Overwrite()//function for overwriting the finger data of a sign
    {
        Sign sign = activeSign;

        List<Vector3> positionalData = new List<Vector3>();//new list for positional data of bones
        foreach(var bone in activeHand.Bones)//run through all bones in the hand
        {
            positionalData.Add(activeHand.transform.InverseTransformPoint(bone.Transform.position));//store positional data
        }

        sign.fingerPositionalData = positionalData;//overwrite the positional data with the new data
        sign.rootRotation = activeHand.Bones[1].Transform.rotation;//overwrite the rotation data with the new (current) rotation

        Debug.Log("Active Profile (index): " + profiles[profileIndex].name);
        Debug.Log("Active Profile (profile): " + activeProfile.name);

        Debug.Log("Active sign (index): " + profiles[profileIndex].rightHandSigns[signIndex].name);
        Debug.Log("Active sign (sign): " + activeSign.name);

        //activeSign = sign;//overwrite the active sign

        if(isLeft)
        {
            profiles[profileIndex].leftHandSigns[signIndex] = sign;
        }
        else
        {
            profiles[profileIndex].rightHandSigns[signIndex] = sign;
        }      
    }

    public void NextSign()//go to next sign
    {
        signIndex++;//increment index

        if(signIndex >= activeList.Count)//if higher than number of signs
        {
            signIndex = 1;//go to beginning
        }

        activeSign = activeList[signIndex];//set active sign
    }

    public void PreviousSign()//go to previous sign
    {
        signIndex--;//decrement index

        if(signIndex < 1)//if lower than lowest value sign
        {
            signIndex = activeList.Count - 1;//go to end of list
        }

        activeSign = activeList[signIndex];//set active sign
    }

    public void SwitchDetect()
    {
        detect = !detect;
    }

    public void DetectOn()
    {
        detect = true;
    }

    public void DetectOff()
    {
        detect = false;
    }
    
    /**********************Sign Detection*************/
    Sign Recognize()//function that checks current hand position
    {
        Sign currentSign = new Sign();//get current sign

        float currentMin = Mathf.Infinity;//set minimum distance from listed sign

        for(int j = 1; j < activeList.Count; j++)//go through list of saved signs to compare
        {
            float sumDistance = 0;//how far away the current sign is from listed signs
            bool isDiscarded = false;//if the sign doesn't match listed signs

            //compare the current sign to listed signs
            for(int i = 0; i < activeHand.Bones.Count; i++)//go through each bone in the hand
            {
                Vector3 currentPosition = activeHand.transform.InverseTransformPoint(activeHand.Bones[i].Transform.position);//get current positional data for all bones

                float distance = Vector3.Distance(currentPosition, activeList[j].fingerPositionalData[i]);//calculate how close each bone is to the bone in the comparing sign

                if(distance > tolerance)//if current sign is not close enough, stop comparing, move on to next sign
                {
                    isDiscarded = true;
                    break;
                }

                sumDistance += distance;//add up the distance of all the bones
            }

            float rotDifference = Mathf.Abs(Quaternion.Dot(activeHand.Bones[1].Transform.rotation, activeList[j].rootRotation));
            if(spawner.spawn){rotDifference = 1f;}

            if(!isDiscarded && sumDistance < currentMin && rotDifference > (1f - angleTolerance))//if the sign is not discarded, close to listed sign, and the wrist rotation is close enough
            {
                currentMin = sumDistance;//set the minimal distance to how close we got to the listed sign against which we are comparing
                //what this does is compare listed signs againt each other. If two signs are similar it is possible that current hand position can satisfy both thresholds
                //so this sets a new threshold of how close the current sign is to the first listed sign, and checks the second listed sign against it to make sure we produce the correct gesture

                currentSign = activeList[j];//set the sign to the one against which we are comparing
            }
        }

        return currentSign;//return the sign
    }

    void Update()
    {
        if(detect)
        {
            Sign currentSign = Recognize();//check if the sign being produced is a saved sign
            bool recognized = !currentSign.Equals(new Sign());//this tests that the Recognize() function returned a listed sign, instead of null

            if(recognized && !currentSign.Equals(previousSign))//check if current sign is different from last sign recognized (no repeats)
            {
                Debug.Log("Last sign found: " + currentSign.name);//print

                previousSign = currentSign;//store the current sign as previous so as to compare and not repeat

                currentSign.OnDetect.Invoke();//invoke OnDetect
            }
        }

        if(Input.GetButton("Save"))
        {
            Debug.Log("SAVED SIGN FROM COMPUTER");
            Overwrite();
        }
        
    }
    /**********************************************************/

    /********************* Saving and loading profiles *********/

    public void SaveProfiles()
    {
        SaverLoader.SaveProfiles2(profiles);
        Debug.Log("Saved in profile manager");
    }

    public void LoadProfiles()
    {
        SaverLoader.LoadProfiles2();
        for(int i = 0; i < SaverLoader.loadedProfiles.Count; i++)
        {
            Profile newProfile = new Profile();
            newProfile = newProfile.DeepCopy(SaverLoader.loadedProfiles[i]);
            profiles[i] = newProfile;
        }
        Debug.Log("Loaded in profile manager");
    }
}//class
