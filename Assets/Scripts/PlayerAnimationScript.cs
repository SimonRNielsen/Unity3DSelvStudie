using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationScript : MonoBehaviour
{

    public InputActionReference moveAction;
    private Animator animator;

    private const float maxValue = 1f;
    private const float defaultValue = 0f;
    private float smoothX;
    private float smoothY;
    public float smoothingIncrement;
    private bool turning;
    private bool momentum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        //Y-axis = W/S, W = +1, S = -1
        //X-axis = A/D, A = -1, D = +1
        //HUSK! De bliver normalized!
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        switch (moveInput.x)
        {
            case < 0:
                smoothX += smoothX >= -maxValue ? -smoothingIncrement : 0;
                break;
            case > 0:
                smoothX += smoothX <= maxValue ? smoothingIncrement : 0;
                break;
            default:
                break;
        }

        switch (moveInput.y)
        {
            case < 0:
                smoothY += smoothY >= -maxValue ? -smoothingIncrement : 0;
                break;
            case > 0:
                smoothY += smoothY <= maxValue ? smoothingIncrement : 0;
                break;
            default:
                break;
        }

        momentum = moveInput.y != 0;
        turning = moveInput.x != 0;

        if (!turning)
            smoothX = Mathf.MoveTowards(smoothX, defaultValue, smoothingIncrement);
        if (!momentum)
            smoothY = Mathf.MoveTowards(smoothY, defaultValue, smoothingIncrement);

        smoothX = Mathf.Clamp(smoothX, -maxValue, maxValue);
        smoothY = Mathf.Clamp(smoothY, -maxValue, maxValue);

        animator.SetBool("Momentum", momentum);
        animator.SetBool("Turning", turning);
        animator.SetFloat("ForwardMovement", smoothY);
        animator.SetFloat("TurningFactor", smoothX);

    }
}
