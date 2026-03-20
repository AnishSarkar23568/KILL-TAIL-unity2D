using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
   
    public GameObject mobileControlPanel; // Assign in Inspector

    public GameObject mainMenuPanel;
    public GameObject attackButton;

    public Button creditButton;
    public GameObject creditPanel;
    public TextMeshProUGUI creditText;
    public Button closeCreditButton;

    public GameObject loadingPanel;
   public Slider loadingBar;

    public GameObject TapToStaetloadingPanel;
    public Slider TTSloadingBar;
    public TextMeshProUGUI tapToStartText;


    void Start()
    {
        //  Set up buttons first
        if (creditButton != null)
            creditButton.onClick.AddListener(OnCreditButtonClicked);
        if (closeCreditButton != null)
            closeCreditButton.onClick.AddListener(OnCloseCreditPanelClicked);

        creditPanel.SetActive(false); // Hide credit panel at start

        //  Check if restarting (skip tap to start)
        if (PlayerPrefs.GetInt("IsRestarting", 0) == 1)
        {
            Time.timeScale = 1f;
            PlayerPrefs.SetInt("IsRestarting", 0);
            TapToStaetloadingPanel.SetActive(false); //  Skip tap-to-start
            mainMenuPanel.SetActive(true);
            attackButton.SetActive(true);
            return;
        }

        //  First time load (show tap-to-start)
        Time.timeScale = 0f;
        TapToStaetloadingPanel.SetActive(true);
        TTSloadingBar.value = 0f;
        tapToStartText.gameObject.SetActive(false);
        mainMenuPanel.SetActive(false);
        attackButton.SetActive(false);
        StartCoroutine(InitialLoading());

        if (mobileControlPanel != null)
            mobileControlPanel.SetActive(false);
    }

    IEnumerator InitialLoading()
    {
        float loadProgress = 0f;
        TTSloadingBar.value = 0f;

        while (loadProgress < 1f)
        {
            loadProgress += Time.unscaledDeltaTime * 0.090f; // Adjust speed if needed
            TTSloadingBar.value = loadProgress;
            yield return null;
        }

        //  Skip tap-to-start, go directly to menu
        TapToStaetloadingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        attackButton.SetActive(true);
    }


    public void OnAttackButtonClicked()
    {
        SoundManager.Instance.PlayGameplayBGM();

        SoundManager.Instance.PlayButtonClick();  // Sound before restart
        
        Time.timeScale = 0f;
        attackButton.SetActive(false);
        mainMenuPanel.SetActive(false);
        creditPanel.SetActive(false);
        loadingPanel.SetActive(true);


        if (mobileControlPanel != null)
            mobileControlPanel.SetActive(false);


        StartCoroutine(StartGameWithLoading());



    }

    IEnumerator StartGameWithLoading()
    {
        float loadProgress = 0f;
        loadingBar.value = 0f;

        while (loadProgress < 1f)
        {
            loadProgress += Time.unscaledDeltaTime * 0.15f;
            loadingBar.value = loadProgress;
            yield return null;
        }

      

        if (mobileControlPanel != null)
            mobileControlPanel.SetActive(true);



        loadingPanel.SetActive(false);
        Time.timeScale = 1f;

    }

    public void OnCreditButtonClicked()
    {
        SoundManager.Instance.PlayButtonClick();  // Sound before restart
      //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        creditPanel.SetActive(true);
        creditText.text = "Developed by: Anish ";
        attackButton.SetActive(false);
        mainMenuPanel.SetActive(false);
    }

    public void OnCloseCreditPanelClicked()
    {
        SoundManager.Instance.PlayButtonClick();  // Sound before restart
      //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        creditPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        attackButton.SetActive(true);
    }

    public void OnExitButtonClicked()
    {
        SoundManager.Instance.PlayButtonClick();  // Sound before restart
      //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Exit Button Clicked");
        Application.Quit();
    }
}

