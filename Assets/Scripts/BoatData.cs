using UnityEngine;

public class BoatData : MonoBehaviour
{
    public float boatMaxPv = 100;
    public float boatPv = 100;

    public int boatDamage = 10;

    public void TakeDamage(float dmg)
    {
        boatPv -= dmg;
        if (boatPv <= 0)
        {
            boatPv = 0;
            Debug.Log("Boat dead !");
        }
    }
}