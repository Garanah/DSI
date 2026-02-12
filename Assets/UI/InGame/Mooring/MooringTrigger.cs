using UnityEngine;
using UnityEngine.UI;

public class MooringTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject pressEUI;
    public GameObject mooringUI;

    [Header("Other UI to Hide")]
    public GameObject[] otherCanvases;

    private BoatController boat;
    private bool playerInTrigger = false;
    private bool isMooringActive = false;

    private void Start()
    {
        if (pressEUI != null) pressEUI.SetActive(false);
        if (mooringUI != null) mooringUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        boat = other.GetComponent<BoatController>();
        if (boat != null)
        {
            playerInTrigger = true;

            if (!isMooringActive && pressEUI != null)
                pressEUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (boat != null && other.GetComponent<BoatController>() == boat)
        {
            playerInTrigger = false;

            if (pressEUI != null) pressEUI.SetActive(false);
            if (mooringUI != null) mooringUI.SetActive(false);

            if (isMooringActive)
            {
                boat.ResumeBoat();
                isMooringActive = false;
            }

            SetCanvasesActive(otherCanvases, true);

            boat = null;
        }
    }

    private void Update()
    {
        if (isMooringActive) return;

        if (playerInTrigger && boat != null)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                OpenMooringUI();
            }
        }
    }

    private void OpenMooringUI()
    {
        if (isMooringActive) return;

        isMooringActive = true;
        
        if (pressEUI != null) pressEUI.SetActive(false);
        if (mooringUI != null) mooringUI.SetActive(true);
        if (boat != null) boat.StopBoat();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetCanvasesActive(otherCanvases, false);
    }

    public void OnReturnButtonPressed()
    {
        if (boat != null && isMooringActive)
        {
            boat.ResumeBoat();
            isMooringActive = false;

            if (mooringUI != null) mooringUI.SetActive(false);

            if (playerInTrigger && pressEUI != null)
                pressEUI.SetActive(true);
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            SetCanvasesActive(otherCanvases, true);
        }
    }

    private void SetCanvasesActive(GameObject[] canvases, bool active)
    {
        if (canvases == null) return;

        foreach (GameObject canvas in canvases)
        {
            if (canvas != null)
                canvas.SetActive(active);
        }
    }
}
