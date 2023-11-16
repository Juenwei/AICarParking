using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum CarObstacleType
    {
        Barrier,
        Environment,
        Car,
        Ground
    }

    [SerializeField] LearningStageManager currentLearningStage;

    [SerializeField] private CarObstacleType carObstacleType;

    public CarObstacleType CarObstacleTypeValue { get { return this.carObstacleType; } }

    private CarParkingAgent agent;

    private void Start()
    {
        currentLearningStage = gameObject.transform.parent.root.GetComponent<LearningStageManager>();
        agent = currentLearningStage.currentAgent;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.ToLower() == "player")
        {
            if(carObstacleType == CarObstacleType.Car)
                agent.failedTask(-1f ,true);
            else
                agent.failedTask(-0.1f);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag.ToLower() == "player")
        {
            if (carObstacleType == CarObstacleType.Car)
            {
                agent.failedTask(-1f, true);
            }
            else
            {
                agent.failedTask(-0.1f);
                //Debug.Log("Collide with Barrier");
            }
        }
    }
}
