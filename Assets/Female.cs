using UnityEngine;
using System.Collections;

public class Female : MonoBehaviour {

  // Use this for initialization
  void Start()
  {
    GetComponent<ItemRecipe>().meshes = new Renderer[1];
    GetComponent<ItemRecipe>().meshes[0] = GameObject.Find("_door_female").GetComponent<Renderer>();
  }
}
