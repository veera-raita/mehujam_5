using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col == null) return;
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(GameManager.instance.PlayOutro());
            inputReader.SetMenuMode();
        }
    }
}
