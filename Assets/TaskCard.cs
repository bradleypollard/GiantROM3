using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum CardType{Demo, Speech, NPC}

public class TaskCard : MonoBehaviour
{
	[SerializeField] ProgramFeed programFeed;

	[SerializeField] public Image characterAvatar;
	[SerializeField] public Text characterName;
	[SerializeField] public Image gameTitle;
	[SerializeField] public Image discIcon;
	[SerializeField] public Image consoleName;
	[SerializeField] public Image vrIcon;

	[SerializeField] string[] gameNames;
	[SerializeField] string[] boyNames;
	[SerializeField] string[] girlNames;

	[SerializeField] CardType type;

	void
	Start()
	{
		//Invoke ("RenderNewTask", 3);
	}


	public void
	UpdateGraphics(TaskCardInfo thisCard)
	{
		if (thisCard.playerID == 1)
		{
			characterAvatar.sprite = Resources.Load ("task_character_johnny", (typeof(Sprite))) as Sprite;
			characterName.text = "Johnny V";
		}
		else if (thisCard.playerID == 2)
		{
			characterAvatar.sprite = Resources.Load ("task_character_adam", (typeof(Sprite))) as Sprite;
			characterName.text = "Adam Boyes";
		}
		else if (thisCard.playerID == 3)
		{
			characterAvatar.sprite = Resources.Load ("task_character_aisha", (typeof(Sprite))) as Sprite;
			characterName.text = "Aisha";
		}
		else if (thisCard.playerID == 4)
		{
			characterAvatar.sprite = Resources.Load ("task_character_phil", (typeof(Sprite))) as Sprite;
			characterName.text = "Phil Spencer";
		}

		if (thisCard.npcName != null)
		{
			characterName.text = thisCard.npcName;
		}
		
		if (thisCard.gameID == 1)
			discIcon.sprite = Resources.Load ("task_disc_cyan", (typeof(Sprite))) as Sprite;
		else if (thisCard.gameID == 2)
			discIcon.sprite = Resources.Load ("task_disc_magenta", (typeof(Sprite))) as Sprite;
		else if (thisCard.gameID == 3)
			discIcon.sprite = Resources.Load ("task_disc_yellow", (typeof(Sprite))) as Sprite;

		if (thisCard.consoleType == "Playstation")
			consoleName.sprite = Resources.Load ("task_console_playstation", (typeof(Sprite))) as Sprite;
		if (thisCard.consoleType == "WiiU")
			consoleName.sprite = Resources.Load ("task_console_wii", (typeof(Sprite))) as Sprite;
		if (thisCard.consoleType == "Xbox")
			consoleName.sprite = Resources.Load ("task_console_xbox", (typeof(Sprite))) as Sprite;

		gameTitle.sprite = Resources.Load ("task_game_" + thisCard.gameName, (typeof(Sprite))) as Sprite;
	}
}

[System.Serializable]
public class TaskCardInfo : System.Object
{
	public CardType type;
	public int playerID;

	public int gameID;
	public string gameName;
	public string consoleType;
	public bool vr;

	public bool isMale;
	public string npcName;
}