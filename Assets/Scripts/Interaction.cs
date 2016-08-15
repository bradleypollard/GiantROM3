using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class Interaction : MonoBehaviour
{

  public Transform characterMesh;

  public GameObject objectInHands;
	public GameObject tempObjectinHands;

  public PlayerMovement playerMovement;

  public GameObject LeftHand;
  public GameObject RightHand;

  public float rayLength = 5f;
  public float grabDistance = 1.5f;

  void Start()
  {
    playerMovement = gameObject.GetComponent<PlayerMovement>();
  }

  void FixedUpdate()
  {
    if (tempObjectinHands != null)
    {
      if (Input.GetButtonDown("B_P" + playerMovement.playerIndex) && objectInHands != null)
      {
        ThrowObject();
      }
    }
    else if (Input.GetButtonDown("A_P" + playerMovement.playerIndex))
    {
      Vector3 fwd = characterMesh.TransformDirection(Vector3.forward);
      Ray ray = new Ray(characterMesh.position, fwd);
      Ray backRay = new Ray(characterMesh.position + rayLength * fwd, -fwd);
      Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green);
      
      // Interact layer is layer 9
      int layer = 1 << 9;

      // Cast a ray backwards and forwards incase we are inside a collider
      RaycastHit[] hits = Physics.RaycastAll(ray, rayLength, layer);
      RaycastHit[] backHits = Physics.RaycastAll(backRay, rayLength, layer);

      float closest = float.MaxValue;
      GameObject hitObject = null;
      // Iterate through all the interactables we found and find the closest
      foreach (RaycastHit h in hits)
      {
        Vector3 hitpos = h.transform.position;
        hitpos.y = 0;
        Vector3 charpos = characterMesh.position;
        charpos.y = 0;
        float distance = (hitpos - charpos).magnitude;
        if (distance <= grabDistance && distance < closest)
        {
          closest = distance;
          hitObject = h.transform.gameObject;
        }
      }
      foreach (RaycastHit h in backHits)
      {
        Vector3 hitpos = h.transform.position;
        hitpos.y = 0;
        Vector3 charpos = characterMesh.position;
        charpos.y = 0;
        float distance = (hitpos - charpos).magnitude;
        if (distance <= grabDistance && distance < closest)
        {
          closest = distance;
          hitObject = h.transform.gameObject;
        }
      }

      // Grab the closest item that was inside the grab distance, if it exists
      if (hitObject != null)
      {
        GrabObject(hitObject);
      }

    }
  }



  public void GrabObject(GameObject gb)
  {

		tempObjectinHands = gb.transform.parent.gameObject;
		tempObjectinHands.transform.parent = characterMesh;
		tempObjectinHands.GetComponent<Rigidbody>().isKinematic = true;
		tempObjectinHands.transform.position = characterMesh.position + new Vector3(0, 2.3f, 0);
		tempObjectinHands.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

    StartCoroutine(RealGrabObject (gb)); 
  }



	IEnumerator
	RealGrabObject(GameObject gb)
	{
		yield return new WaitForSeconds (0.5f);

		objectInHands = tempObjectinHands;
		Debug.Log("Picking up object");
	}

  private void ThrowObject()
  {
    GameObject thrownItem = objectInHands;
    ReleaseObject();
    Debug.Log("Dropping " + thrownItem.transform.name);
    Vector3 fwd = characterMesh.TransformDirection(Vector3.forward);
    thrownItem.transform.position = thrownItem.transform.position + fwd;
    thrownItem.GetComponent<Rigidbody>().AddForce(characterMesh.transform.parent.GetComponent<PlayerMovement>().currSpeed * (fwd * 200 + new Vector3(0, 200, 0)));
  }

  public void ReleaseObject()
  {
    objectInHands.transform.parent = null;
    objectInHands.GetComponent<Rigidbody>().isKinematic = false;
    objectInHands = null;
    tempObjectinHands = null;
  }
}
