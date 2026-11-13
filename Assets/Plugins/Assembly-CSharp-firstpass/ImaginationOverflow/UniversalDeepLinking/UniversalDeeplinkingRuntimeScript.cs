using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ImaginationOverflow.UniversalDeepLinking
{
	public class UniversalDeeplinkingRuntimeScript : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class _003CCallDeepLinkManagerAfterDelay_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public UniversalDeeplinkingRuntimeScript _003C_003E4__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return null;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return null;
				}
			}

			[DebuggerHidden]
			public _003CCallDeepLinkManagerAfterDelay_003Ed__4(int _003C_003E1__state)
			{
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
			}
		}

		private bool _onJob;

		private void Start()
		{
		}

		private void OnApplicationPause(bool pauseStatus)
		{
		}

		private void OnApplicationFocus(bool focus)
		{
		}

		[IteratorStateMachine(typeof(_003CCallDeepLinkManagerAfterDelay_003Ed__4))]
		public IEnumerator CallDeepLinkManagerAfterDelay()
		{
			return null;
		}
	}
}
