using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000490 RID: 1168
public class BubbleGumEvents : MonoBehaviour
{
	// Token: 0x06001CF3 RID: 7411 RVA: 0x0009BD75 File Offset: 0x00099F75
	private void OnEnable()
	{
		this._edible.onBiteWorld.AddListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._edible.onBiteView.AddListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x0009BDAF File Offset: 0x00099FAF
	private void OnDisable()
	{
		this._edible.onBiteWorld.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._edible.onBiteView.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x0009BDE9 File Offset: 0x00099FE9
	public void OnBiteView(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, true);
	}

	// Token: 0x06001CF6 RID: 7414 RVA: 0x0009BDF4 File Offset: 0x00099FF4
	public void OnBiteWorld(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, false);
	}

	// Token: 0x06001CF7 RID: 7415 RVA: 0x0009BE00 File Offset: 0x0009A000
	public void OnBite(VRRig rig, int nextState, bool isViewRig)
	{
		GorillaTagger instance = GorillaTagger.Instance;
		GameObject gameObject = null;
		if (isViewRig && instance != null)
		{
			gameObject = instance.gameObject;
		}
		else if (!isViewRig)
		{
			gameObject = rig.gameObject;
		}
		if (!BubbleGumEvents.gTargetCache.TryGetValue(gameObject, out this._bubble))
		{
			this._bubble = gameObject.GetComponentsInChildren<GumBubble>(true).FirstOrDefault((GumBubble g) => g.transform.parent.name == "$gum");
			if (isViewRig)
			{
				this._bubble.audioSource = instance.offlineVRRig.tagSound;
				this._bubble.targetScale = Vector3.one * 1.36f;
			}
			else
			{
				this._bubble.audioSource = rig.tagSound;
				this._bubble.targetScale = Vector3.one * 2f;
			}
			BubbleGumEvents.gTargetCache.Add(gameObject, this._bubble);
		}
		GumBubble bubble = this._bubble;
		if (bubble != null)
		{
			bubble.transform.parent.gameObject.SetActive(true);
		}
		GumBubble bubble2 = this._bubble;
		if (bubble2 == null)
		{
			return;
		}
		bubble2.InflateDelayed();
	}

	// Token: 0x06001CF8 RID: 7416 RVA: 0x000026E9 File Offset: 0x000008E9
	public BubbleGumEvents()
	{
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x0009BF1F File Offset: 0x0009A11F
	// Note: this type is marked as 'beforefieldinit'.
	static BubbleGumEvents()
	{
	}

	// Token: 0x0400255A RID: 9562
	[SerializeField]
	private EdibleHoldable _edible;

	// Token: 0x0400255B RID: 9563
	[SerializeField]
	private GumBubble _bubble;

	// Token: 0x0400255C RID: 9564
	private static Dictionary<GameObject, GumBubble> gTargetCache = new Dictionary<GameObject, GumBubble>(16);

	// Token: 0x02000491 RID: 1169
	public enum EdibleState
	{
		// Token: 0x0400255E RID: 9566
		A = 1,
		// Token: 0x0400255F RID: 9567
		B,
		// Token: 0x04002560 RID: 9568
		C = 4,
		// Token: 0x04002561 RID: 9569
		D = 8
	}

	// Token: 0x02000492 RID: 1170
	[CompilerGenerated]
	[Serializable]
	private sealed class <>c
	{
		// Token: 0x06001CFA RID: 7418 RVA: 0x0009BF2D File Offset: 0x0009A12D
		// Note: this type is marked as 'beforefieldinit'.
		static <>c()
		{
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x00002050 File Offset: 0x00000250
		public <>c()
		{
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x0009BF39 File Offset: 0x0009A139
		internal bool <OnBite>b__7_0(GumBubble g)
		{
			return g.transform.parent.name == "$gum";
		}

		// Token: 0x04002562 RID: 9570
		public static readonly BubbleGumEvents.<>c <>9 = new BubbleGumEvents.<>c();

		// Token: 0x04002563 RID: 9571
		public static Func<GumBubble, bool> <>9__7_0;
	}
}
