﻿using System;
using System.Runtime.CompilerServices;

// Token: 0x020008B6 RID: 2230
public class AttemptAgeUpdateResponse
{
	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x060037C7 RID: 14279 RVA: 0x001208CF File Offset: 0x0011EACF
	// (set) Token: 0x060037C8 RID: 14280 RVA: 0x001208D7 File Offset: 0x0011EAD7
	public SessionStatus Status
	{
		[CompilerGenerated]
		get
		{
			return this.<Status>k__BackingField;
		}
		[CompilerGenerated]
		set
		{
			this.<Status>k__BackingField = value;
		}
	}

	// Token: 0x060037C9 RID: 14281 RVA: 0x00002050 File Offset: 0x00000250
	public AttemptAgeUpdateResponse()
	{
	}

	// Token: 0x0400448B RID: 17547
	[CompilerGenerated]
	private SessionStatus <Status>k__BackingField;
}
