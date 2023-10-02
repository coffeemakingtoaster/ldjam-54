using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace DefaultNamespace
{
    public class CameraController : MonoBehaviour
    {
        public Camera camera;
        public GameObject CameraCenter;
        
        public float MovementSpeed;
        public float ZoomSpeed;
        public float TurnSpeed;
        public float Radius;
        public int MaxZoom;
        public int MinZoom;
        
        private Transform cameraTransform;
        private Vector3 cameraCenterPosition;
        private float rotation = 40;
        
        private void Start()
        {
            cameraTransform = transform;
            cameraCenterPosition = CameraCenter.transform.position;
        }

        void Update()
        {
            Vector3 direction = new Vector3( Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime,
                0, 
            Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime );
            Vector3 fixedDirection = ApplyPerspective(direction);
            float zoom = Input.mouseScrollDelta.y * ZoomSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.Q))
            {
                rotation -= TurnSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(40f, rotation, 0f);
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotation += TurnSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(40f, rotation, 0f);
            }


            // Move
            Vector3 newPosition = cameraTransform.position + fixedDirection;
            
            if (Vector3.Distance(newPosition, cameraCenterPosition) < Radius)
            {
                cameraTransform.position = newPosition;
            }
            
            // Zoom
            if ((zoom > 0 && camera.fieldOfView > MaxZoom) || (zoom < 0 && camera.fieldOfView < MinZoom) )
            {
                camera.fieldOfView -= zoom;

            }

        }
        
        private Vector3 ApplyPerspective(Vector3 direction)
        {
            // Camera Rotation
            Quaternion newrotation = Quaternion.Euler(0f, rotation, 0f);
            Matrix4x4 isoMatrix = Matrix4x4.Rotate(newrotation);
            Vector3 isoDirection = isoMatrix.MultiplyVector(direction);
            return isoDirection;
        }

    }
}