using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020004EC RID: 1260
public class AtticHider : MonoBehaviour
{
	// Token: 0x06001E99 RID: 7833 RVA: 0x000A1C8A File Offset: 0x0009FE8A
	private void Start()
	{
		this.OnZoneChanged();
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06001E9A RID: 7834 RVA: 0x000A1CB8 File Offset: 0x0009FEB8
	private void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x000A1CE0 File Offset: 0x0009FEE0
	private void OnZoneChanged()
	{
		if (this.AtticRenderer == null)
		{
			return;
		}
		if (ZoneManagement.instance.IsZoneActive(GTZone.attic))
		{
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
			this._coroutine = base.StartCoroutine(this.WaitForAtticLoad());
			return;
		}
		if (this._coroutine != null)
		{
			base.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}
		this.AtticRenderer.enabled = true;
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x000A1D5F File Offset: 0x0009FF5F
	private IEnumerator WaitForAtticLoad()
	{
		while (!ZoneManagement.instance.IsSceneLoaded(GTZone.attic))
		{
			yield return new WaitForSeconds(0.2f);
		}
		yield return null;
		this.AtticRenderer.enabled = false;
		this._coroutine = null;
		yield break;
	}

	// Token: 0x06001E9D RID: 7837 RVA: 0x000026E9 File Offset: 0x000008E9
	public AtticHider()
	{
	}

	// Token: 0x04002746 RID: 10054
	[SerializeField]
	private MeshRenderer AtticRenderer;

	// Token: 0x04002747 RID: 10055
	private Coroutine _coroutine;

	// Token: 0x020004ED RID: 1261
	[CompilerGenerated]
	private sealed class <WaitForAtticLoad>d__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x06001E9E RID: 7838 RVA: 0x000A1D6E File Offset: 0x0009FF6E
		[DebuggerHidden]
		public <WaitForAtticLoad>d__5(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000A1D80 File Offset: 0x0009FF80
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			AtticHider atticHider = this;
			switch (num)
			{
			case 0:
				this.<>1__state = -1;
				break;
			case 1:
				this.<>1__state = -1;
				break;
			case 2:
				this.<>1__state = -1;
				atticHider.AtticRenderer.enabled = false;
				atticHider._coroutine = null;
				return false;
			default:
				return false;
			}
			if (ZoneManagement.instance.IsSceneLoaded(GTZone.attic))
			{
				this.<>2__current = null;
				this.<>1__state = 2;
				return true;
			}
			this.<>2__current = new WaitForSeconds(0.2f);
			this.<>1__state = 1;
			return true;
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001EA1 RID: 7841 RVA: 0x000A1E11 File Offset: 0x000A0011
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06001EA3 RID: 7843 RVA: 0x000A1E11 File Offset: 0x000A0011
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x04002748 RID: 10056
		private int <>1__state;

		// Token: 0x04002749 RID: 10057
		private object <>2__current;

		// Token: 0x0400274A RID: 10058
		public AtticHider <>4__this;
	}
}
