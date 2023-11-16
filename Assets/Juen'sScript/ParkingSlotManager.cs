using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingSlotManager : MonoBehaviour
{
    private List<Vector3> originalPosition = new List<Vector3>();
    private List<Vector3> originalRotation = new List<Vector3>();
    [SerializeField] private List<GameObject> parkingSlotObject;
    [SerializeField] private GameObject goalPrefabs;
    [SerializeField] private int numOfToHide = 1;
    [SerializeField] private Vector3 presetRotation;

    private GameObject currentGoal;
    void Awake()
    {
        //Finallize Original position
        foreach(GameObject car in parkingSlotObject)
        {
            originalPosition.Add(car.transform.localPosition);
            originalRotation.Add(car.transform.eulerAngles);
        }
        currentGoal = Instantiate(goalPrefabs,transform);
        currentGoal.SetActive(false);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            DivideTheCarSlot();
        }
    }

    public void DivideTheCarSlot()
    {
        //Reset all of the car
        for (int i= 0;i < parkingSlotObject.Count;i++)
        {
            ResetParkingSlot(parkingSlotObject[i], originalPosition[i],originalRotation[i]);
            currentGoal.transform.localPosition = Vector3.zero;
            currentGoal.transform.localEulerAngles = Vector3.zero;
        }

        //Assign Random car to hide
            int currentNumOfHideCar = 0;
        List<Transform> tempHideCarPosList = new List<Transform>();
        while(currentNumOfHideCar < numOfToHide)
        { 
            int tempRandomNumber = Random.Range(0, parkingSlotObject.Count-1);
            if (parkingSlotObject[tempRandomNumber].activeSelf == true)
            {
                tempHideCarPosList.Add(parkingSlotObject[tempRandomNumber].transform);
                parkingSlotObject[tempRandomNumber].SetActive(false);
                currentNumOfHideCar++;
            }
        }

        //Assign Car Goal to the empty slot
        if(currentGoal.activeSelf == false)
        {
            currentGoal.SetActive(true);
        }

        int tempIndexForGoal = 0;
        if (tempHideCarPosList.Count > 0)
        {
            tempIndexForGoal = Random.Range(0, tempHideCarPosList.Count - 1);
        }
        currentGoal.transform.localPosition = tempHideCarPosList[tempIndexForGoal].localPosition;
        currentGoal.transform.localEulerAngles = tempHideCarPosList[tempIndexForGoal].localEulerAngles;
    }

    void ResetParkingSlot(GameObject parkingObj ,Vector3 targetPosition ,Vector3 targetRotation)
    {
        //Reset Active status
        if (parkingObj.activeSelf == false)
            parkingObj.SetActive(true);
        //Reset Transform
        parkingObj.transform.localPosition = targetPosition;
        parkingObj.transform.localRotation = Quaternion.Euler(0,targetRotation.y,0);
        //Clear Velocity
        Rigidbody rigidbody = parkingObj.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;

        //Reset CarGoal
        currentGoal.GetComponent<GoalSlot>().HasCarUsedIt = false;
    }

    public GameObject GetCurrentGoal()
    {
        return currentGoal;
    }
}
