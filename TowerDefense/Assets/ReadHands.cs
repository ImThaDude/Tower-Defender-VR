using UnityEngine;
using System.Collections;

public class ReadHands : MonoBehaviour {

    public KeyCode key = KeyCode.B;
    public float graspRad = 5f;
    public bool isPressed = false;
    public SteamVR_Controller.DeviceRelation controller;
    public Transform controllerObject;
    public bool isGrabbing = false;
    public Transform objectLeaving = null;
    bool firstGrab = false;

	// Use this for initialization
	void Start () {

        transform.position = controllerObject.position;
        transform.parent = controllerObject;

    }
	
	// Update is called once per frame
	void Update () {

        //transform.position = SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(controller)).transform.pos;


        isGrabbing = false;

        if (Input.GetKey(key) || isPressed || SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(controller)).GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {

            isGrabbing = true;

            Collider[] col = Physics.OverlapSphere(transform.position, graspRad);
            //for all objects the player encounters
            for (int i = 0; i < col.Length; i++)
            {
                if (col[i].transform.tag == "Thing")
                {
                    if (col[i].transform.parent == null)
                    {
                        transform.rotation = Quaternion.Inverse(col[i].transform.rotation);
                        //Grab the object by attaching it to the first hand
                        col[i].transform.parent = transform;
                        col[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    } else
                    {
                        if (col[i].transform.parent != transform)
                        {
                            if (!firstGrab)
                            {
                                Transform temp = col[i].transform.parent;
                                col[i].transform.parent = null;
                                temp.LookAt(transform);
                                col[i].transform.parent = temp;
                                firstGrab = true;
                            }

                            //Get position A and B and rotate the object by that vector
                            col[i].transform.parent.LookAt(transform);
                        }
                    }
                }
            }
        } else
        {
            firstGrab = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                transform.GetChild(i).parent = objectLeaving;
            }
        }

        
	
	}

}
