﻿using System;
using System.Collections;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000420 RID: 1056
public class CosmeticsControllerUpdateStand : MonoBehaviour
{
	// Token: 0x06001996 RID: 6550 RVA: 0x00089BF0 File Offset: 0x00087DF0
	public GameObject ReturnChildWithCosmeticNameMatch(Transform parentTransform)
	{
		GameObject gameObject = null;
		using (IEnumerator enumerator = parentTransform.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Transform child = (Transform)enumerator.Current;
				if (child.gameObject.activeInHierarchy && this.cosmeticsController.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => child.name == x.itemName) > -1)
				{
					return child.gameObject;
				}
				gameObject = this.ReturnChildWithCosmeticNameMatch(child);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x000026E9 File Offset: 0x000008E9
	public CosmeticsControllerUpdateStand()
	{
	}

	// Token: 0x040021F0 RID: 8688
	public CosmeticsController cosmeticsController;

	// Token: 0x040021F1 RID: 8689
	public bool FailEntitlement;

	// Token: 0x040021F2 RID: 8690
	public bool PlayerUnlocked;

	// Token: 0x040021F3 RID: 8691
	public bool ItemNotGrantedYet;

	// Token: 0x040021F4 RID: 8692
	public bool ItemSuccessfullyGranted;

	// Token: 0x040021F5 RID: 8693
	public bool AttemptToConsumeEntitlement;

	// Token: 0x040021F6 RID: 8694
	public bool EntitlementSuccessfullyConsumed;

	// Token: 0x040021F7 RID: 8695
	public bool LockSuccessfullyCleared;

	// Token: 0x040021F8 RID: 8696
	public bool RunDebug;

	// Token: 0x040021F9 RID: 8697
	public Transform textParent;

	// Token: 0x040021FA RID: 8698
	private CosmeticsController.CosmeticItem outItem;

	// Token: 0x040021FB RID: 8699
	public HeadModel[] inventoryHeadModels;

	// Token: 0x040021FC RID: 8700
	public string headModelsPrefabPath;

	// Token: 0x02000421 RID: 1057
	[CompilerGenerated]
	private sealed class <>c__DisplayClass13_0
	{
		// Token: 0x06001998 RID: 6552 RVA: 0x00002050 File Offset: 0x00000250
		public <>c__DisplayClass13_0()
		{
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x00089CAC File Offset: 0x00087EAC
		internal bool <ReturnChildWithCosmeticNameMatch>b__0(CosmeticsController.CosmeticItem x)
		{
			return this.child.name == x.itemName;
		}

		// Token: 0x040021FD RID: 8701
		public Transform child;
	}
}
