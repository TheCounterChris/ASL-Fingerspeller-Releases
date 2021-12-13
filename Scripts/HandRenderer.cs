using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRenderer : MonoBehaviour
{
    public ProfileManager profileManager;

    public GameObject leftHand;
    public GameObject rightHand;
    SkinnedMeshRenderer leftMeshRen;
    SkinnedMeshRenderer rightMeshRen;

    public Material activeMat;
    public Material inactiveMat;
    
    void Start()
    {
        leftMeshRen = leftHand.GetComponent<SkinnedMeshRenderer>();
        rightMeshRen = rightHand.GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        if(profileManager.isLeft)
        {
            leftMeshRen.material = activeMat;
            rightMeshRen.material = inactiveMat;
        }
        else
        {
            leftMeshRen.material = inactiveMat;
            rightMeshRen.material = activeMat;
        }
    }
}
