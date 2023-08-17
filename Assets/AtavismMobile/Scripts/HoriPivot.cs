


using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Atavism
{
    [System.Serializable]
    public partial class HoriPivot : MonoBehaviour
    {
        // variable that determine the speed of rotation
        public float rotationSpeed;

        // variable that holds the current rotation speed
        private float speed;

        // variable to hold the x-axis from mouse
        private float xAxis;

        private int fingerID = -1;

        bool canRoate0;
        bool canRoate1;
        bool canRoate2;

        bool canRoateMouse0;
        bool canRoateMouse1;

        public bool isOnZoom;



        private Vector2 lastRotPosition;

        public virtual void Update() //
        {


#if UNITY_ANDROID

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
                    }//
                }

            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (canRoate0 && Input.touchCount < 3 && !isOnZoom)
                {
                    Vector3 offset = touch.position - lastRotPosition;
                    lastRotPosition = touch.position;
                    this.transform.Rotate(0f, offset.x * rotationSpeed, 0f, Space.World);
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
                    Vector3 offset = touch1.position - lastRotPosition;
                    lastRotPosition = touch1.position;
                    this.transform.Rotate(0f, offset.x * rotationSpeed, 0f, Space.World);
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

        if (Input.touchCount <= 0)
        {
            lastRotPosition = new Vector2();
            canRoate0 = false;
            canRoate1 = false;
            speed = 0;
        }

#endif


            #region UNITY

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
                this.xAxis = Input.GetAxis("Mouse X");
                this.speed = this.xAxis;
                this.transform.Rotate(0f, this.speed * this.rotationSpeed * 10, 0f, Space.World);
            }

            if (Input.GetMouseButton(1) && canRoateMouse1)
            {
                this.xAxis = Input.GetAxis("Mouse X");
                this.speed = this.xAxis;
                this.transform.Rotate(0f, this.speed * this.rotationSpeed * 10, 0f, Space.World);
            }

#endif

            #endregion
        }

    }
}