using Cpp2ILInjected;
using UnityEngine;

public class TwitterLink : MonoBehaviour
{
	public string LinkUrl;

	public void Click()
	{
		Constants.OpenURL(LinkUrl);
	}

	public TwitterLink()
	{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
		LinkUrl = "https://www.twitter.com/InnerslothDevs";
	}
}
