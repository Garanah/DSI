using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    [Header("Damage Layers")]
    [SerializeField] Image damagedLayer;
    [SerializeField] Image heavyDamagedLayer;
    [SerializeField] Image brokenLayer;

    [Header("Boat")]
    [SerializeField] BoatController boatController;

    // BoatDamageState currentState = BoatDamageState.Normal;
    //
    // void Start()
    // {
    //     ApplyDamageState(BoatDamageState.Normal);
    // }
    //
    // void OnEnable()
    // {
    //     if (boatController != null)
    //         boatController.OnDamageStateChanged += ApplyDamageState;
    // }
    //
    // void OnDisable()
    // {
    //     if (boatController != null)
    //         boatController.OnDamageStateChanged -= ApplyDamageState;
    // }
    //
    // void ApplyDamageState(BoatDamageState state)
    // {
    //     currentState = state;
    //
    //     // Cumul
    //     damagedLayer.enabled       = state >= BoatDamageState.Damaged;
    //     heavyDamagedLayer.enabled  = state >= BoatDamageState.HeavyDamaged;
    //     brokenLayer.enabled        = state >= BoatDamageState.Broken;
    // }
}