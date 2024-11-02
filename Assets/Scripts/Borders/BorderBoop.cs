using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderBoop : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col == null) return;
        if (col.gameObject.CompareTag("Player"))
        {
            DataPersistenceManager.instance.LoadGame();
        }
    }
}
