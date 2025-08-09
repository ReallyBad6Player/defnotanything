﻿using System;
using UnityEngine;

// Token: 0x02000B0D RID: 2829
public class AssetContentAPI : ScriptableObject
{
	// Token: 0x06004418 RID: 17432 RVA: 0x00155954 File Offset: 0x00153B54
	public AssetContentAPI()
	{
	}

	// Token: 0x04004E6A RID: 20074
	public string bundleName;

	// Token: 0x04004E6B RID: 20075
	public LazyLoadReference<TextAsset> bundleFile;

	// Token: 0x04004E6C RID: 20076
	public Object[] assets = new Object[0];
}
