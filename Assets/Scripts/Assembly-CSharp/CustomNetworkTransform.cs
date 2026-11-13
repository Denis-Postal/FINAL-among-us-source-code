using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Hazel;
using InnerNet;
using InnerNet.GizmoHelpers;
using UnityEngine;

[DisallowMultipleComponent]
public class CustomNetworkTransform : InnerNetObject
{
	private const float SEND_MOVEMENT_THRESHOLD = 0.0001f;

	private const float REPLAY_POSITION_THRESHOLD = 0.003f;

	private const float NEUTRAL_BAND_MODIFIER = 0.995f;

	private const float SMOOTHING_BAND_MODIFIER = 0.5f;

	private const float SMOOTHING_LERP_RATE = 3f;

	private const int QUEUE_LENGTH_FOR_SNAPPING = 12;

	private const int QUEUE_LENGTH_FOR_SECOND_SNAP = 14;

	private const int QUEUE_THRESHOLD_FOR_SMOOTHING = 5;

	private const int SNAP_TO_SEQUENCE_ID_BUFFER = 2;

	private PlayerControl myPlayer;

	private Rigidbody2D body;

	private Queue<Vector2> sendQueue= new Queue<Vector2>();

	private Queue<Vector2> incomingPosQueue= new Queue<Vector2>();

	[SerializeField]
	private float rubberbandModifier= 1f;

	[SerializeField]
	private float idealSpeed;

	public bool isPaused;

	private ushort lastSequenceId;

	private Vector2 lastPosition;

	private Vector2 lastPosSent;

	private Vector2? tempSnapPosition;

	private ITransformGhost debugPopPositions= new NullTransformGhost();

	private ITransformGhost debugTargetPositions= new NullTransformGhost();

	private ITransformGhost debugTruePositions= new NullTransformGhost();

