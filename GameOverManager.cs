using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;




public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    public GameObject gameOverUI;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public Button backButton; //  

    private void Awake()
    {
        Instance = this;

        // Add listener for back button if assigned
        if (backButton != null)
            backButton.onClick.AddListener(OnBackToMainMenu);
    }
    void SaveMatchScore(int score)
    {
        string existing = PlayerPrefs.GetString("MatchScores", "");
        int matchNumber = PlayerPrefs.GetInt("MatchNumber", 1);
        existing += $"Match {matchNumber}: {score}\n";
        PlayerPrefs.SetString("MatchScores", existing);

        if (score > PlayerPrefs.GetInt("HighScore", 0))
            PlayerPrefs.SetInt("HighScore", score);

        PlayerPrefs.SetInt("MatchNumber", matchNumber + 1);
        PlayerPrefs.Save();
    }


    public void ShowGameOver(int score)
    {
        Debug.Log("Game Over Called");
        gameOverUI.SetActive(true);
        finalScoreText.text = "Final Score: " + score;

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score;
        }

        highScoreText.text = "High Score: " + highScore;
    }

    public void OnBackToMainMenu() //  This loads the menu
    {
        StartCoroutine(RestartRoutine());
        SoundManager.Instance.PlayBackgroundMusic();
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("IsRestarting", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    IEnumerator RestartRoutine()
    {
        SoundManager.Instance.PlayButtonClick(); // play before scene reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(1f); // Optional delay
        SoundManager.Instance.PlayBGM();     // Start music before or after loading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
