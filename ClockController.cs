﻿using System;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class ClockController : MonoBehaviour
{
	// Token: 0x06000473 RID: 1139 RVA: 0x00019F6A File Offset: 0x0001816A
	public ClockController()
	{
	}

	// Token: 0x0400053A RID: 1338
	public Transform Pendulum;

	// Token: 0x0400053B RID: 1339
	public float MaxAngleDeflection = 10f;

	// Token: 0x0400053C RID: 1340
	public float SpeedOfPendulum = 1f;
}
