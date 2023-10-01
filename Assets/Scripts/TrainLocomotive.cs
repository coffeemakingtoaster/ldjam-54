using UnityEngine;

public class TrainLocomotive : MonoBehaviour
{
    public TrainTrack currentTrainTrack;
    public float SPEED = 5f;

    bool isInTrackTile = false;

    float rotatedAngle = 0;

    public bool isDrivingInReverse = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(currentTrainTrack.entryPoint.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = this.GetNextWaypoint();

        // Has traintrack center been reached?
        if (Vector3.Distance(targetPosition, transform.position) < 0.02 || rotatedAngle >= 90f)
        {
            // Has reached entrypoint => Target exitpoint of same track
            // Has reched exitpoint => Target entrypoint of next track
            if (!isInTrackTile)
            {
                isInTrackTile = true;
            }
            else
            {
                TrainTrack nextPossibleTrack = currentTrainTrack.GetNextTrainTrack(targetPosition);
                if (isDrivingInReverse)
                {
                    nextPossibleTrack = currentTrainTrack.GetPreviousTrainTrack(targetPosition);
                }
                // Is next defined?
                if (nextPossibleTrack != null)
                {
                    if (nextPossibleTrack.isCurve)
                    {
                        if (nextPossibleTrack.flipsDirection && currentTrainTrack.flipsDirection)
                        {
                            isDrivingInReverse = !isDrivingInReverse;
                        }
                    } else{
                        if (currentTrainTrack.flipsDirection){
                            isDrivingInReverse = !isDrivingInReverse;
                        }
                    }
                    currentTrainTrack = nextPossibleTrack;
                    isInTrackTile = false;
                    rotatedAngle = 0f;
                }
            }
        }
        else
        {
            //  if (isInTrackTile && nextTrainTrack.isCurve)
            //  {
            //      transform.RotateAround(nextTrainTrack.rotatePoint.transform.position, Vector3.up, -50f * Time.deltaTime);
            //      rotatedAngle += Mathf.Abs(-10f * Time.deltaTime);
            //  }
            //  else
            //  {
            Vector3 lookVector = (targetPosition - transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(lookVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, SPEED * Time.deltaTime);
            //}
        }
    }

    Vector3 GetNextWaypoint()
    {
        if (isInTrackTile)
        {
            if (isDrivingInReverse)
            {
                return currentTrainTrack.entryPoint.transform.position;
            }
            return currentTrainTrack.exitPoint.transform.position;
        }
        if (isDrivingInReverse)
        {
            return currentTrainTrack.exitPoint.transform.position;
        }
        return currentTrainTrack.entryPoint.transform.position;
    }
}
