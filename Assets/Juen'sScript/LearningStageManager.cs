using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningStageManager : MonoBehaviour
{
    // Start is called before the first frame update
    public CarParkingAgent currentAgent;
    public ReinCarController currentController;
    public GoalSlot currentGoalSlot;
    public ParkingSlotManager currentParkingManager;
    //public Transform currentCarTransform;

    private float cumulativeReward { get; set; } = 0.0f;
    private int  epCount { get; set; } = 0;
    private float successRate { get; set; } = 100.0f;
    private int successCount { get; set; } = 0;
    private bool isCarCollided { get; set; } = false;

    public void CompleteStage(bool success = false)
    {
        epCount += 1;
        if(success)
        {
            successCount += 1;
        }
        successRate = (float)successCount / epCount * 100.0f;
    }

    public void changeCumulativeReward(float amount)
    {
        cumulativeReward += amount;
    }

    public int GetEpisodeCount()
    {
        return epCount;
    }

    public float GetCumulativeReward()
    {
        return cumulativeReward;
    }

    public int GetSuccessCount()
    {
        return successCount;
    }

    public float GetSuccessRate()
    {
        return successRate;
    }

    public bool GetCollisionStatus()
    {
        return isCarCollided;
    }

    public void ReportCollisionState(bool stateMsg)
    {
        isCarCollided = stateMsg;
    }

    public float GetCurrentThrottle()
    {
        return currentController.throttle;
    }

    public float GetCurrentReverse()
    {
        return currentController.reverse;
    }
    
    public float GetCurrentTorque()
    {
        return currentController.torqueValue;
    }
}
