using System.Collections;
using UnityEngine;

public class Game_PlayerMovement : MonoBehaviour
{

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float runBuildUp;
    private float movementSpeed;

    [SerializeField] private float jumpMultiplier;
    [SerializeField] private AnimationCurve jumpFallOff;

    [SerializeField] private float slopForce;
    [SerializeField] private float slopForceRayLength;


    private CharacterController _charcontrol;
    private bool isJumping;

    private void Start()
    {
        _charcontrol = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float verticalInp = Input.GetAxis("Vertical");
        float horizontalInp = Input.GetAxis("Horizontal");

        Vector3 frontMov = transform.forward * verticalInp;
        Vector3 sideMov = transform.right * horizontalInp;

        _charcontrol.SimpleMove(Vector3.ClampMagnitude(frontMov + sideMov, 1) * movementSpeed);
        if((verticalInp != 0 || horizontalInp != 0 ) && OnSlope()) _charcontrol.Move(Vector3.down * _charcontrol.height/2 * slopForce * Time.deltaTime);
        SetMovementSpeed();
        Jump();
    }

    private void SetMovementSpeed()
    {
        if (Input.GetButton("Run"))
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUp);
        else
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUp);

    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        _charcontrol.slopeLimit = 90;
        float timeInAir = 0;
        do
        {
            float jumpPower = jumpFallOff.Evaluate(timeInAir);
            _charcontrol.Move(Vector3.up * jumpPower * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!_charcontrol.isGrounded && _charcontrol.collisionFlags != CollisionFlags.Above);
        _charcontrol.slopeLimit = 45;
        isJumping = false;
    }

    private bool OnSlope()
    {
        if (isJumping) return false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _charcontrol.height / 2 * slopForceRayLength))
            if (hit.normal != Vector3.up) return true;
        return false;
    }
}
