using UnityEngine;
using System.Collections;

public class HardDrive : MonoBehaviour {

  // Use this for initialization
  void Start()
  {
    GetComponent<ItemRecipe>().meshes = new Renderer[4];
    GetComponent<ItemRecipe>().meshes[0] = GameObject.Find("_computer").GetComponent<Renderer>();
    GetComponent<ItemRecipe>().meshes[1] = GameObject.Find("_keyboard").GetComponent<Renderer>();
    GetComponent<ItemRecipe>().meshes[2] = GameObject.Find("_mouse").GetComponent<Renderer>();
    GetComponent<ItemRecipe>().meshes[3] = GameObject.Find("_desktop").GetComponent<Renderer>();
  }
}
