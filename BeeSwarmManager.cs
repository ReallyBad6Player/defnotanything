using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class BeeSwarmManager : MonoBehaviour
{
	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600069C RID: 1692 RVA: 0x00026AFE File Offset: 0x00024CFE
	// (set) Token: 0x0600069D RID: 1693 RVA: 0x00026B06 File Offset: 0x00024D06
	public BeePerchPoint BeeHive
	{
		[CompilerGenerated]
		get
		{
			return this.<BeeHive>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<BeeHive>k__BackingField = value;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x0600069E RID: 1694 RVA: 0x00026B0F File Offset: 0x00024D0F
	// (set) Token: 0x0600069F RID: 1695 RVA: 0x00026B17 File Offset: 0x00024D17
	public float BeeSpeed
	{
		[CompilerGenerated]
		get
		{
			return this.<BeeSpeed>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<BeeSpeed>k__BackingField = value;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x060006A0 RID: 1696 RVA: 0x00026B20 File Offset: 0x00024D20
	// (set) Token: 0x060006A1 RID: 1697 RVA: 0x00026B28 File Offset: 0x00024D28
	public float BeeMaxTravelTime
	{
		[CompilerGenerated]
		get
		{
			return this.<BeeMaxTravelTime>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<BeeMaxTravelTime>k__BackingField = value;
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x060006A2 RID: 1698 RVA: 0x00026B31 File Offset: 0x00024D31
	// (set) Token: 0x060006A3 RID: 1699 RVA: 0x00026B39 File Offset: 0x00024D39
	public float BeeAcceleration
	{
		[CompilerGenerated]
		get
		{
			return this.<BeeAcceleration>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<BeeAcceleration>k__BackingField = value;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x060006A4 RID: 1700 RVA: 0x00026B42 File Offset: 0x00024D42
	// (set) Token: 0x060006A5 RID: 1701 RVA: 0x00026B4A File Offset: 0x00024D4A
	public float BeeJitterStrength
	{
		[CompilerGenerated]
		get
		{
			return this.<BeeJitterStrength>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<BeeJitterStrength>k__BackingField = value;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00026B53 File Offset: 0x00024D53
	// (set) Token: 0x060006A7 RID: 1703 RVA: 0x00026B5B File Offset: 0x00024D5B
	public float BeeJitterDamping
	{
		[CompilerGenerated]
		get
		{
			return this.<BeeJitterDamping>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<BeeJitterDamping>k__BackingField = value;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060006A8 RID: 1704 RVA: 0x00026B64 File Offset: 0x00024D64
	// (set) Token: 0x060006A9 RID: 1705 RVA: 0x00026B6C File Offset: 0x00024D6C
	public float BeeMaxJitterRadius
	{
		[CompilerGenerated]
		get
		{
			return this.<BeeMaxJitterRadius>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<BeeMaxJitterRadius>k__BackingField = value;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060006AA RID: 1706 RVA: 0x00026B75 File Offset: 0x00024D75
	// (set) Token: 0x060006AB RID: 1707 RVA: 0x00026B7D File Offset: 0x00024D7D
	public float BeeNearDestinationRadius
	{
		[CompilerGenerated]
		get
		{
			return this.<BeeNearDestinationRadius>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<BeeNearDestinationRadius>k__BackingField = value;
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060006AC RID: 1708 RVA: 0x00026B86 File Offset: 0x00024D86
	// (set) Token: 0x060006AD RID: 1709 RVA: 0x00026B8E File Offset: 0x00024D8E
	public float AvoidPointRadius
	{
		[CompilerGenerated]
		get
		{
			return this.<AvoidPointRadius>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<AvoidPointRadius>k__BackingField = value;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060006AE RID: 1710 RVA: 0x00026B97 File Offset: 0x00024D97
	// (set) Token: 0x060006AF RID: 1711 RVA: 0x00026B9F File Offset: 0x00024D9F
	public float BeeMinFlowerDuration
	{
		[CompilerGenerated]
		get
		{
			return this.<BeeMinFlowerDuration>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<BeeMinFlowerDuration>k__BackingField = value;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00026BA8 File Offset: 0x00024DA8
	// (set) Token: 0x060006B1 RID: 1713 RVA: 0x00026BB0 File Offset: 0x00024DB0
	public float BeeMaxFlowerDuration
	{
		[CompilerGenerated]
		get
		{
			return this.<BeeMaxFlowerDuration>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<BeeMaxFlowerDuration>k__BackingField = value;
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00026BB9 File Offset: 0x00024DB9
	// (set) Token: 0x060006B3 RID: 1715 RVA: 0x00026BC1 File Offset: 0x00024DC1
	public float GeneralBuzzRange
	{
		[CompilerGenerated]
		get
		{
			return this.<GeneralBuzzRange>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<GeneralBuzzRange>k__BackingField = value;
		}
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x00026BCC File Offset: 0x00024DCC
	private void Awake()
	{
		this.bees = new List<AnimatedBee>(this.numBees);
		for (int i = 0; i < this.numBees; i++)
		{
			AnimatedBee animatedBee = default(AnimatedBee);
			animatedBee.InitVisual(this.beePrefab, this);
			this.bees.Add(animatedBee);
		}
		this.playerCamera = Camera.main.transform;
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x00026C30 File Offset: 0x00024E30
	private void Start()
	{
		foreach (XSceneRef xsceneRef in this.flowerSections)
		{
			GameObject gameObject;
			if (xsceneRef.TryResolve(out gameObject))
			{
				foreach (BeePerchPoint beePerchPoint in gameObject.GetComponentsInChildren<BeePerchPoint>())
				{
					this.allPerchPoints.Add(beePerchPoint);
				}
			}
		}
		this.OnSeedChange();
		RandomTimedSeedManager.instance.AddCallbackOnSeedChanged(new Action(this.OnSeedChange));
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x00026CB0 File Offset: 0x00024EB0
	private void OnDestroy()
	{
		RandomTimedSeedManager.instance.RemoveCallbackOnSeedChanged(new Action(this.OnSeedChange));
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x00026CC8 File Offset: 0x00024EC8
	private void Update()
	{
		Vector3 position = this.playerCamera.transform.position;
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		float num = 1f / (float)this.bees.Count;
		float num2 = float.PositiveInfinity;
		float num3 = this.GeneralBuzzRange * this.GeneralBuzzRange;
		int num4 = 0;
		for (int i = 0; i < this.bees.Count; i++)
		{
			AnimatedBee animatedBee = this.bees[i];
			animatedBee.UpdateVisual(RandomTimedSeedManager.instance.currentSyncTime, this);
			Vector3 position2 = animatedBee.visual.transform.position;
			float sqrMagnitude = (position2 - position).sqrMagnitude;
			if (sqrMagnitude < num2)
			{
				vector = position2;
				num2 = sqrMagnitude;
			}
			if (sqrMagnitude < num3)
			{
				vector2 += position2;
				num4++;
			}
			this.bees[i] = animatedBee;
		}
		this.nearbyBeeBuzz.transform.position = vector;
		if (num4 > 0)
		{
			this.generalBeeBuzz.transform.position = vector2 / (float)num4;
			this.generalBeeBuzz.enabled = true;
			return;
		}
		this.generalBeeBuzz.enabled = false;
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x00026DF8 File Offset: 0x00024FF8
	private void OnSeedChange()
	{
		SRand srand = new SRand(RandomTimedSeedManager.instance.seed);
		List<BeePerchPoint> list = new List<BeePerchPoint>(this.allPerchPoints.Count);
		List<BeePerchPoint> list2 = new List<BeePerchPoint>(this.loopSizePerBee);
		List<float> list3 = new List<float>(this.loopSizePerBee);
		for (int i = 0; i < this.bees.Count; i++)
		{
			AnimatedBee animatedBee = this.bees[i];
			list2 = new List<BeePerchPoint>(this.loopSizePerBee);
			list3 = new List<float>(this.loopSizePerBee);
			this.PickPoints(this.loopSizePerBee, list, this.allPerchPoints, ref srand, list2);
			for (int j = 0; j < list2.Count; j++)
			{
				list3.Add(srand.NextFloat(this.BeeMinFlowerDuration, this.BeeMaxFlowerDuration));
			}
			animatedBee.InitRoute(list2, list3, this);
			animatedBee.InitRouteTimestamps();
			this.bees[i] = animatedBee;
		}
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x00026EEC File Offset: 0x000250EC
	private void PickPoints(int n, List<BeePerchPoint> pickBuffer, List<BeePerchPoint> allPerchPoints, ref SRand rand, List<BeePerchPoint> resultBuffer)
	{
		resultBuffer.Add(this.BeeHive);
		n--;
		int num = 100;
		while (pickBuffer.Count < n && num-- > 0)
		{
			n -= pickBuffer.Count;
			resultBuffer.AddRange(pickBuffer);
			pickBuffer.Clear();
			pickBuffer.AddRange(allPerchPoints);
			rand.Shuffle<BeePerchPoint>(pickBuffer);
		}
		resultBuffer.AddRange(pickBuffer.GetRange(pickBuffer.Count - n, n));
		pickBuffer.RemoveRange(pickBuffer.Count - n, n);
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x00026F6D File Offset: 0x0002516D
	public static void RegisterAvoidPoint(GameObject obj)
	{
		BeeSwarmManager.avoidPoints.Add(obj);
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00026F7A File Offset: 0x0002517A
	public static void UnregisterAvoidPoint(GameObject obj)
	{
		BeeSwarmManager.avoidPoints.Remove(obj);
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00026F88 File Offset: 0x00025188
	public BeeSwarmManager()
	{
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00026F9B File Offset: 0x0002519B
	// Note: this type is marked as 'beforefieldinit'.
	static BeeSwarmManager()
	{
	}

	// Token: 0x0400081A RID: 2074
	[SerializeField]
	private XSceneRef[] flowerSections;

	// Token: 0x0400081B RID: 2075
	[SerializeField]
	private int loopSizePerBee;

	// Token: 0x0400081C RID: 2076
	[SerializeField]
	private int numBees;

	// Token: 0x0400081D RID: 2077
	[SerializeField]
	private MeshRenderer beePrefab;

	// Token: 0x0400081E RID: 2078
	[SerializeField]
	private AudioSource nearbyBeeBuzz;

	// Token: 0x0400081F RID: 2079
	[SerializeField]
	private AudioSource generalBeeBuzz;

	// Token: 0x04000820 RID: 2080
	private GameObject[] flowerSectionsResolved;

	// Token: 0x04000821 RID: 2081
	[CompilerGenerated]
	[SerializeField]
	private BeePerchPoint <BeeHive>k__BackingField;

	// Token: 0x04000822 RID: 2082
	[CompilerGenerated]
	[SerializeField]
	private float <BeeSpeed>k__BackingField;

	// Token: 0x04000823 RID: 2083
	[CompilerGenerated]
	[SerializeField]
	private float <BeeMaxTravelTime>k__BackingField;

	// Token: 0x04000824 RID: 2084
	[CompilerGenerated]
	[SerializeField]
	private float <BeeAcceleration>k__BackingField;

	// Token: 0x04000825 RID: 2085
	[CompilerGenerated]
	[SerializeField]
	private float <BeeJitterStrength>k__BackingField;

	// Token: 0x04000826 RID: 2086
	[CompilerGenerated]
	[SerializeField]
	[Tooltip("Should be 0-1; closer to 1 = less damping")]
	private float <BeeJitterDamping>k__BackingField;

	// Token: 0x04000827 RID: 2087
	[CompilerGenerated]
	[SerializeField]
	[Tooltip("Limits how far the bee can get off course")]
	private float <BeeMaxJitterRadius>k__BackingField;

	// Token: 0x04000828 RID: 2088
	[CompilerGenerated]
	[SerializeField]
	[Tooltip("Bees stop jittering when close to their destination")]
	private float <BeeNearDestinationRadius>k__BackingField;

	// Token: 0x04000829 RID: 2089
	[CompilerGenerated]
	[SerializeField]
	private float <AvoidPointRadius>k__BackingField;

	// Token: 0x0400082A RID: 2090
	[CompilerGenerated]
	[SerializeField]
	private float <BeeMinFlowerDuration>k__BackingField;

	// Token: 0x0400082B RID: 2091
	[CompilerGenerated]
	[SerializeField]
	private float <BeeMaxFlowerDuration>k__BackingField;

	// Token: 0x0400082C RID: 2092
	[CompilerGenerated]
	[SerializeField]
	private float <GeneralBuzzRange>k__BackingField;

	// Token: 0x0400082D RID: 2093
	private List<AnimatedBee> bees;

	// Token: 0x0400082E RID: 2094
	private Transform playerCamera;

	// Token: 0x0400082F RID: 2095
	private List<BeePerchPoint> allPerchPoints = new List<BeePerchPoint>();

	// Token: 0x04000830 RID: 2096
	public static readonly List<GameObject> avoidPoints = new List<GameObject>();
}
