using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class CritterDespawner : MonoBehaviour
{
	// Token: 0x060000CE RID: 206 RVA: 0x00005B26 File Offset: 0x00003D26
	public void DespawnAllCritters()
	{
		CrittersManager.instance.QueueDespawnAllCritters();
	}

	// Token: 0x060000CF RID: 207 RVA: 0x000026E9 File Offset: 0x000008E9
	public CritterDespawner()
	{
	}
}
