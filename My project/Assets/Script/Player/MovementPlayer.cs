using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class MovementPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody playerRb;
    private InputSystem_Actions controls;
    public  int lifeLevel;
    public float jumpForce=10;
    public int runStamina=10;
    public int lampEnergy=20;
    public bool turnLights = true;
    [SerializeField] GameObject centerOfMass;
    public float speed = 15.0f;
    public float rotationSpeed = 12.0f;
    public bool isOnGround=true;

    private void Awake() {
        controls = new InputSystem_Actions();
        playerRb = GetComponent<Rigidbody>();
    }
    private void OnEnable() {
        controls.Player.Enable();
    }
    private void Start()
    {
        playerRb.centerOfMass = transform.InverseTransformPoint(centerOfMass.transform.position);
    }

    // Update is called once per frame
    private void Update()
    {
        jump();
    }
    public void jump()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }
    }
    private void FixedUpdate() {
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();
        float forwardInput = moveInput.y; //Moverse al frente
        playerRb.AddRelativeForce(Vector3.forward*speed*forwardInput);
        transform.Rotate(Vector3.up*Time.deltaTime*30*moveInput.x*rotationSpeed);

    }

    private void reduceLightsOfLamp()
    {
        lampEnergy--;
    }
    private void atackWithLamp()
    {
        lampEnergy-=2;
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}
