using UnityEngine;

public class EnemyBehaviours : MonoBehaviour
{
    [Header("References")]
    public BoatData boatData;
    public EnemySO enemyData;

    [Header("Runtime")]
    public int enemyPv;
    public float enemyDamage;
    public float enemySpeed;

    [Header("Attack")]
    public float attackRange;
    public float attackCooldown;
    private float attackTimer;

    void Start()
    {
        LoadData();
    }

    void Update()
    {
        if (boatData == null || enemyData == null) return;

        float dist = Vector3.Distance(transform.position, boatData.transform.position);

        if (dist > attackRange)
            FollowBoat();

        TryAttack(dist);
    }

    void FollowBoat()
    {
        Vector3 dir = (boatData.transform.position - transform.position).normalized;
        transform.position += dir * (enemySpeed * Time.deltaTime);

        transform.forward = Vector3.Lerp(transform.forward, dir, 8f * Time.deltaTime);
    }

    void TryAttack(float dist)
    {
        attackTimer -= Time.deltaTime;

        if (dist <= attackRange && attackTimer <= 0f)
        {
            boatData.TakeDamage(enemyDamage);
            attackTimer = attackCooldown;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("bullet"))
        {
            Destroy(other.gameObject);
            TakeDamage(boatData.boatDamage);
        }
    }

    void TakeDamage(int damage)
    {
        enemyPv -= damage;
        if (enemyPv <= 0) Destroy(gameObject);
    }

    void LoadData()
    {
        enemyPv = enemyData.enemyPv;
        enemyDamage = enemyData.atkDamage;
        enemySpeed = enemyData.movementSpeed;

        attackRange = enemyData.atkDistance;
        attackCooldown = enemyData.atkSpeed;
    }
}