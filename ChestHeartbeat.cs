using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200056E RID: 1390
public class ChestHeartbeat : MonoBehaviour
{
	// Token: 0x060021F3 RID: 8691 RVA: 0x000B85E0 File Offset: 0x000B67E0
	public void Update()
	{
		if (PhotonNetwork.InRoom)
		{
			if ((PhotonNetwork.ServerTimestamp > this.lastShot + this.millisMin || Mathf.Abs(PhotonNetwork.ServerTimestamp - this.lastShot) > 10000) && PhotonNetwork.ServerTimestamp % 1500 <= 10)
			{
				this.lastShot = PhotonNetwork.ServerTimestamp;
				this.audioSource.GTPlayOneShot(this.audioSource.clip, 1f);
				base.StartCoroutine(this.HeartBeat());
				return;
			}
		}
		else if ((Time.time * 1000f > (float)(this.lastShot + this.millisMin) || Mathf.Abs(Time.time * 1000f - (float)this.lastShot) > 10000f) && Time.time * 1000f % 1500f <= 10f)
		{
			this.lastShot = PhotonNetwork.ServerTimestamp;
			this.audioSource.GTPlayOneShot(this.audioSource.clip, 1f);
			base.StartCoroutine(this.HeartBeat());
		}
	}

	// Token: 0x060021F4 RID: 8692 RVA: 0x000B86EE File Offset: 0x000B68EE
	private IEnumerator HeartBeat()
	{
		float startTime = Time.time;
		while (Time.time < startTime + this.endtime)
		{
			if (Time.time < startTime + this.minTime)
			{
				this.deltaTime = Time.time - startTime;
				this.scaleTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * this.heartMinSize, this.deltaTime / this.minTime);
			}
			else if (Time.time < startTime + this.maxTime)
			{
				this.deltaTime = Time.time - startTime - this.minTime;
				this.scaleTransform.localScale = Vector3.Lerp(Vector3.one * this.heartMinSize, Vector3.one * this.heartMaxSize, this.deltaTime / (this.maxTime - this.minTime));
			}
			else if (Time.time < startTime + this.endtime)
			{
				this.deltaTime = Time.time - startTime - this.maxTime;
				this.scaleTransform.localScale = Vector3.Lerp(Vector3.one * this.heartMaxSize, Vector3.one, this.deltaTime / (this.endtime - this.maxTime));
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000B8700 File Offset: 0x000B6900
	public ChestHeartbeat()
	{
	}

	// Token: 0x04002B7D RID: 11133
	public int millisToWait;

	// Token: 0x04002B7E RID: 11134
	public int millisMin = 300;

	// Token: 0x04002B7F RID: 11135
	public int lastShot;

	// Token: 0x04002B80 RID: 11136
	public AudioSource audioSource;

	// Token: 0x04002B81 RID: 11137
	public Transform scaleTransform;

	// Token: 0x04002B82 RID: 11138
	private float deltaTime;

	// Token: 0x04002B83 RID: 11139
	private float heartMinSize = 0.9f;

	// Token: 0x04002B84 RID: 11140
	private float heartMaxSize = 1.2f;

	// Token: 0x04002B85 RID: 11141
	private float minTime = 0.05f;

	// Token: 0x04002B86 RID: 11142
	private float maxTime = 0.1f;

	// Token: 0x04002B87 RID: 11143
	private float endtime = 0.25f;

	// Token: 0x04002B88 RID: 11144
	private float currentTime;

	// Token: 0x0200056F RID: 1391
	[CompilerGenerated]
	private sealed class <HeartBeat>d__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060021F6 RID: 8694 RVA: 0x000B8755 File Offset: 0x000B6955
		[DebuggerHidden]
		public <HeartBeat>d__13(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x000B8764 File Offset: 0x000B6964
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			ChestHeartbeat chestHeartbeat = this;
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
				startTime = Time.time;
			}
			if (Time.time >= startTime + chestHeartbeat.endtime)
			{
				return false;
			}
			if (Time.time < startTime + chestHeartbeat.minTime)
			{
				chestHeartbeat.deltaTime = Time.time - startTime;
				chestHeartbeat.scaleTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * chestHeartbeat.heartMinSize, chestHeartbeat.deltaTime / chestHeartbeat.minTime);
			}
			else if (Time.time < startTime + chestHeartbeat.maxTime)
			{
				chestHeartbeat.deltaTime = Time.time - startTime - chestHeartbeat.minTime;
				chestHeartbeat.scaleTransform.localScale = Vector3.Lerp(Vector3.one * chestHeartbeat.heartMinSize, Vector3.one * chestHeartbeat.heartMaxSize, chestHeartbeat.deltaTime / (chestHeartbeat.maxTime - chestHeartbeat.minTime));
			}
			else if (Time.time < startTime + chestHeartbeat.endtime)
			{
				chestHeartbeat.deltaTime = Time.time - startTime - chestHeartbeat.maxTime;
				chestHeartbeat.scaleTransform.localScale = Vector3.Lerp(Vector3.one * chestHeartbeat.heartMaxSize, Vector3.one, chestHeartbeat.deltaTime / (chestHeartbeat.endtime - chestHeartbeat.maxTime));
			}
			this.<>2__current = new WaitForFixedUpdate();
			this.<>1__state = 1;
			return true;
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060021F9 RID: 8697 RVA: 0x000B890B File Offset: 0x000B6B0B
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060021FB RID: 8699 RVA: 0x000B890B File Offset: 0x000B6B0B
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x04002B89 RID: 11145
		private int <>1__state;

		// Token: 0x04002B8A RID: 11146
		private object <>2__current;

		// Token: 0x04002B8B RID: 11147
		public ChestHeartbeat <>4__this;

		// Token: 0x04002B8C RID: 11148
		private float <startTime>5__2;
	}
}
