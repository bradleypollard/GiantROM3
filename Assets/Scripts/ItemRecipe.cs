using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemRecipe : MonoBehaviour
{

  public GameObject ITurnInto;
  public string IWorkWithThisMachine;
  public Renderer CopyColourFromHere;

  [SerializeField]
  public Renderer[] meshes;

  void Update()
  {
    if (transform.root.name.Contains("Player"))
    {
      foreach (Renderer r in meshes)
      {
        r.material.SetFloat("_OutlineTransparency", 1);
      }
    }
    else
    {
      RemoveOutline();
    }
  }

  private void RemoveOutline()
  {
    foreach (Renderer r in meshes)
    {
      r.material.SetFloat("_OutlineTransparency", 0);
    }
  }

  void OnDestroy()
  {
    RemoveOutline();
  }

}
