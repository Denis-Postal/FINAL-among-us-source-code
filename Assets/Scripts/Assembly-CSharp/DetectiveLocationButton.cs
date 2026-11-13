using UnityEngine;
using UnityEngine.Audio;

public class DetectiveLocationButton : MonoBehaviour
{
	[SerializeField]
	private AudioClip[] scribbleSFXs;

	private DetectiveNotesMinigame notesMinigame;

	public string locationName = string.Empty;

	public void SetInfo(DetectiveNotesMinigame _notesMinigame, string _locationName)
	{
		notesMinigame = _notesMinigame;
		locationName = _locationName;
	}

	public void OnClick()
	{
		SoundManager.Instance.PlaySound(scribbleSFXs.Random(), loop: false);
		notesMinigame.SetVictimLocation(locationName);
		MapBehaviour.Instance.Close();
	}
}
