using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCase : MonoBehaviour
{
    [SerializeField] private Text CRewardText;
    [SerializeField] private Text noOfEpisodeText;
    [SerializeField] private Text successRateText;
    [SerializeField] private Text successText;
    [SerializeField] private Text throttleText;
    [SerializeField] private Text reverseText;
    [SerializeField] private Text torqueText;
    [SerializeField] private Image collideImage;
    [SerializeField] private LearningStageManager stage1,stage2;

    string crText = "Cumulative Reward : ", noOfEpText = "No of Episode : ", succRateText = "Success Rate : ", 
        succCount = "Success : " ,thrText = "Throttle :  " ,revText= "Reverse : " ,torText = "Torque : ";

    private Camera mainCamera;
    private bool isLeft = true;
    private Animator camAnimator;
    private string goRightId = "GoRight";

    void Start()
    {
        mainCamera = Camera.main;
        camAnimator = mainCamera.GetComponent<Animator>();
    }

    
    void Update()
    {
        if(isLeft)
        {
            CRewardText.text = crText + stage1.GetCumulativeReward();
            noOfEpisodeText.text = noOfEpText + stage1.GetEpisodeCount();
            successRateText.text = succRateText + stage1.GetSuccessRate();
            successText.text = succCount + stage1.GetSuccessCount();
            throttleText.text = thrText + stage1.GetCurrentThrottle().ToString("0.00");
            reverseText.text = revText + stage1.GetCurrentReverse().ToString("0.00");
            torqueText.text = torText + stage1.GetCurrentTorque().ToString("0.00");
            if (stage1.GetCollisionStatus())
                collideImage.color = Color.red;
            else
                collideImage.color = Color.white;
        }
        else
        {
            CRewardText.text = crText + stage2.GetCumulativeReward();
            noOfEpisodeText.text = noOfEpText + stage2.GetEpisodeCount();
            successRateText.text = succRateText + stage2.GetSuccessRate();
            successText.text = succCount + stage2.GetSuccessCount();
            throttleText.text = thrText + stage2.GetCurrentThrottle().ToString(".00");
            reverseText.text = revText + stage2.GetCurrentReverse().ToString(".00");
            torqueText.text = torText + stage2.GetCurrentTorque().ToString(".00");
            if (stage2.GetCollisionStatus())
                collideImage.color = Color.red;
            else
                collideImage.color = Color.white;
        }
    }

    public void SwitchCarpark()
    {
        if (!camAnimator.GetBool(goRightId))
        {
            isLeft = false;
            camAnimator.SetBool(goRightId, true);
        }
        else
        {
            isLeft = true;
            camAnimator.SetBool(goRightId, false);
        }
    }
}
