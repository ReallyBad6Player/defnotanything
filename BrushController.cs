using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000363 RID: 867
public class BrushController : MonoBehaviour
{
	// Token: 0x06001489 RID: 5257 RVA: 0x0006EDA0 File Offset: 0x0006CFA0
	private void Start()
	{
		this.brush.controllerHand = OVRInput.Controller.None;
		if (!this.brush.lineContainer)
		{
			this.brush.lineContainer = new GameObject("LineContainer");
		}
		this.backgroundSphere.material.renderQueue = 3998;
		this.backgroundSphere.transform.parent = null;
		this.backgroundSphere.enabled = false;
		if (base.GetComponent<GrabObject>())
		{
			GrabObject component = base.GetComponent<GrabObject>();
			component.GrabbedObjectDelegate = (GrabObject.GrabbedObject)Delegate.Combine(component.GrabbedObjectDelegate, new GrabObject.GrabbedObject(this.Grab));
			GrabObject component2 = base.GetComponent<GrabObject>();
			component2.ReleasedObjectDelegate = (GrabObject.ReleasedObject)Delegate.Combine(component2.ReleasedObjectDelegate, new GrabObject.ReleasedObject(this.Release));
		}
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x0006EE6D File Offset: 0x0006D06D
	private void Update()
	{
		this.backgroundSphere.transform.position = Camera.main.transform.position;
	}

	// Token: 0x0600148B RID: 5259 RVA: 0x0006EE90 File Offset: 0x0006D090
	public void Grab(OVRInput.Controller grabHand)
	{
		this.brush.controllerHand = grabHand;
		this.brush.lineContainer.SetActive(true);
		this.backgroundSphere.enabled = true;
		if (this.grabRoutine != null)
		{
			base.StopCoroutine(this.grabRoutine);
		}
		if (this.releaseRoutine != null)
		{
			base.StopCoroutine(this.releaseRoutine);
		}
		this.grabRoutine = this.FadeSphere(Color.grey, 0.25f, false);
		base.StartCoroutine(this.grabRoutine);
	}

	// Token: 0x0600148C RID: 5260 RVA: 0x0006EF14 File Offset: 0x0006D114
	public void Release()
	{
		this.brush.controllerHand = OVRInput.Controller.None;
		this.brush.lineContainer.SetActive(false);
		if (this.grabRoutine != null)
		{
			base.StopCoroutine(this.grabRoutine);
		}
		if (this.releaseRoutine != null)
		{
			base.StopCoroutine(this.releaseRoutine);
		}
		this.releaseRoutine = this.FadeSphere(new Color(0.5f, 0.5f, 0.5f, 0f), 0.25f, true);
		base.StartCoroutine(this.releaseRoutine);
	}

	// Token: 0x0600148D RID: 5261 RVA: 0x0006EF9E File Offset: 0x0006D19E
	private IEnumerator FadeCameraClearColor(Color newColor, float fadeTime)
	{
		float timer = 0f;
		Color currentColor = Camera.main.backgroundColor;
		while (timer <= fadeTime)
		{
			timer += Time.deltaTime;
			float num = Mathf.Clamp01(timer / fadeTime);
			Camera.main.backgroundColor = Color.Lerp(currentColor, newColor, num);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x0006EFB4 File Offset: 0x0006D1B4
	private IEnumerator FadeSphere(Color newColor, float fadeTime, bool disableOnFinish = false)
	{
		float timer = 0f;
		Color currentColor = this.backgroundSphere.material.GetColor("_Color");
		while (timer <= fadeTime)
		{
			timer += Time.deltaTime;
			float num = Mathf.Clamp01(timer / fadeTime);
			this.backgroundSphere.material.SetColor("_Color", Color.Lerp(currentColor, newColor, num));
			if (disableOnFinish && timer >= fadeTime)
			{
				this.backgroundSphere.enabled = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600148F RID: 5263 RVA: 0x000026E9 File Offset: 0x000008E9
	public BrushController()
	{
	}

	// Token: 0x04001C11 RID: 7185
	public PassthroughBrush brush;

	// Token: 0x04001C12 RID: 7186
	public MeshRenderer backgroundSphere;

	// Token: 0x04001C13 RID: 7187
	private IEnumerator grabRoutine;

	// Token: 0x04001C14 RID: 7188
	private IEnumerator releaseRoutine;

	// Token: 0x02000364 RID: 868
	[CompilerGenerated]
	private sealed class <FadeCameraClearColor>d__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x06001490 RID: 5264 RVA: 0x0006EFD8 File Offset: 0x0006D1D8
		[DebuggerHidden]
		public <FadeCameraClearColor>d__8(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0006EFE8 File Offset: 0x0006D1E8
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
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
				timer = 0f;
				currentColor = Camera.main.backgroundColor;
			}
			if (timer > fadeTime)
			{
				return false;
			}
			timer += Time.deltaTime;
			float num2 = Mathf.Clamp01(timer / fadeTime);
			Camera.main.backgroundColor = Color.Lerp(currentColor, newColor, num2);
			this.<>2__current = null;
			this.<>1__state = 1;
			return true;
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x0006F090 File Offset: 0x0006D290
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x0006F090 File Offset: 0x0006D290
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x04001C15 RID: 7189
		private int <>1__state;

		// Token: 0x04001C16 RID: 7190
		private object <>2__current;

		// Token: 0x04001C17 RID: 7191
		public float fadeTime;

		// Token: 0x04001C18 RID: 7192
		public Color newColor;

		// Token: 0x04001C19 RID: 7193
		private float <timer>5__2;

		// Token: 0x04001C1A RID: 7194
		private Color <currentColor>5__3;
	}

	// Token: 0x02000365 RID: 869
	[CompilerGenerated]
	private sealed class <FadeSphere>d__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x06001496 RID: 5270 RVA: 0x0006F098 File Offset: 0x0006D298
		[DebuggerHidden]
		public <FadeSphere>d__9(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0006F0A8 File Offset: 0x0006D2A8
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			BrushController brushController = this;
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
				timer = 0f;
				currentColor = brushController.backgroundSphere.material.GetColor("_Color");
			}
			if (timer > fadeTime)
			{
				return false;
			}
			timer += Time.deltaTime;
			float num2 = Mathf.Clamp01(timer / fadeTime);
			brushController.backgroundSphere.material.SetColor("_Color", Color.Lerp(currentColor, newColor, num2));
			if (disableOnFinish && timer >= fadeTime)
			{
				brushController.backgroundSphere.enabled = false;
			}
			this.<>2__current = null;
			this.<>1__state = 1;
			return true;
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x0006F198 File Offset: 0x0006D398
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x0006F198 File Offset: 0x0006D398
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x04001C1B RID: 7195
		private int <>1__state;

		// Token: 0x04001C1C RID: 7196
		private object <>2__current;

		// Token: 0x04001C1D RID: 7197
		public BrushController <>4__this;

		// Token: 0x04001C1E RID: 7198
		public float fadeTime;

		// Token: 0x04001C1F RID: 7199
		public Color newColor;

		// Token: 0x04001C20 RID: 7200
		public bool disableOnFinish;

		// Token: 0x04001C21 RID: 7201
		private float <timer>5__2;

		// Token: 0x04001C22 RID: 7202
		private Color <currentColor>5__3;
	}
}
