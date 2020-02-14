﻿// ----------- CAR TUTORIAL SAMPLE PROJECT, ? Andrew Gotow 2009 -----------------

// Here's the basic AI driven car script described in my tutorial at www.gotow.net/andrew/blog.
// A Complete explaination of how this script works can be found at the link above, along
// with detailed instructions on how to write one of your own, and tips on what values to 
// assign to the script variables for it to work well for your application.

// Contact me at Maxwelldoggums@Gmail.com for more information.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICar_Script : MonoBehaviour {

    public bool _aiOnPlayersCar;

    // These variables allow the script to power the wheels of the car.
    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;

    // These variables are for the gears, the array is the list of ratios. The script
    // uses the defined gear ratios to determine how much torque to apply to the wheels.
    public float[] GearRatio;
    public int CurrentGear = 0;

    // These variables are just for applying torque to the wheels and shifting gears.
    // using the defined Max and Min Engine RPM, the script can determine what gear the
    // car needs to be in.
    public float EngineTorque = 400.0f;
    public float MaxEngineRPM = 2000.0f;
    public float MinEngineRPM = 500.0f;
    private float EngineRPM = 0.0f;

    // Here's all the variables for the AI, the waypoints are determined in the "GetWaypoints" function.
    // the waypoint container is used to search for all the waypoints in the scene, and the current
    // waypoint is used to determine which waypoint in the array the car is aiming for.
    public GameObject waypointContainer;
    private List<Transform> waypoints;
    private int currentWaypoint = 0;

    // input steer and input torque are the values substituted out for the player input. The 
    // "NavigateTowardsWaypoint" function determines values to use for these variables to move the car
    // in the desired direction.
    private float inputSteer = 0.0f;
    private float inputTorque = 0.0f;

    private Rigidbody rigidBody;
    private AudioSource audioSource;



    /* ATTRIBUTES TO HANDLE AI ON PLAYER'S CAR */
    private PlayerCar_Script _playerCarScript;
    private bool checkForLastPointDist;
    private bool checkForAccidentSite;

    private bool checkForOverTakeSite;
    public float speed;
    public TextMesh speedText;
    public TextMesh dashMessages;


    private ActivateAiOnMyCar _aiEnabledOrNot;

    public GameObject steeringWheel;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();


        if (_aiOnPlayersCar)
        {
            _aiEnabledOrNot = FindObjectOfType<ActivateAiOnMyCar>();
            _playerCarScript = GetComponent<PlayerCar_Script>();
            checkForLastPointDist = true;
            checkForAccidentSite = true;

        }
        else
        {
            checkForLastPointDist = false;
            checkForAccidentSite = false;
        }

        // I usually alter the center of mass to make the car more stable. I'ts less likely to flip this way.
        rigidBody.centerOfMass = new Vector3(GetComponent<Rigidbody>().centerOfMass.x, -1.5f, rigidBody.centerOfMass.z);
        // Call the function to determine the array of waypoints. This sets up the array of points by finding
        // transform components inside of a source container.

        // Needed for better RPM readings for heavy vehciles
        FrontLeftWheel.ConfigureVehicleSubsteps(5f, 10, 10);
        GetWaypoints();
    }

    void FixedUpdate()
    {
        // This is to limith the maximum speed of the car, adjusting the drag probably isn't the best way of doing it,
        // but it's easy, and it doesn't interfere with the physics processing.
        rigidBody.drag = rigidBody.velocity.magnitude / 500;
        speed = rigidBody.velocity.magnitude;
        speedText.text = "" + System.Math.Round(speed * 2, 0);
        // dashMessages.text = "Self-driving";
        dashMessages.color = Color.green;

        // Call the funtion to determine the desired input values for the car. This essentially steers and
        // applies gas to the engine.
        NavigateTowardsWaypoint();

        // Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
        EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm) / 2 * GearRatio[CurrentGear];
        ShiftGears();

        // set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
        // up to twice it's pitch, where it will suddenly drop when it switches gears.

        audioSource.pitch = Mathf.Clamp(Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0f, 0f, 2.0f);

        // finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
        // multiplied by the calculated AI input variable.
        FrontLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * inputTorque;
        FrontRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * inputTorque;

        // the steer angle is an arbitrary value multiplied by the calculated AI input.
        FrontLeftWheel.steerAngle = 10 * inputSteer;
        FrontRightWheel.steerAngle = 10 * inputSteer;





        /********************************** HASHIM009 **************************************************/
        // From this point onwards, i wrote these LOC
        steeringWheel.transform.localEulerAngles = new Vector3(steeringWheel.transform.localEulerAngles.x, steeringWheel.transform.localEulerAngles.y, FrontRightWheel.steerAngle);

        /* 5 SECS BEFORE LAST POINT */
        if (_aiOnPlayersCar && checkForLastPointDist && _aiEnabledOrNot.activateAI)
        {
            float _distanceToLastWaypoint = Vector3.Distance(waypoints[waypoints.Count - 1].position, _playerCarScript.transform.position);
            if (_playerCarScript.GetCarSpeed() > 0)
            {
                float _time = _distanceToLastWaypoint / _playerCarScript.GetCarSpeed();
                if (_time <= 5.0f)
                {
                    checkForLastPointDist = false;
                    _playerCarScript.ShowReachingLastPointMessageOnDash();
                }
            }
        }

        /* 5 SECS BEFORE ACCIDENT SITE */
        if (_aiOnPlayersCar && checkForAccidentSite && _aiEnabledOrNot.activateAI)
        {
            float _distanceToAccidentSite = Vector3.Distance(_playerCarScript.accidentCar.position, _playerCarScript.transform.position);
            if (_playerCarScript.GetCarSpeed() > 0)
            {
                float _time = _distanceToAccidentSite / _playerCarScript.GetCarSpeed();
                if (_time <= 5.0f)
                {
                    checkForAccidentSite = false;
                    _playerCarScript.ShowMessageOnReachingAccidentSite();
                }
            }
        }
        /************************************************************************************/
    }


    void ShiftGears() {
        // this funciton shifts the gears of the vehcile, it loops through all the gears, checking which will make
        // the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
        int AppropriateGear = CurrentGear;

        if (EngineRPM >= MaxEngineRPM) {
            for (int i = 0; i < GearRatio.Length; i++) {
                if (FrontLeftWheel.rpm * GearRatio[i] < MaxEngineRPM) {
                    AppropriateGear = i;
                    break;
                }
            }

            CurrentGear = AppropriateGear;
        }

        if (EngineRPM <= MinEngineRPM) {
            AppropriateGear = CurrentGear;

            for (int j = GearRatio.Length - 1; j >= 0; j--) {
                if (FrontLeftWheel.rpm * GearRatio[j] > MinEngineRPM) {
                    AppropriateGear = j;
                    break;
                }
            }

            CurrentGear = AppropriateGear;
        }
    }




    /************************ HASHIM009 **************************/
    void GetWaypoints() {
        // Now, this function basically takes the container object for the waypoints, then finds all of the transforms in it,
        // once it has the transforms, it checks to make sure it's not the container, and adds them to the array of waypoints.
        Transform[] potentialWaypoints = waypointContainer.GetComponentsInChildren<Transform>();
        waypoints = new List<Transform>();

        foreach (Transform potentialWaypoint in potentialWaypoints) {
            if (potentialWaypoint != waypointContainer.transform) {
                waypoints.Add(potentialWaypoint);

            }
        }

        // Find the closest waypoint and start from there if the script is on players car
        List<float> closestWayPointDistance = new List<float>();
        if (_aiOnPlayersCar)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                Vector3 relPoistion = transform.InverseTransformPoint(waypoints[i].position);
                closestWayPointDistance.Add(relPoistion.z);
            }

            float minDis = 1000000.0f;
            for (int i = 0; i < closestWayPointDistance.Count; i++)
            {
                if (closestWayPointDistance[i] > 0 && minDis > closestWayPointDistance[i])
                {
                    minDis = closestWayPointDistance[i];
                    currentWaypoint = i;
                }
            }

            // If closest point in > 50m, disable AI
            if (closestWayPointDistance[currentWaypoint] > 300)
            {
                FindObjectOfType<ActivateAiOnMyCar>().OnClick_ActivateAIOnMyCar();
            }
            Debug.Log("Closest waypoint: " + currentWaypoint);
        }
    }


    /**************************************************************/


    void NavigateTowardsWaypoint() {
        // now we just find the relative position of the waypoint from the car transform,
        // that way we can determine how far to the left and right the waypoint is.
        Vector3 RelativeWaypointPosition = transform.InverseTransformPoint(new Vector3(
                                                                                    waypoints[currentWaypoint].position.x,
                                                                                    transform.position.y,
                                                                                    waypoints[currentWaypoint].position.z));


        // by dividing the horizontal position by the magnitude, we get a decimal percentage of the turn angle that we can use to drive the wheels
        inputSteer = RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude;

        // now we do the same for torque, but make sure that it doesn't apply any engine torque when going around a sharp turn...
        if (Mathf.Abs(inputSteer) < 0.5f) {
            inputTorque = RelativeWaypointPosition.z / RelativeWaypointPosition.magnitude - Mathf.Abs(inputSteer);
        } else {
            inputTorque = 0.0f;
        }

        // this just checks if the car's position is near enough to a waypoint to count as passing it, if it is, then change the target waypoint to the
        // next in the list.
        if (RelativeWaypointPosition.magnitude < 20)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Count)
            {
                currentWaypoint = 0;
            }
        }
    }



    /************************** HASHIM009 *********************************/
    private void OnEnable()
    {
        Debug.Log("Called! on enable");
        GetWaypoints();
    }
    /**********************************************************************/




    
}


