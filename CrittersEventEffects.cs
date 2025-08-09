using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class CrittersEventEffects : MonoBehaviour
{
	// Token: 0x06000192 RID: 402 RVA: 0x0000A0B0 File Offset: 0x000082B0
	private void Awake()
	{
		if (this.manager == null)
		{
			GTDev.LogError<string>("CrittersEventEffects missing reference to CrittersManager", null);
			return;
		}
		this.effectResponse = new Dictionary<CrittersManager.CritterEvent, GameObject>();
		for (int i = 0; i < this.eventEffects.Length; i++)
		{
			if (this.eventEffects[i].effect != null)
			{
				this.effectResponse.Add(this.eventEffects[i].eventType, this.eventEffects[i].effect);
			}
		}
		this.manager.OnCritterEventReceived += this.HandleReceivedEvent;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000A148 File Offset: 0x00008348
	private void HandleReceivedEvent(CrittersManager.CritterEvent eventType, int sourceActor, Vector3 position, Quaternion rotation)
	{
		GameObject gameObject;
		if (this.effectResponse.TryGetValue(eventType, out gameObject))
		{
			GameObject pooled = CrittersPool.GetPooled(gameObject);
			if (pooled.IsNotNull())
			{
				pooled.transform.position = position;
				pooled.transform.rotation = rotation;
			}
		}
	}

	// Token: 0x06000194 RID: 404 RVA: 0x000026E9 File Offset: 0x000008E9
	public CrittersEventEffects()
	{
	}

	// Token: 0x040001E0 RID: 480
	public CrittersManager manager;

	// Token: 0x040001E1 RID: 481
	public CrittersEventEffects.CrittersEventResponse[] eventEffects;

	// Token: 0x040001E2 RID: 482
	private Dictionary<CrittersManager.CritterEvent, GameObject> effectResponse;

	// Token: 0x02000053 RID: 83
	[Serializable]
	public class CrittersEventResponse
	{
		// Token: 0x06000195 RID: 405 RVA: 0x00002050 File Offset: 0x00000250
		public CrittersEventResponse()
		{
		}

		// Token: 0x040001E3 RID: 483
		public CrittersManager.CritterEvent eventType;

		// Token: 0x040001E4 RID: 484
		public GameObject effect;
	}
}
