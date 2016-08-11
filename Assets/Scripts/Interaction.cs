using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour
{

	public Transform characterMesh;

	public GameObject objectInHands;

	public PlayerMovement playerMovement;

  public float rayLength = 5f;
  public float grabDistance = 1.5f;

	void Start ()
	{
		playerMovement = gameObject.GetComponent<PlayerMovement> ();
	}

	void FixedUpdate ()
	{
		if (objectInHands) {
			if (Input.GetButtonDown ("B_P" + playerMovement.playerIndex)) {
				ReleaseObject ();
			}
		} else {

			Vector3 fwd = characterMesh.TransformDirection (Vector3.forward);
      Ray ray = new Ray(characterMesh.position, fwd);
      Ray backRay = new Ray(characterMesh.position + rayLength * fwd, -fwd);
      Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green);

			RaycastHit hit;
      bool foundItem = false;
      int layer = 1 << 9;
      if (!Physics.Raycast(ray, out hit, rayLength, layer))
      {
        foundItem = Physics.Raycast(backRay, out hit, rayLength, layer);
      }
      else
      {
        foundItem = true;
      }

      if (foundItem) {
        Vector3 hitpos = hit.transform.position;
        hitpos.y = 0;
        Vector3 charpos = characterMesh.position;
        charpos.y = 0;
        float distance = (hitpos - charpos).magnitude;
				if (distance <= grabDistance && Input.GetButtonDown ("A_P" + playerMovement.playerIndex)) {
					GrabObject (hit.transform.gameObject);
				}
			}

		}
	}

	public void GrabObject (GameObject gb)
	{
		objectInHands = gb.transform.parent.gameObject;
		objectInHands.transform.parent = characterMesh;
		objectInHands.GetComponent<Rigidbody> ().isKinematic = true;
		objectInHands.transform.position = characterMesh.position + new Vector3 (0, 2.3f, 0);
		objectInHands.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		print ("Picking up object");
	}

	private void ReleaseObject ()
	{
		print ("Dropping " + objectInHands.transform.name);
		Vector3 fwd = characterMesh.TransformDirection (Vector3.forward);
		objectInHands.transform.parent = null;
		objectInHands.GetComponent<Rigidbody> ().isKinematic = false;
		objectInHands.GetComponent<Rigidbody> ().AddForce (fwd * 200 + new Vector3 (0, 200, 0));
		objectInHands = null;
	}
}
