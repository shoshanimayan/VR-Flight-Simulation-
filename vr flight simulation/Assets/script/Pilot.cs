using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilot : MonoBehaviour {
    [SerializeField]
    float eulerAngx;
    [SerializeField]
    float eulerAngy;
    [SerializeField]
    float eulerAngz;
    //barrel and ammo
    public GameObject barrel;
    public GameObject Ammo;
    //bools
    private bool turnLeft = false;
    private bool turnRight = false;
    private bool dive = false;
    private bool climb = false;
    private bool fire = false;
    private bool boost = false;

    //speed
    public float speed = 40f;
    //trun speed
    public float turnSpeed = .4f;
    //time for gun
    private float nextFire = 0f;
    private float fireDelay = .1f;

    //shoot power
    float shootPower = 1000f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetAngles();
        GetTurn();
        Turn();
        Shoot();
        ForwardMovement();
    }


    private void GetTurn()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.LTouch))
        {
            turnLeft = true;
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.LTouch))
        {
            turnLeft = false;
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.LTouch))
        {
            turnRight = true;
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.LTouch))
        {
            turnRight = false;
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.LTouch))
        {
            dive = true;
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.LTouch))
        {
            dive = false;
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.LTouch))
        {
            climb = true;
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.LTouch))
        {
            climb = false;
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            boost = true;
            Debug.Log("boost");

        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
       
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            boost = false;
        }
        
    }
        private void ForwardMovement() {
        if (boost) { speed = 50; }
        else
            speed = 40;

        speed += transform.forward.y * Time.deltaTime * 50f;
        if (speed < 10)
            speed = 10;
        transform.position += transform.forward * Time.deltaTime * speed;

    }
    private void GetAngles() {

        eulerAngx = transform.localEulerAngles.x;
        eulerAngy = transform.localEulerAngles.y;
        eulerAngz = transform.localEulerAngles.z;

    }

    private void Shoot() {
        if (fire && nextFire < Time.time) {
            var fired = Instantiate(Ammo, barrel.transform);
            fired.GetComponent<Rigidbody>().velocity= new Vector3(transform.forward.x, transform.forward.y, transform.forward.z + shootPower);
            Destroy(fired, 5f);
            nextFire =Time.time + fireDelay;
        
        }
    }
    private void Turn() {
        if (turnLeft) {
            eulerAngz = eulerAngz + turnSpeed;
            transform.eulerAngles = new Vector3(eulerAngx, eulerAngy, eulerAngz);  
        }
        if (turnRight) {
            eulerAngz = eulerAngz - turnSpeed;
            transform.eulerAngles = new Vector3(eulerAngx, eulerAngy, eulerAngz);
        }
        if (dive){
            eulerAngx = eulerAngx + turnSpeed;
            transform.eulerAngles = new Vector3(eulerAngx, eulerAngy, eulerAngz);

        }
        if (climb) {
            bool sharpTurnNoLiftLeft = eulerAngz < 92 && eulerAngz > 69;
            bool sharpTurnLittleLiftLeft = eulerAngz < 69 && eulerAngz > 49;
            bool mildTurnAndLiftLeft = eulerAngz < 49 && eulerAngz > 29;
            bool sharpTurnNoLiftRight = eulerAngz > 269 && eulerAngz < 292;
            bool sharpTurnLittleLiftRight = eulerAngz > 292 && eulerAngz < 312;
            bool mildTurnAndLiftRight = eulerAngz > 312 && eulerAngz < 332;

            if (sharpTurnNoLiftLeft)
                eulerAngy -= turnSpeed * Time.deltaTime * 100f;
            else if (sharpTurnLittleLiftLeft)
            {
                eulerAngy -= turnSpeed * Time.deltaTime * 85f;
                eulerAngx -= turnSpeed * Time.deltaTime * 20f;
            }
            else if (mildTurnAndLiftLeft)
            {
                eulerAngy -= turnSpeed * Time.deltaTime * 40f;
                eulerAngx -= turnSpeed * Time.deltaTime * 60f;
            }
            else if (sharpTurnNoLiftRight)
                eulerAngy += turnSpeed * Time.deltaTime * 100f;
            else if (sharpTurnLittleLiftRight)
            {
                eulerAngy += turnSpeed * Time.deltaTime * 85f;
                eulerAngx -= turnSpeed * Time.deltaTime * 20f;
            }
            else if (mildTurnAndLiftRight)
            {
                eulerAngy += turnSpeed * Time.deltaTime * 40f;
                eulerAngx -= turnSpeed * Time.deltaTime * 60f;
            }
            else { eulerAngx -= turnSpeed; }
            transform.eulerAngles = new Vector3(eulerAngx, eulerAngy, eulerAngz);


        }

    }
}
