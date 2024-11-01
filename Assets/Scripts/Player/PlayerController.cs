using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private void Awake()
    {
        inputReader.MoveEvent += HandleMove;
        inputReader.JumpEvent += Jump;
        inputReader.UseEvent += Use;
        inputReader.ClickEvent += Click;
        inputReader.ClickCanceledEvent += ClickCanceled;
        inputReader.ScrollEvent += Scroll;
        inputReader.SelectEvent += Select;
    }

    private void HandleMove(float _dir)
    {
        //dir is -1 when a is pressed and 1 when d is pressed
        Debug.Log(_dir);
    }

    private void Jump()
    {

    }

    private void Use()
    {

    }

    private void Click()
    {

    }

    private void ClickCanceled()
    {

    }

    private void Scroll(float _dir)
    {
        //dir is -1 when s is pressed and 1 when w is pressed
    }

    private void Select()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
