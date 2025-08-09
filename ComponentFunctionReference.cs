using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

// Token: 0x020007ED RID: 2029
[Serializable]
public sealed class ComponentFunctionReference<TResult>
{
	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x060032D0 RID: 13008 RVA: 0x001089BC File Offset: 0x00106BBC
	public bool IsValid
	{
		get
		{
			return this._selection.component || !string.IsNullOrEmpty(this._selection.methodName);
		}
	}

	// Token: 0x060032D1 RID: 13009 RVA: 0x001089E5 File Offset: 0x00106BE5
	private IEnumerable<ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>> GetMethodOptions()
	{
		if (this._target == null)
		{
			yield break;
		}
		yield return new ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>("NONE", default(ComponentFunctionReference<TResult>.MethodRef));
		Type type = typeof(GameObject);
		BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		foreach (MethodInfo methodInfo in type.GetMethods(flags))
		{
			if (methodInfo.GetParameters().Length == 0 && methodInfo.ReturnType == typeof(TResult))
			{
				string text = type.Name + "/" + methodInfo.Name;
				yield return new ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>(text, new ComponentFunctionReference<TResult>.MethodRef(this._target, methodInfo));
			}
		}
		MethodInfo[] array = null;
		foreach (Component comp in this._target.GetComponents<Component>())
		{
			type = comp.GetType();
			foreach (MethodInfo methodInfo2 in type.GetMethods(flags))
			{
				if (methodInfo2.GetParameters().Length == 0 && methodInfo2.ReturnType == typeof(TResult))
				{
					string text2 = type.Name + "/" + methodInfo2.Name;
					yield return new ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>(text2, new ComponentFunctionReference<TResult>.MethodRef(comp, methodInfo2));
				}
			}
			array = null;
			comp = null;
		}
		Component[] array2 = null;
		yield break;
	}

	// Token: 0x060032D2 RID: 13010 RVA: 0x001089F8 File Offset: 0x00106BF8
	public TResult Invoke()
	{
		if (this._cached == null)
		{
			this.Cache();
		}
		if (this._cached == null)
		{
			return default(TResult);
		}
		return this._cached();
	}

