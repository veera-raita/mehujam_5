using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandEnter : MonoBehaviour
{
    private bool calledOnce = false;
    [SerializeField] private GameObject stopper;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col == null) return;
        if (calledOnce) return;
        if (col.gameObject.CompareTag("Player"))
        {
            stopper.SetActive(true);
            calledOnce = true;
        }
    }
}
