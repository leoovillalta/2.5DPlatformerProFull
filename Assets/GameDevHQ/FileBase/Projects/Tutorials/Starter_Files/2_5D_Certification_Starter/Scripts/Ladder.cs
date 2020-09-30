using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    //[SerializeField]
    private Vector3 _handPosTopOfLadder, _standPos, _handPosBottomOfLadder;
    [SerializeField]
    private GameObject _handPosGOTopOfLadder, _standPosGO, _handPosGOBottomOfLadder;

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
    public Vector3 GetHanPosBottomOfLadder()
    {
        _handPosBottomOfLadder = new Vector3(_handPosGOBottomOfLadder.transform.position.x, _handPosGOBottomOfLadder.transform.position.y, _handPosGOBottomOfLadder.transform.position.z);
        return _handPosBottomOfLadder;
    }
}
