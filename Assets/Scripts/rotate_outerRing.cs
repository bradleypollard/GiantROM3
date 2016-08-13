using UnityEngine;
using System.Collections;

public class rotate_outerRing : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
    
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 2, 0 * Time.deltaTime);
    }
}
