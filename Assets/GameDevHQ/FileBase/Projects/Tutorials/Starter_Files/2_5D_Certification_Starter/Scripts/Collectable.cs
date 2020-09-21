using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType {Gem, BeerMug };
    [SerializeField]
    private CollectableType _collectableType = CollectableType.Gem;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && _collectableType==CollectableType.Gem)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AddGems();
            }
            Destroy(this.gameObject);
        }
        if(other.tag == "Player" && _collectableType == CollectableType.BeerMug)
        {
            //Collect Special Collectable Beer Mug
        }
    }
}
