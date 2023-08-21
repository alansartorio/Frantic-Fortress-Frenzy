using UnityEngine;
using System.Collections;

using TDTK;

namespace TDTK
{

    public class CameraControl : MonoBehaviour
    {

        private float initialMousePosX;
        private float initialMousePosY;

        private float initialRotX;
        private float initialRotY;

        public Camera cameraComponent;

        public float panSpeed = 5;
        public float zoomSpeed = 5;


        public bool enableMouseRotate = true;
        public bool enableKeyRotate = true;
        public bool enableMousePanning = false;
        public bool enableKeyPanning = true;

        public int mousePanningZoneWidth = 10;

        public int keyRotateSpeed = 10;

        public float minPosX = -10;
        public float maxPosX = 10;

        public float minPosZ = -10;
        public float maxPosZ = 10;

        public float minZoomDistance = 8;
        public float maxZoomDistance = 30;

        public float minRotateAngle = 10;
        public float maxRotateAngle = 89;

        public bool avoidClipping = false;
        private bool obstacle = false;

        public bool showGizmo = true;

        //calculated deltaTime based on timeScale so camera movement speed always remain constant
        private float deltaT;

        private float currentZoom = 0;

        private Transform thisT;
        public static CameraControl instance;

        public static void Disable() { if (instance != null) instance.enabled = false; }
        public static void Enable() { if (instance != null) instance.enabled = true; }

        void Awake()
        {
            thisT = transform;

            instance = this;

            cameraComponent = Camera.main;
        }

        void Start()
        {

            minRotateAngle = Mathf.Max(10, minRotateAngle);
            maxRotateAngle = Mathf.Min(89, maxRotateAngle);

            minZoomDistance = Mathf.Max(1, minZoomDistance);

            currentZoom = cameraComponent.transform.localPosition.z;

        }

        void Update()
        {

            if (Time.timeScale == 1) deltaT = Time.deltaTime;
            else if (Time.timeScale > 1) deltaT = Time.deltaTime / Time.timeScale;
            else deltaT = 0.015f;


            //mouse and keyboard
            if (enableMouseRotate)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    initialMousePosX = Input.mousePosition.x;
                    initialMousePosY = Input.mousePosition.y;
                    initialRotX = thisT.eulerAngles.y;
                    initialRotY = thisT.eulerAngles.x;
                }

                if (Input.GetMouseButton(1))
                {
                    float deltaX = Input.mousePosition.x - initialMousePosX;
                    float deltaRotX = (.1f * (initialRotX / Screen.width));
                    float rotX = deltaX + deltaRotX;

                    float deltaY = initialMousePosY - Input.mousePosition.y;
                    float deltaRotY = -(.1f * (initialRotY / Screen.height));
                    float rotY = deltaY + deltaRotY;
                    float y = rotY + initialRotY;

                    //limit the rotation
                    if (y > maxRotateAngle)
                    {
                        initialRotY -= (rotY + initialRotY) - maxRotateAngle;
                        y = maxRotateAngle;
                    }
                    else if (y < minRotateAngle)
                    {
                        initialRotY += minRotateAngle - (rotY + initialRotY);
                        y = minRotateAngle;
                    }

                    thisT.rotation = Quaternion.Euler(y, rotX + initialRotX, 0);
                }
            }

            Quaternion direction = Quaternion.Euler(0, thisT.eulerAngles.y, 0);


            if (enableKeyPanning)
            {
                if (Input.GetButton("Horizontal"))
                {
                    Vector3 dir = transform.InverseTransformDirection(direction * Vector3.right);
                    thisT.Translate(dir * panSpeed * deltaT * Input.GetAxisRaw("Horizontal"));
                }

                if (Input.GetButton("Vertical"))
                {
                    Vector3 dir = transform.InverseTransformDirection(direction * Vector3.forward);
                    thisT.Translate(dir * panSpeed * deltaT * Input.GetAxisRaw("Vertical"));
                }
            }
            if (enableMousePanning)
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 dirHor = transform.InverseTransformDirection(direction * Vector3.right);
                if (mousePos.x <= 0) thisT.Translate(dirHor * panSpeed * deltaT * -3);
                else if (mousePos.x <= mousePanningZoneWidth) thisT.Translate(dirHor * panSpeed * deltaT * -1);
                else if (mousePos.x >= Screen.width) thisT.Translate(dirHor * panSpeed * deltaT * 3);
                else if (mousePos.x > Screen.width - mousePanningZoneWidth) thisT.Translate(dirHor * panSpeed * deltaT * 1);

