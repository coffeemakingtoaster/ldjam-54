using System;
using UnityEngine;

public class TrainWagon : MonoBehaviour
{

    public BallCoupling PreviousBallCoupling;

    public BallCoupling OwnFrontBallCoupling;

    public GameObject payload;

    private Vector3 previousPosition;

    public float SPEED = 0.05f;
    void Start()
    {
        if (PreviousBallCoupling != null)
        {
            transform.LookAt(PreviousBallCoupling.GetPosition());
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition;
        try
        {
            targetPosition = PreviousBallCoupling.GetPosition();
        }
        catch (Exception _)
        {
            // This occurs when either: The ball coupling has not been set yet OR the ball coupling has been deleted
            // previous position is not null => ball coupling existed before
            if (!previousPosition.Equals(new Vector3()))
            {
                Destroy(this.gameObject, 2);
            }
            return;
        }

        if (targetPosition == previousPosition)
        {
            return;
        }
        Vector3 lookVector = (targetPosition - OwnFrontBallCoupling.GetPosition()).normalized;
        Quaternion toRotation = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime);
        Vector3 moveVector = PreviousBallCoupling.GetPosition() - OwnFrontBallCoupling.GetPosition();
        transform.position += moveVector;
        previousPosition = targetPosition;
    }

    public GameObject TryToRetrievePayload()
    {
        if (this.payload != null)
        {
            GameObject outgoingCargo = this.payload;
            this.payload = null;
            return outgoingCargo;
        }
        return null;
    }

    public bool TryToAddPayload(GameObject payload)
    {
        if (this.payload == null)
        {
            this.payload = payload;
        }
        return false;
    }

    void OnTriggerEnter(Collider collision)
    {
                if (collision.gameObject.Equals(this)){
            return;
        }
        TrainWagon trainWagon = collision.gameObject.GetComponent<TrainWagon>();
        TrainLocomotive trainLocomotive = collision.gameObject.GetComponent<TrainLocomotive>();
        if (trainWagon != null || trainLocomotive != null)
        {
            Destroy(this.gameObject);
        }
    }
}
