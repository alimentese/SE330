using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] public float health = 100f;
    [SerializeField] public float score = 0f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;

    // Cached component references
    Rigidbody2D PlayerRigidbody;
    BoxCollider2D PlayerFeetCollider;
    Animator PlayerAnimator;


    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerFeetCollider = GetComponent<BoxCollider2D>();
        PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Attack1();
        Run();
        Jump();
        FlipSprite();

    }

    private void Attack1() {
        if (Input.GetButtonDown("Fire1")) {          
            PlayerAnimator.SetTrigger("Attack1");    
        }
    }

    private void Run() {
        float controlThrow2 = Input.GetAxis("Vertical"); // value is between -1 to +1       
            float controlThrow = Input.GetAxisRaw("Horizontal"); // value is between -1 to +1
            Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, PlayerRigidbody.velocity.y);
            PlayerRigidbody.velocity = playerVelocity;
            bool stateOfRunning = Mathf.Abs(PlayerRigidbody.velocity.x) > Mathf.Epsilon;
            if (PlayerFeetCollider.IsTouchingLayers()) {
                PlayerAnimator.SetBool("Running", stateOfRunning);
            }        
    }

    private void FlipSprite() {
        // if the player is moving horizontally
        if (!PlayerAnimator.GetBool("Crouch") && PlayerAnimator.GetBool("Running") || !PlayerAnimator.GetBool("Crouch") && PlayerAnimator.GetBool("DrawSwordRunning")) {
            bool playerHorizontalSpeed = Mathf.Abs(PlayerRigidbody.velocity.x) > Mathf.Epsilon;
            if (playerHorizontalSpeed) {
                transform.localScale = new Vector2(Mathf.Sign(PlayerRigidbody.velocity.x) * 5f, 5f);
            }
        }
    }

    private void Jump() {
        if (PlayerFeetCollider.IsTouchingLayers()) { //(LayerMask.GetMask("Ground"))
            PlayerAnimator.SetBool("Jumping", false);
            //print("Touching ground!");
            if (Input.GetButtonDown("Jump") && !PlayerAnimator.GetBool("Crouch")) {
                PlayerAnimator.SetBool("Jumping", true);
                //effector.rotationalOffset = 0f;
                Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
                PlayerRigidbody.velocity += jumpVelocityToAdd;
            }
            /*if (Input.GetKey("down") && Input.GetButtonDown("Jump")) {
                if (effector.rotationalOffset == 0) {
                    effector.rotationalOffset = 180f;
                    Debug.Log("rotate 180");

                }
                else {
                    effector.rotationalOffset = 0f;
                }

                //Thread.Sleep(timeDelay * 1000);
                //System.Threading.Thread.Sleep(3000);

            }*/

        }
        else {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Coin")) {
            score += 10;
            Destroy(collision.gameObject);
        }

        if(collision.CompareTag("LeftCollider")) {
            Debug.Log("sol duvara carpildi!");
        }
    }

}
