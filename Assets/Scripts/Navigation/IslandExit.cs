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
        if (col.gameObject.CompareTag("Player"))
        {
            if (!calledOnce)
            {
                GameManager.instance.SaveLastIsland(leavingIslandNumber);
                calledOnce = true;
                DataPersistenceManager.instance.SaveGame();
            }
            GameManager.instance.UpdatePointer(leavingIslandNumber);
        }
    }
}
