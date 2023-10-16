using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsMovment : MonoBehaviour
{
    public static PhysicsMovment instance;


    public WheelCollider[] wheels = new WheelCollider[4];
    public Transform[] tires= new Transform[4];
    public float power = 100f;
    public float rot = 45f;
    public float downForceValue = 50f;
    public float targetSpeed = 1f;
    Rigidbody rb;
    private float standardAngle = 0f;
    private float standardDistanceR = 400f;
    private float linecoordinate = 0f;
    private GameManager gm;
    public GameObject robot;
    public float followAngle = 80;
    public bool framecounter  = false;
    private bool notuTurn = true;
    private float[] distanceArray = new float[2];
    private float distanceDifferenceL = 0;
    private float distanceDifferenceR = 0;
    private void Awake()
    {
        instance = this;
        gm = GameManager.instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1, 0);
    }

    private void FixedUpdate()
    {
        GameManager.Situation situation = gm.ReturnSituation();
        if (situation == GameManager.Situation.BeforeEntering/* && gm.streamStart()*/)
        {
            gostrait();
            if (gm.returnFrameCounter() > 2)
            {
                constantVelocityMovment();
            }
            Debug.Log("FrameCounter : " + gm.returnFrameCounter());
        }
        else if (situation == GameManager.Situation.Painting || situation == GameManager.Situation.Passing)
        {
            linecoordinate = gm.returnCordinate();
            float distancenow = gm.returnDistance();
            if (linecoordinate > 300)
            {
                distanceDifferenceR = standardDistanceR - distancenow;
            }
            else
            {
                distanceDifferenceL = -1215 + standardDistanceR + distancenow;
                
            }
            
            if (Mathf.Abs(distanceDifferenceL) > 10 && Mathf.Abs(distanceDifferenceR) > 10)
            {
               followAngle = 80 * -1 * distanceDifferenceR/Mathf.Abs(distanceDifferenceR);
                Debug.Log("R : " + distanceDifferenceR);
                Debug.Log("L : " + distanceDifferenceL);
            }
            else
            {
                followAngle = 90;
            }

            if (followAngle != 90)
            {
                lateralMovement();
                //framecounter = 300 > gm.returnDistance(); // 테스트용 잘 됨
            }
            else
            {
                gostrait();
            }
            constantVelocityMovment();
        }
        else if (situation == GameManager.Situation.ReEntry || !notuTurn)
        {
            uTurn();
            if (standardDistanceR == 400)
            {
                float temp = distanceDifferenceL;
                distanceDifferenceL = distanceDifferenceR;
                distanceDifferenceR = temp;
                Debug.Log("실행됨을 알림");
            }
            standardDistanceR = 500;
        }
        else
        {
            targetSpeed = 0f;
            constantVelocityMovment();
        }
    }
    void gostrait()
    {
        standardAngle = gm.returnAngle();
        for (int i = 0; i < 2; i++)
        {
            //wheels[i].steerAngle = Input.GetAxis("Horizontal") * rot;
            if (Mathf.Abs(standardAngle) < 89.5)
            {
                if (standardAngle < 0)
                {
                    wheels[i].steerAngle = (90 + standardAngle) * -1;
                }
                else
                {
                    wheels[i].steerAngle = (90 - standardAngle);
                }
                //Debug.Log(i + "th wheels steerAngle = " + wheels[i].steerAngle);
            }
            else
            {
                wheels[i].steerAngle = 0; 
            }
        }
    }
    void constantVelocityMovment()
    {
       
        rb.AddForce(-transform.up * downForceValue * rb.velocity.magnitude);

        float currentSpeed = 0;
        foreach (WheelCollider wheel in wheels)
        {
            currentSpeed += wheel.rpm * wheel.radius * 2 * Mathf.PI / 60.0f;
        }
        currentSpeed /= wheels.Length;

        float speedError = targetSpeed - currentSpeed;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].motorTorque += speedError * Mathf.Abs(speedError) * power / wheels.Length;
        }
        //Debug.Log("curretn motorTorque : " + wheels[0].motorTorque);
        //Debug.Log("currentSpeed : " + currentSpeed);
        //Debug.Log("speedError: " + speedError);
    }
    void uTurn()
    {
        Debug.Log(robot.transform.eulerAngles);
        if (robot.transform.eulerAngles.y < 179 || robot.transform.eulerAngles.y > 181 )
        {
            notuTurn = false;
            for (int i = 0; i < 2; i++)
            {
                wheels[i].steerAngle = 45f;
            }
        }
        else
        {
            notuTurn = true;
            for (int i = 0; i < 2; i++)
            {
                wheels[i].steerAngle = 0;
            }
        }
        float currentSpeed = 0;
        foreach (WheelCollider wheel in wheels)
        {
            currentSpeed += wheel.rpm * wheel.radius * 2 * Mathf.PI / 60.0f;
        }
        currentSpeed /= wheels.Length;

        float speedError = targetSpeed - currentSpeed;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].motorTorque += speedError * Mathf.Abs(speedError) * power / wheels.Length;
        }
    }
    void lateralMovement() {
        standardAngle = gm.returnAngle();
        for (int i =0; i < 2; i++)
        {
            if (Mathf.Abs(Mathf.Abs(standardAngle) - Mathf.Abs(followAngle)) > 0.5)
            {
                if (followAngle > 0)
                {
                    if (standardAngle > 0)
                    {
                        wheels[i].steerAngle = followAngle - standardAngle;
                    }
                    else
                    {
                        wheels[i].steerAngle = -180 + followAngle - standardAngle;
                    }
                }
                else
                {
                    if (standardAngle < 0)
                    {
                        wheels[i].steerAngle = followAngle - standardAngle;
                    }
                    else
                    {
                        wheels[i].steerAngle = 180 + followAngle - standardAngle;
                    }
                }
            }
            else
            {
                wheels[i].steerAngle = 0;
            }
        }
    }
    public bool returnuTurn() => notuTurn;
    // Update is called once per frame
    void Update()
    {
        UpdateMeshesPostion();
    }
    
    void UpdateMeshesPostion()
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 pos;
            wheels[i].GetWorldPose(out pos, out quat);
            tires[i].position = pos;
            tires[i].rotation = quat;
        }
    }
}
