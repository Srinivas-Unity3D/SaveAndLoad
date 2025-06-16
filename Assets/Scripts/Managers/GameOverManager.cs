using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    private Animator anim;
    private ScoreManager scoreManager;
    private SaveManager saveManager;
    private bool isGameOver = false;

    [SerializeField] GameObject saveLoadButton;

    void Awake()
    {
        anim = GetComponent<Animator>();
        scoreManager = FindObjectOfType<ScoreManager>();
        saveManager = FindObjectOfType<SaveManager>();
    }

    void Update()
    {
        if (playerHealth.CurrentHealth <= 0 && !isGameOver)
        {
            isGameOver = true;
            anim.SetTrigger("GameOver");
            saveLoadButton.SetActive(false);
        }
    }

    public void RestartLevel()
    {
        isGameOver = false;
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadGame()
    {
        if (saveManager != null && saveManager.HasSaveFile())
        {
            isGameOver = false;
            anim.ResetTrigger("GameOver");
            
            if (playerHealth != null)
            {
                playerHealth.enabled = true;
            }

            saveManager.LoadGame();
        }
    }
}
