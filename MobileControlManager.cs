using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MobileControlManager : MonoBehaviour
{
    public GameObject controlButtonsPanel; // Assign the UI panel in Inspector
    public float delayBeforeShowingControls = 1f; // Delay in seconds

    public Snakemove snakeMove;
    void Start()
    {
        controlButtonsPanel.SetActive(false); // Hide at first
    }
    // Assign your snake movement script here
    public void OnGameStart()
    {
        ShowControls(true);  //Show during gameplay
    }

    public void OnGameOver()
    {
        ShowControls(false); // Hide after game ends
    }
    public void ShowControlsWithDelay()
    {
        StartCoroutine(ShowControlsAfterDelay());
    }

    IEnumerator ShowControlsAfterDelay()
    {
        controlButtonsPanel.SetActive(false);
        yield return new WaitForSeconds(delayBeforeShowingControls);
        controlButtonsPanel.SetActive(true);
    }
    public void HideControls()
    {
        ShowControls(false);
    }

    public void ShowControls(bool show)
    {
        Debug.Log("Mobile UI: " + (show ? "SHOW" : "HIDE")); // Debug
        controlButtonsPanel.SetActive(show);
    }


    // Control functions to link in button OnClick()
    public void MoveUp() => snakeMove.SetDirection(Vector2.up);
    public void MoveDown() => snakeMove.SetDirection(Vector2.down);
    public void MoveLeft() => snakeMove.SetDirection(Vector2.left);
    public void MoveRight() => snakeMove.SetDirection(Vector2.right);
}
