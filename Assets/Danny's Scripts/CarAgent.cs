﻿using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static CarController;

public class CarAgent : BaseAgent
{
    private Vector3 originalPosition;

    private BehaviorParameters behaviorParameters;

    private CarController carController;

    private Rigidbody carControllerRigidBody;

    private CarSpots carSpots;


    public float cumulativeReward = 0.0f;
    public float noOfEpisode = -1.0f;
    public float noSuccess = 0.0f;
    public float noSuccessRate = 0.0f;

    public override void Initialize()
    {
        originalPosition = transform.localPosition;
        behaviorParameters = GetComponent<BehaviorParameters>();
        carController = GetComponent<CarController>();
        carControllerRigidBody = carController.GetComponent<Rigidbody>();
        carSpots = transform.parent.GetComponentInChildren<CarSpots>();
    }

    public override void OnEpisodeBegin()
    {
        ResetParkingLotArea();
        noOfEpisode++;
        OutputManager.instance.noOfEpisode = noOfEpisode;
        noSuccessRate = (noSuccess / noOfEpisode) * 100.0f;
        OutputManager.instance.noSuccessRate = noSuccessRate;

    }

    private void ResetParkingLotArea()
    {
        // important to set car to automonous during default behavior
        carController.IsAutonomous = behaviorParameters.BehaviorType == BehaviorType.Default;
        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.identity;
        carControllerRigidBody.velocity = Vector3.zero;
        carControllerRigidBody.angularVelocity = Vector3.zero;

        // reset which cars show or not show
        carSpots.Setup();

    }

    void Update()
    {
        if (transform.localPosition.y <= 0)
        {
            TakeAwayPoints();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.rotation);

        sensor.AddObservation(carSpots.CarGoal.transform.position);
        sensor.AddObservation(carSpots.CarGoal.transform.rotation);

        sensor.AddObservation(carControllerRigidBody.velocity);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var direction = Mathf.FloorToInt(vectorAction[0]);

        switch (direction)
        {
            case 0: // idle
                carController.CurrentDirection = Direction.Idle;
                break;
            case 1: // forward
                carController.CurrentDirection = Direction.MoveForward;
                break;
            case 2: // backward
                carController.CurrentDirection = Direction.MoveBackward;
                break;
            case 3: // turn left
                carController.CurrentDirection = Direction.TurnLeft;
                break;
            case 4: // turn right
                carController.CurrentDirection = Direction.TurnRight;
                break;
        }

        AddReward(-1f / MaxStep);
    }

    public void GivePoints(float amount = 1.0f, bool isFinal = false)
    {
        AddReward(amount);

        if (isFinal)
        {
            StartCoroutine(SwapGroundMaterial(successMaterial, 0.5f));
            noSuccess++;
            OutputManager.instance.noSuccess = noSuccess;

            cumulativeReward += GetCumulativeReward();
            OutputManager.instance.cumulativeReward = cumulativeReward;

            EndEpisode();
        }
    }

    public void TakeAwayPoints()
    {
        StartCoroutine(SwapGroundMaterial(failureMaterial, 0.5f));

        AddReward(-0.01f);

        cumulativeReward += GetCumulativeReward();
        OutputManager.instance.cumulativeReward = cumulativeReward;


        EndEpisode();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut[0] = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut[0] = 2;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && carController.canApplyTorque())
        {
            actionsOut[0] = 3;
        }

        if (Input.GetKey(KeyCode.RightArrow) && carController.canApplyTorque())
        {
            actionsOut[0] = 4;
        }
    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.tag == "barrier")
    //    {
    //        AddReward(-0.005f);
    //        cumulativeReward += GetCumulativeReward();
    //    }
    //}
}