using System.Collections.Generic;

namespace ImaginationOverflow.UniversalDeepLinking
{
	public class LinkActivation
	{
		public string Uri { get; private set; }

		public string RawQueryString { get; private set; }

		public Dictionary<string, string> QueryString { get; private set; }

		public LinkActivation(string uri, string rawQueryString, Dictionary<string, string> queryStringParams)
		{
		}

		public override string ToString()
		{
			return null;
		}
	}
}
