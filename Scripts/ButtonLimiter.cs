using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLimiter : MonoBehaviour
{
    public Transform upPos;//position of the button when unpressed
    public Transform downPos;//position of the button when pressed

    float downDistance;//float for the distance between up and down
    
    void Start()
    {
        downDistance = upPos.position.y - downPos.position.y;//calculate down distance
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistance = upPos.position.y - transform.position.y;//calculate the current position of the button
        
        if(currentDistance < 0)//if the button is positioned above the up position
        {
            transform.position = upPos.position;//keep the button in the up position
        }

        if(currentDistance > downDistance)//if the button is further than the down position
        {
            transform.position = downPos.position;//keep the position in the down position
        }
    }
}
