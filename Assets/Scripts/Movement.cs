using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

  public float speed = 5f;
  public int playerIndex = -1;

  private CharacterController cc;

  // Use this for initialization
  void Start()
  {
    cc = GetComponent<CharacterController>();
  }

  // Update is called once per frame
  void Update()
  {
    float dx = Input.GetAxis("Horizontal_P" + playerIndex.ToString());
    float dy = Input.GetAxis("Vertical_P" + playerIndex.ToString());

    cc.SimpleMove(speed * new Vector3(dx, 0, dy).normalized);
  }
}
