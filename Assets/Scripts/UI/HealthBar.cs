using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject healthbar;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI healthcount;
    
    public void MaxHealth(int health)
    {       
        slider.maxValue = health;
        slider.value = health;
        healthcount.text = health + "/100";

        fill.color = gradient.Evaluate(1f);
    }
    public void Sethealth(int health)
    {
        slider.value = health;
        healthcount.text = health + "/100";
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
