using System;
using System.Collections;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
	private sealed class _003CCoWaitForDateConfirmation_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public PlayerParticles _003C_003E4__this;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CCoWaitForDateConfirmation_003Ed__14(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false; // TODO(verify): default, original logic unrecoverable
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		void IEnumerator.Reset()
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			NotSupportedException ex = new NotSupportedException();
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			throw ex;
		}
	}

	public PlayerParticleInfo[] Sprites;

	public PlayerParticleInfo[] HorseSprites;

	public PlayerParticleInfo[] ClassicSprites;

	public FloatRange velocity;

	public FloatRange scale;

	public ObjectPoolBehavior pool;

	public float StartRadius;

	public PoolableBehavior HorsePrefab;

	public PoolableBehavior ClassicParticlePrefab;

	public Camera FollowCamera;

	private RandomFill<PlayerParticleInfo> fill;

	private float scaleModifier = 1f;

	private bool readyToMove;

	public void Start()
	{
		readyToMove = false;
		StartCoroutine(CoWaitForDateConfirmation());
	}

	private IEnumerator CoWaitForDateConfirmation()
	{
		// TODO(verify): offline adaptation: use normal sprites without waiting for an EOS timestamp.
		// The current coroutine gates seasonal selection on HasServerTimestamp; the 2019
		// baseline initializes immediately. No server connection is needed for normal particles.
		fill = new RandomFill<PlayerParticleInfo>();
		EOSManager eos = DestroyableSingleton<EOSManager>.Instance;
		if (eos != null && eos.HasServerTimestamp && AprilFoolsMode.ShouldHorseModeMainMenu())
		{
			fill.Set(HorseSprites);
			pool.Prefab = HorsePrefab;
			scaleModifier = 0.7f;
		}
		else if (eos != null && eos.HasServerTimestamp && AprilFoolsMode.ShouldClassicMainMenuMode())
		{
			fill.Set(ClassicSprites);
			scaleModifier = 4f;
		}
		else
		{
			fill.Set(Sprites);
			scaleModifier = 1f;
		}
		int color = 0;
		if (pool != null)
		{
			while (pool.NotInUse > 0)
			{
				PlayerParticle particle = pool.Get<PlayerParticle>();
				PlayerMaterial.SetColors(color++, particle.myRend);
				PlacePlayer(particle, initial: true);
			}
		}
		readyToMove = true;
		yield break;
	}

	public void Update()
	{ /* spliced-from-2019 */
		if (pool == null || !readyToMove) return;
		while (pool.NotInUse > 0)
		{
			PlayerParticle part = pool.Get<PlayerParticle>();
			PlacePlayer(part, initial: false);
		}
	}

	private unsafe void PlacePlayer(PlayerParticle part, bool initial)
	{
		// TODO(verify): position/velocity reconstruction from the 2019 baseline and decomp9.
		Vector3 position = UnityEngine.Random.insideUnitCircle;
		if (!initial) position.Normalize();
		position *= StartRadius;
		// TODO(verify): current seasonal scaleModifier multiplies the sampled scale (load lost in decomp9).
		float size = scale.Next() * scaleModifier;
		part.transform.localScale = new Vector3(size, size, size);
		position.z = -size * 0.001f;
		if (FollowCamera)
		{
			Vector3 cameraPosition = FollowCamera.transform.position;
			cameraPosition.z = 0f;
			position += cameraPosition;
			part.FollowCamera = FollowCamera;
		}
		part.transform.localPosition = position;
		PlayerParticleInfo info = fill.Get();
		part.myRend.sprite = info.image;
		part.myRend.flipX = BoolRange.Next();
		Vector2 direction = -position.normalized;
		direction = Extensions.Rotate(direction, FloatRange.Next(-45f, 45f));
		part.velocity = direction * velocity.Next();
		part.angularVelocity = info.angularVel.Next();
		if (info.alignToVel)
		{
			part.transform.localEulerAngles = new Vector3(0f, 0f, Extensions.AngleSigned(Vector2.up, direction));
		}
	}
}
