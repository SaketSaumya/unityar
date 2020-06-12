using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ObjPlacement : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnedobject;
    [SerializeField]
    private GameObject placableprefab;
    static List<ARRaycastHit> s_hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }
    bool TryGetTouchPosition(out Vector2 touchposition)
    {
        if (Input.touchCount > 0)
        {
            touchposition = Input.GetTouch(0).position;
            return true;
        }
        touchposition = default;
        return false;
    }
    private void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchposition))
        {
            return;
        }
        if (raycastManager.Raycast(touchposition, s_hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPos = s_hits[0].pose;
            //next line attempt at changing rotation
            //next line works for changing the rotation of an object
            hitPos.rotation = new Quaternion(0, 180, 0, 1);
            //hitPos.rotation=hitPos.
            if (spawnedobject == null)
            {
                spawnedobject = Instantiate(placableprefab, hitPos.position, hitPos.rotation);
            }
            else
            {
                spawnedobject.transform.position = hitPos.position;
                spawnedobject.transform.rotation = hitPos.rotation;
                //didn't work
                //transform.rotation*= Quaternion.Euler(0, 180f, 0);
            }
        }
    }
}
