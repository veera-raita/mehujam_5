using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandExit : MonoBehaviour
{
    [SerializeField] int leavingIslandNumber;
    private bool calledOnce = false;

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col == null) return;
        if (calledOnce) return;
        if (col.gameObject.CompareTag("Player"))
        {
            GameManager.instance.UpdatePointer(leavingIslandNumber);
            calledOnce = true;
        }
    }
}
