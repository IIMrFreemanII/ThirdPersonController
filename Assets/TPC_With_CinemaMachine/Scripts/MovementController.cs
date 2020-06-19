using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private new Camera camera;
    private Transform camTrans;

    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private bool crouch;
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Crouch = Animator.StringToHash("Crouch");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
        camTrans = camera.transform;
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRotation();
    }

    private void HandleInput()
    {
        float lerpSpeed = Time.deltaTime * 7.5f;
        
        float moveMultiplier = 1.5f;
        
        float currentHorizontal = Input.GetAxis("Horizontal") * moveMultiplier;
        float currentVertical = Input.GetAxis("Vertical") * moveMultiplier;
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            vertical = Mathf.Lerp(vertical, currentVertical, lerpSpeed);
            horizontal = Mathf.Lerp(horizontal, currentHorizontal, lerpSpeed);
        }
        else
        {
            vertical = Mathf.Lerp(vertical, currentVertical / 1.5f, lerpSpeed);
            horizontal = Mathf.Lerp(horizontal, currentHorizontal / 1.5f, lerpSpeed);
        }
    }

    private void HandleMovement()
    {
        animator.SetFloat(Vertical, vertical);
        animator.SetFloat(Horizontal, horizontal);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouch = !crouch;
            animator.SetBool(Crouch, crouch);
        }
    }

    private void HandleRotation()
    {
        float absVertical = Mathf.Abs(vertical);
        float absHorizontal = Mathf.Abs(horizontal);

        if (absVertical > 0.1 || absHorizontal > 0.1)
        {
            Vector3 newRotation = new Vector3(0, camTrans.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * 5f);
        }
    }
}
