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
        public float Radius;
        public int MaxZoom;
        public int MinZoom;
        
        private Transform cameraTransform;
        private Vector3 cameraCenterPosition;
        
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
            Quaternion rotation = Quaternion.Euler(0,40,0);
            Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
            Vector3 isoDirection = isoMatrix.MultiplyVector(direction);
            return isoDirection;
        }

    }
}