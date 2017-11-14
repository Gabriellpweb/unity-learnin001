using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected Rigidbody2D rb2d;

    [SerializeField]
    protected GameObject spr;

    [SerializeField]
    protected Transform footerRaycastOring;

    [SerializeField]
    protected float speed = 0.1f;

    [SerializeField]
    protected float fallSpeed = 0.8f;

    [SerializeField]
    protected float fallSpeedMax = 2f;

    [SerializeField]
    protected float jumpForce = 10f;

    [SerializeField]
    protected bool grounded = false;

    [SerializeField]
    protected float vspeed = 0;

    [SerializeField]
    protected float hspeed = 0;

    [SerializeField]
    protected bool isJumping = false;

    protected bool isFacingLeft = false;

    protected SpriteRenderer renderer;

    protected Vector2 groundNormal;

    // Use this for initialization
    void Start()
    {
        renderer = spr.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

        // Processar inputs
        UpdateInput();

        // Processar moviemnto
        UpdateMovement();

        // Processa animacao
        UpdateRender();
    }

    // Processa dados de entrada (Teclado ou Joystick)
    void UpdateInput()
    {
        hspeed = speed * Input.GetAxis("Horizontal") * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && grounded)
        {
            grounded = false;
            isJumping = true;
            vspeed = jumpForce;
        }

    }

    // Atualiza dados fisicos de movimento do personagem
    void UpdateMovement()
	{

        if (grounded)
        {
            vspeed = 0;
        }
        else
        {
            vspeed -= fallSpeed;
            //vspeed = Mathf.Clamp(vspeed, Physics2D.gravity.y * Time.deltaTime, jumpForce);
        }
        

        RaycastHit2D hit = Physics2D.Raycast(
            footerRaycastOring.position,
            -Vector2.up,
            Mathf.Infinity
        );

        if (hit.collider != null && hit.collider.gameObject.tag != "Player")
        {
            groundNormal = hit.normal.normalized;
            Debug.Log(groundNormal);
            //var direction = new Vector2(groundNormal.y, -groundNormal.x);
            var direction = new Vector2(-groundNormal.x, groundNormal.y);
            var distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
            Vector2 move = (Vector2.right + direction.normalized);

            if (distance > hspeed) {
                move = new Vector2(move.x/distance*hspeed, move.y/distance*hspeed);  
            } 
            //Vector2 move = ((Vector2.right + direction.normalized) / distance * hspeed) + (Vector2.up * vspeed);
            Debug.Log(direction);
            float product = Vector2.Dot(move, direction);

            //transform.Translate(move);
            //move = Vector2.up * vspeed;

            transform.Translate(move);

            if (!grounded && !isJumping && hit.distance <= Mathf.Abs(vspeed))
            {
                vspeed = 0;
                grounded = true;

            }

            if (grounded) {
                //transform.position = new Vector3(transform.position.x, hit.point.y + (renderer.bounds.size.y / 2) + 0.046f, 0);    
            }


        } else {

            if (grounded) {
                grounded = false;
            }

            Vector2 movement = new Vector2(hspeed, vspeed);
            //transform.Translate(Vector2.right);
        }
            
        if (isJumping)
        {
            if (vspeed <= 0)
            {
                isJumping = false;
            }
        }


	}

	// Atualiza estado visual do personagem (animaçao e sprites)
	void UpdateRender()
	{
        
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(footerRaycastOring.position, (Vector2)footerRaycastOring.position + (Vector2.up * (vspeed + 0.1f)));
    }
}
