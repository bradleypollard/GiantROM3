using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour
{

	public Transform characterMesh;
	public Transform characterHands;

	public GameObject objectInHands;

	void FixedUpdate ()
	{
		if (objectInHands) {
			if (Input.GetKeyDown (KeyCode.LeftAlt)) {
				print ("Dropping " + objectInHands.transform.name);
				objectInHands.transform.parent = null;
				objectInHands.GetComponent<Rigidbody> ().isKinematic = false;
				objectInHands = null;
			}
		} else {

			Vector3 fwd = characterMesh.TransformDirection (Vector3.forward);
			Debug.DrawRay (characterMesh.position, fwd * 1, Color.green);

			RaycastHit hit;

			if (Physics.Raycast (characterMesh.position, fwd, out hit, 1)) {
				if (hit.transform.tag == "Interactable") {
					print ("You can interact " + hit.transform.name);
					if (Input.GetKeyDown (KeyCode.Space)) {
						objectInHands = hit.transform.gameObject;
						objectInHands.transform.parent = this.transform;
						objectInHands.GetComponent<Rigidbody> ().isKinematic = true;
						print ("Picking up object");
					}
				} else {
					print ("You can't interact " + hit.transform.name);
				}
			}

		}
	}
}
