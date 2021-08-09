using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Gradient gradient;
    public Image healthFill;
    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        // healthSlider.value = health;
        healthFill.color = gradient.Evaluate(1f); 
    }

    public void SetHealth(float health)
    {
        healthSlider.value = health;
        healthFill.color = gradient.Evaluate(healthSlider.normalizedValue);
    }

 

}
