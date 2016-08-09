using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour
{

	public Transform characterMesh;

	public GameObject objectInHands;

	void FixedUpdate ()
	{
		if (objectInHands) {
			if (Input.GetButtonDown ("B_P1")) {
                ReleaseObject();
            }
		} else {

			Vector3 fwd = characterMesh.TransformDirection (Vector3.forward);
			Debug.DrawRay (characterMesh.position, fwd * 1, Color.green);

			RaycastHit hit;

			if (Physics.Raycast (characterMesh.position, fwd, out hit, 5)) {
				if (hit.transform.tag == "Interactable") {
					print ("You can interact " + hit.transform.name);
					if (Input.GetButtonDown ("A_P1")) {
                        GrabObject(hit.transform.gameObject);
					}
				} else {
					print ("You can't interact " + hit.transform.name);
				}
			}

		}
	}

    private void GrabObject(GameObject gb)
    {
        objectInHands = gb.transform.parent.gameObject;
        objectInHands.transform.parent = characterMesh;
        objectInHands.GetComponent<Rigidbody>().isKinematic = true;
        objectInHands.transform.position = characterMesh.position + new Vector3(0, 2.3f, 0);
        objectInHands.transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
        print("Picking up object");
    }

    private void ReleaseObject()
    {
        print("Dropping " + objectInHands.transform.name);
        Vector3 fwd = characterMesh.TransformDirection(Vector3.forward);
        objectInHands.transform.position = characterMesh.position + fwd + new Vector3(0, 2, 0);
        objectInHands.transform.parent = null;
        objectInHands.GetComponent<Rigidbody>().isKinematic = false;
        objectInHands = null;
    }
}
