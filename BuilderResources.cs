using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054E RID: 1358
[CreateAssetMenu(fileName = "BuilderMaterialResources", menuName = "Gorilla Tag/Builder/Resources", order = 0)]
public class BuilderResources : ScriptableObject
{
	// Token: 0x06002125 RID: 8485 RVA: 0x00010210 File Offset: 0x0000E410
	public BuilderResources()
	{
	}

	// Token: 0x04002A71 RID: 10865
	public List<BuilderResourceQuantity> quantities;
}
