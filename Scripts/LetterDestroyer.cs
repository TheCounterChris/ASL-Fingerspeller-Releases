﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterDestroyer : MonoBehaviour
{
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 5f)
        {
            Destroy(gameObject);
        }        
    }
}
