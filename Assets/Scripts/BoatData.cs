using UnityEngine;

public class BoatData : MonoBehaviour
{
    public int boatMaxPv = 100;
    public int boatPv = 100;

    public int boatDamage = 10;

    public void TakeDamage(int dmg)
    {
        boatPv -= dmg;
        if (boatPv <= 0)
        {
            boatPv = 0;
            Debug.Log("Boat dead !");
        }
    }
}