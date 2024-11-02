using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    [Header("Engine Stuff")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D pickupCollider;
    [SerializeField] private GameObject cameraFollowObject;
    [SerializeField] private CinemachineVirtualCamera cam;
    private GameObject interactObject;

    [Header("Animation Stuff")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip idleAnim;
    [SerializeField] private AnimationClip walkAnim;
    [SerializeField] private AnimationClip fallAnim;
    [SerializeField] private AnimationClip jumpAnim;
    private Coroutine jumpTimer;

    [Header("Const Variables")]
    private const float speed = 3f;
    private const float voidSpeed = 1.5f;
    private const float baseJumpForce = 3f;
    private const float interactRange = 5f;
    private const float islandOrthoSize = 3.5f;
    private const float voidOrthoSize = 5f;
    private const float orthoTransitionTime = 1f;

    [Header("Dynamic Variables")]
    private Vector2 mousePos;
    private float realJumpForce = baseJumpForce;
    private int jumpUpgrades = 0;
    public bool facingRight { get; private set; } = true;
    private bool jumping;
    public int currency { get; private set; } = 0;

    [Header("State Variables")]
    private float moveTo = 0f;
    private bool clicked = false;
    private bool clickMoving = false;
    private float dir = 0f;

    private void Awake()
    {
        inputReader.MoveEvent += HandleMove;
        inputReader.MouseEvent += MouseMove;
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
        dir = _dir;
        clickMoving = false;
    }

    private void MouseMove(Vector2 _pos)
    {
        mousePos = _pos;
    }

    private void Jump()
    {
        Debug.Log("jumped, or so they say");
        rb.AddForce(Vector2.up * realJumpForce, ForceMode2D.Impulse);
        jumping = true;
        if (jumpTimer != null) StopCoroutine(jumpTimer);
        jumpTimer = StartCoroutine(JumpTimer());
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
        clickMoving = true;
        clicked = true;
        moveTo = Camera.main.ScreenToWorldPoint(mousePos).x;
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
        if (col == null) return;

        if (col.gameObject.CompareTag("Interactable"))
            interactObject = col.gameObject;
        else if (col.gameObject.CompareTag("IslandArea"))
        {
            inputReader.SetIslandMovement();
            StartCoroutine(lerpOrthoSize(true));
            GameManager.instance.HidePointer();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Interactable"))
        interactObject = null;
        else if (col.gameObject.CompareTag("IslandExit"))
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
        {
            rb.velocity = new Vector2(voidSpeed, rb.velocity.y);
            if (!facingRight) CarFlipper();
            clickMoving = false;
        }
    }

    private void ClickMove()
    {
        float playerX = transform.position.x;
        if (moveTo < playerX && moveTo - playerX < -0.1f)
        {
            rb.velocity = new Vector2(-speed, transform.position.y);
            if (facingRight) CarFlipper();
        }
        else if (moveTo > playerX && moveTo - playerX > 0.1f)
        {
            rb.velocity = new Vector2(speed, transform.position.y);
            if (!facingRight) CarFlipper();
        }
        else clickMoving = false;
        clicked = false;
    }

    private void AnimationHandler()
    {
        if (!inputReader.voidMode)
        {
            if (rb.velocity.x > 0.1f || rb.velocity.x < -0.1f)
            {
                animator.Play(walkAnim.name);
            }
            else
            {
                animator.Play(idleAnim.name);
            }
        }
        else
        {
            if (jumping)
            {
                animator.Play(jumpAnim.name);
            }
            else
            {
                animator.Play(fallAnim.name);
            }
        }
    }

    private IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds (jumpAnim.length * 3);
        jumping = false;
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
        if (!GameManager.instance.introRunning)
        {
            if (clickMoving) ClickMove();
            else Move();
        }
        AnimationHandler();
    }

    public void LoadData(GameData data)
    {
        transform.position = data.position;
        currency = data.currency;
    }

    public void SaveData(ref GameData data)
    {
        data.currency = currency;
    }
}