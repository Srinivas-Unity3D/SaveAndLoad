using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

public class EnemyHealth : MonoBehaviour, ISaveable
{
    public int startingHealth = 100;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

    public int currentHealth { get; private set; }
    private bool isDead;
    private bool isSinking;
    private Animator anim;
    private AudioSource enemyAudio;
    private ParticleSystem hitParticles;
    private CapsuleCollider capsuleCollider;
    private EnemyMovement enemyMovement;
    private string enemyId;

    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemyMovement = GetComponent<EnemyMovement>();
        currentHealth = startingHealth;
        enemyId = System.Guid.NewGuid().ToString();
    }

    void Update()
    {
        if (isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead)
            return;

        enemyAudio.Play();
        currentHealth -= amount;
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        capsuleCollider.isTrigger = true;
        anim.SetTrigger("Dead");
        enemyAudio.clip = deathClip;
        enemyAudio.Play();
    }

    public void StartSinking()
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy(gameObject, 2f);
    }

    public void SetEnemyId(string id)
    {
        enemyId = id;
    }

    public void Save(SaveData saveData)
    {
        if (!isSinking)
        {
            saveData.enemyData.Add(new EnemySaveData
            {
                enemyId = enemyId,
                enemyType = gameObject.name,
                health = currentHealth,
                position = new SerializableVector3(transform.position),
                rotation = new SerializableQuaternion(transform.rotation),
                isActive = gameObject.activeSelf,
                isDead = isDead
            });
        }
    }

    public void Load(SaveData saveData)
    {
        var enemyData = saveData.enemyData.FirstOrDefault(e => e.enemyId == enemyId);
        if (enemyData != null)
        {
            currentHealth = Mathf.RoundToInt(enemyData.health);
            transform.position = enemyData.position.ToVector3();
            transform.rotation = enemyData.rotation.ToQuaternion();
            gameObject.SetActive(enemyData.isActive);

            if (enemyData.isDead)
            {
                isDead = true;
                capsuleCollider.isTrigger = true;
                anim.SetTrigger("Dead");
                if (enemyMovement != null)
                {
                    enemyMovement.enabled = false;
                }
            }
            else if (enemyMovement != null)
            {
                enemyMovement.ResetNavMesh();
            }
        }
    }
}
