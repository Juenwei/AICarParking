using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OutputManager : MonoBehaviour
{
    public static OutputManager instance;

    public float cumulativeReward = 0.0f;
    public float noOfEpisode = 0.0f;
    public float noSuccess = 0.0f;
    public float noSuccessRate = 0.0f;

    [SerializeField] Text cumulativeRewardTxt;
    [SerializeField] Text noOfEpisodeTxt;
    [SerializeField] Text noSuccessTxt;
    [SerializeField] Text noSuccessRateTxt;
    // Start is called before the first frame update

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        noOfEpisodeTxt.text = noOfEpisode.ToString("0");
        if (noSuccess >= 1)
        {
            noSuccessRateTxt.text = noSuccessRate.ToString("0.00");
        }
        noSuccessTxt.text = noSuccess.ToString();
        cumulativeRewardTxt.text = cumulativeReward.ToString("0.00");


    }
}