	private INetTransformLogger debugNetLogger= new NullNetLogger();

	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		myPlayer = GetComponent<PlayerControl>();
		lastPosSent = transform.position;
		incomingPosQueue.Enqueue(transform.position);
	}

	public unsafe void OnDrawGizmos()
	{
		debugTargetPositions.Render(Color.red, crosses: true);
		debugPopPositions.Render(new Color(1f, 1f, 0f, 0.25f), crosses: false);
		debugTruePositions.Render(new Color(0f, 0f, 1f, 0.25f), crosses: false);
		debugNetLogger.Render(transform.position, rubberbandModifier);
	}

	public void OnEnable()
	{
		DummyBehaviour component = GetComponent<DummyBehaviour>();
		if ((bool)component && component.enabled)
		{
			enabled = false;
		}
		SetDirtyBit(3u);
	}

	public void SetPaused(bool isPaused)
	{
		this.isPaused = isPaused;
	}

	public void Halt()
	{
		ushort minSid = (ushort)(lastSequenceId + 1);
		SnapTo(transform.position, minSid);
	}

	public void RpcSnapTo(Vector2 position)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			SnapTo(position, (ushort)(lastSequenceId + 1));
		}
		ushort value = (ushort)(lastSequenceId + 2);
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 21, SendOption.Reliable);
		NetHelpers.WriteVector2(position, messageWriter);
		messageWriter.Write(value);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void SnapTo(Vector2 position)
	{
		ushort minSid = (ushort)(lastSequenceId + 2);
		SnapTo(position, minSid);
	}

	public void ClearPositionQueues()
	{
		if (AmOwner)
		{
			sendQueue.Clear();
		}
		else
		{
			incomingPosQueue.Clear();
		}
	}

	private bool IsInMiddleOfAnimationThatMakesPlayerInvisible()
	{
		if (!myPlayer.MyPhysics.Animations.IsPlayingEnterVentAnimation())
		{
			return myPlayer.walkingToVent;
		}
		return true;
	}

	private unsafe void SnapTo(Vector2 position, ushort minSid)
	{
		if (NetHelpers.SidGreaterThan(minSid, lastSequenceId))
		{
			if (IsInMiddleOfAnimationThatMakesPlayerInvisible())
			{
				tempSnapPosition = position;
				return;
			}
			ClearPositionQueues();
			lastSequenceId = minSid;
			tempSnapPosition = null;
			Transform transform = base.transform;
			Vector2 vector = (body.position = position);
			transform.position = vector;
			body.linearVelocity = Vector2.zero;
		}
	}

	private unsafe void FixedUpdate()
	{
		if (isPaused || !myPlayer)
		{
			return;
		}
		if (AmOwner)
		{
			if (HasMoved())
			{
				sendQueue.Enqueue(body.position);
				SetDirtyBit(2u);
			}
			return;
		}
		if (tempSnapPosition.HasValue && !IsInMiddleOfAnimationThatMakesPlayerInvisible())
		{
			SnapTo(tempSnapPosition.Value);
			tempSnapPosition = null;
		}
		if (incomingPosQueue.Count >= 1)
		{
			SkipExcessiveFrames();
			SetMovementSmoothingModifier();
			MoveTowardNextPoint();
		}
	}

	private bool HasMoved()
	{
		float num = 0f;
		num = ((!(body != null)) ? Vector2.Distance(transform.position, lastPosSent) : Vector2.Distance(body.position, lastPosSent));
		if (num > 0.0001f)
		{
			return true;
		}
		return false;
	}

	public override void HandleRpc(byte callId, MessageReader reader)
	{
		if (isActiveAndEnabled && callId == 21)
		{
			Vector2 position = NetHelpers.ReadVector2(reader);
			ushort minSid = reader.ReadUInt16();
			SnapTo(position, minSid);
		}
	}

	public override void ClearOrDecrementDirt()
	{
		if (DirtyBits != 0)
		{
			DirtyBits--;
		}
	}

	public override bool Serialize(MessageWriter writer, bool initialState)
	{
		if (isPaused)
		{
			return false;
		}
		if (initialState)
		{
			writer.Write(lastSequenceId);
			NetHelpers.WriteVector2(body.position, writer);
			return true;
		}
		if (!isActiveAndEnabled)
		{
			ClearDirtyBits();
			return false;
		}
		if (sendQueue.Count == 0)
		{
			sendQueue.Enqueue(lastPosSent);
		}
		lastSequenceId++;
		writer.Write(lastSequenceId);
		ushort num = (ushort)sendQueue.Count;
		writer.WritePacked(num);
		foreach (Vector2 item in sendQueue)
		{
			NetHelpers.WriteVector2(item, writer);
			lastPosSent = item;
		}
		sendQueue.Clear();
		lastSequenceId += (ushort)(num - 1);
		return true;
	}

	public unsafe override void Deserialize(MessageReader reader, bool initialState)
	{
		if (isPaused)
		{
			return;
		}
		if (initialState)
		{
			lastSequenceId = reader.ReadUInt16();
			transform.position = NetHelpers.ReadVector2(reader);
			incomingPosQueue.Clear();
			incomingPosQueue.Enqueue(transform.position);
		}
		else
		{
			if (AmOwner)
			{
				return;
			}
			ushort num = reader.ReadUInt16();
			int num2 = reader.ReadPackedInt32();
			Vector2 value = ((incomingPosQueue.Count <= 0) ? body.position : incomingPosQueue.Last());
			for (int i = 0; i < num2; i++)
			{
				ushort newSid = (ushort)(num + i);
				Vector2 vector = NetHelpers.ReadVector2(reader);
				if (NetHelpers.SidGreaterThan(newSid, lastSequenceId))
				{
					lastSequenceId = newSid;
					incomingPosQueue.Enqueue(vector);
					debugTargetPositions.AddPt(vector);
					value = vector;
				}
			}
			debugTruePositions.AddPt(transform.position);
			if (IsInMiddleOfAnimationThatMakesPlayerInvisible())
			{
				tempSnapPosition = value;
			}
			else
			{
				tempSnapPosition = null;
			}
		}
	}

	private void MoveTowardNextPoint()
	{
		Vector2 vector = incomingPosQueue.Peek();
		Vector2 position = body.position;
		Vector2 vector2 = vector - position;
		if (ShouldExtendCurrentFrame(vector, position))
		{
			debugNetLogger.SetSlowbandingLog(isSlowbanding: true);
			Vector2 vector3 = vector2.normalized * idealSpeed * rubberbandModifier;
			vector3 = Vector2.ClampMagnitude(vector3, 10f);
			body.linearVelocity = vector3;
			lastPosition = body.position;
			return;
		}
		debugNetLogger.SetSlowbandingLog(isSlowbanding: false);
		if (incomingPosQueue.Count <= 1)
		{
			if (Vector2.Distance(body.position, vector) > 0.01f)
			{
				body.position = vector;
				body.linearVelocity = Vector2.zero;
				lastPosition = body.position;
			}
			return;
		}
		debugPopPositions.AddPt(position);
		Vector2 vector4 = incomingPosQueue.Dequeue();
		if (Vector2.Distance(body.position, vector4) > 0.05f)
		{
			body.position = vector4;
		}
		vector = incomingPosQueue.Peek();
		position = body.position;
		vector2 = vector - position;
		idealSpeed = vector2.magnitude / Time.fixedDeltaTime;
		Vector2 vector5 = vector2.normalized * idealSpeed * rubberbandModifier;
		vector5 = Vector2.ClampMagnitude(vector5, 10f);
		body.linearVelocity = vector5;
		lastPosition = body.position;
	}

	private unsafe void SkipExcessiveFrames()
	{
		if (incomingPosQueue.Count < 12)
		{
			return;
		}
		if ((bool)body)
		{
			body.position = incomingPosQueue.Peek();
			MoveTowardNextPoint();
			if (incomingPosQueue.Count >= 14)
			{
				body.position = incomingPosQueue.Peek();
				MoveTowardNextPoint();
			}
		}
		else
		{
			transform.position = incomingPosQueue.Dequeue();
		}
	}

	private bool ShouldExtendCurrentFrame(Vector2 nextPos, Vector2 currentPos)
	{
		if (!DidPassPosition(nextPos, lastPosition, currentPos))
		{
			return incomingPosQueue.Count <= 5;
		}
		return false;
	}

	private bool DidPassPosition(Vector2 nextPos, Vector2 lastPos, Vector2 currentPos)
	{
		float num = Vector2.Distance(lastPos, currentPos);
		float num2 = Vector2.Distance(currentPos, nextPos);
		float num3 = Vector2.Distance(lastPos, nextPos);
		return Mathf.Abs(num - (num3 + num2)) < 0.003f;
	}

	private void SetMovementSmoothingModifier()
	{
		float b = ((incomingPosQueue.Count <= 5) ? 0.5f : 0.995f);
		rubberbandModifier = Mathf.Lerp(rubberbandModifier, b, Time.fixedDeltaTime * 3f);
	}

	public CustomNetworkTransform()
	{ }
}
