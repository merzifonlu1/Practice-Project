using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class cooldownUI : MonoBehaviour
{
    public PlayerMovement pm;

    public Image slide;
    [SerializeField] private TextMeshProUGUI slidetext;
    private float slideCooldowntext;

    public Image attack;
    [SerializeField] private TextMeshProUGUI attacktext;
    private float attackCooldowntext;

    public Image dash;
    [SerializeField] private TextMeshProUGUI dashtext;
    private float dashCooldowntext;

    private void Start()
    {
        slideCooldowntext = pm.slidingCooldown;

        dashCooldowntext = pm.dashingCooldown;

        attackCooldowntext = pm.attackcooldown;
    }

    void Update()
    {
        if (!pm.canSlide)
        {
            slide.enabled = true;
            slidetext.enabled = true;          
            slideCooldowntext -= Time.deltaTime;
            float vInslide = slideCooldowntext; int vOutslide = Convert.ToInt32(vInslide);
            slidetext.text = vOutslide.ToString();           
        }
        else
        {
            slideCooldowntext = pm.slidingCooldown;
            slide.enabled = false;
            slidetext.enabled = false;
        }


        if (!pm.canDash)
        {
            dash.enabled = true;
            dashtext.enabled = true;
            dashCooldowntext -= Time.deltaTime;
            float vIndash = dashCooldowntext; int vOutdash = Convert.ToInt32(vIndash);
            dashtext.text = vOutdash.ToString();
        }
        else
        {
            dashCooldowntext = pm.dashingCooldown;
            dash.enabled = false;
            dashtext.enabled = false;
        }


        if (!pm.Canattack)
        {
            attack.enabled = true;
            attacktext.enabled = true;
            attackCooldowntext -= Time.deltaTime;
            float vInattack = attackCooldowntext; int vOutattack = Convert.ToInt32(vInattack);
            attacktext.text = vOutattack.ToString();
        }
        else
        {
            attackCooldowntext = pm.attackcooldown;
            attack.enabled = false;
            attacktext.enabled = false;
        }
    }
}
