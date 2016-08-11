using UnityEngine;
using System.Collections;

public class TeleprompterCrank : MonoBehaviour
{
  [SerializeField]
  Transform crankHandle;
  [SerializeField]
  float animationDuration = 0.5f; //in seconds
  [SerializeField]
  Teleprompter teleprompter;
  bool isCranking;

  private int playerID = 0;
  private bool canUseCrank = false;

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
    if (canUseCrank && playerID != 0 && Input.GetButton("A_P" + playerID))
    {
      RotateHandle();
    }
  }

  void OnTriggerEnter(Collider collider)
  {
    canUseCrank = true;
    playerID = collider.gameObject.GetComponent<PlayerMovement>().playerIndex;

  }

  void OnTriggerExit(Collider collider)
  {
    canUseCrank = false;
    playerID = 0;
  }
}