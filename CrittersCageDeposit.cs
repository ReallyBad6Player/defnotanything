using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class CrittersCageDeposit : CrittersActorDeposit
{
	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600017F RID: 383 RVA: 0x00009B24 File Offset: 0x00007D24
	// (remove) Token: 0x06000180 RID: 384 RVA: 0x00009B5C File Offset: 0x00007D5C
	public event Action<Menagerie.CritterData, int> OnDepositCritter
	{
		[CompilerGenerated]
		add
		{
			Action<Menagerie.CritterData, int> action = this.OnDepositCritter;
			Action<Menagerie.CritterData, int> action2;
			do
			{
				action2 = action;
				Action<Menagerie.CritterData, int> action3 = (Action<Menagerie.CritterData, int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange<Action<Menagerie.CritterData, int>>(ref this.OnDepositCritter, action3, action2);
			}
			while (action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Menagerie.CritterData, int> action = this.OnDepositCritter;
			Action<Menagerie.CritterData, int> action2;
			do
			{
				action2 = action;
				Action<Menagerie.CritterData, int> action3 = (Action<Menagerie.CritterData, int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange<Action<Menagerie.CritterData, int>>(ref this.OnDepositCritter, action3, action2);
			}
			while (action != action2);
		}
	}

	// Token: 0x06000181 RID: 385 RVA: 0x00009B91 File Offset: 0x00007D91
	private void Awake()
	{
		this.attachPoint.OnGrabbedChild += this.StartProcessCage;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x00009BAA File Offset: 0x00007DAA
	protected override bool CanDeposit(CrittersActor depositActor)
	{
		return base.CanDeposit(depositActor) && !this.isHandlingDeposit;
	}

	// Token: 0x06000183 RID: 387 RVA: 0x00009BC0 File Offset: 0x00007DC0
	private void StartProcessCage(CrittersActor depositedActor)
	{
		this.currentCage = depositedActor;
		base.StartCoroutine(this.ProcessCage());
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00009BD6 File Offset: 0x00007DD6
	private IEnumerator ProcessCage()
	{
		this.isHandlingDeposit = true;
		bool isLocalDeposit = this.currentCage.lastGrabbedPlayer == PhotonNetwork.LocalPlayer.ActorNumber;
		this.depositAudio.GTPlayOneShot(this.depositStartSound, isLocalDeposit ? 1f : 0.5f);
		float transition = 0f;
		CrittersPawn crittersPawn = this.currentCage.GetComponentInChildren<CrittersPawn>();
		int lastGrabbedPlayer = this.currentCage.lastGrabbedPlayer;
		Menagerie.CritterData critterData;
		if (crittersPawn.IsNotNull())
		{
			critterData = new Menagerie.CritterData(crittersPawn.visuals);
		}
		else
		{
			critterData = new Menagerie.CritterData();
		}
		while (transition < this.submitDuration)
		{
			transition += Time.deltaTime;
			this.attachPoint.transform.localPosition = Vector3.Lerp(this.depositStartLocation, this.depositEndLocation, Mathf.Min(transition / this.submitDuration, 1f));
			yield return null;
		}
		if (crittersPawn.IsNotNull())
		{
			Action<Menagerie.CritterData, int> onDepositCritter = this.OnDepositCritter;
			if (onDepositCritter != null)
			{
				onDepositCritter(critterData, lastGrabbedPlayer);
			}
			CrittersActor crittersActor = crittersPawn;
			bool flag = false;
			Vector3 zero = Vector3.zero;
			crittersActor.Released(flag, default(Quaternion), zero, default(Vector3), default(Vector3));
			crittersPawn.gameObject.SetActive(false);
			this.depositAudio.GTPlayOneShot(this.depositCritterSound, isLocalDeposit ? 1f : 0.5f);
		}
		else
		{
			this.depositAudio.GTPlayOneShot(this.depositEmptySound, isLocalDeposit ? 1f : 0.5f);
		}
		this.currentCage.transform.position = Vector3.zero;
		this.currentCage.gameObject.SetActive(false);
		this.currentCage = null;
		transition = 0f;
		while (transition < this.returnDuration)
		{
			transition += Time.deltaTime;
			this.attachPoint.transform.localPosition = Vector3.Lerp(this.depositEndLocation, this.depositStartLocation, Mathf.Min(transition / this.returnDuration, 1f));
			yield return null;
		}
		this.isHandlingDeposit = false;
		yield break;
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00009BE8 File Offset: 0x00007DE8
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.TransformPoint(this.depositStartLocation), 0.1f);
		Gizmos.DrawLine(base.transform.TransformPoint(this.depositStartLocation), base.transform.TransformPoint(this.depositEndLocation));
		Gizmos.DrawWireSphere(base.transform.TransformPoint(this.depositEndLocation), 0.1f);
	}

	// Token: 0x06000186 RID: 390 RVA: 0x00009C5C File Offset: 0x00007E5C
	public CrittersCageDeposit()
	{
	}

	// Token: 0x040001BC RID: 444
	private bool isHandlingDeposit;

	// Token: 0x040001BD RID: 445
	public Vector3 depositStartLocation;

	// Token: 0x040001BE RID: 446
	public Vector3 depositEndLocation;

	// Token: 0x040001BF RID: 447
	public float submitDuration = 0.5f;

	// Token: 0x040001C0 RID: 448
	public float returnDuration = 1f;

	// Token: 0x040001C1 RID: 449
	public AudioSource depositAudio;

	// Token: 0x040001C2 RID: 450
	public AudioClip depositStartSound;

	// Token: 0x040001C3 RID: 451
	public AudioClip depositEmptySound;

	// Token: 0x040001C4 RID: 452
	public AudioClip depositCritterSound;

	// Token: 0x040001C5 RID: 453
	private CrittersActor currentCage;

	// Token: 0x040001C6 RID: 454
	[CompilerGenerated]
	private Action<Menagerie.CritterData, int> OnDepositCritter;

	// Token: 0x0200004F RID: 79
	[CompilerGenerated]
	private sealed class <ProcessCage>d__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x06000187 RID: 391 RVA: 0x00009C7A File Offset: 0x00007E7A
		[DebuggerHidden]
		public <ProcessCage>d__16(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00009C8C File Offset: 0x00007E8C
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			CrittersCageDeposit crittersCageDeposit = this;
			switch (num)
			{
			case 0:
				this.<>1__state = -1;
				crittersCageDeposit.isHandlingDeposit = true;
				isLocalDeposit = crittersCageDeposit.currentCage.lastGrabbedPlayer == PhotonNetwork.LocalPlayer.ActorNumber;
				crittersCageDeposit.depositAudio.GTPlayOneShot(crittersCageDeposit.depositStartSound, isLocalDeposit ? 1f : 0.5f);
				transition = 0f;
				crittersPawn = crittersCageDeposit.currentCage.GetComponentInChildren<CrittersPawn>();
				lastGrabbedPlayer = crittersCageDeposit.currentCage.lastGrabbedPlayer;
				if (crittersPawn.IsNotNull())
				{
					critterData = new Menagerie.CritterData(crittersPawn.visuals);
				}
				else
				{
					critterData = new Menagerie.CritterData();
				}
				break;
			case 1:
				this.<>1__state = -1;
				break;
			case 2:
				this.<>1__state = -1;
				goto IL_0295;
			default:
				return false;
			}
			if (transition < crittersCageDeposit.submitDuration)
			{
				transition += Time.deltaTime;
				crittersCageDeposit.attachPoint.transform.localPosition = Vector3.Lerp(crittersCageDeposit.depositStartLocation, crittersCageDeposit.depositEndLocation, Mathf.Min(transition / crittersCageDeposit.submitDuration, 1f));
				this.<>2__current = null;
				this.<>1__state = 1;
				return true;
			}
			if (crittersPawn.IsNotNull())
			{
				Action<Menagerie.CritterData, int> onDepositCritter = crittersCageDeposit.OnDepositCritter;
				if (onDepositCritter != null)
				{
					onDepositCritter(critterData, lastGrabbedPlayer);
				}
				CrittersActor crittersActor = crittersPawn;
				bool flag = false;
				Vector3 zero = Vector3.zero;
				crittersActor.Released(flag, default(Quaternion), zero, default(Vector3), default(Vector3));
				crittersPawn.gameObject.SetActive(false);
				crittersCageDeposit.depositAudio.GTPlayOneShot(crittersCageDeposit.depositCritterSound, isLocalDeposit ? 1f : 0.5f);
			}
			else
			{
				crittersCageDeposit.depositAudio.GTPlayOneShot(crittersCageDeposit.depositEmptySound, isLocalDeposit ? 1f : 0.5f);
			}
			crittersCageDeposit.currentCage.transform.position = Vector3.zero;
			crittersCageDeposit.currentCage.gameObject.SetActive(false);
			crittersCageDeposit.currentCage = null;
			transition = 0f;
			IL_0295:
			if (transition >= crittersCageDeposit.returnDuration)
			{
				crittersCageDeposit.isHandlingDeposit = false;
				return false;
			}
			transition += Time.deltaTime;
			crittersCageDeposit.attachPoint.transform.localPosition = Vector3.Lerp(crittersCageDeposit.depositEndLocation, crittersCageDeposit.depositStartLocation, Mathf.Min(transition / crittersCageDeposit.returnDuration, 1f));
			this.<>2__current = null;
			this.<>1__state = 2;
			return true;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00009F44 File Offset: 0x00008144
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00009F44 File Offset: 0x00008144
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040001C7 RID: 455
		private int <>1__state;

		// Token: 0x040001C8 RID: 456
		private object <>2__current;

		// Token: 0x040001C9 RID: 457
		public CrittersCageDeposit <>4__this;

		// Token: 0x040001CA RID: 458
		private bool <isLocalDeposit>5__2;

		// Token: 0x040001CB RID: 459
		private float <transition>5__3;

		// Token: 0x040001CC RID: 460
		private CrittersPawn <crittersPawn>5__4;

		// Token: 0x040001CD RID: 461
		private Menagerie.CritterData <critterData>5__5;

		// Token: 0x040001CE RID: 462
		private int <lastGrabbedPlayer>5__6;
	}
}
