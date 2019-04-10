using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowMachine {

#region Init
	public FlowMachine(string flowMachineName) {
		this._flowMachineName = flowMachineName;
	}
#endregion Init
	
#region Flow Machine
	private string _flowMachineName;
	
	private List<IFlow> _flows = new List<IFlow>();
	private Coroutine _flowCor = null;

	public void AddFlow(IFlow flow) {
		_flows.Add(flow);
	}

	public void StartFlow(Action onFlowCompleteCallback = null) {
		if (_flowCor != null) {
			Debug.LogError($"{_flowMachineName}.FlowMachine.StartFlow: Flow is already running!");
			return;
		}

		_flowCor = RoutineRunner.StartRoutine(FlowCor(onFlowCompleteCallback));
	}

	private IEnumerator FlowCor(Action onFlowCompleteCallback) {
		Debug.Log($"{_flowMachineName}.FlowMachine.FlowCor: Starting flow");

		// Loop through each flow.
		// Check if it should be run or not.
		// Wait for a flow to run before moving on to the next.
		for (int i = 0, n = _flows.Count; i < n; i++) {
			if (_flows[i].ShouldRun()) {
				Debug.Log($"{_flowMachineName}.FlowMachine.FlowCor: Starting flow step: {_flows[i].FlowName()}");

				yield return _flows[i].RunFlow();
			}
		}
		
		Debug.Log($"{_flowMachineName}.FlowMachine.FlowCor: Finished flow");

		onFlowCompleteCallback?.Invoke();
	}
#endregion Flow Machine

}