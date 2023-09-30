using UnityEngine;

public class TrainWagon : MonoBehaviour
{

    public BallCoupling PreviousBallCoupling;

    public BallCoupling OwnFrontBallCoupling;

    public float SPEED = 0.05f; 
    void Start()
    {
        transform.LookAt(PreviousBallCoupling.GetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = PreviousBallCoupling.GetPosition();
        Vector3 lookVector = (targetPosition - OwnFrontBallCoupling.GetPosition()).normalized;
        Quaternion toRotation = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime);
        Vector3 moveVector = PreviousBallCoupling.GetPosition() - OwnFrontBallCoupling.GetPosition();
        transform.position += moveVector;
    }
}
