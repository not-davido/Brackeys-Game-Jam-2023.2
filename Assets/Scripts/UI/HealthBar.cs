using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup layout;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject hpPrefab;

    private List<GameObject> bars = new();

    // Start is called before the first frame update
    void Start() {
        Player player = GameManager.Instance.GetPlayer();
        Health health = player.GetComponent<Health>();
        print(health.CurrentHealth);
        health.OnDamaged += OnDamage;
        health.OnHealed += OnHeal;

        //int barCount = parent.transform.childCount - health.CurrentHealth;
        //int i = 0;

        //foreach (Transform child in parent.transform) {
        //    if (i == barCount) break;
        //    child.gameObject.SetActive(false);
        //    i++;
        //}

        //foreach (Transform child in parent.transform) {
        //    if (child.gameObject.activeInHierarchy) {
        //        bars.Add(child.gameObject);
        //    }
        //}

        for (int i = 0; i < health.CurrentHealth; i++) {
            var bar = Instantiate(hpPrefab, parent.transform);
            bars.Add(bar);
        }

        //if (bars.Count <= 25) {
        //    AdjustCellSize(new Vector2(45, 45));
        //} else {
        //    AdjustCellSize(new Vector2(25, 25));
        //}
    }

    void OnDamage(float amount, GameObject g, Transform t) {
        int i = 0;
        foreach (var b in bars.Where(b => b.activeSelf)) {
            if (i == (int)amount) break;
            b.SetActive(false);
            i++;
        }

        //if (bars.Where(b => b.activeSelf).Count() <= 25) {
        //    AdjustCellSize(new Vector2(45, 45));
        //} else {
        //    AdjustCellSize(new Vector2(25, 25));
        //}
    }

    void OnHeal(float amount) {
        int i = 0;
        foreach (var b in bars.Where(b => !b.activeSelf)) {
            if (i == (int)amount) break;
            b.SetActive(true);
            i++;
        }

        //if (bars.Where(b => b.activeSelf).Count() <= 25) {
        //    AdjustCellSize(new Vector2(45, 45));
        //} else {
        //    AdjustCellSize(new Vector2(25, 25));
        //}
    }

    //void AdjustCellSize(Vector2 size) {
    //    layout.cellSize = size;
    //}
}
