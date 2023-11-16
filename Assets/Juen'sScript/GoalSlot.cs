using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSlot : MonoBehaviour
{
    public enum GoalType
    {
        Milestone,
        FinalDestination
    }

    [SerializeField] LearningStageManager currentLearningStage; 

    private CarParkingAgent agent = null;

    [SerializeField]
    private float goalReward;

    [SerializeField]
    private bool enforceGoalMinRotation = false;

    [SerializeField]
    private float goalMinRotation = 10.0f;

    // to avoid AI from cheating ;)
    public bool HasCarUsedIt { get; set; } = false;

    [SerializeField]private GoalType goalType = GoalType.Milestone;

    private void Start()
    {
        currentLearningStage = gameObject.transform.parent.root.GetComponent<LearningStageManager>();
        currentLearningStage.currentGoalSlot = this;
        agent = currentLearningStage.currentAgent;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag.ToLower() == "player" && !HasCarUsedIt)
        {
            if (goalType == GoalType.Milestone)
            {
                HasCarUsedIt = true;
                agent.completeTask(goalReward);
                //Debug.Log("Hit MileStone");
            }
            else
            {
                agent.completeTask(goalReward, true);

                // Advance this will ensure the car tries to align when parking
                //if (Mathf.Abs(agent.transform.rotation.y) <= goalMinRotation || !enforceGoalMinRotation)
                //{
                //    HasCarUsedIt = true;
                //    agent.GivePoints(goalReward, true);
                //}
                //else
                //{
                //    agent.TakeAwayPoints();
                //}
            }

        }
    }
}
