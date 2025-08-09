using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x0200056A RID: 1386
internal class ConnectedControllerHandler : MonoBehaviour
{
	// Token: 0x17000360 RID: 864
	// (get) Token: 0x060021C5 RID: 8645 RVA: 0x000B768A File Offset: 0x000B588A
	// (set) Token: 0x060021C6 RID: 8646 RVA: 0x000B7691 File Offset: 0x000B5891
	public static ConnectedControllerHandler Instance
	{
		[CompilerGenerated]
		get
		{
			return ConnectedControllerHandler.<Instance>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			ConnectedControllerHandler.<Instance>k__BackingField = value;
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x060021C7 RID: 8647 RVA: 0x000B7699 File Offset: 0x000B5899
	public bool RightValid
	{
		get
		{
			return this.rightValid;
		}
	}

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x060021C8 RID: 8648 RVA: 0x000B76A1 File Offset: 0x000B58A1
	public bool LeftValid
	{
		get
		{
			return this.leftValid;
		}
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x000B76AC File Offset: 0x000B58AC
	private void Awake()
	{
		if (ConnectedControllerHandler.Instance != null && ConnectedControllerHandler.Instance != this)
		{
			Object.Destroy(this);
			return;
		}
		ConnectedControllerHandler.Instance = this;
		if (this.leftHandFollower == null || this.rightHandFollower == null || this.rightXRController == null || this.leftXRController == null || this.snapTurnController == null)
		{
			base.enabled = false;
			return;
		}
		this.rightControllerList = new List<XRController>();
		this.leftcontrollerList = new List<XRController>();
		this.rightControllerList.Add(this.rightXRController);
		this.leftcontrollerList.Add(this.leftXRController);
		InputDevice deviceAtXRNode = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		InputDevice deviceAtXRNode2 = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
		Debug.Log(string.Format("right controller? {0}", (deviceAtXRNode.characteristics & (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right)) == (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right)));
		this.rightControllerValid = deviceAtXRNode.isValid;
		this.leftControllerValid = deviceAtXRNode2.isValid;
		InputDevices.deviceConnected += this.DeviceConnected;
		InputDevices.deviceDisconnected += this.DeviceDisconnected;
		this.UpdateControllerStates();
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x000B77D4 File Offset: 0x000B59D4
	private void Start()
	{
		if (this.leftHandFollower == null || this.rightHandFollower == null || this.leftXRController == null || this.rightXRController == null || this.snapTurnController == null)
		{
			return;
		}
		this.playerHandler = GTPlayer.Instance;
		this.rightHandFollower.followTransform = GorillaTagger.Instance.offlineVRRig.transform;
		this.leftHandFollower.followTransform = GorillaTagger.Instance.offlineVRRig.transform;
	}

	// Token: 0x060021CB RID: 8651 RVA: 0x000B785B File Offset: 0x000B5A5B
	private void OnEnable()
	{
		base.StartCoroutine(this.ControllerValidator());
	}

	// Token: 0x060021CC RID: 8652 RVA: 0x000B786A File Offset: 0x000B5A6A
	private void OnDisable()
	{
		base.StopCoroutine(this.ControllerValidator());
	}

	// Token: 0x060021CD RID: 8653 RVA: 0x000B7878 File Offset: 0x000B5A78
	private void OnDestroy()
	{
		if (ConnectedControllerHandler.Instance != null && ConnectedControllerHandler.Instance == this)
		{
			ConnectedControllerHandler.Instance = null;
		}
		InputDevices.deviceConnected -= this.DeviceConnected;
		InputDevices.deviceDisconnected -= this.DeviceDisconnected;
	}

	// Token: 0x060021CE RID: 8654 RVA: 0x000B78C7 File Offset: 0x000B5AC7
	private void LateUpdate()
	{
		if (!this.rightValid)
		{
			this.rightHandFollower.UpdatePositionRotation();
		}
		if (!this.leftValid)
		{
			this.leftHandFollower.UpdatePositionRotation();
		}
	}

	// Token: 0x060021CF RID: 8655 RVA: 0x000B78EF File Offset: 0x000B5AEF
	private IEnumerator ControllerValidator()
	{
		yield return null;
		this.lastRightPos = ControllerInputPoller.DevicePosition(XRNode.RightHand);
		this.lastLeftPos = ControllerInputPoller.DevicePosition(XRNode.LeftHand);
		for (;;)
		{
			yield return new WaitForSeconds(this.overridePollRate);
			this.updateControllers = false;
			if (!this.playerHandler.inOverlay)
			{
				if (this.rightControllerValid)
				{
					this.tempRightPos = ControllerInputPoller.DevicePosition(XRNode.RightHand);
					if (this.tempRightPos == this.lastRightPos)
					{
						if ((this.overrideController & OverrideControllers.RightController) != OverrideControllers.RightController)
						{
							this.overrideController |= OverrideControllers.RightController;
							this.updateControllers = true;
						}
					}
					else if ((this.overrideController & OverrideControllers.RightController) == OverrideControllers.RightController)
					{
						this.overrideController &= ~OverrideControllers.RightController;
						this.updateControllers = true;
					}
					this.lastRightPos = this.tempRightPos;
				}
				if (this.leftControllerValid)
				{
					this.tempLeftPos = ControllerInputPoller.DevicePosition(XRNode.LeftHand);
					if (this.tempLeftPos == this.lastLeftPos)
					{
						if ((this.overrideController & OverrideControllers.LeftController) != OverrideControllers.LeftController)
						{
							this.overrideController |= OverrideControllers.LeftController;
							this.updateControllers = true;
						}
					}
					else if ((this.overrideController & OverrideControllers.LeftController) == OverrideControllers.LeftController)
					{
						this.overrideController &= ~OverrideControllers.LeftController;
						this.updateControllers = true;
					}
					this.lastLeftPos = this.tempLeftPos;
				}
				if (this.updateControllers)
				{
					this.overrideEnabled = this.overrideController > OverrideControllers.None;
					this.UpdateControllerStates();
				}
			}
		}
		yield break;
	}

	// Token: 0x060021D0 RID: 8656 RVA: 0x000B78FE File Offset: 0x000B5AFE
	private void DeviceDisconnected(InputDevice device)
	{
		if ((device.characteristics & (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right)) == (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right))
		{
			this.rightControllerValid = false;
		}
		if ((device.characteristics & (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left)) == (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left))
		{
			this.leftControllerValid = false;
		}
		this.UpdateControllerStates();
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x000B793C File Offset: 0x000B5B3C
	private void DeviceConnected(InputDevice device)
	{
		if ((device.characteristics & (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right)) == (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right))
		{
			this.rightControllerValid = true;
		}
		if ((device.characteristics & (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left)) == (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left))
		{
			this.leftControllerValid = true;
		}
		this.UpdateControllerStates();
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x000B797C File Offset: 0x000B5B7C
	private void UpdateControllerStates()
	{
		if (this.overrideEnabled && this.overrideController != OverrideControllers.None)
		{
			this.rightValid = this.rightControllerValid && (this.overrideController & OverrideControllers.RightController) != OverrideControllers.RightController;
			this.leftValid = this.leftControllerValid && (this.overrideController & OverrideControllers.LeftController) != OverrideControllers.LeftController;
		}
		else
		{
			this.rightValid = this.rightControllerValid;
			this.leftValid = this.leftControllerValid;
		}
		this.rightXRController.enabled = this.rightValid;
		this.leftXRController.enabled = this.leftValid;
		this.AssignSnapturnController();
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000B7A1C File Offset: 0x000B5C1C
	private void AssignSnapturnController()
	{
		if (!this.leftValid && this.rightValid)
		{
			this.snapTurnController.controllers = this.rightControllerList;
			return;
		}
		if (!this.rightValid && this.leftValid)
		{
			this.snapTurnController.controllers = this.leftcontrollerList;
			return;
		}
		this.snapTurnController.controllers = this.rightControllerList;
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x000B7A80 File Offset: 0x000B5C80
	public bool GetValidForXRNode(XRNode controllerNode)
	{
		bool flag;
		if (controllerNode != XRNode.LeftHand)
		{
			flag = controllerNode != XRNode.RightHand || this.rightValid;
		}
		else
		{
			flag = this.leftValid;
		}
		return flag;
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x000B7AAC File Offset: 0x000B5CAC
	public ConnectedControllerHandler()
	{
	}

	// Token: 0x04002B37 RID: 11063
	[CompilerGenerated]
	[OnEnterPlay_SetNull]
	private static ConnectedControllerHandler <Instance>k__BackingField;

	// Token: 0x04002B38 RID: 11064
	[SerializeField]
	private HandTransformFollowOffest rightHandFollower;

	// Token: 0x04002B39 RID: 11065
	[SerializeField]
	private HandTransformFollowOffest leftHandFollower;

	// Token: 0x04002B3A RID: 11066
	[SerializeField]
	private XRController rightXRController;

	// Token: 0x04002B3B RID: 11067
	[SerializeField]
	private XRController leftXRController;

	// Token: 0x04002B3C RID: 11068
	[SerializeField]
	private GorillaSnapTurn snapTurnController;

	// Token: 0x04002B3D RID: 11069
	private List<XRController> rightControllerList;

	// Token: 0x04002B3E RID: 11070
	private List<XRController> leftcontrollerList;

	// Token: 0x04002B3F RID: 11071
	private const InputDeviceCharacteristics rightCharecteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right;

	// Token: 0x04002B40 RID: 11072
	private const InputDeviceCharacteristics leftCharecteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;

	// Token: 0x04002B41 RID: 11073
	private bool rightControllerValid = true;

	// Token: 0x04002B42 RID: 11074
	private bool leftControllerValid = true;

	// Token: 0x04002B43 RID: 11075
	[SerializeField]
	private bool rightValid = true;

	// Token: 0x04002B44 RID: 11076
	[SerializeField]
	private bool leftValid = true;

	// Token: 0x04002B45 RID: 11077
	[SerializeField]
	private Vector3 lastRightPos;

	// Token: 0x04002B46 RID: 11078
	[SerializeField]
	private Vector3 lastLeftPos;

	// Token: 0x04002B47 RID: 11079
	private Vector3 tempRightPos;

	// Token: 0x04002B48 RID: 11080
	private Vector3 tempLeftPos;

	// Token: 0x04002B49 RID: 11081
	private bool updateControllers;

	// Token: 0x04002B4A RID: 11082
	private GTPlayer playerHandler;

	// Token: 0x04002B4B RID: 11083
	[Tooltip("The rate at which controllers are checked to be moving, if they not moving, overrides and enables one hand mode")]
	[SerializeField]
	private float overridePollRate = 15f;

	// Token: 0x04002B4C RID: 11084
	[SerializeField]
	private bool overrideEnabled;

	// Token: 0x04002B4D RID: 11085
	[SerializeField]
	private OverrideControllers overrideController;

	// Token: 0x0200056B RID: 1387
	[CompilerGenerated]
	private sealed class <ControllerValidator>d__36 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060021D6 RID: 8662 RVA: 0x000B7ADB File Offset: 0x000B5CDB
		[DebuggerHidden]
		public <ControllerValidator>d__36(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x000B7AEC File Offset: 0x000B5CEC
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			ConnectedControllerHandler connectedControllerHandler = this;
			switch (num)
			{
			case 0:
				this.<>1__state = -1;
				this.<>2__current = null;
				this.<>1__state = 1;
				return true;
			case 1:
				this.<>1__state = -1;
				connectedControllerHandler.lastRightPos = ControllerInputPoller.DevicePosition(XRNode.RightHand);
				connectedControllerHandler.lastLeftPos = ControllerInputPoller.DevicePosition(XRNode.LeftHand);
				break;
			case 2:
				this.<>1__state = -1;
				connectedControllerHandler.updateControllers = false;
				if (!connectedControllerHandler.playerHandler.inOverlay)
				{
					if (connectedControllerHandler.rightControllerValid)
					{
						connectedControllerHandler.tempRightPos = ControllerInputPoller.DevicePosition(XRNode.RightHand);
						if (connectedControllerHandler.tempRightPos == connectedControllerHandler.lastRightPos)
						{
							if ((connectedControllerHandler.overrideController & OverrideControllers.RightController) != OverrideControllers.RightController)
							{
								connectedControllerHandler.overrideController |= OverrideControllers.RightController;
								connectedControllerHandler.updateControllers = true;
							}
						}
						else if ((connectedControllerHandler.overrideController & OverrideControllers.RightController) == OverrideControllers.RightController)
						{
							connectedControllerHandler.overrideController &= ~OverrideControllers.RightController;
							connectedControllerHandler.updateControllers = true;
						}
						connectedControllerHandler.lastRightPos = connectedControllerHandler.tempRightPos;
					}
					if (connectedControllerHandler.leftControllerValid)
					{
						connectedControllerHandler.tempLeftPos = ControllerInputPoller.DevicePosition(XRNode.LeftHand);
						if (connectedControllerHandler.tempLeftPos == connectedControllerHandler.lastLeftPos)
						{
							if ((connectedControllerHandler.overrideController & OverrideControllers.LeftController) != OverrideControllers.LeftController)
							{
								connectedControllerHandler.overrideController |= OverrideControllers.LeftController;
								connectedControllerHandler.updateControllers = true;
							}
						}
						else if ((connectedControllerHandler.overrideController & OverrideControllers.LeftController) == OverrideControllers.LeftController)
						{
							connectedControllerHandler.overrideController &= ~OverrideControllers.LeftController;
							connectedControllerHandler.updateControllers = true;
						}
						connectedControllerHandler.lastLeftPos = connectedControllerHandler.tempLeftPos;
					}
					if (connectedControllerHandler.updateControllers)
					{
						connectedControllerHandler.overrideEnabled = connectedControllerHandler.overrideController > OverrideControllers.None;
						connectedControllerHandler.UpdateControllerStates();
					}
				}
				break;
			default:
				return false;
			}
			this.<>2__current = new WaitForSeconds(connectedControllerHandler.overridePollRate);
			this.<>1__state = 2;
			return true;
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060021D9 RID: 8665 RVA: 0x000B7C96 File Offset: 0x000B5E96
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x060021DB RID: 8667 RVA: 0x000B7C96 File Offset: 0x000B5E96
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x04002B4E RID: 11086
		private int <>1__state;

		// Token: 0x04002B4F RID: 11087
		private object <>2__current;

		// Token: 0x04002B50 RID: 11088
		public ConnectedControllerHandler <>4__this;
	}
}
