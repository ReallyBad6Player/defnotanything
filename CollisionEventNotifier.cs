using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

// Token: 0x02000AA6 RID: 2726
public class CollisionEventNotifier : MonoBehaviour
{
	// Token: 0x14000076 RID: 118
	// (add) Token: 0x060041F2 RID: 16882 RVA: 0x0014C474 File Offset: 0x0014A674
	// (remove) Token: 0x060041F3 RID: 16883 RVA: 0x0014C4AC File Offset: 0x0014A6AC
	public event CollisionEventNotifier.CollisionEvent CollisionEnterEvent
	{
		[CompilerGenerated]
		add
		{
			CollisionEventNotifier.CollisionEvent collisionEvent = this.CollisionEnterEvent;
			CollisionEventNotifier.CollisionEvent collisionEvent2;
			do
			{
				collisionEvent2 = collisionEvent;
				CollisionEventNotifier.CollisionEvent collisionEvent3 = (CollisionEventNotifier.CollisionEvent)Delegate.Combine(collisionEvent2, value);
				collisionEvent = Interlocked.CompareExchange<CollisionEventNotifier.CollisionEvent>(ref this.CollisionEnterEvent, collisionEvent3, collisionEvent2);
			}
			while (collisionEvent != collisionEvent2);
		}
		[CompilerGenerated]
		remove
		{
			CollisionEventNotifier.CollisionEvent collisionEvent = this.CollisionEnterEvent;
			CollisionEventNotifier.CollisionEvent collisionEvent2;
			do
			{
				collisionEvent2 = collisionEvent;
				CollisionEventNotifier.CollisionEvent collisionEvent3 = (CollisionEventNotifier.CollisionEvent)Delegate.Remove(collisionEvent2, value);
				collisionEvent = Interlocked.CompareExchange<CollisionEventNotifier.CollisionEvent>(ref this.CollisionEnterEvent, collisionEvent3, collisionEvent2);
			}
			while (collisionEvent != collisionEvent2);
		}
	}

	// Token: 0x14000077 RID: 119
	// (add) Token: 0x060041F4 RID: 16884 RVA: 0x0014C4E4 File Offset: 0x0014A6E4
	// (remove) Token: 0x060041F5 RID: 16885 RVA: 0x0014C51C File Offset: 0x0014A71C
	public event CollisionEventNotifier.CollisionEvent CollisionExitEvent
	{
		[CompilerGenerated]
		add
		{
			CollisionEventNotifier.CollisionEvent collisionEvent = this.CollisionExitEvent;
			CollisionEventNotifier.CollisionEvent collisionEvent2;
			do
			{
				collisionEvent2 = collisionEvent;
				CollisionEventNotifier.CollisionEvent collisionEvent3 = (CollisionEventNotifier.CollisionEvent)Delegate.Combine(collisionEvent2, value);
				collisionEvent = Interlocked.CompareExchange<CollisionEventNotifier.CollisionEvent>(ref this.CollisionExitEvent, collisionEvent3, collisionEvent2);
			}
			while (collisionEvent != collisionEvent2);
		}
		[CompilerGenerated]
		remove
		{
			CollisionEventNotifier.CollisionEvent collisionEvent = this.CollisionExitEvent;
			CollisionEventNotifier.CollisionEvent collisionEvent2;
			do
			{
				collisionEvent2 = collisionEvent;
				CollisionEventNotifier.CollisionEvent collisionEvent3 = (CollisionEventNotifier.CollisionEvent)Delegate.Remove(collisionEvent2, value);
				collisionEvent = Interlocked.CompareExchange<CollisionEventNotifier.CollisionEvent>(ref this.CollisionExitEvent, collisionEvent3, collisionEvent2);
			}
			while (collisionEvent != collisionEvent2);
		}
	}

	// Token: 0x060041F6 RID: 16886 RVA: 0x0014C551 File Offset: 0x0014A751
	private void OnCollisionEnter(Collision collision)
	{
		CollisionEventNotifier.CollisionEvent collisionEnterEvent = this.CollisionEnterEvent;
		if (collisionEnterEvent == null)
		{
			return;
		}
		collisionEnterEvent(this, collision);
	}

	// Token: 0x060041F7 RID: 16887 RVA: 0x0014C565 File Offset: 0x0014A765
	private void OnCollisionExit(Collision collision)
	{
		CollisionEventNotifier.CollisionEvent collisionExitEvent = this.CollisionExitEvent;
		if (collisionExitEvent == null)
		{
			return;
		}
		collisionExitEvent(this, collision);
	}

	// Token: 0x060041F8 RID: 16888 RVA: 0x000026E9 File Offset: 0x000008E9
	public CollisionEventNotifier()
	{
	}

	// Token: 0x04004D5B RID: 19803
	[CompilerGenerated]
	private CollisionEventNotifier.CollisionEvent CollisionEnterEvent;

	// Token: 0x04004D5C RID: 19804
	[CompilerGenerated]
	private CollisionEventNotifier.CollisionEvent CollisionExitEvent;

	// Token: 0x02000AA7 RID: 2727
	// (Invoke) Token: 0x060041FA RID: 16890
	public delegate void CollisionEvent(CollisionEventNotifier notifier, Collision collision);
}
