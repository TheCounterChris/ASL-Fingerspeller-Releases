using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Sign
{
    public string name;//name of the sign
    public List<Vector3> fingerPositionalData;//list hold the positional data for each bone in the hand
    public Quaternion rootRotation;//holds the rotational data of the root of the hand (the wrist)

    public UnityEvent OnDetect;//event to execute when the sign is recognized

    public Sign DeepCopySign(Sign oldSign)
    {
        Sign newSign = new Sign();

        newSign.name = oldSign.name;
        newSign.fingerPositionalData = oldSign.fingerPositionalData;
        newSign.rootRotation = oldSign.rootRotation;
        newSign.OnDetect = oldSign.OnDetect;

        return newSign;
    }
}