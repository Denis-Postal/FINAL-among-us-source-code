using Cpp2ILInjected;

namespace Assets.InnerNet
{
	public class PinRequestResponse
	{
		public string Pin;

		public string Url;

		public PinRequestError[] Errors= new PinRequestError[0];

		public PinRequestResponse()
		{ }
	}
}
