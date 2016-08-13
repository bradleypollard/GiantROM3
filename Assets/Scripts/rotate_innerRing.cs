using UnityEngine;
using System.Collections;

public class rotate_innerRing : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, -1, 0 * Time.deltaTime);
    }
}
