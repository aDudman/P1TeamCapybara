using Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : MonoBehaviour, IPushable
{
    private Rigidbody body;

    /// <summary>
    /// Reacts to the push spell by being pushed.
    /// </summary>
    /// <param name="direction">Direction to move</param>
    /// <param name="force">Affects initial speed</param>
    public void Push(Vector3 direction, float force)
    {
        body.AddForce(direction * force);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
