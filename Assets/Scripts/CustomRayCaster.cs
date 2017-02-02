using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRayCaster : MonoBehaviour
{
    private RaycastHit raycastHit;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out raycastHit))
        {
            InteractiveVRItem item = raycastHit.collider.GetComponent<InteractiveVRItem>();
            if (item != null && !item.isOver)
            {
                StartCoroutine(item.StartCheckingViewPoint(GetComponent<Camera>()));
            }
        }
    }
}
