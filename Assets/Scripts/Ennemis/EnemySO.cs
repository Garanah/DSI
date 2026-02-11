using UnityEngine;


[CreateAssetMenu(menuName = "Enemies/EnemyData")]
public class EnemySO : ScriptableObject
{
    [Header("enemyInfo")]
    public int enemyID;
    public string enemyName;
    
    [Header("Enemy Base Stats")]
    public int enemyPv;
    public int movementSpeed;

    [Header("Enemy Atk Stats")] 
    public float atkDistance;
    public float atkDamage;
    public float atkSpeed; 
    
}
