using System.Collections.Generic;

public class Win10Manager : DestroyableSingleton<Win10Manager>
{
	private Dictionary<string, ulong> puid2xuid= new Dictionary<string, ulong>();

	private List<KeyValuePair<string, ulong>> xUIDsToCheckIfBlocked= new List<KeyValuePair<string, ulong>>();

	private HashSet<string> nonXuids= new HashSet<string>();

	public Win10Manager()
	{ }
}
