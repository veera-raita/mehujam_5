using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] islandPointers;
    public GameObject pointTo { get; private set; }
    [SerializeField] private GameObject pointer;

    //singleton
    public static GameManager instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        Destroy(this);
        else
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (pointTo != null)
        RotatePointer();
    }

    private void RotatePointer()
    {
        Vector2 dir = (player.transform.position - pointTo.transform.position).normalized;
        float zRotation = Vector2.SignedAngle(Vector2.up, dir);
        pointer.transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    public void UpdatePointer(int _islandNumber)
    {
        pointer.SetActive(true);
        pointTo = GameManager.instance.islandPointers[_islandNumber];
    }

    public void HidePointer()
    {
        pointer.SetActive(false);
    }
}
