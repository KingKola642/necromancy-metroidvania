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
    public bool Jumping;
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

    //accelation and decelaration PLEASE


    void OnEnable()
    {
        MovementKeys.Enable();
    }

    void OnDisable()
    {
        MovementKeys.Disable();
    }

    void Start()
    {
        RB.gravityScale = NormalGravity;
    }

    void FixedUpdate()
    {
        //sets direction of movement
        MoveDir = MovementKeys.ReadValue<Vector2>();

        //checks if Grounded
        Grounded = Physics2D.OverlapBox(GroundCheck.position, OverlapSize, 0f, GroundLayer);

        if (RB.linearVelocity.y < 0)
        {
            // if falling gravity is normal
            RB.gravityScale = NormalGravity * FallGravityMultiplier;
        }
        else if (RB.linearVelocity.y > 0 & !Jumping)
        {
            RB.gravityScale = NormalGravity * LowJumpMultiplier;
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

        //registers jump key
        if (Input.GetKey(KeyCode.Space))
        {
            Jumping = true;
        }
        else
        {
            Jumping = false;
        }

        if (Jumping & !Grounded)
        {
            NoJump = true;
        }

        if (Grounded & !Jumping)
        {
            NoJump = false;
        }

        //jumps
        if (Jumping & !NoJump)
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
