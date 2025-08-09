using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200034B RID: 843
public class ButtonDownListener : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x1400003A RID: 58
	// (add) Token: 0x06001411 RID: 5137 RVA: 0x0006B3E0 File Offset: 0x000695E0
	// (remove) Token: 0x06001412 RID: 5138 RVA: 0x0006B418 File Offset: 0x00069618
	public event Action onButtonDown
	{
		[CompilerGenerated]
		add
		{
			Action action = this.onButtonDown;
			Action action2;
			do
			{
				action2 = action;
				Action action3 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange<Action>(ref this.onButtonDown, action3, action2);
			}
			while (action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.onButtonDown;
			Action action2;
			do
			{
				action2 = action;
				Action action3 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange<Action>(ref this.onButtonDown, action3, action2);
			}
			while (action != action2);
		}
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x0006B44D File Offset: 0x0006964D
	public void OnPointerDown(PointerEventData eventData)
	{
		if (this.onButtonDown != null)
		{
			this.onButtonDown();
		}
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x000026E9 File Offset: 0x000008E9
	public ButtonDownListener()
	{
	}

	// Token: 0x04001B8B RID: 7051
	[CompilerGenerated]
	private Action onButtonDown;
}
