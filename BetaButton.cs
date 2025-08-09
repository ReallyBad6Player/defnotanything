using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000417 RID: 1047
public class BetaButton : GorillaPressableButton
{
	// Token: 0x06001978 RID: 6520 RVA: 0x00089444 File Offset: 0x00087644
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		this.count++;
		base.StartCoroutine(this.ButtonColorUpdate());
		if (this.count >= 10)
		{
			this.betaParent.SetActive(false);
			PlayerPrefs.SetString("CheckedBox2", "true");
			PlayerPrefs.Save();
		}
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x0008949C File Offset: 0x0008769C
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.buttonFadeTime);
		this.buttonRenderer.material = this.unpressedMaterial;
		yield break;
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x000894AB File Offset: 0x000876AB
	public BetaButton()
	{
	}

	// Token: 0x040021D7 RID: 8663
	public GameObject betaParent;

	// Token: 0x040021D8 RID: 8664
	public int count;

	// Token: 0x040021D9 RID: 8665
	public float buttonFadeTime = 0.25f;

	// Token: 0x040021DA RID: 8666
	public Text messageText;

	// Token: 0x02000418 RID: 1048
	[CompilerGenerated]
	private sealed class <ButtonColorUpdate>d__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0600197B RID: 6523 RVA: 0x000894BE File Offset: 0x000876BE
		[DebuggerHidden]
		public <ButtonColorUpdate>d__5(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x000894D0 File Offset: 0x000876D0
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			BetaButton betaButton = this;
			if (num == 0)
			{
				this.<>1__state = -1;
				betaButton.buttonRenderer.material = betaButton.pressedMaterial;
				this.<>2__current = new WaitForSeconds(betaButton.buttonFadeTime);
				this.<>1__state = 1;
				return true;
			}
			if (num != 1)
			{
				return false;
			}
			this.<>1__state = -1;
			betaButton.buttonRenderer.material = betaButton.unpressedMaterial;
			return false;
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600197E RID: 6526 RVA: 0x0008953F File Offset: 0x0008773F
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x0008953F File Offset: 0x0008773F
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040021DB RID: 8667
		private int <>1__state;

		// Token: 0x040021DC RID: 8668
		private object <>2__current;

		// Token: 0x040021DD RID: 8669
		public BetaButton <>4__this;
	}
}
