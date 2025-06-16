using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, ISaveable
{
    public int startingHealth = 100;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    private int currentHealth;
    private bool isDead;
    private bool damaged;
    private Animator anim;
    private AudioSource playerAudio;
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;

    public int CurrentHealth { get { return currentHealth; } }

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = startingHealth;
    }

    void Update()
    {
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        playerShooting.DisableEffects();
        anim.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void Save(SaveData saveData)
    {
        saveData.playerData = new PlayerSaveData
        {
            health = currentHealth,
            position = new SerializableVector3(transform.position),
            rotation = new SerializableQuaternion(transform.rotation),
            score = healthSlider.value
        };
    }

    public void Load(SaveData saveData)
    {
        if (saveData.playerData != null)
        {
            currentHealth = Mathf.RoundToInt(saveData.playerData.health);
            transform.position = saveData.playerData.position.ToVector3();
            transform.rotation = saveData.playerData.rotation.ToQuaternion();
            healthSlider.value = saveData.playerData.score;
            
            isDead = false;
            playerMovement.enabled = true;
            playerShooting.enabled = true;
            
            anim.ResetTrigger("Die");
            
            if (currentHealth <= 0)
            {
                currentHealth = startingHealth;
                healthSlider.value = currentHealth;
            }
        }
    }
}
