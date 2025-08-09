using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000032 RID: 50
public class CosmeticPlaySoundOnColision : MonoBehaviour
{
	// Token: 0x060000BB RID: 187 RVA: 0x00005288 File Offset: 0x00003488
	private void Awake()
	{
		this.transferrableObject = base.GetComponentInParent<TransferrableObject>();
		this.soundLookup = new Dictionary<int, int>();
		this.audioSource = base.GetComponent<AudioSource>();
		for (int i = 0; i < this.soundIdRemappings.Length; i++)
		{
			this.soundLookup.Add(this.soundIdRemappings[i].SoundIn, this.soundIdRemappings[i].SoundOut);
		}
	}

	// Token: 0x060000BC RID: 188 RVA: 0x000052F0 File Offset: 0x000034F0
	private void OnTriggerEnter(Collider other)
	{
		GorillaSurfaceOverride gorillaSurfaceOverride;
		if (this.speed >= this.minSpeed && other.TryGetComponent<GorillaSurfaceOverride>(out gorillaSurfaceOverride))
		{
			int num;
			if (this.soundLookup.TryGetValue(gorillaSurfaceOverride.overrideIndex, out num))
			{
				this.playSound(num, this.invokeEventOnOverideSound);
				return;
			}
			this.playSound(this.defaultSound, this.invokeEventOnDefaultSound);
		}
	}

	// Token: 0x060000BD RID: 189 RVA: 0x0000534C File Offset: 0x0000354C
	private void playSound(int soundIndex, bool invokeEvent)
	{
		if (soundIndex > -1 && soundIndex < GTPlayer.Instance.materialData.Count)
		{
			if (this.audioSource.isPlaying)
			{
				this.audioSource.GTStop();
				if (this.invokeEventsOnAllClients || this.transferrableObject.IsMyItem())
				{
					this.OnStopPlayback.Invoke();
				}
				if (this.crWaitForStopPlayback != null)
				{
					base.StopCoroutine(this.crWaitForStopPlayback);
					this.crWaitForStopPlayback = null;
				}
			}
			this.audioSource.clip = GTPlayer.Instance.materialData[soundIndex].audio;
			this.audioSource.GTPlay();
			if (invokeEvent && (this.invokeEventsOnAllClients || this.transferrableObject.IsMyItem()))
			{
				this.OnStartPlayback.Invoke();
				this.crWaitForStopPlayback = base.StartCoroutine(this.waitForStopPlayback());
			}
		}
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00005428 File Offset: 0x00003628
	private IEnumerator waitForStopPlayback()
	{
		while (this.audioSource.isPlaying)
		{
			yield return null;
		}
		if (this.invokeEventsOnAllClients || this.transferrableObject.IsMyItem())
		{
			this.OnStopPlayback.Invoke();
		}
		this.crWaitForStopPlayback = null;
		yield break;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00005437 File Offset: 0x00003637
	private void FixedUpdate()
	{
		this.speed = Vector3.Distance(base.transform.position, this.previousFramePosition) * Time.fixedDeltaTime * 100f;
		this.previousFramePosition = base.transform.position;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00005472 File Offset: 0x00003672
	public CosmeticPlaySoundOnColision()
	{
	}

	// Token: 0x040000CE RID: 206
	[GorillaSoundLookup]
	[SerializeField]
	private int defaultSound = 1;

	// Token: 0x040000CF RID: 207
	[SerializeField]
	private SoundIdRemapping[] soundIdRemappings;

	// Token: 0x040000D0 RID: 208
	[SerializeField]
	private UnityEvent OnStartPlayback;

	// Token: 0x040000D1 RID: 209
	[SerializeField]
	private UnityEvent OnStopPlayback;

	// Token: 0x040000D2 RID: 210
	[SerializeField]
	private float minSpeed = 0.1f;

	// Token: 0x040000D3 RID: 211
	private TransferrableObject transferrableObject;

	// Token: 0x040000D4 RID: 212
	private Dictionary<int, int> soundLookup;

	// Token: 0x040000D5 RID: 213
	private AudioSource audioSource;

	// Token: 0x040000D6 RID: 214
	private Coroutine crWaitForStopPlayback;

	// Token: 0x040000D7 RID: 215
	private float speed;

	// Token: 0x040000D8 RID: 216
	private Vector3 previousFramePosition;

	// Token: 0x040000D9 RID: 217
	[SerializeField]
	private bool invokeEventsOnAllClients;

	// Token: 0x040000DA RID: 218
	[SerializeField]
	private bool invokeEventOnOverideSound = true;

	// Token: 0x040000DB RID: 219
	[SerializeField]
	private bool invokeEventOnDefaultSound;

	// Token: 0x02000033 RID: 51
	[CompilerGenerated]
	private sealed class <waitForStopPlayback>d__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00005493 File Offset: 0x00003693
		[DebuggerHidden]
		public <waitForStopPlayback>d__17(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000054A4 File Offset: 0x000036A4
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			CosmeticPlaySoundOnColision cosmeticPlaySoundOnColision = this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this.<>1__state = -1;
			}
			else
			{
				this.<>1__state = -1;
			}
			if (!cosmeticPlaySoundOnColision.audioSource.isPlaying)
			{
				if (cosmeticPlaySoundOnColision.invokeEventsOnAllClients || cosmeticPlaySoundOnColision.transferrableObject.IsMyItem())
				{
					cosmeticPlaySoundOnColision.OnStopPlayback.Invoke();
				}
				cosmeticPlaySoundOnColision.crWaitForStopPlayback = null;
				return false;
			}
			this.<>2__current = null;
			this.<>1__state = 1;
			return true;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000551D File Offset: 0x0000371D
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000551D File Offset: 0x0000371D
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040000DC RID: 220
		private int <>1__state;

		// Token: 0x040000DD RID: 221
		private object <>2__current;

		// Token: 0x040000DE RID: 222
		public CosmeticPlaySoundOnColision <>4__this;
	}
}
