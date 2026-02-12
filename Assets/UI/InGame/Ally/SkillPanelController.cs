using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillPanelController : MonoBehaviour
{
    public RectTransform panel;
    public float slideSpeed = 10f;

    public List<SkillBubble> bubbles;
    private int currentIndex = 0;

    private bool panelOpen = false;
    private Vector2 hiddenPos;
    private Vector2 shownPos;

    void Start()
    {
        hiddenPos = new Vector2(-600, panel.anchoredPosition.y);
        shownPos = new Vector2(0, panel.anchoredPosition.y);
        panel.anchoredPosition = hiddenPos;

        UpdateSelection();
    }

    void Update()
    {
        HandleInput();
        SlidePanel();
    }

    private bool l2Pressed = false;
    private bool r2Pressed = false;

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.JoystickButton3))
            panelOpen = !panelOpen;

        if (!panelOpen) return;

        float l2 = Input.GetAxis("L2");
        float r2 = Input.GetAxis("R2");

        if (Input.GetKeyDown(KeyCode.Q) || (l2 > 0.5f && !l2Pressed))
        {
            ChangeSelection(-1);
            l2Pressed = true;
        }
        if (l2 < 0.5f)
            l2Pressed = false;

        if (Input.GetKeyDown(KeyCode.E) || (r2 > 0.5f && !r2Pressed))
        {
            ChangeSelection(1);
            r2Pressed = true;
        }
        if (r2 < 0.5f)
            r2Pressed = false;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton2))
            bubbles[currentIndex].UseSkill();
    }

    void ChangeSelection(int dir)
    {
        currentIndex = (currentIndex + dir + bubbles.Count) % bubbles.Count;
        UpdateSelection();
    }

    void UpdateSelection()
    {
        for (int i = 0; i < bubbles.Count; i++)
            bubbles[i].SetSelected(i == currentIndex);
    }

    void SlidePanel()
    {
        Vector2 target = panelOpen ? shownPos : hiddenPos;
        panel.anchoredPosition = Vector2.Lerp(
            panel.anchoredPosition,
            target,
            Time.deltaTime * slideSpeed
        );
    }
}