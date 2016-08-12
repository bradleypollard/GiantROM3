using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleprompterCrank : MonoBehaviour
{
  [SerializeField]
  Transform crankHandle;
  [SerializeField]
  float animationDuration = 0.5f; //in seconds
  [SerializeField]
  Teleprompter teleprompter;
  bool isCranking;

	private List<int> playerID = new List<int>();

  void RotateHandle()
  {
    if (!isCranking)
    {
      isCranking = true;
      StartCoroutine(LerpHandle());
      GetComponent<AudioSource>().Play();
      if (teleprompter != null)
      {
        teleprompter.AddMorePrompts();
      }
    }
  }

  IEnumerator LerpHandle()
  {
    float zRotation = 0;
    float lerpTime = 0f;

    while (lerpTime <= 1f)
    {
      zRotation = Mathf.Lerp(0, 360, lerpTime);
      crankHandle.rotation = Quaternion.Euler(crankHandle.rotation.eulerAngles.x, crankHandle.rotation.eulerAngles.y, zRotation);

      animationDuration = Mathf.Clamp(animationDuration, 0.5f, 4f);
      lerpTime += Time.deltaTime / animationDuration;

      yield return null;
    }

    isCranking = false;
  }

  void Update()
	{
		foreach (int id in playerID)
		{
			if (Input.GetButton ("A_P" + id))
			{
				RotateHandle ();
			}
		}
  }

  void OnTriggerEnter(Collider collider)
  {
		if (collider.GetComponent<PlayerMovement> ())
		{
			playerID.Add(collider.gameObject.GetComponent<PlayerMovement> ().playerIndex);
		}
  }

  void OnTriggerExit(Collider collider)
  {
		if (collider.GetComponent<PlayerMovement> ())
		{	
			playerID.Remove (collider.gameObject.GetComponent<PlayerMovement> ().playerIndex);
		}
  }
}