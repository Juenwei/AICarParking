using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinCarController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private float torque = 1.0f;

    [SerializeField]
    private float minSpeedBeforeTorque = 0.3f;

    [SerializeField]
    private float minSpeedBeforeIdle = 0.2f;

    [SerializeField]
    private Animator carAnimator;

    public Direction CurrentDirection { get; set; } = Direction.Idle;

    public bool IsAutonomous { get; set; } = false;

    private Rigidbody carRigidBody;

    public float throttle = 0.0f, reverse = 0.0f, torqueValue = 0.0f;

    public enum Direction
    {
        Idle,
        MoveForward,
        MoveBackward,
        TurnLeft,
        TurnRight
    }

    void Awake()
    {
        carRigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (carRigidBody.velocity.magnitude <= minSpeedBeforeIdle)
        {
            CurrentDirection = Direction.Idle;
            ApplyAnimatorState(Direction.Idle);
        }
    }

    void FixedUpdate() => ApplyMovement();

    public void ApplyMovement()
    {
        throttle = 0;
        reverse = 0;
        torqueValue = 0;

        if (Input.GetKey(KeyCode.UpArrow) || (CurrentDirection == Direction.MoveForward && IsAutonomous))
        {
            ApplyAnimatorState(Direction.MoveForward);
            Vector3 tempThrottle = transform.forward * speed * Time.deltaTime;
            throttle = tempThrottle.magnitude;
            carRigidBody.AddForce(tempThrottle, ForceMode.VelocityChange);
        }

        if (Input.GetKey(KeyCode.DownArrow) || (CurrentDirection == Direction.MoveBackward && IsAutonomous))
        {
            ApplyAnimatorState(Direction.MoveBackward);
            Vector3 tempReverse = -transform.forward * speed * Time.deltaTime;
            reverse = tempReverse.magnitude;
            carRigidBody.AddForce(tempReverse, ForceMode.VelocityChange);
        }

        if ((Input.GetKey(KeyCode.LeftArrow) && canApplyTorque()) || (CurrentDirection == Direction.TurnLeft && IsAutonomous))
        {
            ApplyAnimatorState(Direction.TurnLeft);
            Vector3 tempTorque = transform.up * -torque * Time.deltaTime;
            torqueValue = -tempTorque.magnitude;
            carRigidBody.AddTorque(tempTorque);
        }

        if (Input.GetKey(KeyCode.RightArrow) && canApplyTorque() || (CurrentDirection == Direction.TurnRight && IsAutonomous))
        {
            ApplyAnimatorState(Direction.TurnRight);
            Vector3 tempTorque = transform.up * torque * Time.deltaTime;
            torqueValue = tempTorque.magnitude;
            carRigidBody.AddTorque(tempTorque);
        }
    }

    void ApplyAnimatorState(Direction direction)
    {
        carAnimator.SetBool(direction.ToString(), true);

        switch (direction)
        {
            case Direction.Idle:
                carAnimator.SetBool(Direction.MoveBackward.ToString(), false);
                carAnimator.SetBool(Direction.MoveForward.ToString(), false);
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
                break;
            case Direction.MoveForward:
                carAnimator.SetBool(Direction.Idle.ToString(), false);
                carAnimator.SetBool(Direction.MoveBackward.ToString(), false);
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
                break;
            case Direction.MoveBackward:
                carAnimator.SetBool(Direction.Idle.ToString(), false);
                carAnimator.SetBool(Direction.MoveForward.ToString(), false);
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
                break;
            case Direction.TurnLeft:
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
                break;
            case Direction.TurnRight:
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
                break;
        }
    }

    public bool canApplyTorque()
    {
        Vector3 velocity = carRigidBody.velocity;
        return Mathf.Abs(velocity.x) >= speed * minSpeedBeforeTorque || Mathf.Abs(velocity.z) >= speed * minSpeedBeforeTorque;
    }
}
