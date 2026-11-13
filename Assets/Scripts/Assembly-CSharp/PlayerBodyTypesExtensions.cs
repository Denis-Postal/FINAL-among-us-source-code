using System;
using System.Collections.Generic;

public static class PlayerBodyTypesExtensions
{
	private static readonly Dictionary<PlayerBodyTypes, HatOptions> CustomHatOptions= new Dictionary<PlayerBodyTypes, HatOptions>
	{
		{
			PlayerBodyTypes.Normal,
			new HatOptions
			{
				ShowForClimb = true,
				VisorBlockingHatsAllowed = true,
				ShowHelmets = true
			}
		},
		{
			PlayerBodyTypes.Horse,
			new HatOptions
			{
				ShowForClimb = false,
				VisorBlockingHatsAllowed = true,
				ShowHelmets = true
			}
		},
		{
			PlayerBodyTypes.Seeker,
			new HatOptions
			{
				ShowForClimb = true,
				VisorBlockingHatsAllowed = false,
				ShowHelmets = false
			}
		},
		{
			PlayerBodyTypes.Long,
			new HatOptions
			{
				ShowForClimb = true,
				VisorBlockingHatsAllowed = true,
				ShowHelmets = true
			}
		},
		{
			PlayerBodyTypes.LongSeeker,
			new HatOptions
			{
				ShowForClimb = false,
				VisorBlockingHatsAllowed = true,
				ShowHelmets = true
			}
		},
		{
			PlayerBodyTypes.Classic,
			new HatOptions
			{
				ShowForClimb = true,
				VisorBlockingHatsAllowed = true,
				ShowHelmets = true
			}
		}
	} ;

	private static readonly Dictionary<PlayerBodyTypes, SkinOptions> CustomSkinOptions= new Dictionary<PlayerBodyTypes, SkinOptions>
	{
		{
			PlayerBodyTypes.Normal,
			new SkinOptions
			{
				Enabled = true
			}
		},
		{
			PlayerBodyTypes.Long,
			new SkinOptions
			{
				Enabled = true
			}
		},
		{
			PlayerBodyTypes.Horse,
			new SkinOptions
			{
				Enabled = false
			}
		},
		{
			PlayerBodyTypes.Seeker,
			new SkinOptions
			{
				Enabled = false
			}
		},
		{
			PlayerBodyTypes.LongSeeker,
			new SkinOptions
			{
				Enabled = false
			}
		},
		{
			PlayerBodyTypes.Classic,
			new SkinOptions
			{
				Enabled = false
			}
		}
	} ;

	private static readonly Dictionary<PlayerBodyTypes, VisorOptions> CustomVisorOptions= new Dictionary<PlayerBodyTypes, VisorOptions>
	{
		{
			PlayerBodyTypes.Normal,
			new VisorOptions
			{
				Enabled = true,
				HideDuringClimb = false
			}
		},
		{
			PlayerBodyTypes.Long,
			new VisorOptions
			{
				Enabled = true,
				HideDuringClimb = true
			}
		},
		{
			PlayerBodyTypes.Horse,
			new VisorOptions
			{
				Enabled = true,
				HideDuringClimb = false
			}
		},
		{
			PlayerBodyTypes.Seeker,
			new VisorOptions
			{
				Enabled = false,
				HideDuringClimb = true
			}
		},
		{
			PlayerBodyTypes.LongSeeker,
			new VisorOptions
			{
				Enabled = true,
				HideDuringClimb = true
			}
		},
		{
			PlayerBodyTypes.Classic,
			new VisorOptions
			{
				Enabled = true,
				HideDuringClimb = false
			}
		}
	} ;

	public static HatOptions GetHatOptions(this PlayerBodyTypes bodyTypes)
	{
		return CustomHatOptions[bodyTypes];
	}

	public static SkinOptions GetSkinOptions(this PlayerBodyTypes bodyTypes)
	{
		return CustomSkinOptions[bodyTypes];
	}

	public static VisorOptions GetVisorOptions(this PlayerBodyTypes bodyTypes)
	{
		return CustomVisorOptions[bodyTypes];
	}

	static PlayerBodyTypesExtensions()
	{
		//IL_0018: Expected VT, but got I4
		//IL_002b: Expected VT, but got I4
		//IL_0043: Expected VT, but got I4
		//IL_0056: Expected VT, but got I4
		//IL_006e: Expected VT, but got I4
		//IL_0081: Expected VT, but got I4
		//IL_00b6: Expected VT, but got I4
		//IL_00c9: Expected VT, but got I4
		//IL_00e1: Expected VT, but got I4
		//IL_00f4: Expected VT, but got I4
		//IL_010c: Expected VT, but got I4
		//IL_011f: Expected VT, but got I4
		//IL_0154: Expected VT, but got I4
		//IL_0167: Expected VT, but got I4
		//IL_017f: Expected VT, but got I4
		//IL_0192: Expected VT, but got I4
		//IL_01aa: Expected VT, but got I4
		//IL_01bd: Expected VT, but got I4
	}
}
