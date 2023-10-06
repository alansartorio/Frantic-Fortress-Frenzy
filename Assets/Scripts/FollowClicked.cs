using Cinemachine;
using UnityEngine;


public class FollowClicked : MonoBehaviour
{
   private Camera mainCamera;
   public LayerMask layerMask;
   
   private void Start()
   {
       mainCamera = Camera.main;
   }

   private void Update()
   {
       // Esto es el click izquierdo
       if (Input.GetMouseButtonDown(0))
       {
           Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
           RaycastHit hit;

           if (Physics.Raycast(ray, out hit))
           {
               var targetToFollow = hit.transform.GetComponent<Enemy>();

               if (targetToFollow != null)
               {
                   CinemachineVirtualCamera vcam = GetComponent<CinemachineVirtualCamera>();
                   vcam.Follow = targetToFollow.transform;
                   vcam.LookAt = targetToFollow.transform;
                   
               }
           }
       }
   }
}
