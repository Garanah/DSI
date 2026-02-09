using UnityEngine;


[CreateAssetMenu(menuName = "Enemies/EnemyData")]
public class EnemySO : ScriptableObject
{
    public int enemyID;
    public string enemyName;
    public int enemyPv;
    public int enemyDamage;
    public int enemySpeed;
}
