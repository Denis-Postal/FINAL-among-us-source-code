using System;
namespace AmongUs.GameOptions
{
	[Flags]
	public enum GameKeywords : uint
	{
		All = 0u,
		English = 256u,
		SpanishLA = 2u,
		Brazilian = 2048u,
		Portuguese = 16u,
		Korean = 4u,
		Russian = 8u,
		Dutch = 4096u,
		Filipino = 64u,
		French = 8192u,
		German = 16384u,
		Italian = 32768u,
		Japanese = 512u,
		SpanishEU = 1024u,
		Arabic = 32u,
		Polish = 128u,
		SChinese = 65536u,
		TChinese = 131072u,
		Irish = 262144u,
		Other = 1u
	}
}
