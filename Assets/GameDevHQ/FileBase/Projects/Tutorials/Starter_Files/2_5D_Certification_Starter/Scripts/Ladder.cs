using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    //[SerializeField]
    private Vector3 _handPosTopOfLadder, _standPos;
    [SerializeField]
    private GameObject _handPosGOTopOfLadder, _standPosGO;

    private void OnTriggerEnter(Collider other)
    {
        //if player collided
        //disable the character controller
        if (other.CompareTag("LadderGrab"))
        {
            Player player = other.transform.parent.GetComponent<Player>();
            if (player != null)
            {

                _handPosTopOfLadder = new Vector3(_handPosGOTopOfLadder.transform.position.x, _handPosGOTopOfLadder.transform.position.y, _handPosGOTopOfLadder.transform.position.z);
                player.GrabLadder(_handPosTopOfLadder, this);
            }
            //other.GetComponentInParent<CharacterController>().enabled = false;
        }
    }

    public Vector3 GetStandPos()
    {
        _standPos = new Vector3(_standPosGO.transform.position.x, _standPosGO.transform.position.y, _standPosGO.transform.position.z);
        return _standPos;
    }
    public Vector3 GetHandPosTopOfLadder()
    {
        _handPosTopOfLadder = new Vector3(_handPosGOTopOfLadder.transform.position.x, _handPosGOTopOfLadder.transform.position.y, _handPosGOTopOfLadder.transform.position.z);
        return _handPosTopOfLadder;
    }
}
