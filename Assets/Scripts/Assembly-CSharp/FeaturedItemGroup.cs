using System;
using System.Collections.Generic;
using Cpp2ILInjected;

[Serializable]
public class FeaturedItemGroup
{
	public LimitedTimeStartEnd limitedTime;

	public List<CosmeticData> featuredItems;

	public List<BundleData> featuredBundles;

	public List<CosmicubeData> featuredCubes;

	public unsafe bool IsAlwaysAvailable => !limitedTime.IsLimitedTime();

	public unsafe LimitedTimeStartEnd LimitedTimeAvailable => limitedTime;
}
