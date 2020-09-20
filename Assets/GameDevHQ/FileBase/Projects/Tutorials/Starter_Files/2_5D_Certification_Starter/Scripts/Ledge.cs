using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField]
    private Vector3 _handPos, _standPos;
    private void OnTriggerEnter(Collider other)
    {
        //if player collided
        //disable the character controller
        if (other.CompareTag("Ledge_Grab_Checker"))
        {
            Player player = other.transform.parent.GetComponent<Player>();
            if(player != null)
            {
                player.GrabLedge(_handPos,this);
            }
            //other.GetComponentInParent<CharacterController>().enabled = false;
        }
    }

    public Vector3 GetStandPos()
    {
        return _standPos;
    }
}
