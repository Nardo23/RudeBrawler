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
    public int currentJumps = 0;
    [SerializeField] public bool onBase = false;
    [SerializeField] private Transform jumpDetector;
    [SerializeField] private float detectionDistance;
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private float jumpingGravityScale;
    [SerializeField] private float fallingGravityScale;
    public GameObject shadow;
    private bool jump;
    Vector2 _velocity;

    public ParticleSystem runParticlesR;
    public ParticleSystem runParticlesL;
    public ParticleSystem jumpParticleL;
    public ParticleSystem jumpParticleR;

    [HideInInspector] public bool facingRight = true;
    private Vector3 velocity = Vector3.zero;
   
    PlayerInput input;
    Controls controls = new Controls();
    public bool runBoost = false;
    bool prevFacingRight = true;
    float prevHor, prevVert;
    bool prevStoped;
    bool startedMove = false;
    public Vector2 maxBoost;
    Vector2 curBoost = Vector2.zero;
    public float boostDuration = 2f;
    bool canCycleBoost = true;
    public float knockbackMultiplyer;
    public bool canStop = false;
    public bool dragChangeEnabled = true;
    bool lagRoutineRunning = false;
    public bool jumpableAnim;
    // 
    private Vector3 charDefaultRelPos, baseDefPos;

    bool stoped = false;
    public float landLag =0;
    bool landing = false;
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
        if (controls.JumpState && currentJumps == 0)
        {
            jump = true;
        }

        //Debug.Log("jump " + jump);
    }

    public void startjump()
    {
        if (currentJumps < possibleJumps)
        {
            jump = true;
        }
    }
    private void FixedUpdate()
    {
        if (!stoped)
        {
            Move();
        }
        
    }

    public void enableHitStop(float duration)
    {
        if(!stoped )
            StartCoroutine(hitStop(duration));
    }
    IEnumerator hitStop(float duration)
    {
        Animator anim = GetComponentInChildren<Animator>();
        anim.speed = .5f;
        float curTime = 0;
        stoped = true;
        bool wasKin = charRB.isKinematic;
        baseRB.isKinematic = true;
        charRB.isKinematic = true;
        Vector3 velStore = baseRB.velocity;
        baseRB.velocity = Vector3.zero;
        while (curTime < duration)
        {
            curTime += Time.deltaTime;
            yield return null;

        }
        baseRB.velocity = velStore;
        Debug.Log("endHitStop");
        anim.speed = 1;
        baseRB.isKinematic = false;
        charRB.isKinematic = wasKin;
        stoped = false;
        yield return null;
    }


    public void knockback(float force, bool hitRight)
    {
        baseRB.velocity =Vector2.zero;
        if (hitRight)
        {
            if (facingRight)
            {
                baseRB.AddForce((Vector2.left * force * knockbackMultiplyer), ForceMode2D.Impulse);
                Debug.Log("1");
            }
            else
            {
                baseRB.AddForce((Vector2.left * force * knockbackMultiplyer), ForceMode2D.Impulse);
                Debug.Log("2");
            }
        }
        else
        {
            if (facingRight)
            {
                baseRB.AddForce((Vector2.right * force * knockbackMultiplyer), ForceMode2D.Impulse);
                Debug.Log("3");
            }
            else
            {
                baseRB.AddForce((Vector2.right * force * knockbackMultiplyer), ForceMode2D.Impulse);
                Debug.Log("4");
            }
            
        }

        Debug.Log("KNOCKED "+ force * 1 * knockbackMultiplyer);
    }

    IEnumerator speedBoost()
    {
        canCycleBoost = false;
        float maxTime = boostDuration;
        float curTime = 0f;
        while (curTime< maxTime)
        {
            curBoost = Vector3.Lerp(curBoost, Vector2.zero, (curTime / maxTime));
            curTime += Time.deltaTime;
            yield return null;
        }
        canCycleBoost = true;
        curBoost = Vector2.zero;
        yield return null;
    }

    IEnumerator landingLag()
    {
        float curTime = 0f;
        lagRoutineRunning = true;
        while (curTime < landLag)
        {
            landing = true;
            curTime += Time.deltaTime;
            yield return null;
        }
        landLag = 0;
        landing = false;
        lagRoutineRunning = false;
        yield return null;
    }
    public void boost()
    {
        if (canCycleBoost)
        {
            StopAllCoroutines();
            //Debug.Log("boost");
            curBoost = maxBoost;
            StartCoroutine(speedBoost());
        }
        
    }
    private void Move()
    {
        if (!onBase && doesCharacterJump && charRB.velocity.y <= 4) //pay attention to the float used to determine when to check for base
        {
            detectBase();
        }


        if (canMove && !landing)
        {

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
            if (prevStoped)
            {
                if (controls.HorizontalMove != 0 || controls.VerticalMove != 0)
                {
                    startedMove = true;
                }
                else
                {
                    startedMove = false;
                }
            }
            else
            {
                startedMove = false;
            }

            if (runBoost)
            {
                if (prevFacingRight != facingRight || startedMove)
                {
                    StopAllCoroutines();
                    //Debug.Log("boost");
                    curBoost = maxBoost;
                    StartCoroutine(speedBoost());

                }
            }
            else
            {
                curBoost = Vector2.zero;
            }
            
            
            Vector2 piss = new Vector2(controls.HorizontalMove, controls.VerticalMove).normalized;
            Vector3 targetVelocity = new Vector2(piss.x * (hSpeed+Mathf.Abs(curBoost.x)), piss.y * (vSpeed+ Mathf.Abs(curBoost.y)));

            _velocity = Vector3.SmoothDamp(baseRB.velocity, targetVelocity, ref velocity, movementSmooth);
            Debug.Log("Uhhhh");
            baseRB.velocity = _velocity;
                                     
        }
        else
        {
            Debug.Log("UhUhUhUh");
            baseRB.velocity = Vector3.SmoothDamp(baseRB.velocity, Vector2.zero, ref velocity, .1f);
        }
        
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
                if(!jumpableAnim)
                    this.gameObject.layer = 8; // set the layer back to jumpable when grounded
                else
                    this.gameObject.layer = 9; // set to jumping layer for stirfry sidespecial attack
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


                if (dragChangeEnabled)//controlled by animation functions on animtorController Script
                {
                    if (charRB.velocity.y < 0)
                    {
                        charRB.drag = 1 + baseRB.velocity.y / vSpeed * 3;
                    }
                    else
                        charRB.drag = 1 - baseRB.velocity.y / vSpeed * 3;
                }
                else
                    charRB.drag = 3;


                charRB.velocity = new Vector2(_velocity.x, charRB.velocity.y);

                //baseCol.isTrigger=true;
                this.gameObject.layer = 9; // layer nine is the jumping layer. this layer wont collide with jumpable layer

            }

            if (jump && canMove &&!landing)
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
            if(!onBase ) // for debonesby teleport
            {
                if (!dragChangeEnabled)                
                    charRB.gravityScale = 0;
                else
                    charRB.gravityScale = jumpingGravityScale;

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

        prevVert = controls.VerticalMove;
        prevHor = controls.HorizontalMove;
        if (prevHor == 0 && prevVert == 0)
        {
            prevStoped = true;
        }
        else
        {
            prevStoped = false;
        }
        prevFacingRight = facingRight;
    }

    public void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        
        
    }

    private void detectBase()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(jumpDetector.position, -Vector2.up, detectionDistance, detectLayer);

        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject == shadow)
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
                if(landLag > 0 && !lagRoutineRunning)
                {
                    StartCoroutine(landingLag());
                }
                Debug.Log("setting velocity to zero");
            }
        }
       
        if(charRB.transform.localPosition.y+charDefaultRelPos.y < baseRB.transform.localPosition.y)
        {
            charRB.transform.localPosition = new Vector3 (charRB.transform.localPosition.x, charDefaultRelPos.y, charRB.transform.localPosition.z);
            onBase = true;
            charRB.isKinematic = true;
            currentJumps = 0;
            Debug.Log("AAARGGGRHRHRHR");
            if (landLag > 0 && !lagRoutineRunning)
            {
                StartCoroutine(landingLag());
            }
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
