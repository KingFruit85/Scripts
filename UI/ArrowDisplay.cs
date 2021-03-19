using UnityEngine;
using UnityEngine.UI;

public class ArrowDisplay : MonoBehaviour
{
    private int remainingArrows = 0;
    public Text arrowText;

    void Update()
    {
        remainingArrows = GameObject.Find("Player").GetComponent<PlayerStats>().getArrowCount();
        arrowText.text = "x" + remainingArrows;
    }

}
