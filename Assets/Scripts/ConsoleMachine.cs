using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConsoleMachine : MonoBehaviour
{

	public bool machineUsable = false;
	public GameObject acceptedInputItem;
	public ItemRecipe itemRecipe;
	public GameObject playerUsingMachine;
	public int playerIndexUsingMachine;
	public bool machineInUse;

	[Range (1, 4)]
	public float machineUseTimer;

	public enum InputType
	{
		Hold,
		Tap}

	;

	public InputType selectedInputType;

	[Range (5, 20)]
	public int machineTapCount;

	public float counter;

	public Slider progressSlider;

	void Update ()
	{
		if (machineUsable) {
			if (selectedInputType == InputType.Hold) {
				#region Hold Input Type
				if (Input.GetButton ("A_P" + playerIndexUsingMachine)) {
					print ("Machine Useable and Button is Held Down");
					playerUsingMachine.GetComponent<PlayerMovement> ().enabled = false;
					if (!machineInUse) {
						acceptedInputItem.SetActive (false);
						machineInUse = true;
						progressSlider.value = 0;
						progressSlider.gameObject.SetActive (true);
					} else if (machineInUse) {
						print ("Counter should start here");
						if (counter < machineUseTimer) {
							counter += Time.deltaTime;
							progressSlider.value = counter / machineUseTimer;
						} else {
							print ("Machine finished, reseting and giving item to player");
							playerUsingMachine.GetComponent<Interaction> ().GrabObject (transform.parent.GetChild (0).gameObject);
							playerUsingMachine.GetComponent<PlayerMovement> ().enabled = true;
							Destroy (acceptedInputItem);
							ResetMachine ();
						}
					}
				}

				if (Input.GetButtonUp ("A_P" + playerIndexUsingMachine) && counter < machineUseTimer) {
					ResetProgress ();
					playerUsingMachine.GetComponent<PlayerMovement> ().enabled = true;
					acceptedInputItem.SetActive (true);
				}
				#endregion
			} else {
				#region Hold Input Type
				if (Input.GetButtonDown ("A_P" + playerIndexUsingMachine)) {
					print ("Machine Useable and Button is Held Down");
					playerUsingMachine.GetComponent<PlayerMovement> ().enabled = false;
					if (!machineInUse) {
						acceptedInputItem.SetActive (false);
						machineInUse = true;
						progressSlider.value = 0;
						progressSlider.gameObject.SetActive (true);
					} else if (machineInUse) {
						print ("Counter should start here");
						if (counter < machineTapCount - 1) {
							counter += 1;
							progressSlider.value = counter / machineTapCount;
						} else {
							print ("Machine finished, reseting and giving item to player");
							playerUsingMachine.GetComponent<Interaction> ().GrabObject (transform.parent.GetChild (0).gameObject);
							playerUsingMachine.GetComponent<PlayerMovement> ().enabled = true;
							Destroy (acceptedInputItem);
							ResetMachine ();
						}
					}
				}

				if (Input.GetButtonUp ("A_P" + playerIndexUsingMachine) && counter < machineUseTimer) {
					playerUsingMachine.GetComponent<PlayerMovement> ().enabled = true;
				}
				#endregion
			}
		}

	}

	void OnTriggerEnter (Collider collider)
	{
		if (collider.transform.parent.GetComponent<ItemRecipe> ()) {
			print ("Player " + collider.transform.root.GetComponent<PlayerMovement> ().playerIndex + " entered with " + collider.transform.parent.name);
			itemRecipe = collider.transform.parent.GetComponent<ItemRecipe> ();

			if (itemRecipe.IWorkWithThisMachine == this.name) {
				print ("Compatible with me!");
				machineUsable = true;
				acceptedInputItem = collider.transform.parent.gameObject;
				playerIndexUsingMachine = collider.transform.root.GetComponent<PlayerMovement> ().playerIndex;
				playerUsingMachine = collider.transform.root.gameObject;
			} else {
				print ("Not Compatible with me, only works with " + itemRecipe.IWorkWithThisMachine);
			}
		} else {
			print ("Not Compatible with machines, no iterm recipe");
		}

	}

	void OnTriggerExit (Collider collider)
	{
		print ("Player " + collider.transform.root.GetComponent<PlayerMovement> ().playerIndex + " left with " + collider.transform.parent.name);
		machineUsable = false;
		ResetMachine ();
	}

	private void ResetMachine ()
	{
		acceptedInputItem = null;
		machineUsable = false;
		counter = 0;
		machineInUse = false;
		playerUsingMachine = null;
		progressSlider.gameObject.SetActive (false);
	}

	private void ResetProgress ()
	{
		counter = 0;
		progressSlider.gameObject.SetActive (false);
		machineInUse = false;
	}
}
