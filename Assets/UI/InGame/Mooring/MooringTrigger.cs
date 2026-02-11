using UnityEngine;
using UnityEngine.UI;

public class MooringTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject pressEUI;    // Indicateur "Appuyez sur E"
    public GameObject mooringUI;   // Panneau de mouillage

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

            boat = null;
        }
    }

    private void Update()
    {
        // Tant que le panneau est ouvert, ignore toutes les touches
        if (isMooringActive) return;

        if (playerInTrigger && boat != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenMooringUI();
            }
        }
    }

    private void OpenMooringUI()
    {
        if (isMooringActive) return;

        isMooringActive = true;

        // Masque PressEUI et affiche MooringUI
        if (pressEUI != null) pressEUI.SetActive(false);
        if (mooringUI != null) mooringUI.SetActive(true);

        // Stop le bateau et bloque les inputs
        if (boat != null) boat.StopBoat();

        // Activer le curseur pour le UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Fonction publique pour le bouton "Retour" dans le UI
    public void OnReturnButtonPressed()
    {
        if (boat != null && isMooringActive)
        {
            // Reprend le contrôle du bateau
            boat.ResumeBoat();
            isMooringActive = false;

            if (mooringUI != null) mooringUI.SetActive(false);

            if (playerInTrigger && pressEUI != null)
                pressEUI.SetActive(true);

            // Réactiver le curseur du joueur s’il est géré par la caméra ou le joueur
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
