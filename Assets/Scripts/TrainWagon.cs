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
        transform.LookAt(PreviousBallCoupling.GetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = PreviousBallCoupling.GetPosition();
        if (targetPosition == previousPosition){
            return;
        }
        Vector3 lookVector = (targetPosition - OwnFrontBallCoupling.GetPosition()).normalized;
        Quaternion toRotation = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime);
        Vector3 moveVector = PreviousBallCoupling.GetPosition() - OwnFrontBallCoupling.GetPosition();
        transform.position += moveVector;
        previousPosition = targetPosition;
    }

    public GameObject TryToRetrievePayload(){
        if (this.payload != null){
            GameObject outgoingCargo = this.payload;
            this.payload = null;
            return outgoingCargo;
        }
        return null;
    }

    public bool TryToAddPayload(GameObject payload){
        if (this.payload == null){
            this.payload = payload;
        }
        return false;
    }
}
