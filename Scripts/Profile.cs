using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Profile
{
    public string name;//name of profile

    public List<Sign> leftHandSigns;//list of signs for left hand
    public List<Sign> rightHandSigns;//list of signs for right hand

    public Profile DeepCopy(Profile oldProfile)
    {
        Profile newProfile = new Profile();        
        newProfile.name = "New Profile";

        newProfile.leftHandSigns = new List<Sign>(oldProfile.leftHandSigns.Count);
        newProfile.rightHandSigns = new List<Sign>(oldProfile.rightHandSigns.Count);

        for(int i = 0; i < oldProfile.leftHandSigns.Count; i++)
        {
            newProfile.leftHandSigns.Add(oldProfile.leftHandSigns[i].DeepCopySign(oldProfile.leftHandSigns[i]));
        }
        for(int j = 0; j < oldProfile.rightHandSigns.Count; j++)
        {
           newProfile.rightHandSigns.Add(oldProfile.rightHandSigns[j].DeepCopySign(oldProfile.rightHandSigns[j]));
        }

        return newProfile;
    }
}
