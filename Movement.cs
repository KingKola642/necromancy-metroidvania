using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public InputAction MovementKeys;
    public Vector2 MoveDir;
    public float LastDir;
    public Rigidbody2D RB;
    public SpriteRenderer SR;
    public float MovementSpeed = 5f;
    public float Jumping;
    public bool Grounded;
    public Transform GroundCheck;
    public LayerMask GroundLayer;
    public Vector2 OverlapSize;
    public float JumpStrenth = 5f;
    public float NormalGravity = 1f;
    public float FallGravityMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;
    public bool NoJump = false;
    public bool NoChange;
    public InputAction Jump;
    [Header("Attack")]
    public InputAction AttackKeys;
    public bool Attack;
    public SpriteRenderer SRHB;

    //accelation and decelaration PLEASE


    void OnEnable()
    {
        MovementKeys.Enable();
        Jump.Enable();
        AttackKeys.Enable();
    }

    void OnDisable()
    {
        MovementKeys.Disable();
        Jump.Disable();
        AttackKeys.Disable();
    }

    void Start()
    {
        RB.gravityScale = NormalGravity;
    }

    void FixedUpdate()
    {
        //sets direction of movement
        MoveDir = MovementKeys.ReadValue<Vector2>();

        Jumping = Jump.ReadValue<float>();
        Attack = AttackKeys.ReadValue<float>() > 0.5f;

        //checks if Grounded
        Grounded = Physics2D.OverlapBox(GroundCheck.position, OverlapSize, 0f, GroundLayer);

        if (RB.linearVelocity.y < 0)
        {
            // if falling gravity is normal
            RB.gravityScale = NormalGravity * FallGravityMultiplier;
        }
        else if (RB.linearVelocity.y > 0 & Jumping == 0)
        {
            RB.gravityScale = NormalGravity * LowJumpMultiplier;
        }

        if (Attack)
        {
            SRHB.enabled = true;
        }
        else
        {
            SRHB.enabled = false;
        }
    }

    void Update()
    {
        RB.gravityScale = NormalGravity;

        if (!Grounded & !NoChange)
        {
            LastDir = MoveDir.x;
            NoChange = true;
        }
        else if (Grounded)
        {
            NoChange = false;
        }

        //moves player
        if (!Grounded & LastDir == MoveDir.x)
        {
            RB.linearVelocity = new Vector2(MoveDir.x * MovementSpeed, RB.linearVelocityY);
        }
        else if (Grounded)
        {
            RB.linearVelocity = new Vector2(MoveDir.x * MovementSpeed, RB.linearVelocityY);
        }

        

        if (Jumping == 1 & !Grounded)
        {
            NoJump = true;
        }

        if (Grounded & Jumping == 0)
        {
            NoJump = false;
        }

        //jumps
        if (Jumping == 1 & !NoJump)
        {
            RB.linearVelocity = Vector2.up * JumpStrenth;
        }

        //flips player
        if (MoveDir.x == 1)
        {
           SR.flipX = false; 
        }
        else if (MoveDir.x == -1)
        {
            SR.flipX = true;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GroundCheck.position, OverlapSize);
    }
}
