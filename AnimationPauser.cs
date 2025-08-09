using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000910 RID: 2320
public class AnimationPauser : StateMachineBehaviour
{
	// Token: 0x06003949 RID: 14665 RVA: 0x00129228 File Offset: 0x00127428
	public override async void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		this._animPauseDuration = Random.Range(this._minTimeBetweenAnims, this._maxTimeBetweenAnims);
		await Task.Delay(this._animPauseDuration * 1000);
		animator.SetTrigger(AnimationPauser.Restart_Anim_Name);
	}

	// Token: 0x0600394A RID: 14666 RVA: 0x00129277 File Offset: 0x00127477
	public AnimationPauser()
	{
	}

	// Token: 0x0600394B RID: 14667 RVA: 0x0012928D File Offset: 0x0012748D
	// Note: this type is marked as 'beforefieldinit'.
	static AnimationPauser()
	{
	}

	// Token: 0x0600394C RID: 14668 RVA: 0x00129299 File Offset: 0x00127499
	[CompilerGenerated]
	[DebuggerHidden]
	private void <>n__0(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	// Token: 0x04004668 RID: 18024
	[SerializeField]
	private int _maxTimeBetweenAnims = 5;

	// Token: 0x04004669 RID: 18025
	[SerializeField]
	private int _minTimeBetweenAnims = 1;

	// Token: 0x0400466A RID: 18026
	private int _animPauseDuration;

	// Token: 0x0400466B RID: 18027
	private static readonly string Restart_Anim_Name = "RestartAnim";

	// Token: 0x02000911 RID: 2321
	[CompilerGenerated]
	[StructLayout(LayoutKind.Auto)]
	private struct <OnStateEnter>d__4 : IAsyncStateMachine
	{
		// Token: 0x0600394D RID: 14669 RVA: 0x001292A4 File Offset: 0x001274A4
		void IAsyncStateMachine.MoveNext()
		{
			int num2;
			int num = num2;
			AnimationPauser animationPauser = this;
			try
			{
				TaskAwaiter taskAwaiter;
				if (num != 0)
				{
					animationPauser.<>n__0(animator, stateInfo, layerIndex);
					animationPauser._animPauseDuration = Random.Range(animationPauser._minTimeBetweenAnims, animationPauser._maxTimeBetweenAnims);
					taskAwaiter = Task.Delay(animationPauser._animPauseDuration * 1000).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 0;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, AnimationPauser.<OnStateEnter>d__4>(ref taskAwaiter, ref this);
						return;
					}
				}
				else
				{
					TaskAwaiter taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
				}
				taskAwaiter.GetResult();
				animator.SetTrigger(AnimationPauser.Restart_Anim_Name);
			}
			catch (Exception ex)
			{
				num2 = -2;
				this.<>t__builder.SetException(ex);
				return;
			}
			num2 = -2;
			this.<>t__builder.SetResult();
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x001293A0 File Offset: 0x001275A0
		[DebuggerHidden]
		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			this.<>t__builder.SetStateMachine(stateMachine);
		}

		// Token: 0x0400466C RID: 18028
		public int <>1__state;

		// Token: 0x0400466D RID: 18029
		public AsyncVoidMethodBuilder <>t__builder;

		// Token: 0x0400466E RID: 18030
		public AnimationPauser <>4__this;

		// Token: 0x0400466F RID: 18031
		public Animator animator;

		// Token: 0x04004670 RID: 18032
		public AnimatorStateInfo stateInfo;

		// Token: 0x04004671 RID: 18033
		public int layerIndex;

		// Token: 0x04004672 RID: 18034
		private TaskAwaiter <>u__1;
	}
}
