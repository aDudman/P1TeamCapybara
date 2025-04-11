using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Globals;
using MainCharacter;

public class Companion : MonoBehaviour
{
    [SerializeField, Tooltip("The relative position that the companion should be when following the player")]
    private Vector3 playerFollowPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Joins the player when it enters the area
    /// </summary>
    /// <param name="other">an object which entered the area</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StaticTagStrings.PLAYER))
        {
            var player = other.GetComponent<MCAgent>();
            player.EnableSpells();
            transform.SetParent(other.transform, false);
            transform.SetLocalPositionAndRotation(playerFollowPosition, Quaternion.identity);
        }
    }
}
