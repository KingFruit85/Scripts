using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Transform MainCanvas;

    void Start()
    {
        if(Instance != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public TerminalPopUp CreateTerminal()
    {
        GameObject popUpGameobject = Instantiate(Resources.Load("UI/Terminal")) as GameObject;
        return popUpGameobject.GetComponent<TerminalPopUp>();
    }
}
