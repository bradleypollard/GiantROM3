using UnityEngine;
using System.Collections;

public class Male : MonoBehaviour
{
  // Use this for initialization
  void Start()
  {
    GetComponent<ItemRecipe>().meshes = new Renderer[1];
    GetComponent<ItemRecipe>().meshes[0] = GameObject.Find("_door_male").GetComponent<Renderer>();
  }
}
