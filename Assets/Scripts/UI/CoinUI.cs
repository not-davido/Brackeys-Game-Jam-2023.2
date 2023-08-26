using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CoinCountTxt;
    CollectibleBag playerBag;

    private void Start()
    {
        playerBag = GameManager.Instance.GetPlayer().GetComponent<CollectibleBag>();
        CoinCountTxt.text = playerBag.CollectibleCount.ToString();
    }

    private void Update()
    {
        CoinCountTxt.text = playerBag.CollectibleCount.ToString();
    }
}
