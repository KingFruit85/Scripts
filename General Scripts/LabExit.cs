﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabExit : MonoBehaviour
{
    private LevelLoader levelLoader;

    void Awake()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {   
        Debug.Log("The player has walked over me");
        levelLoader.LoadPCG();
    }
}
