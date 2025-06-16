using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;
    private NavMeshAgent nav;
    private bool isInitialized = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        Invoke("InitializeNavMesh", 0.1f);
    }

    void InitializeNavMesh()
    {
        if (nav != null && nav.isOnNavMesh)
        {
            isInitialized = true;
        }
        else
        {
            Invoke("InitializeNavMesh", 0.1f);
        }
    }

    void Update()
    {
        if (!isInitialized || nav == null || !nav.isOnNavMesh)
            return;

        if (enemyHealth.currentHealth > 0 && playerHealth.CurrentHealth > 0)
        {
            nav.SetDestination(player.position);
        }
        else
        {
            nav.enabled = false;
        }
    }

    public void ResetNavMesh()
    {
        isInitialized = false;
        InitializeNavMesh();
    }
}
