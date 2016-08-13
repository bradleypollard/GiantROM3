using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemRecipe : MonoBehaviour {

    public GameObject ITurnInto;
    public string IWorkWithThisMachine;
    public Renderer CopyColourFromHere;

    public Shader shaderOutline;

    private List<Material> setFloatMats = new List<Material>();

    void Update()
    {
        if (transform.root.name.Contains("Player"))
        {
            if (IWorkWithThisMachine == "Disk Burner")
            {
                GameObject.Find(IWorkWithThisMachine).transform.FindChild("computer/_computer").GetComponent<Renderer>().material.SetFloat("_OutlineTransparency", 1);
                setFloatMats.Add(GameObject.Find(IWorkWithThisMachine).transform.FindChild("computer/_computer").GetComponent<Renderer>().material);
            }

            if (IWorkWithThisMachine == "Console")
            {
                foreach (GameObject console in GameObject.FindGameObjectsWithTag("Console"))
                {
                    console.transform.FindChild("mesh").GetComponent<Renderer>().material.SetFloat("_OutlineTransparency", 1);
                    setFloatMats.Add(console.transform.FindChild("mesh").GetComponent<Renderer>().material);
                }
            }
        }
        else
        {
            RemoveOutline();
        }
    }

    private void RemoveOutline()
    {
        foreach(Material m in setFloatMats)
        {
            m.SetFloat("_OutlineTransparency", 0);
        }

        setFloatMats.Clear();
    }

    void OnDestroy()
    {
        RemoveOutline();
    }

}
