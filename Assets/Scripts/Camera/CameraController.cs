#if ENABLE_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM_PACKAGE
#define USE_INPUT_SYSTEM
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Controls;
#endif

using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace UnityTemplateProjects
{
    public class CameraController : MonoBehaviour
    {
        [System.Serializable]
        class CameraState
        {
            public float yaw;
            public float pitch;
            public float roll;
            public float x;
            public float y;
            public float z;

            public void SetFromTransform(Transform t)
            {
                pitch = t.eulerAngles.x;
                yaw = t.eulerAngles.y;
                roll = t.eulerAngles.z;
                x = t.position.x;
                y = t.position.y;
                z = t.position.z;
            }

            public void Translate(Vector3 translation)
            {
                Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

                x += rotatedTranslation.x;
                y += rotatedTranslation.y;
                z += rotatedTranslation.z;
            }

            public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
            {
                yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
                pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
                roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);
                
                x = Mathf.Lerp(x, target.x, positionLerpPct);
                y = Mathf.Lerp(y, target.y, positionLerpPct);
                z = Mathf.Lerp(z, target.z, positionLerpPct);
            }

            public void UpdateTransform(Transform t)
            {
                t.eulerAngles = new Vector3(pitch, yaw, roll);
                t.position = new Vector3(x, y, z);
            }
        }

        float inputX;
        float inputZ;
        
        CameraState m_TargetCameraState = new CameraState();
        CameraState m_InterpolatingCameraState = new CameraState();

        [Header("Movement Settings")]
        [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
        public float boost = 3.5f;

        [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
        public float positionLerpTime = 0.2f;

        [Header("Rotation Settings")]
        [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
        public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
        public float rotationLerpTime = 0.01f;

        [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
        public bool invertY = false;

        [Tooltip("Whether or not to invert our X axis for mouse input to rotation.")]
        public bool invertX = false;

        [Tooltip("The maximum amount that the Y axis can be rotated."), Range(00, 90)]
        public float verticalRestriction = 30;

        [Tooltip("Whether or not the camera should follow target.")]
        public bool followObject = false;
        public bool isAnimating = false;

        public EclipticPoles eclipticPoles;
        public EclipticDrawer eclipticDrawer;
        public Camera childCamera;

        public Transform targetObject;

        Task cameraFollow;
        float cameraLerpTime = 0.75f;

        void OnEnable()
        {
            EventManager.Instance.OnAnimationStart += AnimationStateOn;
            EventManager.Instance.OnAnimationEnd += AnimationStateOff;
            EventManager.Instance.On2DPlanetClicked += TargetPlanet;
            EventManager.Instance.On2DSignClicked += TargetSign;
            EventManager.Instance.OnFollowPlanet += ToggleCameraControls;

            m_TargetCameraState.SetFromTransform(transform);
            m_InterpolatingCameraState.SetFromTransform(transform);
        }

        private void OnDestroy()
        {
            EventManager.Instance.OnAnimationStart -= AnimationStateOn;
            EventManager.Instance.OnAnimationEnd -= AnimationStateOff;
            EventManager.Instance.On2DPlanetClicked -= TargetPlanet;
            EventManager.Instance.On2DSignClicked -= TargetSign;
            EventManager.Instance.OnFollowPlanet -= ToggleCameraControls;
        }

        void Update()
        {
            if (cameraFollow != null && !cameraFollow.IsCompleted) return;

            if (followObject)
            {
                LookAtSun();
                return;
            }

            Vector3 translation = Vector3.zero;

#if ENABLE_LEGACY_INPUT_MANAGER


            inputX = Input.GetAxis("Horizontal");
            inputZ = Input.GetAxis("Vertical");
                
            // Hide and lock cursor when right mouse button pressed
            if (Input.GetMouseButtonDown(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // Unlock and show cursor when right mouse button released
            if (Input.GetMouseButtonUp(1))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            // Rotation
            if (Input.GetMouseButton(1))
            {
                RotateCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            } 
            else if(inputX != 0 || inputZ != 0)
            {
                RotateCamera(-inputX, -inputZ);
            }

#elif USE_INPUT_SYSTEM 
            // TODO: make the new input system work
#endif

            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
            m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

            m_InterpolatingCameraState.UpdateTransform(transform);
        }

        void RotateCamera(float horizontal, float vertical)
        {
            var arrowMovement = new Vector2(horizontal * (invertX ? 1 : -1), vertical * (invertY ? 1 : -1));

            var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(arrowMovement.magnitude);

            m_TargetCameraState.pitch += arrowMovement.y * mouseSensitivityFactor;
            m_TargetCameraState.yaw += arrowMovement.x * mouseSensitivityFactor;

            // maintain pitch within vertical restriction
            // value will be clamped if it doesn't pass both as positive and negative value (within 360 range)
            if (m_TargetCameraState.pitch > verticalRestriction || m_TargetCameraState.pitch < -verticalRestriction)
            {
                if (m_TargetCameraState.pitch - 360 > verticalRestriction || m_TargetCameraState.pitch - 360 < -verticalRestriction)
                {
                    m_TargetCameraState.pitch = Mathf.Clamp(m_TargetCameraState.pitch, -verticalRestriction, verticalRestriction);
                }
            }
        }

        void ToggleCameraControls(bool val)
        {
            followObject = val;
            if(!val) CollectCameraState();
        }

        void LookAtSun()
        {
            if(GeoData.ActiveData.NorthernHemisphere)
                transform.LookAt(targetObject, eclipticPoles.northPolePosition);
            else
                transform.LookAt(targetObject, eclipticPoles.southPolePosition);
        }
        public void TargetPlanet(int planetID)
        {
            if (followObject) return;
            Vector3 position = PlanetData.PlanetDataList[planetID].realPlanet.planet.transform.GetChild(0).position;
            LookAtPlanet(position);
        }

        public void TargetAngle(int angleID)
        {
            if (followObject) return;
            Vector3 position = AngleData.AngleDataList[angleID].Angle3D.planet.transform.GetChild(0).position;
            LookAtPlanet(position);
        }


        public void TargetSign(int signID)
        {
            if (followObject) return;
            Vector3 position = EclipticDrawer.midSignsObjects[signID].transform.GetChild(0).position;
            LookAtPlanet(position);
        }

        public async void LookAtPlanet(Vector3 position)
        {
            cameraFollow = LookAtPlanetAsync(position, cameraLerpTime);
            await Task.WhenAll(cameraFollow);
            CollectCameraState();


            async Task LookAtPlanetAsync(Vector3 position, float maxTime)
            {
                Vector3 forwardVector = transform.forward;

                float t = 0;

                while (t < maxTime)
                {
                    transform.forward = Vector3.Slerp(forwardVector, position, t / maxTime);
                    t += Time.deltaTime;

                    await Task.Yield();
                }

                transform.forward = position;
                await Task.Yield();
            }

        }


        void CollectCameraState()
        {
            // collect pitch and yaw values
            float tempX = transform.eulerAngles.x;
            float tempY = transform.eulerAngles.y;

            // get positive values for pitch and yaw to prevent errors with the camera angle restriction
            if (transform.eulerAngles.x > 85)
                tempX -= 360;
            if (transform.eulerAngles.y > 85)
                tempY -= 360;

            // assign positive values to transform rotation
            transform.eulerAngles = new Vector3(tempX, tempY);

            m_TargetCameraState.SetFromTransform(transform);
        }

        void ToggleFollowPlanet(bool val)
        {
            followObject = val;
        }

        void AnimationStateOn()
        {
            isAnimating = true;
        }

        void AnimationStateOff()
        {
            isAnimating = false;
            childCamera.transform.localEulerAngles = Vector3.zero;
        }

    }

}