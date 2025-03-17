using System.Collections;
using UnityEngine;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    [SerializeField] private int score;
    [SerializeField] private ScoreUI scoreCounter;
    [SerializeField] private LivesUI livesCounter;
    [SerializeField] private GameObject gameOverPanel;

    private int currentBrickCount;
    private int totalBrickCount;

    // adding brick explosion particles
    [SerializeField] private GameObject explosionParticles;
    private GameObject explosionParticlesInstance;

    private void OnEnable()
    {
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        // fire audio here
        AudioManager.Instance.PlaySFX("break-brick");
        // implement particle effect here
        explosionParticlesInstance = Instantiate(explosionParticles, position, Quaternion.identity);
        Destroy(explosionParticlesInstance.gameObject, 1);

        // add camera shake here
        currentBrickCount--;
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
        if(currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();
    }

    public void KillBall()
    {
        maxLives--;

        // update lives on HUD here
        livesCounter.UpdateLives(maxLives);

        // game over UI if maxLives < 0, then exit to main menu after delay
        if (maxLives < 1)
        {
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
            AudioManager.Instance.StopMusic("background-music"); //stops music in background
            AudioManager.Instance.PlaySFX("game-over"); //plays game over sound
            StartCoroutine(ReturnToMenu());
        }

        ball.ResetBall();
    }

    public void IncreaseScore()
    {
        score++;
        scoreCounter.UpdateScore(score);
    }

    private IEnumerator ReturnToMenu()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1.5f);
        gameOverPanel.SetActive(false);
        SceneHandler.Instance.LoadMenuScene();
    }
}
