using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;

//Only One Car For One Brain
using static ReinCarController;

public class CarParkingAgent : Agent
{
    //Postion Related
    [SerializeField] private Transform initialTransformForReset;
    [SerializeField] private Transform goalTransform;

    //Car Controller Related
    [SerializeField] private ReinCarController controller;
    [SerializeField] private Rigidbody carRigidbody;

    //Agent Class Related
    private BehaviorParameters behaviorParameters;

    //Others
    [SerializeField] private MeshRenderer groundMesh;
    [SerializeField] private Material winMaterial ,loseMaterial ,defaultMaterial;
    [SerializeField] LearningStageManager currentLearningStage;
    private ParkingSlotManager parkingManager;

    //same as Start()
    public override void Initialize()
    {
        behaviorParameters = GetComponent<BehaviorParameters>();
        controller.IsAutonomous = behaviorParameters.BehaviorType == BehaviorType.Default;
        currentLearningStage = gameObject.transform.parent.root.GetComponent<LearningStageManager>();
        parkingManager = currentLearningStage.currentParkingManager;
    }

    //RESET everthing here
    public override void OnEpisodeBegin()
    {
        transform.localPosition = initialTransformForReset.localPosition;
        transform.localRotation = Quaternion.identity;

        carRigidbody.velocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;

        parkingManager.DivideTheCarSlot();
        goalTransform = parkingManager.GetCurrentGoal().transform;
        currentLearningStage.ReportCollisionState(false);
    }

    //NN Input
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.rotation);

        sensor.AddObservation(goalTransform.position);
        sensor.AddObservation(goalTransform.rotation);

        sensor.AddObservation(carRigidbody.velocity);
    }

    //NN Output
    public override void OnActionReceived(float[] vectorAction)
    {
        var direction = Mathf.FloorToInt(vectorAction[0]);
        switch (direction)
        {
            case 0: // idle
                controller.CurrentDirection = Direction.Idle;
                break;
            case 1: // forward
                controller.CurrentDirection = Direction.MoveForward;
                break;
            case 2: // backward
                controller.CurrentDirection = Direction.MoveBackward;
                break;
            case 3: // turn left
                controller.CurrentDirection = Direction.TurnLeft;
                break;
            case 4: // turn right
                controller.CurrentDirection = Direction.TurnRight;
                break;
        }
        //Continuosly give Punishment to Machine to prevent agent do nothing
        AddReward(-1f / MaxStep);
    }

    //Only for get called by other object
    public void completeTask(float amount = 1.0f, bool isFinal = false)
    {
        AddReward(amount);
        currentLearningStage.changeCumulativeReward(amount);
        if (isFinal)
        {
            //Change Ground Material
            currentLearningStage.CompleteStage(true);
            StartCoroutine(SwapGroundMaterial(winMaterial, 0.5f));
            EndEpisode();
        }
    }

    public void failedTask(float amount = -0.5f, bool endEpisode = false)
    {
        //Change Ground Material
        if (amount > 0)
            amount *= -1;
        AddReward(amount);
        currentLearningStage.changeCumulativeReward(amount);

        if (endEpisode)
        {
            currentLearningStage.CompleteStage(false);
            EndEpisode();
            StartCoroutine(SwapGroundMaterial(loseMaterial, 0.5f));
        }
    }

    //Coroutine for swaping the environemnt material
    protected IEnumerator SwapGroundMaterial(Material mat, float time)
    {
        groundMesh.material = mat;
        yield return new WaitForSeconds(time);
        groundMesh.material = defaultMaterial;
    }


    //Manual Control
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

        if (Input.GetKey(KeyCode.LeftArrow) && controller.canApplyTorque())
        {
            actionsOut[0] = 3;
        }

        if (Input.GetKey(KeyCode.RightArrow) && controller.canApplyTorque())
        {
            actionsOut[0] = 4;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Obstacle>() != null)
        {
            currentLearningStage.ReportCollisionState(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        currentLearningStage.ReportCollisionState(false);
    }
}
