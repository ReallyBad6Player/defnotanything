using System;
using UnityEngine;

// Token: 0x02000A98 RID: 2712
public class BetterBaker : MonoBehaviour
{
	// Token: 0x060041CE RID: 16846 RVA: 0x000026E9 File Offset: 0x000008E9
	public BetterBaker()
	{
	}

	// Token: 0x04004D32 RID: 19762
	public string bakeryLightmapDirectory;

	// Token: 0x04004D33 RID: 19763
	public string dayNightLightmapsDirectory;

	// Token: 0x04004D34 RID: 19764
	public GameObject[] allLights;

	// Token: 0x02000A99 RID: 2713
	public struct LightMapMap
	{
		// Token: 0x04004D35 RID: 19765
		public string timeOfDayName;

		// Token: 0x04004D36 RID: 19766
		public GameObject lightObject;
	}
}
