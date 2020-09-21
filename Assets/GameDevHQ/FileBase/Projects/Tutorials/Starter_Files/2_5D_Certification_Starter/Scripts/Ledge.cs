using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    //[SerializeField]
    private Vector3 _handPos, _standPos;
    [SerializeField]
    private GameObject _handPosGO, _standPosGO;

    private void OnTriggerEnter(Collider other)
    {
        //if player collided
        //disable the character controller
        if (other.CompareTag("Ledge_Grab_Checker"))
        {
            Player player = other.transform.parent.GetComponent<Player>();
            if(player != null)
            {
                _handPos = new Vector3(_handPosGO.transform.position.x, _handPosGO.transform.position.y, _handPosGO.transform.position.z);
                player.GrabLedge(_handPos,this);
            }
            //other.GetComponentInParent<CharacterController>().enabled = false;
        }
    }

    public Vector3 GetStandPos()
    {
        _standPos = new Vector3(_standPosGO.transform.position.x,_standPosGO.transform.position.y,_standPosGO.transform.position.z);
        return _standPos;
    }
}
