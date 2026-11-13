using System.Collections;
using UnityEngine;

public class RecordsMinigame : Minigame
{
	public GameObject FoldersContent;

	public SpriteRenderer[] Folders;

	public GameObject DrawerContent;

	public Transform Drawer;

	public SpriteRenderer DrawerFolder;

	public GameObject ShelfContent;

	public SpriteRenderer[] Books;

	public Sprite[] BookCovers;

	private SpriteRenderer targetBook;

	public AudioClip recordFilePlace;

	public AudioClip recordBookPlace;

	public AudioClip grabDocument;

	public AudioClip drawerOpen;

	public AudioClip drawerClose;

	public Transform bookInputPrompt;

	public ControllerButtonBehaviourComplex slideFolderHotkey;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		FoldersContent.SetActive(value: false);
		ShelfContent.SetActive(value: false);
		DrawerContent.SetActive(value: false);
		SetupInput(disableCursor: true);
		if (ConsoleId == 0)
		{
			FoldersContent.SetActive(value: true);
			for (int i = 0; i < Folders.Length; i++)
			{
				if (MyNormTask.Data[i] != 0)
				{
					Folders[i].gameObject.SetActive(value: false);
				}
			}
		}
		else if (ConsoleId <= 4)
		{
			DrawerContent.SetActive(value: true);
		}
		else if (ConsoleId <= 8)
		{
			ShelfContent.SetActive(value: true);
			Sprite sprite = BookCovers.Random();
			SpriteRenderer[] books = Books;
			for (int j = 0; j < books.Length; j++)
			{
				books[j].sprite = sprite;
			}
			targetBook = Books.Random();
			targetBook.enabled = false;
			targetBook.GetComponent<PassiveButton>().enabled = true;
			ControllerButtonBehavior component = targetBook.GetComponent<ControllerButtonBehavior>();
			component.enabled = true;
			Vector3 localPosition = bookInputPrompt.transform.localPosition;
			localPosition.x = targetBook.transform.localPosition.x;
			bookInputPrompt.transform.localPosition = localPosition;
			bookInputPrompt.GetComponentInChildren<ActionMapGlyphDisplay>(includeInactive: true).actionToDisplayMappedGlyphFor = component.Action;
			bookInputPrompt.gameObject.SetActive(value: true);
		}
	}

	protected override IEnumerator CoAnimateOpen()
	{
		if (ConsoleId == 0)
		{
			TransType = TransitionType.SlideBottom;
		}
		else if (ConsoleId <= 4)
		{
			yield return CoOpenDrawer();
		}
		else if (ConsoleId <= 8)
		{
			TransType = TransitionType.SlideBottom;
		}
		yield return base.CoAnimateOpen();
	}

	protected override IEnumerator CoDestroySelf()
	{
		if (ConsoleId > 0 && ConsoleId <= 4)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(drawerClose, loop: false);
			}
			yield return Effects.Slide2D(Drawer, Vector2.zero, new Vector2(0f, 7f), 0.3f);
		}
		yield return base.CoDestroySelf();
	}

	public void PlaceBook()
	{
		if (MarkConsoleFinished())
		{
			MyNormTask.NextStep();
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(recordBookPlace, loop: false);
			}
			StartCoroutine(CoSlideBook());
		}
	}

	private IEnumerator CoSlideBook()
	{
		targetBook.GetComponent<PassiveButton>().enabled = false;
		targetBook.enabled = true;
		yield return Effects.ColorFade(targetBook, Palette.ClearWhite, Color.white, 0.4f);
		yield return CoStartClose(0.35f);
	}

	public void FileDocument()
	{
		if (MarkConsoleFinished())
		{
			MyNormTask.NextStep();
			slideFolderHotkey.enabled = false;
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(recordFilePlace, loop: false);
			}
			StartCoroutine(CoSlideFolder());
		}
	}

	private bool MarkConsoleFinished()
	{
		bool result = false;
		for (int i = 0; i < MyNormTask.Data.Length; i++)
		{
			byte b = MyNormTask.Data[i];
			if (b != 0 && b != byte.MaxValue)
			{
				MyNormTask.Data[i] = byte.MaxValue;
				result = true;
			}
		}
		return result;
	}

	private IEnumerator CoOpenDrawer()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(drawerOpen, loop: false);
		}
		yield return Effects.Slide2D(Drawer, new Vector2(0f, 7f), Vector2.zero, 0.5f);
	}

	private IEnumerator CoSlideFolder()
	{
		DrawerFolder.gameObject.SetActive(value: true);
		yield return Effects.Slide2D(DrawerFolder.transform, new Vector2(0f, 5f), Vector2.zero, 0.4f);
		yield return CoStartClose(0.35f);
	}

	public void GrabFolder(SpriteRenderer folder)
	{
		if (amClosing == CloseState.None)
		{
			int num = Folders.IndexOf(folder);
			folder.gameObject.SetActive(value: false);
			MyNormTask.Data[num] = IntRange.NextByte(1, 9);
			MyNormTask.UpdateArrowAndLocation();
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(grabDocument, loop: false);
			}
			StartCoroutine(CoStartClose());
		}
	}
}
