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
    protected float skinSpacing = 0.036f;

    protected bool isJumping = false;

    protected bool isFacingLeft = false;

    protected SpriteRenderer renderer;

    [SerializeField]
    protected Vector2 velocity = Vector2.zero;

    protected Vector2 groundNormal;

    [SerializeField]
    protected float height;

    private List<Vector2> hits;

    // Use this for initialization
    void Start()
    {
        renderer = spr.GetComponent<SpriteRenderer>();
        hits = new List<Vector2>();
        height = renderer.bounds.size.y;
    }


    void Update()
    {

        // Processar inputs
        UpdateInput();

        // Apply the gravity movement
        ProcessGravity();

        // Processar moviemnto
        UpdateMovement();

        // Processa animacao
        UpdateRender();
    }

    // Processa dados de entrada (Teclado ou Joystick)
    void UpdateInput()
    {
        velocity = Vector2.right * speed * Input.GetAxis("Horizontal") * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && grounded)
        {
            grounded = false;
            isJumping = true;
            velocity.y = jumpForce;
        }

    }

    void ProcessGravity()
    {
        if (!grounded)
        {
            velocity += fallSpeed * Physics2D.gravity * Time.deltaTime;
        }

        if (isJumping && velocity.y <= 0) {
            isJumping = false;
        }
    }

    // Atualiza dados fisicos de movimento do personagem
    void UpdateMovement()
	{
        Vector2 origin = footerRaycastOring.position;

		float distance = height;

        RaycastHit2D hit = Physics2D.Raycast(origin, -Vector2.up, distance);

        if (hit && !isJumping) {
			
            hits.Add(hit.point);
            
            if (!grounded) {

				if (hit.distance < Mathf.Abs(velocity.y)) {
					velocity.y = -hit.distance;
                    grounded = true;
                }

            }
        }

        transform.Translate(velocity, Space.World);
	}

	// Atualiza estado visual do personagem (animaçao e sprites)
	void UpdateRender()
	{
        
	}

    private void OnDrawGizmos()
    {
        foreach (Vector2 point in hits)
        {
            Gizmos.color = Color.red;
            float scale = 0.2f;
            Gizmos.DrawCube(point, new Vector3(scale, scale, scale));
        }

        Gizmos.DrawLine(footerRaycastOring.position, (Vector2)footerRaycastOring.position + (-Vector2.up * (height + velocity.magnitude)));
    }
}
