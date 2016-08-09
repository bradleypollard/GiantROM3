using UnityEngine;
using System.Collections;

public class CdBurner : MonoBehaviour {

    public bool machineUsable = false;
    public GameObject acceptedInputItem;

    void Update()
    {
        if(machineUsable && Input.GetButtonDown("A_P1"))
        {
            Destroy(acceptedInputItem);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        print(collider.transform.parent.name + " entered!");
        if (collider.transform.parent.GetComponent<ItemRecipe>())
        {
            ItemRecipe itemRecipe = collider.transform.parent.GetComponent<ItemRecipe>();

            if (itemRecipe.IWorkWithThisMachine == this.name)
            {
                print("Compatible with me! I covert item into " + itemRecipe.ITurnInto);
                machineUsable = true;
                acceptedInputItem = collider.transform.parent.gameObject;
            } else
            {
                print("Not Compatible with me, only works with " + itemRecipe.IWorkWithThisMachine);
            }
        } else
        {
            print("Not Compatible with machines, no iterm recipe");
        }

    }

    void OnTriggerExit(Collider collider)
    {
        print(collider.transform.parent.name + " left!");
        machineUsable = false;
    }
}
