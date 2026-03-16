using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FollowCam : MonoBehaviour
{

    static private FollowCam S;

    static public GameObject POI;

    public enum eView{none, slingshot, castle, both};

    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXy = Vector2.zero;
    public GameObject viewBothGO;

    [Header("Dynamic")]
    public float camZ;
    public eView nextVIew = eView.slingshot;
    void Awake()
    {
        S = this;
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        Vector3 destination = Vector3.zero;

        if (POI != null)
        {
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if((poiRigid != null) && poiRigid.IsSleeping())
            {
                POI = null;
            }
        }

        if(POI != null)
        {
            destination = POI.transform.position;
        }
        destination.x = Mathf.Max(minXy.x, destination.x);
        destination.y = Mathf.Max(minXy.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10;
    }

    public void SwitchView(eView newView)
    {
        if (newView == eView.none)
        {
            newView = nextVIew;
        }
        switch (newView)
        {
            case eView.slingshot:
                POI = null;
                nextVIew = eView.castle;
                break;
            case eView.castle:
                POI = MissionDemolition.GET_CASTLE();
                nextVIew = eView.both;
                break;
            case eView.both:
                POI = viewBothGO;
                nextVIew = eView.slingshot;
                break;
    

        }
    }

    public void SwitchView()
    {
        SwitchView(eView.none);
    }
  

  static public void SWITCH_VIEW(eView newView)
    {
        S.SwitchView(newView);
    }

   
}
