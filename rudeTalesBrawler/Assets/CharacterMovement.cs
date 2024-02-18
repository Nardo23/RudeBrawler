using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public bool canMove = true;
    [Tooltip(("If your character does not jump, ignore all below 'Jumping' Character"))]
    [SerializeField] private bool doesCharacterJump = false;

    [Header("Base / Root")]
    [SerializeField] private Rigidbody2D baseRB;
    CircleCollider2D baseCol;
    [SerializeField] private float hSpeed = 10f;
    [SerializeField] private float vSpeed = 6f;
    [Range(0, 1.0f)]
    [SerializeField] float movementSmooth = 0.5f;

    [Header("'Jumping' Character")]
    [SerializeField] private Rigidbody2D charRB;
    [SerializeField] private float jumpVal = 10f;
    [SerializeField] private int possibleJumps = 1;
    [SerializeField] private int currentJumps = 0;
    [SerializeField] public bool onBase = false;
    [SerializeField] private Transform jumpDetector;
    [SerializeField] private float detectionDistance;
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private float jumpingGravityScale;
    [SerializeField] private float fallingGravityScale;
    
    private bool jump;

    public ParticleSystem runParticlesR;
    public ParticleSystem runParticlesL;
    public ParticleSystem jumpParticleL;
    public ParticleSystem jumpParticleR;

    private bool facingRight = true;
    private Vector3 velocity = Vector3.zero;
   
    PlayerInput input;
    Controls controls = new Controls();
    
    // 
    private Vector3 charDefaultRelPos, baseDefPos;


    // Start is called before the first frame update
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        charDefaultRelPos = charRB.transform.localPosition;
        baseCol = GetComponent<CircleCollider2D>();
        
    }
    
    private void Update()
    {
        controls = input.GetInput();
        if (controls.JumpState && currentJumps < possibleJumps)
        {
            jump = true;
        }
        //Debug.Log("jump " + jump);
    }

    private void FixedUpdate()
    {
        Move();
    }
    
    private void Move()
    {
        if (!onBase && doesCharacterJump && charRB.velocity.y <= 4) //pay attention to the float used to determine when to check for base
        {
            detectBase();
        }

        
        if (canMove)
        {
            Vector2 piss = new Vector2(controls.HorizontalMove, controls.VerticalMove).normalized;
            Vector3 targetVelocity = new Vector2(piss.x * hSpeed, piss.y * vSpeed);

            Vector2 _velocity = Vector3.SmoothDamp(baseRB.velocity, targetVelocity, ref velocity, movementSmooth);
            baseRB.velocity = _velocity;
 
            //----- 
            if (doesCharacterJump)
            {
                if (onBase)
                {
                    //baseCol.isTrigger = false;
                    // charRB.velocity = baseRB.velocity;
                    //charRB.velocity = Vector2.zero;

                    // vertical check
                    if (charRB.transform.localPosition != charDefaultRelPos)
                    {
                        var charTransform = charRB.transform;
                        charTransform.localPosition = new Vector2(charTransform.localPosition.x,
                            charDefaultRelPos.y);
                    }

                    this.gameObject.layer = 0; // set the layer back to default when grounded
                    charRB.drag = 3;
                    charRB.velocity = Vector2.zero;
                }
                else
                {
                    // falling
                    // if (charRB.velocity.y < 0)
                    // {
                    //     // charRB.gravityScale = fallingGravityScale;
                    // }
                    // else
                    // { // moving upward from jump
                    //     // charRB.gravityScale = jumpingGravityScale;
                    // }

                    
                        
                    
                    if(charRB.velocity.y < 0)
                    {
                        charRB.drag = 1 + baseRB.velocity.y / vSpeed*3;
                    }
                    else
                        charRB.drag = 1 - baseRB.velocity.y / vSpeed*3;

                    charRB.velocity = new Vector2(_velocity.x, charRB.velocity.y);
                    
                    //baseCol.isTrigger=true;
                    this.gameObject.layer = 9; // layer nine is the jumping layer. this layer wont collide with jumpable layer
                                   
                    
                    
                }

                if (jump)
                {
                    charRB.isKinematic = false;
                    charRB.velocity = Vector2.zero;
                    charRB.AddForce(Vector2.up * jumpVal, ForceMode2D.Impulse);
                    charRB.gravityScale = jumpingGravityScale;
                    charRB.drag = 1 - baseRB.velocity.y / vSpeed;
                    jump = false;
                    currentJumps++;
                    onBase = false;
                }
                
                // --- horizontal position check
                if (charRB.transform.localPosition != charDefaultRelPos)
                {
                    //print("pos diff- local: " + charRB.transform.localPosition + "  --default: " + charDefaultRelPos );
                    var charTransform = charRB.transform;
                    charTransform.localPosition = new Vector2(charDefaultRelPos.x,
                        charTransform.localPosition.y);
                }
            }
            // --- 

            // rotate if we're facing the wrong way
            if (onBase)
            {
                if (controls.HorizontalMove > 0 && !facingRight)
                {
                    
                    flip();
                    runParticlesR.Play();
                }
                else if (controls.HorizontalMove < 0 && facingRight)
                {
                    
                    flip();
                    runParticlesL.Play();
                }
            }
            
        }
        else
        {
            baseRB.velocity = Vector3.SmoothDamp(baseRB.velocity, Vector3.zero, ref velocity, .1f);
        }
    }

    private void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        
        
    }

    private void detectBase()
    {
        RaycastHit2D hit = Physics2D.Raycast(jumpDetector.position, -Vector2.up, detectionDistance, detectLayer);
        if(hit.collider != null)
        {
            onBase = true;
            charRB.isKinematic = true;
            currentJumps = 0;
            charRB.transform.localPosition = new Vector3(charRB.transform.localPosition.x, charDefaultRelPos.y, charRB.transform.localPosition.z);
            charRB.velocity = Vector2.zero;
            baseRB.velocity = Vector2.zero;
            if (facingRight)
            {
                Debug.Log("runR, jumpL");
                runParticlesR.Play();
                jumpParticleL.Play();
            }
            else
            {
                Debug.Log("runL, jumpR");
                runParticlesL.Play();
                jumpParticleR.Play();
            }
            
            Debug.Log("setting velocity to zero");
        }
        if(charRB.transform.localPosition.y+charDefaultRelPos.y < baseRB.transform.localPosition.y)
        {
            charRB.transform.localPosition = new Vector3 (charRB.transform.localPosition.x, charDefaultRelPos.y, charRB.transform.localPosition.z);
            onBase = true;
            charRB.isKinematic = true;
            currentJumps = 0;
            //Debug.Log("AAARGGGRHRHRHR");
        }
    }

    private void OnDrawGizmos()
    {
        if (doesCharacterJump)
        {
            Gizmos.DrawRay(jumpDetector.transform.position, -Vector3.up * detectionDistance);
        }
    }
}