                Vector3 dirVer = transform.InverseTransformDirection(direction * Vector3.forward);
                if (mousePos.y <= 0) thisT.Translate(dirVer * panSpeed * deltaT * -3);
                else if (mousePos.y <= mousePanningZoneWidth) thisT.Translate(dirVer * panSpeed * deltaT * -1);
                else if (mousePos.y >= Screen.height) thisT.Translate(dirVer * panSpeed * deltaT * 3);
                else if (mousePos.y > Screen.height - mousePanningZoneWidth) thisT.Translate(dirVer * panSpeed * deltaT * 1);
            }


            float zoomInput = Input.GetAxis("Mouse ScrollWheel");
            if (zoomInput != 0)
            {
                currentZoom += zoomSpeed * zoomInput;
                currentZoom = Mathf.Clamp(currentZoom, -maxZoomDistance, -minZoomDistance);
            }

            // Camera rotation
            if (enableKeyRotate)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, keyRotateSpeed * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, -keyRotateSpeed * Time.deltaTime);
                }
            }

            if (cameraComponent.orthographic)
            {
                cameraComponent.orthographicSize = -currentZoom;
            }
            else
            {
                if (avoidClipping)
                {
                    Vector3 aPos = thisT.TransformPoint(new Vector3(0, 0, currentZoom));
                    Vector3 dirC = aPos - thisT.position;
                    float dist = Vector3.Distance(aPos, thisT.position);
                    RaycastHit hit;
                    obstacle = Physics.Raycast(thisT.position, dirC, out hit, dist);

                    if (!obstacle)
                    {
                        float camZ = Mathf.Lerp(cameraComponent.transform.localPosition.z, currentZoom, Time.deltaTime * 4);
                        cameraComponent.transform.localPosition = new Vector3(cameraComponent.transform.localPosition.x, cameraComponent.transform.localPosition.y, camZ);
                    }
                    else
                    {
                        dist = Vector3.Distance(hit.point, thisT.position) * 0.85f;
                        float camZ = Mathf.Lerp(cameraComponent.transform.localPosition.z, -dist, Time.deltaTime * 50);
                        cameraComponent.transform.localPosition = new Vector3(cameraComponent.transform.localPosition.x, cameraComponent.transform.localPosition.y, camZ);
                    }
                }
                else
                {
                    float camZ = Mathf.Lerp(cameraComponent.transform.localPosition.z, currentZoom, Time.deltaTime * 4);
                    cameraComponent.transform.localPosition = new Vector3(cameraComponent.transform.localPosition.x, cameraComponent.transform.localPosition.y, camZ);
                }
            }

            float x = Mathf.Clamp(thisT.position.x, minPosX, maxPosX);
            float z = Mathf.Clamp(thisT.position.z, minPosZ, maxPosZ);

            thisT.position = new Vector3(x, thisT.position.y, z);

        }

        void OnDrawGizmos()
        {
            if (showGizmo)
            {
                Vector3 p1 = new Vector3(minPosX, transform.position.y, maxPosZ);
                Vector3 p2 = new Vector3(maxPosX, transform.position.y, maxPosZ);
                Vector3 p3 = new Vector3(maxPosX, transform.position.y, minPosZ);
                Vector3 p4 = new Vector3(minPosX, transform.position.y, minPosZ);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawLine(p2, p3);
                Gizmos.DrawLine(p3, p4);
                Gizmos.DrawLine(p4, p1);
            }
        }

    }

}