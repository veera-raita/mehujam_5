using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Engine Stuff")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D pickupCollider;
    [SerializeField] private GameObject cameraFollowObject;
    [SerializeField] private CinemachineVirtualCamera cam;
    private GameObject interactObject;

    [Header("Const Variables")]
    private const float speed = 3f;
    private const float voidSpeed = 1.5f;
    private const float baseJumpForce = 10f;
    private const float interactRange = 5f;
    private const float islandOrthoSize = 3.5f;
    private const float voidOrthoSize = 5f;
    private const float orthoTransitionTime = 1f;

    [Header("Dynamic Variables")]
    private float realJumpForce = baseJumpForce;
    private int jumpUpgrades = 0;
    public bool facingRight { get; private set; } = true;

    [Header("State Variables")]
    private float dir = 0f;

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
        dir = _dir;
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * realJumpForce, ForceMode2D.Impulse);
    }

    private void Use()
    {
        if (interactObject == null) return;
        interactObject.GetComponent<Interactable>().Interact();
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null)
        {
            if (col.gameObject.CompareTag("Interactable"))
            interactObject = col.gameObject;
            else if (col.gameObject.CompareTag("IslandArea"))
            {
                inputReader.SetIslandMovement();
                StartCoroutine(lerpOrthoSize(true));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Interactable"))
        interactObject = null;
        else if (col.gameObject.CompareTag("IslandArea"))
        {
            inputReader.SetVoidMovement();
            StartCoroutine(lerpOrthoSize(false));
        }
    }

    private void Move()
    {
        if (inputReader.islandMode)
        {
            rb.velocity = new Vector2(dir * speed, rb.velocity.y);
            if (rb.velocity.x < 0 && facingRight) CarFlipper();
            else if (rb.velocity.x > 0 && !facingRight) CarFlipper();
        }
        else if (inputReader.voidMode)
        rb.velocity = new Vector2(voidSpeed, rb.velocity.y);
    }

    private void CarFlipper()
    {
        if (facingRight)
        {
            Vector3 newRotation = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(newRotation);
            facingRight = !facingRight;

            //turn camera to follow object with small delay, handled in different script
            StartCoroutine(cameraFollowObject.GetComponent<CameraFollowObject>().FlipYLerp());
        }
        else
        {
            Vector3 newRotation = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(newRotation);
            facingRight = !facingRight;

            //turn camera to follow object with small delay, handled in different script
            StartCoroutine(cameraFollowObject.GetComponent<CameraFollowObject>().FlipYLerp());
        }
    }

    private IEnumerator lerpOrthoSize(bool toIsland)
    {
        float initialOrthoSize = cam.m_Lens.OrthographicSize;
        float finalOrthoSize = toIsland ? islandOrthoSize : voidOrthoSize;
        float currentOrtho = initialOrthoSize;
        float takenTime = 0f;

        while (takenTime < orthoTransitionTime)
        {
            takenTime += Time.deltaTime;
            currentOrtho = Mathf.Lerp(initialOrthoSize, finalOrthoSize, (takenTime / orthoTransitionTime));
            cam.m_Lens.OrthographicSize = currentOrtho;
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
