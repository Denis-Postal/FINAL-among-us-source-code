using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DateHide : MonoBehaviour
{
	public int MonthStart;

	public int DayStart;

	public int HourStart;

	public int MonthEnd;

	public int DayEnd;

	public int HourEnd;

	[SerializeField]
	private bool IsYearly = true;

	[SerializeField]
	private int YearStart;

	[SerializeField]
	private int YearEnd;

	[SerializeField]
	private bool ControlledByParent;

	[SerializeField]
	private List<int> DisabledYears= new List<int>();

	private void Awake()
	{
		if (!ControlledByParent && ShouldHide())
		{
			gameObject.SetActive(value: false);
		}
	}

	public bool ShouldHide()
	
	{
		try
		{
			DateTime utcNow = AmongUsDateTime.UtcNow;
			if (DisabledYears.Contains(utcNow.Year))
			{
				return true;
			}
			DateTime dateTime = new DateTime(IsYearly ? utcNow.Year : YearStart, MonthStart, DayStart, HourStart, 0, 0);
			DateTime dateTime2 = new DateTime(IsYearly ? utcNow.Year : YearEnd, MonthEnd, DayEnd, HourEnd, 0, 0);
			if (dateTime <= utcNow && utcNow <= dateTime2)
			{
				return false;
			}
		}
		catch
		{
		}
		return true;
	}

	public DateHide()
	{ }
}
