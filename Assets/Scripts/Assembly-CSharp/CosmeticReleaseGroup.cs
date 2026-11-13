using UnityEngine;
using System;

public class CosmeticReleaseGroup : AbstractCosmeticGroup
{
	[SerializeField]
	private string date;

	public string DisplayName => name;

	public string DateString => date;

	public DateTime Date => Constants.ParseDate(date);
}
