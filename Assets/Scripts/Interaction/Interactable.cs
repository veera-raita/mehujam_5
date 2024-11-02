using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private bool checkingInteractions = false;
    private GameObject player;
    private PlayerController controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!checkingInteractions) return;
        if (!controller.interacted) return;
        Interact();
        controller.interacted = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider == null) return;
        if (collider.gameObject.CompareTag("Player"))
        {
            checkingInteractions = true;
            player = collider.gameObject;
            controller = player.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider == null) return;
        if (collider.gameObject.CompareTag("Player"))
        {
            checkingInteractions = false;
            player = null;
        }
    }

    public abstract void Interact();
}
