using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // ----------------------------------------------------------------VARIABLES---------------------------------------------------------------//
    // Keybinds
    private KeyCode MoveLeft = KeyCode.A;
    private KeyCode MoveRight = KeyCode.D;
    private KeyCode Jump = KeyCode.Space;

    // horizontal movement
    public float speed = 1.0f;
    public float startingSpeed = 1.0f;
    public float accelerationRate = 10.0f;
    public float maxSpeed = 50.0f;
    public float nearMaxSpeed = 40.0f;

    public float stoppingPower = 75.0f;
    public float turnSpeed = 1000.0f;
    public float stuckSpeed = 10.0f;

    public bool topSpeed = false;
    public bool losingSpeed = false;

    // vertical movement
    public float jumpHeight = 10.0f;
    public float startingJump = 10.0f;
    public float jumpChargeRate = 5.0f;
    public float maxHeight = 50.0f;
    public float nearMaxHeight = 40.0f;
    public bool highJump = false;
    public float wallJumpHeight = 700.0f;

    // collision
    Rigidbody2D rBody;
    public bool isOnGround = true;
    public bool touchWall = false;
    public bool stuck = false;
    public float bounceHeight = 10.0f;

    // health
    public float health = 3;
    public bool damageBuffer = false;
    public GameObject health3;
    public GameObject health2;
    public GameObject health1;

    // animation
    public SpriteRenderer sprite;
    public float rotateSpeed = 20.0f;
    public float startingRotationSpeed = 20.0f;
    public float rotationAcceleration = 5.0f;
    public float maxRotationSpeed = 50.0f;
    public float rotateTurnSpeed = 100.0f;

    // -----------------------------------------------------------START---------------------------------------------------------//
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    // -----------------------------------------------------------UPDATE---------------------------------------------------------//
    // Update is called once per frame
    void Update()
    {
        // ----------------------------------------WALKING-------------------------------------------//
        // walk right
        if (Input.GetKey(MoveRight) && stuck == false)
        {
            rBody.AddForce(transform.right * speed * Time.deltaTime, ForceMode2D.Force);
            losingSpeed = false;
            sprite.flipX = true;
            sprite.transform.Rotate(0.0f, 0.0f, -rotateSpeed * Time.deltaTime);
            // acceleration
            speed += accelerationRate * Time.deltaTime;
            rBody.drag = 0;
            if (speed > nearMaxSpeed && stuck == false) 
            {
                topSpeed = true;
            }
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            // jump height acceleration
            jumpHeight += jumpChargeRate * Time.deltaTime;
            if (jumpHeight > nearMaxHeight && stuck == false) 
            {
                highJump = true;
            }
            if (jumpHeight > maxHeight)
            {
                jumpHeight = maxHeight;
            }
            // sprite rotation acceleration
            rotateSpeed += rotationAcceleration * Time.deltaTime;
            if (rotateSpeed > maxRotationSpeed) 
            {
                rotateSpeed = maxRotationSpeed;
            }
            if (rotateSpeed < startingRotationSpeed) 
            { 
                rotateSpeed = startingRotationSpeed;
            }
        }
        // walk left
        if (Input.GetKey(MoveLeft) && stuck == false)
        {
            rBody.AddForce(transform.right * -speed * Time.deltaTime, ForceMode2D.Force);
            losingSpeed = false;
            sprite.flipX = false;
            sprite.transform.Rotate(0.0f, 0.0f, rotateSpeed * Time.deltaTime);
            // acceleration
            speed += accelerationRate * Time.deltaTime;
            rBody.drag = 0;
            if (speed > nearMaxSpeed && stuck == false)
            {
                topSpeed = true;
            }
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            // jump height acceleration
            jumpHeight += jumpChargeRate * Time.deltaTime;
            if (jumpHeight > nearMaxHeight && stuck == false)
            {
                highJump = true;
            }
            if (jumpHeight > maxHeight)
            {
                jumpHeight = maxHeight;
            }
            // sprite rotation acceleration
            rotateSpeed += rotationAcceleration * Time.deltaTime;
            if (rotateSpeed > maxRotationSpeed)
            {
                rotateSpeed = maxRotationSpeed;
            }
            if (rotateSpeed < startingRotationSpeed)
            {
                rotateSpeed = startingRotationSpeed;
            }
        }
        // stop walking right
        if (Input.GetKeyUp(MoveRight))
        {
            losingSpeed = true;
            rBody.AddForce(transform.right * -stoppingPower * Time.deltaTime, ForceMode2D.Force);
            speed -= turnSpeed;
            rotateSpeed -= rotateTurnSpeed;
            sprite.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        }
        // stop walking left
        if (Input.GetKeyUp(MoveLeft))
        {
            losingSpeed = true;
            rBody.AddForce(transform.right * stoppingPower * Time.deltaTime, ForceMode2D.Force);
            speed -= turnSpeed;
            rotateSpeed -= rotateTurnSpeed;
            sprite.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        }
        // loses speed while stopped
        if (losingSpeed == true) 
        {
            speed -= accelerationRate * Time.deltaTime;
            if (speed < startingSpeed)
            {
                speed = startingSpeed;
            }
            jumpHeight -= 30 * Time.deltaTime;
            if (jumpHeight < startingJump)
            {
                jumpHeight = startingJump;
            }
            rotateSpeed -= rotationAcceleration * Time.deltaTime;
            if (rotateSpeed < startingRotationSpeed)
            {
                rotateSpeed = startingRotationSpeed;
            }
        }
        if (speed < nearMaxSpeed && stuck == false) 
        {
            topSpeed = false;
        }
        if (speed < startingSpeed)
        {
            speed = startingSpeed;
        }
        // if player gets stuck in speedtrap:
        if (stuck == true)
        {
            losingSpeed = false;
            topSpeed = false;
            speed = stuckSpeed;
            jumpHeight = startingJump;
            rBody.drag = 4;
            if (Input.GetKey(MoveLeft) && stuck == true)
            {
                rBody.AddForce(transform.right * -speed * Time.deltaTime, ForceMode2D.Force);
            }
            if (Input.GetKey(MoveRight) && stuck == true)
            {
                rBody.AddForce(transform.right * speed * Time.deltaTime, ForceMode2D.Force);
            }
        }
        if (stuck == false) 
        {
            rBody.drag = 0;
        }

        // ----------------------------------------HEALTH-------------------------------------------//
        if (damageBuffer == true)
        {
            StartCoroutine(DamageBuffer());
        }
        IEnumerator DamageBuffer()
        {
            yield return new WaitForSeconds(0);
            damageBuffer = false;
        }
        if (health == 3)
        {
            health3.SetActive(true);
            health2.SetActive(false);
            health1.SetActive(false);
        }
        if (health == 2)
        {
            health3.SetActive(false);
            health2.SetActive(true);
            health1.SetActive(false);
        }
        if (health == 1)
        {
            health3.SetActive(false);
            health2.SetActive(false);
            health1.SetActive(true);
        }
        if (health == 0)
        {
            SceneManager.LoadScene("Lose_Screen_1");
        }
        // ----------------------------------------JUMPING-------------------------------------------//
        // jump
        if (Input.GetKeyDown(Jump) && isOnGround && touchWall == false)
        {
            rBody.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
        }
        // wall jump
        if (Input.GetKeyDown(Jump) && isOnGround == false && touchWall)
        {
            rBody.AddForce(transform.up * wallJumpHeight, ForceMode2D.Impulse);
            touchWall = false;
        }
    }
    //------------------------------------------------------------------COLLISION-------------------------------------------------------------------------//

    //-----------------------------------TRIGGER-------------------------------------//
    private void OnTriggerEnter2D(Collider2D other)
    {
        // break through breakable walls if collide while at top speed
        if (other.CompareTag("Breakable") && topSpeed)
        {
            Destroy(other.gameObject);
            speed -= 500;
            rotateSpeed -= 50;
        }

        // speed trap / sticky ground
        if (other.CompareTag("SpeedTrap"))
        {
            stuck = true;
        }

        // damage trap
        if (other.CompareTag("DamageTrap") && damageBuffer == false)
        {
            damageBuffer = true;
            health -= 1;
        }

        // if the player gets caught by the chasing thing
        if (other.CompareTag("Death"))
        {
            SceneManager.LoadScene("Lose_Screen_1");
        }

        // bounce pad
        if (other.CompareTag("BouncePad"))
        {
            rBody.AddForce(Vector3.up * bounceHeight, ForceMode2D.Impulse);
        }

        // win
        if (other.CompareTag("Win"))
        {
            SceneManager.LoadScene("Win_Screen_1");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // if player escapes speed trap
        if (other.CompareTag("SpeedTrap"))
        {
            stuck = false;
            speed = startingSpeed;
        }
    }

    //---------------------------------------------------------COLLISION COLLISION---------------------------------------------------------------//
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        // resets jump after landing
        if (collision2D.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            highJump = false;
        }
        // touching wall lets player wall jump
        if (collision2D.gameObject.CompareTag("Wall"))
        {
            touchWall = true;
        }
    }
    // wall jump buffer time
    IEnumerator walljumpBuffer()
    {
        yield return new WaitForSeconds(1);
        touchWall = false;
    }
    // wall jumping
    private void OnCollisionExit2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(walljumpBuffer());
        }
        if (collision2D.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }
}
