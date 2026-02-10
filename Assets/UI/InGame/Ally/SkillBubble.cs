using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillBubble : MonoBehaviour
{
    public Image cooldownOverlay;
    public Image selectionFrame;

    public float cooldownTime = 3f;
    private bool onCooldown = false;

    public void SetSelected(bool selected)
    {
        selectionFrame.enabled = selected;
    }

    public void UseSkill()
    {
        if (onCooldown) return;
        
        Debug.Log(name + " skill used");

        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        onCooldown = true;
        float t = 0;

        while (t < cooldownTime)
        {
            t += Time.deltaTime;
            cooldownOverlay.fillAmount = 1 - (t / cooldownTime);
            yield return null;
        }

        cooldownOverlay.fillAmount = 0;
        onCooldown = false;
    }
}