	// Token: 0x060032D3 RID: 13011 RVA: 0x00108A30 File Offset: 0x00106C30
	public void Cache()
	{
		this._cached = null;
		if (this._selection.component == null || string.IsNullOrEmpty(this._selection.methodName))
		{
			return;
		}
		MethodInfo method = this._selection.component.GetType().GetMethod(this._selection.methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
		if (method != null)
		{
			this._cached = (Func<TResult>)Delegate.CreateDelegate(typeof(Func<TResult>), this._selection.component, method);
		}
	}

	// Token: 0x060032D4 RID: 13012 RVA: 0x00002050 File Offset: 0x00000250
	public ComponentFunctionReference()
	{
	}

	// Token: 0x04003FBD RID: 16317
	[SerializeField]
	private GameObject _target;

	// Token: 0x04003FBE RID: 16318
	[SerializeField]
	private ComponentFunctionReference<TResult>.MethodRef _selection;

	// Token: 0x04003FBF RID: 16319
	private Func<TResult> _cached;

	// Token: 0x020007EE RID: 2030
	[Serializable]
	private struct MethodRef
	{
		// Token: 0x060032D5 RID: 13013 RVA: 0x00108AC3 File Offset: 0x00106CC3
		public MethodRef(Object obj, MethodInfo m)
		{
			this.component = obj;
			this.methodName = m.Name;
		}

		// Token: 0x04003FC0 RID: 16320
		public Object component;

		// Token: 0x04003FC1 RID: 16321
		public string methodName;
	}

	// Token: 0x020007EF RID: 2031
	[CompilerGenerated]
	private sealed class <GetMethodOptions>d__6 : IEnumerable<ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>>, IEnumerable, IEnumerator<ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>>, IEnumerator, IDisposable
	{
		// Token: 0x060032D6 RID: 13014 RVA: 0x00108AD8 File Offset: 0x00106CD8
		[DebuggerHidden]
		public <GetMethodOptions>d__6(int <>1__state)
		{
			this.<>1__state = <>1__state;
			this.<>l__initialThreadId = Environment.CurrentManagedThreadId;
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x00108AF4 File Offset: 0x00106CF4
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			ComponentFunctionReference<TResult> componentFunctionReference = this;
			switch (num)
			{
			case 0:
				this.<>1__state = -1;
				if (componentFunctionReference._target == null)
				{
					return false;
				}
				this.<>2__current = new ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>("NONE", default(ComponentFunctionReference<TResult>.MethodRef));
				this.<>1__state = 1;
				return true;
			case 1:
				this.<>1__state = -1;
				type = typeof(GameObject);
				flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				array = type.GetMethods(flags);
				i = 0;
				goto IL_0123;
			case 2:
				this.<>1__state = -1;
				break;
			case 3:
				this.<>1__state = -1;
				goto IL_021A;
			default:
				return false;
			}
			IL_0115:
			i++;
			IL_0123:
			if (i >= array.Length)
			{
				array = null;
				array2 = componentFunctionReference._target.GetComponents<Component>();
				i = 0;
				goto IL_0257;
			}
			MethodInfo methodInfo = array[i];
			if (methodInfo.GetParameters().Length == 0 && methodInfo.ReturnType == typeof(TResult))
			{
				string text = type.Name + "/" + methodInfo.Name;
				this.<>2__current = new ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>(text, new ComponentFunctionReference<TResult>.MethodRef(componentFunctionReference._target, methodInfo));
				this.<>1__state = 2;
				return true;
			}
			goto IL_0115;
			IL_021A:
			j++;
			IL_0228:
			if (j >= array.Length)
			{
				array = null;
				comp = null;
				i++;
			}
			else
			{
				MethodInfo methodInfo2 = array[j];
				if (methodInfo2.GetParameters().Length == 0 && methodInfo2.ReturnType == typeof(TResult))
				{
					string text2 = type.Name + "/" + methodInfo2.Name;
					this.<>2__current = new ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>(text2, new ComponentFunctionReference<TResult>.MethodRef(comp, methodInfo2));
					this.<>1__state = 3;
					return true;
				}
				goto IL_021A;
			}
			IL_0257:
			if (i >= array2.Length)
			{
				array2 = null;
				return false;
			}
			comp = array2[i];
			type = comp.GetType();
			array = type.GetMethods(flags);
			j = 0;
			goto IL_0228;
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060032D9 RID: 13017 RVA: 0x00108D73 File Offset: 0x00106F73
		ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef> IEnumerator<ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060032DB RID: 13019 RVA: 0x00108D7B File Offset: 0x00106F7B
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x00108D88 File Offset: 0x00106F88
		[DebuggerHidden]
		IEnumerator<ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>> IEnumerable<ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>>.GetEnumerator()
		{
			ComponentFunctionReference<TResult>.<GetMethodOptions>d__6 <GetMethodOptions>d__;
			if (this.<>1__state == -2 && this.<>l__initialThreadId == Environment.CurrentManagedThreadId)
			{
				this.<>1__state = 0;
				<GetMethodOptions>d__ = this;
			}
			else
			{
				<GetMethodOptions>d__ = new ComponentFunctionReference<TResult>.<GetMethodOptions>d__6(0);
				<GetMethodOptions>d__.<>4__this = this;
			}
			return <GetMethodOptions>d__;
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x00108DCB File Offset: 0x00106FCB
		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.System.Collections.Generic.IEnumerable<Sirenix.OdinInspector.ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef>>.GetEnumerator();
		}

		// Token: 0x04003FC2 RID: 16322
		private int <>1__state;

		// Token: 0x04003FC3 RID: 16323
		private ValueDropdownItem<ComponentFunctionReference<TResult>.MethodRef> <>2__current;

		// Token: 0x04003FC4 RID: 16324
		private int <>l__initialThreadId;

		// Token: 0x04003FC5 RID: 16325
		public ComponentFunctionReference<TResult> <>4__this;

		// Token: 0x04003FC6 RID: 16326
		private Type <type>5__2;

		// Token: 0x04003FC7 RID: 16327
		private BindingFlags <flags>5__3;

		// Token: 0x04003FC8 RID: 16328
		private MethodInfo[] <>7__wrap3;

		// Token: 0x04003FC9 RID: 16329
		private int <>7__wrap4;

		// Token: 0x04003FCA RID: 16330
		private Component[] <>7__wrap5;

		// Token: 0x04003FCB RID: 16331
		private Component <comp>5__7;

		// Token: 0x04003FCC RID: 16332
		private int <>7__wrap7;
	}
}
