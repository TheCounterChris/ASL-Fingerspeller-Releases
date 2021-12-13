using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEventActivator : MonoBehaviour
{
    public UnityEvent OnButtonPress;

    public Material red;
    public Material green;

    MeshRenderer ren;

    void Start()
    {
        ren = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("ButtonTrigger"))
        {
            ren.material = green;
            OnButtonPress.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals("ButtonTrigger"))
        {
            ren.material = red;
        }
    }
}
