using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TruckDelivery : MonoBehaviour
{

	public GameObject hardDrivePrefab;
	public string childNameForColour;
	public Transform spawnPoint;
	public Transform deliveryPointForTruck;
	[Range (1, 4)]
	public float truckAnimationTime;

	public Vector3 forceToApply;

	private Sequence truckDeliverySequence;

	private Vector3 startPos;

	void Start ()
	{
		startPos = transform.position;
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			HardDriveDelivery ();
		}
	}

	public void HardDriveDelivery ()
	{
		truckDeliverySequence = DOTween.Sequence ();


		truckDeliverySequence.Append (transform.DOMove (deliveryPointForTruck.position, truckAnimationTime).OnComplete (ChuckHardDriveOutBack));
		truckDeliverySequence.AppendInterval (0.5f);
		truckDeliverySequence.Append (transform.DOMove (startPos, truckAnimationTime));
		truckDeliverySequence.Play ();
	}

	public void ChuckHardDriveOutBack ()
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
