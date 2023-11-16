using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoal : Agent
{

    [SerializeField] private Transform targetTransform ,initialPosition;
    [SerializeField] float moveSpeed = 1f;

    [SerializeField] Material winMaterial, loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    public override void OnEpisodeBegin()
    {
        //Fixed Initial Point
        //transform.localPosition = initialPosition.localPosition;

        //Random Initial Poit
        transform.localPosition = new Vector3(Random.Range(-6, 6), 0, Random.Range(-4, 4));
        targetTransform.localPosition = new Vector3(Random.Range(-3, 3), 0, Random.Range(-2, 2));

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0 , moveZ) * moveSpeed * Time.deltaTime;
    }

    //Testing
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuosActions = actionsOut.ContinuousActions;
        continuosActions[0] = Input.GetAxis("Horizontal");
        continuosActions[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.TryGetComponent<Goal>(out Goal goal))
        //{
        //    SetReward(+1f);
        //    floorMeshRenderer.material = winMaterial;
        //    EndEpisode();
        //}
        //if (other.TryGetComponent<Wall>(out Wall wall))
        //{
        //    SetReward(-1f);
        //    floorMeshRenderer.material = loseMaterial;
        //    EndEpisode();
        //}
    }
}
