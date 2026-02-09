using System;
using UnityEngine;

public class EnemyBehaviours : MonoBehaviour
{
    [Header("Data")]
    public BoatData boatData;
    public EnemySO enemyData;
    
    [Header("EnemyData")]
    public int enemyPv;
    public int enemyDamage;

    public void Start()
    {
        LoadData();
    }

    void Update()
    {
        
    }


    public void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("bullet"))
        {
            Destroy(other.gameObject);
            TakeDamage(boatData.boatDamage);
            Debug.Log("bullet entered");
        }
    }

    void TakeDamage(int damage)
    {
        enemyPv -= damage;
        if(enemyPv <= 0) Destroy(gameObject);
    }

    void LoadData()
    {
        enemyPv = enemyData.enemyPv;
        enemyDamage = enemyData.enemyDamage;
    }
}
