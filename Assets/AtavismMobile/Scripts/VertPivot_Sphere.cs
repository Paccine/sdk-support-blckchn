using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Atavism
{
    [System.Serializable]
    public partial class VertPivot_Sphere : MonoBehaviour
    {
        // hold timer to prevent the camera from jerking in mobile device
        private float holdTimer;

        // holds the x-axis & y-axis retrieved from mouse
        private float yaxis;

        private float xAxis;

        // contains the speed of every frame

        public float vertSpeed;

        private float horiSpeed;

        // default variables
        private float rotationY;

        private Quaternion originalRotation;

        public float rotationSpeed;

        public float minimumTilt;

        public float maximumTilt;



        bool canRoate0;
        bool canRoate1;
        bool canRoate2;

        public bool isOnZoom;


        private Vector2 lastRotPosition;
        private bool canRoateMouse0;
        private bool canRoateMouse1;

        public virtual void Start()
        {
            this.originalRotation = this.transform.localRotation;
        }

        public virtual void Update()
        {

            if (Input.touchCount > 0)
            {

                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        if (!canRoate1)
                        {
                            lastRotPosition = touch.position;
                            canRoate0 = true;
                        }
                    }

                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    if (canRoate0 && Input.touchCount < 3 && !isOnZoom)
                    {
                        //////////////////////////////////////////////////////////////////////////
                        Vector3 offset = touch.position - lastRotPosition;
                        lastRotPosition = touch.position;
                        this.rotationY = this.rotationY + ((offset.y * this.rotationSpeed) * 0.8f);
                        this.rotationY = VertPivot_Sphere.ClampAngle(this.rotationY, -this.maximumTilt, -this.minimumTilt);
                        Quaternion yQuaternion0 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
                        this.transform.localRotation = this.originalRotation * yQuaternion0;
                        this.transform.Rotate(0f, this.horiSpeed * this.rotationSpeed, 0f, Space.World);
                    }

                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (canRoate0)
                    {
                        canRoate0 = false;
                        lastRotPosition = new Vector2();
                    }
                }
                else if (touch.phase == TouchPhase.Canceled)
                {
                    if (canRoate0)
                    {
                        canRoate0 = false;
                        lastRotPosition = new Vector2();
                    }
                }
            }

            if (Input.touchCount > 1)
            {

                Touch touch1 = Input.GetTouch(1);
                if (touch1.phase == TouchPhase.Began)
                {

                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
                    {
                        if (!canRoate0)
                        {
                            lastRotPosition = touch1.position;
                            canRoate1 = true;
                        }
                    }
                }
                else if (touch1.phase == TouchPhase.Moved)
                {
                    if (canRoate1 && Input.touchCount < 3 && !isOnZoom)
                    {
                        /////////////////////////////////////////////////////////////////////////////////
                        Vector3 offset = touch1.position - lastRotPosition;
                        lastRotPosition = touch1.position;
                        this.rotationY = this.rotationY + ((offset.y * this.rotationSpeed) * 0.8f);
                        this.rotationY = VertPivot_Sphere.ClampAngle(this.rotationY, -this.maximumTilt, -this.minimumTilt);
                        Quaternion yQuaternion0 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
                        this.transform.localRotation = this.originalRotation * yQuaternion0;
                        this.transform.Rotate(0f, this.horiSpeed * this.rotationSpeed, 0f, Space.World);
                    }

                }
                else if (touch1.phase == TouchPhase.Ended)
                {
                    if (canRoate1)
                    {
                        canRoate1 = false;
                        lastRotPosition = new Vector2();
                    }

                }
                else if (touch1.phase == TouchPhase.Canceled)
                {
                    if (canRoate1)
                    {
                        canRoate1 = false;
                        lastRotPosition = new Vector2();
                    }
                }
            }






            //this.yaxis = -Input.GetAxis("Mouse Y");
            //this.vertSpeed = this.yaxis;
            //this.rotationY = this.rotationY + ((this.vertSpeed * -this.rotationSpeed) * 0.8f);
            //this.rotationY = VertPivot_Sphere.ClampAngle(this.rotationY, -this.maximumTilt, -this.minimumTilt);
            //Quaternion yQuaternion2 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
            //this.transform.localRotation = this.originalRotation * yQuaternion2;
            //this.transform.Rotate(0f, this.horiSpeed * this.rotationSpeed, 0f, Space.World);






            if (Input.touchCount <= 0)
            {
                lastRotPosition = new Vector2();
                canRoate0 = false;
                canRoate1 = false;
                vertSpeed = 0;
            }

#if UNITY_EDITOR


            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    canRoateMouse0 = false;
                }
                else
                {
                    canRoateMouse0 = true;
                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    canRoateMouse1 = false;
                }
                else
                {
                    canRoateMouse1 = true;
                }

            }


            if (Input.GetMouseButton(0) && canRoateMouse0)
            {
                this.yaxis = -Input.GetAxis("Mouse Y");
                this.vertSpeed = this.yaxis;
                this.rotationY = this.rotationY + ((this.vertSpeed * -this.rotationSpeed * 10) * 0.8f);
                this.rotationY = VertPivot_Sphere.ClampAngle(this.rotationY, -this.maximumTilt, -this.minimumTilt);
                Quaternion yQuaternion2 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
                this.transform.localRotation = this.originalRotation * yQuaternion2;
                this.transform.Rotate(0f, this.horiSpeed * this.rotationSpeed * 10, 0f, Space.World);
            }

            if (Input.GetMouseButton(1) && canRoateMouse1)
            {
                this.yaxis = -Input.GetAxis("Mouse Y");
                this.vertSpeed = this.yaxis;
                this.rotationY = this.rotationY + ((this.vertSpeed * -this.rotationSpeed * 10) * 0.8f);
                this.rotationY = VertPivot_Sphere.ClampAngle(this.rotationY, -this.maximumTilt, -this.minimumTilt);
                Quaternion yQuaternion2 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
                this.transform.localRotation = this.originalRotation * yQuaternion2;
                this.transform.Rotate(0f, this.horiSpeed * this.rotationSpeed * 10, 0f, Space.World);
            }

#endif


        }

        // to clamp the angles so that the rotation will not behave erraticly
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
            {
                angle = angle + 360;
            }

            if (angle > 360)
            {
                angle = angle - 360;
            }

            return Mathf.Clamp(angle, min, max);
        }

        public virtual void OnGUI()
        {
            // to change to the pan orbit scene
            //  if (GUI.Button(new Rect(Screen.width - 100, 25, 75, 50), "Pan\nOrbit"))
            //  {
            //      UnityEngine.SceneManagement.SceneManager.LoadScene("SetUpSample", UnityEngine.SceneManagement.LoadSceneMode.Single);
            //  }
        }

        public VertPivot_Sphere()
        {
            this.minimumTilt = -30f;
            this.maximumTilt = 30f;
        }

    }
}