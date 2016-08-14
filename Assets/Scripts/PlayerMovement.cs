using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
  [Header("References")]
  [SerializeField]
  Transform movementOrientation;
  [SerializeField]
  Transform characterMesh;

  [Header("Settings")]
  [Range(0.18f, 0.05f)]
  [SerializeField]
  float speed;
  [Range(1, 4)]
  [SerializeField]
  public int playerIndex;
  [SerializeField]
  public bool lockMovement = false;

  public float currSpeed = 0f;
  public bool isHoldingNPC;

  private Vector3 lockedPos = Vector3.zero;



  void
  Update()
  {
    if (!lockMovement)
    {
      lockedPos = Vector3.zero;

      Vector3 moveDirection = movementOrientation.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal_P" + playerIndex.ToString()), 0, Input.GetAxisRaw("Vertical_P" + playerIndex.ToString())));

      if (moveDirection != Vector3.zero)
      {
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
        currSpeed = moveDirection.sqrMagnitude;
        transform.Translate(moveDirection * speed);

        Vector3 rotationDirection = moveDirection;
        //rotationDirection = new Vector3 (rotationDirection.x, -0.2f, rotationDirection.z);
        characterMesh.rotation = Quaternion.LookRotation(rotationDirection);
      }
      else
      {
        currSpeed = 0f;
      }
    }
    else
    {
      if (lockedPos == Vector3.zero)
      {
        lockedPos = transform.position;
      }
      transform.position = lockedPos;
    }
  }
}