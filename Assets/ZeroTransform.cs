using UnityEngine;
using System.Collections;

public class ZeroTransform : MonoBehaviour
{
	void
	OnValidate ()
	{
		transform.position = new Vector3 (0, 0, 0);
	}
}