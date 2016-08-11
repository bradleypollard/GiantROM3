using UnityEngine;
using System.Collections;

public enum Gender{Male, Female}

public class DressingRoomDoor : MonoBehaviour
{
	[Header("REFERENCES")]
	[SerializeField] Material maleMaterial;
	[SerializeField] Material femaleMaterial;
	[SerializeField] MeshRenderer door;

	[Header("SETTINGS")]
	[SerializeField] Gender gender;

	void
	OnValidate()
	{
		if (gameObject.activeInHierarchy)
		{
			if (gender == Gender.Male)
				door.material = maleMaterial;
			else
				door.material = femaleMaterial;
		}
	}
}