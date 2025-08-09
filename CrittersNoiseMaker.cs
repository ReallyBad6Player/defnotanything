using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class CrittersNoiseMaker : CrittersToolThrowable
{
	// Token: 0x06000223 RID: 547 RVA: 0x0000D872 File Offset: 0x0000BA72
	protected override void OnImpact(Vector3 hitPosition, Vector3 hitNormal)
	{
		if (CrittersManager.instance.LocalAuthority())
		{
			if (this.destroyOnImpact || this.playOnce)
			{
				this.PlaySingleNoise();
				return;
			}
			this.StartPlayingRepeatNoise();
		}
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000D89F File Offset: 0x0000BA9F
	protected override void OnImpactCritter(CrittersPawn impactedCritter)
	{
		this.OnImpact(impactedCritter.transform.position, impactedCritter.transform.up);
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000D8BD File Offset: 0x0000BABD
	protected override void OnPickedUp()
	{
		this.StopPlayRepeatNoise();
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000D8C8 File Offset: 0x0000BAC8
	private void PlaySingleNoise()
	{
		CrittersLoudNoise crittersLoudNoise = (CrittersLoudNoise)CrittersManager.instance.SpawnActor(CrittersActor.CrittersActorType.LoudNoise, this.soundSubIndex);
		if (crittersLoudNoise == null)
		{
			return;
		}
		crittersLoudNoise.MoveActor(base.transform.position, base.transform.rotation, false, true, true);
		crittersLoudNoise.SetImpulseVelocity(Vector3.zero, Vector3.zero);
		CrittersManager.instance.TriggerEvent(CrittersManager.CritterEvent.NoiseMakerTriggered, this.actorId, base.transform.position);
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000D945 File Offset: 0x0000BB45
	private void StartPlayingRepeatNoise()
	{
		this.StopPlayRepeatNoise();
		this.repeatPlayNoise = base.StartCoroutine(this.PlayRepeatNoise());
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0000D95F File Offset: 0x0000BB5F
	private void StopPlayRepeatNoise()
	{
		if (this.repeatPlayNoise != null)
		{
			base.StopCoroutine(this.repeatPlayNoise);
			this.repeatPlayNoise = null;
		}
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0000D97C File Offset: 0x0000BB7C
	private IEnumerator PlayRepeatNoise()
	{
		int num = Mathf.FloorToInt(this.repeatNoiseDuration / this.repeatNoiseRate);
		int num2;
		for (int i = num; i > 0; i = num2 - 1)
		{
			this.PlaySingleNoise();
			yield return new WaitForSeconds(this.repeatNoiseRate);
			num2 = i;
		}
		if (this.destroyAfterPlayingRepeatNoise)
		{
			this.shouldDisable = true;
		}
		yield break;
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000D98B File Offset: 0x0000BB8B
	public CrittersNoiseMaker()
	{
	}

	// Token: 0x04000282 RID: 642
	[Header("Noise Maker")]
	public int soundSubIndex = 3;

	// Token: 0x04000283 RID: 643
	public bool playOnce = true;

	// Token: 0x04000284 RID: 644
	public float repeatNoiseDuration;

	// Token: 0x04000285 RID: 645
	public float repeatNoiseRate;

	// Token: 0x04000286 RID: 646
	public bool destroyAfterPlayingRepeatNoise = true;

	// Token: 0x04000287 RID: 647
	private Coroutine repeatPlayNoise;

	// Token: 0x02000061 RID: 97
	[CompilerGenerated]
	private sealed class <PlayRepeatNoise>d__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0600022B RID: 555 RVA: 0x0000D9A8 File Offset: 0x0000BBA8
		[DebuggerHidden]
		public <PlayRepeatNoise>d__12(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000D9B8 File Offset: 0x0000BBB8
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			CrittersNoiseMaker crittersNoiseMaker = this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this.<>1__state = -1;
				int num2 = i;
				i = num2 - 1;
			}
			else
			{
				this.<>1__state = -1;
				int num3 = Mathf.FloorToInt(crittersNoiseMaker.repeatNoiseDuration / crittersNoiseMaker.repeatNoiseRate);
				i = num3;
			}
			if (i <= 0)
			{
				if (crittersNoiseMaker.destroyAfterPlayingRepeatNoise)
				{
					crittersNoiseMaker.shouldDisable = true;
				}
				return false;
			}
			crittersNoiseMaker.PlaySingleNoise();
			this.<>2__current = new WaitForSeconds(crittersNoiseMaker.repeatNoiseRate);
			this.<>1__state = 1;
			return true;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000DA4F File Offset: 0x0000BC4F
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000DA4F File Offset: 0x0000BC4F
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x04000288 RID: 648
		private int <>1__state;

		// Token: 0x04000289 RID: 649
		private object <>2__current;

		// Token: 0x0400028A RID: 650
		public CrittersNoiseMaker <>4__this;

		// Token: 0x0400028B RID: 651
		private int <i>5__2;
	}
}
