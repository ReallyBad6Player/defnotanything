using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000380 RID: 896
public class BouncingBallLogic : MonoBehaviour
{
	// Token: 0x06001521 RID: 5409 RVA: 0x00073186 File Offset: 0x00071386
	private void OnCollisionEnter()
	{
		this.audioSource.PlayOneShot(this.bounce);
	}

	// Token: 0x06001522 RID: 5410 RVA: 0x00073199 File Offset: 0x00071399
	private void Start()
	{
		this.audioSource = base.GetComponent<AudioSource>();
		this.audioSource.PlayOneShot(this.loadball);
		this.centerEyeCamera = OVRManager.instance.GetComponentInChildren<OVRCameraRig>().centerEyeAnchor;
	}

	// Token: 0x06001523 RID: 5411 RVA: 0x000731D0 File Offset: 0x000713D0
	private void Update()
	{
		if (!this.isReleased)
		{
			return;
		}
		this.UpdateVisibility();
		this.timer += Time.deltaTime;
		if (!this.isReadyForDestroy && this.timer >= this.TTL)
		{
			this.isReadyForDestroy = true;
			float length = this.pop.length;
			this.audioSource.PlayOneShot(this.pop);
			base.StartCoroutine(this.PlayPopCallback(length));
		}
	}

	// Token: 0x06001524 RID: 5412 RVA: 0x00073248 File Offset: 0x00071448
	private void UpdateVisibility()
	{
		Vector3 vector = this.centerEyeCamera.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(base.transform.position, vector), out raycastHit, vector.magnitude))
		{
			if (raycastHit.collider.gameObject != base.gameObject)
			{
				this.SetVisible(false);
				return;
			}
		}
		else
		{
			this.SetVisible(true);
		}
	}

	// Token: 0x06001525 RID: 5413 RVA: 0x000732BC File Offset: 0x000714BC
	private void SetVisible(bool setVisible)
	{
		if (this.isVisible && !setVisible)
		{
			base.GetComponent<MeshRenderer>().material = this.hiddenMat;
			this.isVisible = false;
		}
		if (!this.isVisible && setVisible)
		{
			base.GetComponent<MeshRenderer>().material = this.visibleMat;
			this.isVisible = true;
		}
	}

	// Token: 0x06001526 RID: 5414 RVA: 0x00073311 File Offset: 0x00071511
	public void Release(Vector3 pos, Vector3 vel, Vector3 angVel)
	{
		this.isReleased = true;
		base.transform.position = pos;
		base.GetComponent<Rigidbody>().isKinematic = false;
		base.GetComponent<Rigidbody>().velocity = vel;
		base.GetComponent<Rigidbody>().angularVelocity = angVel;
	}

	// Token: 0x06001527 RID: 5415 RVA: 0x0007334A File Offset: 0x0007154A
	private IEnumerator PlayPopCallback(float clipLength)
	{
		yield return new WaitForSeconds(clipLength);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x00073360 File Offset: 0x00071560
	public BouncingBallLogic()
	{
	}

	// Token: 0x04001CBA RID: 7354
	[SerializeField]
	private float TTL = 5f;

	// Token: 0x04001CBB RID: 7355
	[SerializeField]
	private AudioClip pop;

	// Token: 0x04001CBC RID: 7356
	[SerializeField]
	private AudioClip bounce;

	// Token: 0x04001CBD RID: 7357
	[SerializeField]
	private AudioClip loadball;

	// Token: 0x04001CBE RID: 7358
	[SerializeField]
	private Material visibleMat;

	// Token: 0x04001CBF RID: 7359
	[SerializeField]
	private Material hiddenMat;

	// Token: 0x04001CC0 RID: 7360
	private AudioSource audioSource;

	// Token: 0x04001CC1 RID: 7361
	private Transform centerEyeCamera;

	// Token: 0x04001CC2 RID: 7362
	private bool isVisible = true;

	// Token: 0x04001CC3 RID: 7363
	private float timer;

	// Token: 0x04001CC4 RID: 7364
	private bool isReleased;

	// Token: 0x04001CC5 RID: 7365
	private bool isReadyForDestroy;

	// Token: 0x02000381 RID: 897
	[CompilerGenerated]
	private sealed class <PlayPopCallback>d__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x06001529 RID: 5417 RVA: 0x0007337A File Offset: 0x0007157A
		[DebuggerHidden]
		public <PlayPopCallback>d__18(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0007338C File Offset: 0x0007158C
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			BouncingBallLogic bouncingBallLogic = this;
			if (num == 0)
			{
				this.<>1__state = -1;
				this.<>2__current = new WaitForSeconds(clipLength);
				this.<>1__state = 1;
				return true;
			}
			if (num != 1)
			{
				return false;
			}
			this.<>1__state = -1;
			Object.Destroy(bouncingBallLogic.gameObject);
			return false;
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x000733E4 File Offset: 0x000715E4
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x000733E4 File Offset: 0x000715E4
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x04001CC6 RID: 7366
		private int <>1__state;

		// Token: 0x04001CC7 RID: 7367
		private object <>2__current;

		// Token: 0x04001CC8 RID: 7368
		public float clipLength;

		// Token: 0x04001CC9 RID: 7369
		public BouncingBallLogic <>4__this;
	}
}
