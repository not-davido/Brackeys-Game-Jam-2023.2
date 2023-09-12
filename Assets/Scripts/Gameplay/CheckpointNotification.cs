using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointNotification : MonoBehaviour
{
    [SerializeField] GameObject NotificationText;

    private void OnEnable()
    {
        EventManager.AddListener<CheckpointReachedEvent>(OnCheckpointReached);

        NotificationText.SetActive(false);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<CheckpointReachedEvent>(OnCheckpointReached);
    }

    void OnCheckpointReached(CheckpointReachedEvent evt) {
        StartCoroutine(Notification());
    }

    IEnumerator Notification() {
        NotificationText.SetActive(true);
        yield return new WaitForSeconds(3);
        NotificationText.SetActive(false);
    }
}
