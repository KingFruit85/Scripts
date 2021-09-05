using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NarrativeManager : MonoBehaviour
{

    public List<KeyValuePair<int,string>> LabDialog = new List<KeyValuePair<int, string>>()
    {
        new KeyValuePair<int, string>(1,"It's a giant glass tank full of water"),
        new KeyValuePair<int, string>(2,"It's a computer"),
        new KeyValuePair<int, string>(3,"It's a locked door")

    };

    public string GetDialog(List<KeyValuePair<int,string>> list, int id)
    {
        return list[id].Value;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(GetDialog(LabDialog,1));
        }
    }

    // Set up the dialogs, maybe read from an external file?

    // Function to fetch the correct dialog
    // Items that generate dialog should have a unique ID


    

    
}