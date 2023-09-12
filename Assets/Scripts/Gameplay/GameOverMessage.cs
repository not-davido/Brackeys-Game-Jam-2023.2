using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMessage : Singleton<GameOverMessage>
{
    [SerializeField] GameObject Root;
    [SerializeField] TextMeshProUGUI MessageTMP;
    [SerializeField] TextMeshProUGUI CollectiblesCollected;
    [SerializeField] TextMeshProUGUI RetryButtonText;
    [SerializeField] GameObject WonText;

    CollectibleBag playerCollectible;
    Collectible[] totalCollectiblesInTheGame;

    private void Start()
    {
        playerCollectible = GameManager.Instance.GetPlayer().GetComponent<CollectibleBag>();
        totalCollectiblesInTheGame = FindObjectsByType<Collectible>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        Root.SetActive(false);
        WonText.SetActive(false);
    }

    public void Message(string top, bool win) {
        Root.SetActive(true);
        WonText.SetActive(win);
        RetryButtonText.text = win ? "Play Again" : "Retry";
        MessageTMP.text = top;
        CollectiblesCollected.text = $"Collected {playerCollectible.CollectibleCount} out of {totalCollectiblesInTheGame.Length}";
    }

    public void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        var checkpointInitializer = FindFirstObjectByType<CheckpointInitializer>();
        if (checkpointInitializer != null) {
            Destroy(checkpointInitializer.gameObject);
        }

        SceneManager.LoadScene(0);
    }
}
