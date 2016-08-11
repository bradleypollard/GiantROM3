using UnityEngine;
using System.Collections;

public class TruckDelivery : MonoBehaviour
{

	public GameObject hardDrivePrefab;
	public string childNameForColour;
	public Transform spawnPoint;

	public Vector3 forceToApply;

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			HardDriveDelivery ();
		}
	}

	public void HardDriveDelivery ()
	{
		GameObject newHD = Instantiate (hardDrivePrefab, spawnPoint.position, Quaternion.identity) as GameObject;
		int randomNumber = Mathf.RoundToInt (Random.Range (1, 4));

		Renderer harddriveRenderer = newHD.transform.FindChild (childNameForColour).GetComponent<Renderer> ();

		switch (randomNumber) {
		case 1:
			harddriveRenderer.material.SetColor ("_Color", Color.red);
			break;
		case 2:
			harddriveRenderer.material.SetColor ("_Color", Color.yellow);
			break;
		case 3:
			harddriveRenderer.material.SetColor ("_Color", Color.magenta);
			break;
		default:
			break;
		}

		newHD.GetComponent<Rigidbody> ().AddForce (forceToApply);
	}

}
