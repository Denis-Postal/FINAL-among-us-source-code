using System;
using System.Collections;
using System.IO;
using Epic.OnlineServices;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.PlayerDataStorage;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerStorageManager : DestroyableSingleton<PlayerStorageManager>
{
	public struct CloudPlayerPrefs
	{
		public string ActivePodType;

		public string receiptRedemptionAccounts;

		public uint PlatformsLimitedItemsValidated;
	}

	private const int MaxEOSChunkLength = 40960;

	private const string PlayerPrefsFilename = "player.prefs";

	private PlayerDataStorageInterface playerStorage;

	public Coroutine saveRoutine;

	public CloudPlayerPrefs PlayerPrefs { get; private set; }

	public UpdateState PlayerStorageLoadState { get; private set; }

	public void RefreshAll(PlatformInterface platformInterface)
	{
		if (platformInterface == null)
		{
			Debug.LogWarning("Player Storage: Platform Interface was null.");
			PlayerStorageLoadState = UpdateState.Success;
			return;
		}
		playerStorage = platformInterface.GetPlayerDataStorageInterface();
		if (playerStorage == null)
		{
			Debug.LogWarning("Player storage null, setting state to Success.");
			PlayerStorageLoadState = UpdateState.Success;
			return;
		}
		try
		{
			ReadFileOptions readOptions = new ReadFileOptions
			{
				ReadChunkLengthBytes = 40960u,
				Filename = "player.prefs",
				LocalUserId = ProductUserId.FromString(DestroyableSingleton<EOSManager>.Instance.ProductUserId)
			};
			MemoryStream clientData = new MemoryStream();
			readOptions.ReadFileDataCallback = OnReadChunkFromEOS;
			playerStorage.ReadFile(ref readOptions, clientData, OnReadPlayerPrefsComplete);
		}
		catch (Exception ex)
		{
			Debug.LogWarning("PlayerStorage ReadFile exception: " + ex.Message);
		}
		PlayerStorageLoadState = UpdateState.Success;
	}

	public void SaveChanges(CloudPlayerPrefs newPrefs)
	{
		PlayerPrefs = newPrefs;
		if (saveRoutine == null)
		{
			saveRoutine = StartCoroutine(CoSaveChanges());
		}
	}

	private IEnumerator CoSaveChanges()
	{
		yield return new WaitForSeconds(5f);
		saveRoutine = null;
		WriteFileOptions writeOptions = new WriteFileOptions
		{
			ChunkLengthBytes = 40960u,
			Filename = "player.prefs",
			LocalUserId = ProductUserId.FromString(DestroyableSingleton<EOSManager>.Instance.ProductUserId),
			WriteFileDataCallback = OnWriteChunkToEOS
		};
		playerStorage?.WriteFile(ref writeOptions, ObjectToStream(PlayerPrefs), OnWriteFileComplete);
	}

	private WriteResult OnWriteChunkToEOS(ref WriteFileDataCallbackInfo data, out ArraySegment<byte> outDataBuffer)
	{
		Stream stream = (Stream)data.ClientData;
		Math.Min(stream.Length - stream.Position, 40960L);
		outDataBuffer = default;
		if (outDataBuffer.Count > 0)
		{
			stream.Read(outDataBuffer.ToArray(), 0, outDataBuffer.Count);
		}
		if (stream.Position >= stream.Length)
		{
			return WriteResult.CompleteRequest;
		}
		return WriteResult.ContinueWriting;
	}

	private void OnWriteFileComplete(ref WriteFileCallbackInfo data)
	{
		using ((MemoryStream)data.ClientData)
		{
			if (data.ResultCode != Result.Success)
			{
				Debug.LogError($"Failed to store EOS file {data.Filename}: {data.ResultCode}");
			}
		}
	}

	private ReadResult OnReadChunkFromEOS(ref ReadFileDataCallbackInfo data)
	{
		((Stream)data.ClientData).Write(data.DataChunk.ToArray(), 0, data.DataChunk.Count);
		return ReadResult.ContinueReading;
	}

	private void OnReadPlayerPrefsComplete(ref ReadFileCallbackInfo data)
	{
		using MemoryStream stream = (MemoryStream)data.ClientData;
		OnReadPlayerPrefsCompleteInternal(stream, data.ResultCode);
	}

	private void OnReadPlayerPrefsCompleteInternal(MemoryStream stream, Result resultCode)
	{
		if (resultCode != Result.Success)
		{
			Debug.LogWarning("Didn't read EOS player prefs: " + resultCode);
			PlayerStorageLoadState = UpdateState.Success;
			return;
		}
		stream.Position = 0L;
		using StreamReader streamReader = new StreamReader(stream);
		string text = streamReader.ReadToEnd();
		try
		{
			PlayerPrefs = JsonConvert.DeserializeObject<CloudPlayerPrefs>(text);
		}
		catch
		{
			Debug.LogWarning("Got invalid data from EOS player prefs: " + text);
			PlayerStorageLoadState = UpdateState.Failed;
			return;
		}
		PlayerStorageLoadState = UpdateState.Success;
	}

	public IEnumerator WaitForLoad()
	{
		while (PlayerStorageLoadState == UpdateState.Connecting)
		{
			yield return null;
		}
	}

	private static MemoryStream ObjectToStream(object dataToStream)
	{
		string value = JsonConvert.SerializeObject(dataToStream);
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter streamWriter = new StreamWriter(memoryStream);
		streamWriter.Write(value);
		streamWriter.Flush();
		memoryStream.Position = 0L;
		return memoryStream;
	}
}
