using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CollectHealth : MonoBehaviour, IDataPersistence
{
    [SerializeField] private int healAmount = 15;
    [SerializeField] private int currencyAmount = 0;

    public void LoadData(GameData data)
    {
        if (currencyAmount != 0)
        gameObject.SetActive(true);
    }

    public void SaveData(ref GameData data) {}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col == null) return;
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerController controller = col.GetComponent<PlayerController>();
            controller.Heal(healAmount);
            if (currencyAmount != 0) controller.AddCurrency(currencyAmount);
        }
        gameObject.SetActive(false);
    }
}
