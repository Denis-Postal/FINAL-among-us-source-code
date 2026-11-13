using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class TaskFolder : MonoBehaviour
{
	public enum FolderColor
	{
		Tan,
		Red,
		Blue
	}

	public string FolderName;

	public TextMeshPro Text;

	public TaskAdderGame Parent;

	public List<TaskFolder> SubFolders= new List<TaskFolder>();

	public List<NormalPlayerTask> TaskChildren= new List<NormalPlayerTask>();

	public List<RoleBehaviour> RoleChildren= new List<RoleBehaviour>();

	[HideInInspector]
	public PassiveButton Button;

	[SerializeField]
	private ButtonRolloverHandler buttonRolloverHandler;

	[SerializeField]
	private SpriteRenderer folderSpriteRenderer;

	[SerializeField]
	private Color currentFolderColor;

	private void Awake()
	{
		Button = GetComponent<PassiveButton>();
	}

	// TODO(verify): decomp9 Start was lost; the folder text shows FolderName (as in the 2019 baseline)
	public void Start()
	{
		Text.text = FolderName;
	}

	public void OnClick()
	{
		Parent.ShowFolder(this);
	}

	// TODO(verify): the three folder colours were lost in decomp9; Tan keeps the prefab colour, Red/Blue use the palette role colours
	public void SetFolderColor(FolderColor folderColor)
	{
		switch (folderColor)
		{
		case FolderColor.Tan:
			currentFolderColor = new Color(0.937f, 0.811f, 0.592f);
			break;
		case FolderColor.Red:
			currentFolderColor = new Color(1f, 0.09f, 0.09f);
			break;
		case FolderColor.Blue:
			currentFolderColor = new Color(0.545f, 1f, 1f);
			break;
		default:
			currentFolderColor = new Color(1f, 1f, 1f);
			break;
		}
		Debug.Log("Setting folder color: " + folderColor);
		folderSpriteRenderer.color = currentFolderColor;
		buttonRolloverHandler.OutColor = currentFolderColor;
		buttonRolloverHandler.UnselectedColor = currentFolderColor;
	}

	internal List<TaskFolder> OrderBy()
	{
		throw new NotImplementedException();
	}

	public TaskFolder()
	{ }
}
