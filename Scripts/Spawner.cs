using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public AudioManager audioManager;

    public List<GameObject> letterFabs;
    public bool spawn;

    public void Spawn(int index)
    {
        Debug.LogWarning("Spawn Function called");
        if(spawn)
        {
            Instantiate(letterFabs[index]);
            audioManager.Play("Spawn");
            Debug.LogWarning("Spawn is on, object should be instantiated");
        }        
    }

    public void SpawnOn()
    {
        spawn = true;
    }

    public void SpawnOff()
    {
        spawn = false;
    }
}//class