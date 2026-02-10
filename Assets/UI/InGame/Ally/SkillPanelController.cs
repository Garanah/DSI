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

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            panelOpen = true;

        if (Input.GetKeyDown(KeyCode.H))
            panelOpen = false;

        if (!panelOpen) return;

        if (Input.GetKeyDown(KeyCode.Q))
            ChangeSelection(-1);

        if (Input.GetKeyDown(KeyCode.E))
            ChangeSelection(1);

        if (Input.GetKeyDown(KeyCode.Space))
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