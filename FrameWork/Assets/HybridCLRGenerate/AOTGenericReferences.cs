using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"Core.dll",
		"Framework.UI.dll",
		"Game.dll",
		"System.Core.dll",
		"UnityEngine.CoreModule.dll",
		"UnityFx.Async.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// FancyScrollView.FancyCell<System.UIntPtr,object>
	// FancyScrollView.FancyCell<object,object>
	// FancyScrollView.FancyCellGroup.<>c<object,object>
	// FancyScrollView.FancyCellGroup<object,object>
	// FancyScrollView.FancyScrollView<object,object>
	// Knight.Core.Dict<int,object>
	// Knight.Core.Dict<object,int>
	// Knight.Core.Dict<object,object>
	// Knight.Core.IndexedDict<object,object>
	// Knight.Core.ObservableList<object>
	// System.Action<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Action<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Action<byte>
	// System.Action<float>
	// System.Action<int>
	// System.Action<object,UnityEngine.Color>
	// System.Action<object,UnityEngine.Vector2>
	// System.Action<object,UnityEngine.Vector3>
	// System.Action<object,byte>
	// System.Action<object,float>
	// System.Action<object,int>
	// System.Action<object,long>
	// System.Action<object,object>
	// System.Action<object>
	// System.Collections.Concurrent.ConcurrentStack.<GetEnumerator>d__35<object>
	// System.Collections.Concurrent.ConcurrentStack.Node<object>
	// System.Collections.Concurrent.ConcurrentStack<object>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ArraySortHelper<float>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.Comparer<float>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<float>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IComparer<float>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<float>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<float>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IList<float>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List.Enumerator<float>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.List<float>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ObjectComparer<float>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.ObjectModel.ReadOnlyCollection<float>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Comparison<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Comparison<float>
	// System.Comparison<object>
	// System.Func<System.Threading.Tasks.VoidTaskResult>
	// System.Func<UnityEngine.Color>
	// System.Func<UnityEngine.Vector2>
	// System.Func<UnityEngine.Vector3>
	// System.Func<byte>
	// System.Func<float>
	// System.Func<int,object>
	// System.Func<int>
	// System.Func<long>
	// System.Func<object,System.Threading.Tasks.VoidTaskResult>
	// System.Func<object,byte>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.Nullable<int>
	// System.Predicate<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Predicate<float>
	// System.Predicate<object>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<byte>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<byte>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<System.Threading.Tasks.VoidTaskResult>
	// System.Runtime.CompilerServices.TaskAwaiter<byte>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<byte>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.Task<byte>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskCompletionSource<byte>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<byte>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<object>
	// System.Threading.Tasks.TaskFactory<System.Threading.Tasks.VoidTaskResult>
	// System.Threading.Tasks.TaskFactory<byte>
	// System.Threading.Tasks.TaskFactory<object>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<object>
	// UnityFx.Async.CompilerServices.AsyncAwaiter<object>
	// UnityFx.Async.IAsyncOperation<object>
	// }}

	public void RefMethods()
	{
		// Knight.Core.Dict<int,object> Knight.Core.DictExpand.Sort<int,object>(Knight.Core.Dict<int,object>,System.Comparison<System.Collections.Generic.KeyValuePair<int,object>>)
		// System.Void Knight.Core.IndexedDictExpand.Clear<object,object>(Knight.Core.IndexedDict<object,object>)
		// System.Collections.Generic.KeyValuePair<object,object> Knight.Core.IndexedDictExpand.Last<object,object>(Knight.Core.IndexedDict<object,object>)
		// object Knight.Core.IndexedDictExpand.LastKey<object,object>(Knight.Core.IndexedDict<object,object>)
		// object Knight.Core.IndexedDictExpand.LastValue<object,object>(Knight.Core.IndexedDict<object,object>)
		// bool Knight.Core.IndexedDictExpand.Remove<object,object>(Knight.Core.IndexedDict<object,object>,object)
		// bool Knight.Core.IndexedDictExpand.TryGetValue<object,object>(Knight.Core.IndexedDict<object,object>,object,object&)
		// object Knight.Core.ObjectExpand.ReceiveComponent<object>(UnityEngine.GameObject)
		// System.Void Knight.Core.UtilTool.SafeExecute<object,int>(System.Action<object,int>,object,int)
		// System.Void Knight.Core.UtilTool.SafeExecute<object>(System.Action<object>,object)
		// object ProtoMsgEventArgExpand.Get<object>(Knight.Core.ProtoMsgEventArg)
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// bool System.Linq.Enumerable.Contains<object>(System.Collections.Generic.IEnumerable<object>,object)
		// bool System.Linq.Enumerable.Contains<object>(System.Collections.Generic.IEnumerable<object>,object,System.Collections.Generic.IEqualityComparer<object>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitOnCompleted<UnityFx.Async.CompilerServices.AsyncAwaiter<object>,Knight.Hotfix.Core.GameStage.<Run_Async>d__4>(UnityFx.Async.CompilerServices.AsyncAwaiter<object>&,Knight.Hotfix.Core.GameStage.<Run_Async>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitOnCompleted<UnityFx.Async.CompilerServices.AsyncAwaiter<object>,UnityEngine.UI.FrameManager.<CheckBeforeClose>d__22>(UnityFx.Async.CompilerServices.AsyncAwaiter<object>&,UnityEngine.UI.FrameManager.<CheckBeforeClose>d__22&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitOnCompleted<UnityFx.Async.CompilerServices.AsyncAwaiter<object>,UnityEngine.UI.ViewManager.<MaybeCloseTopView>d__40>(UnityFx.Async.CompilerServices.AsyncAwaiter<object>&,UnityEngine.UI.ViewManager.<MaybeCloseTopView>d__40&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitOnCompleted<UnityFx.Async.CompilerServices.AsyncAwaiter<object>,Knight.Hotfix.Core.GameStage.<Run_Async>d__4>(UnityFx.Async.CompilerServices.AsyncAwaiter<object>&,Knight.Hotfix.Core.GameStage.<Run_Async>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitOnCompleted<UnityFx.Async.CompilerServices.AsyncAwaiter<object>,UnityEngine.UI.FrameManager.<CheckBeforeClose>d__22>(UnityFx.Async.CompilerServices.AsyncAwaiter<object>&,UnityEngine.UI.FrameManager.<CheckBeforeClose>d__22&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitOnCompleted<UnityFx.Async.CompilerServices.AsyncAwaiter<object>,UnityEngine.UI.ViewManager.<MaybeCloseTopView>d__40>(UnityFx.Async.CompilerServices.AsyncAwaiter<object>&,UnityEngine.UI.ViewManager.<MaybeCloseTopView>d__40&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.BattleHotfix.<Initialize>d__4>(System.Runtime.CompilerServices.TaskAwaiter&,Game.BattleHotfix.<Initialize>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.GameState_Battle.<OnEnter>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,Game.GameState_Battle.<OnEnter>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.GameState_Battle.<OnExit>d__6>(System.Runtime.CompilerServices.TaskAwaiter&,Game.GameState_Battle.<OnExit>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.LoginView.<OnOpen>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,Game.LoginView.<OnOpen>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.MainLogic_Hotfix.<Initialize_Async>d__1>(System.Runtime.CompilerServices.TaskAwaiter&,Game.MainLogic_Hotfix.<Initialize_Async>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.Model.<Initialize>d__7>(System.Runtime.CompilerServices.TaskAwaiter&,Game.Model.<Initialize>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.GameMode.<Destroy>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.GameMode.<Destroy>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.GameStageManager.<StageRunning>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.GameStageManager.<StageRunning>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StageTask.<Run_Async>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StageTask.<Run_Async>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateBaseAsync.<Enter>d__8<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateBaseAsync.<Enter>d__8<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateBaseAsync.<Exit>d__11<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateBaseAsync.<Exit>d__11<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateBaseAsync.<Initialize>d__6<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateBaseAsync.<Initialize>d__6<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateBaseAsync.<PreEnter>d__7<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateBaseAsync.<PreEnter>d__7<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateBaseAsync.<ReEnter>d__9<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateBaseAsync.<ReEnter>d__9<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.FrameManager.<BackViewAsync>d__38>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.FrameManager.<BackViewAsync>d__38&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.FrameManager.<CloseView>d__23>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.FrameManager.<CloseView>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.FrameManager.<CloseWithAnim>d__37>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.FrameManager.<CloseWithAnim>d__37&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.View.<Open>d__31>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.View.<Open>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.ViewController.<Open>d__36>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.ViewController.<Open>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,Game.GameStateMachine.<OnInitialize>d__4>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,Game.GameStateMachine.<OnInitialize>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,Game.MainLogic_Hotfix.<Initialize_Async>d__1>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,Game.MainLogic_Hotfix.<Initialize_Async>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.GameState_BattleResult.<OnEnter>d__3>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.GameState_BattleResult.<OnEnter>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.GameState_Lobby.<OnEnter>d__5>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.GameState_Lobby.<OnEnter>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.GameState_Lobby.<OnReEnter>d__4>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.GameState_Lobby.<OnReEnter>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.GameState_Login.<OnLoadAsset>d__6>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.GameState_Login.<OnLoadAsset>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.GameState_Login.<OnReEnter>d__8>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.GameState_Login.<OnReEnter>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.FrameManager.<BackViewAsync>d__38>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.FrameManager.<BackViewAsync>d__38&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.FrameManager.<OpenMainPopupFixedViews>d__43>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.FrameManager.<OpenMainPopupFixedViews>d__43&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewContainer.<PreloadFixedUIAsync>d__5>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewContainer.<PreloadFixedUIAsync>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewContainer.<PreloadPopUIAsync>d__6>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewContainer.<PreloadPopUIAsync>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewContainer.<ReOpenAsync>d__15>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewContainer.<ReOpenAsync>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.BattleHotfix.<Initialize>d__4>(System.Runtime.CompilerServices.TaskAwaiter&,Game.BattleHotfix.<Initialize>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.GameState_Battle.<OnEnter>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,Game.GameState_Battle.<OnEnter>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.GameState_Battle.<OnExit>d__6>(System.Runtime.CompilerServices.TaskAwaiter&,Game.GameState_Battle.<OnExit>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.LoginView.<OnOpen>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,Game.LoginView.<OnOpen>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.MainLogic_Hotfix.<Initialize_Async>d__1>(System.Runtime.CompilerServices.TaskAwaiter&,Game.MainLogic_Hotfix.<Initialize_Async>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Game.Model.<Initialize>d__7>(System.Runtime.CompilerServices.TaskAwaiter&,Game.Model.<Initialize>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.GameMode.<Destroy>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.GameMode.<Destroy>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.GameStageManager.<StageRunning>d__5>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.GameStageManager.<StageRunning>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StageTask.<Run_Async>d__3>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StageTask.<Run_Async>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateBaseAsync.<Enter>d__8<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateBaseAsync.<Enter>d__8<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateBaseAsync.<Exit>d__11<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateBaseAsync.<Exit>d__11<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateBaseAsync.<Initialize>d__6<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateBaseAsync.<Initialize>d__6<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateBaseAsync.<PreEnter>d__7<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateBaseAsync.<PreEnter>d__7<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateBaseAsync.<ReEnter>d__9<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateBaseAsync.<ReEnter>d__9<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.FrameManager.<BackViewAsync>d__38>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.FrameManager.<BackViewAsync>d__38&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.FrameManager.<CloseView>d__23>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.FrameManager.<CloseView>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.FrameManager.<CloseWithAnim>d__37>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.FrameManager.<CloseWithAnim>d__37&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.View.<Open>d__31>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.View.<Open>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.ViewController.<Open>d__36>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.ViewController.<Open>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,Game.GameStateMachine.<OnInitialize>d__4>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,Game.GameStateMachine.<OnInitialize>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,Game.MainLogic_Hotfix.<Initialize_Async>d__1>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,Game.MainLogic_Hotfix.<Initialize_Async>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.GameState_BattleResult.<OnEnter>d__3>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.GameState_BattleResult.<OnEnter>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.GameState_Lobby.<OnEnter>d__5>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.GameState_Lobby.<OnEnter>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.GameState_Lobby.<OnReEnter>d__4>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.GameState_Lobby.<OnReEnter>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.GameState_Login.<OnLoadAsset>d__6>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.GameState_Login.<OnLoadAsset>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,Game.GameState_Login.<OnReEnter>d__8>(System.Runtime.CompilerServices.TaskAwaiter<object>&,Game.GameState_Login.<OnReEnter>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.FrameManager.<BackViewAsync>d__38>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.FrameManager.<BackViewAsync>d__38&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.FrameManager.<OpenMainPopupFixedViews>d__43>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.FrameManager.<OpenMainPopupFixedViews>d__43&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewContainer.<PreloadFixedUIAsync>d__5>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewContainer.<PreloadFixedUIAsync>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewContainer.<PreloadPopUIAsync>d__6>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewContainer.<PreloadPopUIAsync>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<System.Threading.Tasks.VoidTaskResult>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewContainer.<ReOpenAsync>d__15>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewContainer.<ReOpenAsync>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateMachineBaseAsync.<TryAddState>d__14<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateMachineBaseAsync.<TryAddState>d__14<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.StateMachineBaseAsync.<TrySwitchState>d__16<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.StateMachineBaseAsync.<TrySwitchState>d__16<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,Knight.Hotfix.Core.StateMachineBaseAsync.<TrySwitchCacheState>d__17<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,Knight.Hotfix.Core.StateMachineBaseAsync.<TrySwitchCacheState>d__17<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<byte>,Knight.Hotfix.Core.StateMachineBaseAsync.<TrySwitchState>d__16<object,object,object,object>>(System.Runtime.CompilerServices.TaskAwaiter<byte>&,Knight.Hotfix.Core.StateMachineBaseAsync.<TrySwitchState>d__16<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.FrameManager.<OpenPageUIAsync>d__19>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.FrameManager.<OpenPageUIAsync>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.ViewManager.<OpenAsync>d__28>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.ViewManager.<OpenAsync>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,UnityEngine.UI.ViewManager.<OpenView>d__33>(System.Runtime.CompilerServices.TaskAwaiter&,UnityEngine.UI.ViewManager.<OpenView>d__33&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.FrameManager.<OpenFixedAsync>d__32>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.FrameManager.<OpenFixedAsync>d__32&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.FrameManager.<OpenPageUIAsync>d__19>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.FrameManager.<OpenPageUIAsync>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.FrameManager.<OpenPageUIAsync>d__25>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.FrameManager.<OpenPageUIAsync>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.FrameManager.<OpenPageUIAsyncBackView>d__26>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.FrameManager.<OpenPageUIAsyncBackView>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.FrameManager.<OpenPopUIAsync>d__29>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.FrameManager.<OpenPopUIAsync>d__29&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.FrameManager.<OpenPopupFixedUIAsync>d__34>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.FrameManager.<OpenPopupFixedUIAsync>d__34&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewContainer.<OpenFixedAsync>d__9>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewContainer.<OpenFixedAsync>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewContainer.<OpenPageUIAsync>d__7>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewContainer.<OpenPageUIAsync>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewContainer.<OpenPopUIAsync>d__8>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewContainer.<OpenPopUIAsync>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewContainer.<OpenPopupFixedUIAsync>d__10>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewContainer.<OpenPopupFixedUIAsync>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewManager.<OpenAsync>d__28>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewManager.<OpenAsync>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewManager.<OpenViewAsync>d__29>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewManager.<OpenViewAsync>d__29&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewManager.<PreLoadAsync>d__25>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewManager.<PreLoadAsync>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UnityEngine.UI.ViewManager.<PreLoadViewAsync>d__26>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UnityEngine.UI.ViewManager.<PreLoadViewAsync>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.BattleHotfix.<Destroy>d__6>(Game.BattleHotfix.<Destroy>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.BattleHotfix.<Initialize>d__4>(Game.BattleHotfix.<Initialize>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.BattleHotfix.<InitializeLogicBattle>d__5>(Game.BattleHotfix.<InitializeLogicBattle>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameStateMachine.<OnInitialize>d__4>(Game.GameStateMachine.<OnInitialize>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameState_Battle.<OnEnter>d__5>(Game.GameState_Battle.<OnEnter>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameState_Battle.<OnExit>d__6>(Game.GameState_Battle.<OnExit>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameState_Battle.<OnPreEnter>d__4>(Game.GameState_Battle.<OnPreEnter>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameState_BattleResult.<OnEnter>d__3>(Game.GameState_BattleResult.<OnEnter>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameState_BattleResult.<OnExit>d__4>(Game.GameState_BattleResult.<OnExit>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameState_Lobby.<OnEnter>d__5>(Game.GameState_Lobby.<OnEnter>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameState_Lobby.<OnExit>d__6>(Game.GameState_Lobby.<OnExit>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameState_Lobby.<OnPreEnter>d__3>(Game.GameState_Lobby.<OnPreEnter>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameState_Lobby.<OnReEnter>d__4>(Game.GameState_Lobby.<OnReEnter>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameState_Login.<OnLoadAsset>d__6>(Game.GameState_Login.<OnLoadAsset>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.GameState_Login.<OnReEnter>d__8>(Game.GameState_Login.<OnReEnter>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.LoginView.<OnOpen>d__5>(Game.LoginView.<OnOpen>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.MainLogic_Hotfix.<InitializeRenderSetting>d__4>(Game.MainLogic_Hotfix.<InitializeRenderSetting>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.MainLogic_Hotfix.<Initialize_Async>d__1>(Game.MainLogic_Hotfix.<Initialize_Async>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.Model.<Initialize>d__7>(Game.Model.<Initialize>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Game.ModelBase.<OnInitialize>d__6>(Game.ModelBase.<OnInitialize>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.GameMode.<Destroy>d__3>(Knight.Hotfix.Core.GameMode.<Destroy>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.GameMode.<OnDestoryLogout>d__6>(Knight.Hotfix.Core.GameMode.<OnDestoryLogout>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.GameMode.<OnDestroy>d__5>(Knight.Hotfix.Core.GameMode.<OnDestroy>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.GameStage.<Run_Async>d__4>(Knight.Hotfix.Core.GameStage.<Run_Async>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.GameStageManager.<StageRunning>d__5>(Knight.Hotfix.Core.GameStageManager.<StageRunning>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StageTask.<OnRun_Async>d__5>(Knight.Hotfix.Core.StageTask.<OnRun_Async>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StageTask.<Run_Async>d__3>(Knight.Hotfix.Core.StageTask.<Run_Async>d__3&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<Enter>d__8<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<Enter>d__8<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<Exit>d__11<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<Exit>d__11<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<Initialize>d__6<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<Initialize>d__6<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<OnEnter>d__16<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<OnEnter>d__16<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<OnExit>d__19<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<OnExit>d__19<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<OnInitialize>d__13<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<OnInitialize>d__13<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<OnLoadAsset>d__14<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<OnLoadAsset>d__14<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<OnPreEnter>d__15<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<OnPreEnter>d__15<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<OnReEnter>d__17<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<OnReEnter>d__17<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<OnSwitchStateComplete>d__23<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<OnSwitchStateComplete>d__23<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<PreEnter>d__7<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<PreEnter>d__7<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateBaseAsync.<ReEnter>d__9<object,object,object,object>>(Knight.Hotfix.Core.StateBaseAsync.<ReEnter>d__9<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.StateMachineBaseAsync.<OnInitialize>d__19<object,object,object,object>>(Knight.Hotfix.Core.StateMachineBaseAsync.<OnInitialize>d__19<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<Knight.Hotfix.Core.THotfixKnightObject.<Initialize>d__2<object>>(Knight.Hotfix.Core.THotfixKnightObject.<Initialize>d__2<object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.FrameManager.<BackViewAsync>d__38>(UnityEngine.UI.FrameManager.<BackViewAsync>d__38&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.FrameManager.<CheckBeforeClose>d__22>(UnityEngine.UI.FrameManager.<CheckBeforeClose>d__22&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.FrameManager.<CloseView>d__23>(UnityEngine.UI.FrameManager.<CloseView>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.FrameManager.<CloseWithAnim>d__37>(UnityEngine.UI.FrameManager.<CloseWithAnim>d__37&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.FrameManager.<OpenMainPopupFixedViews>d__43>(UnityEngine.UI.FrameManager.<OpenMainPopupFixedViews>d__43&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.FrameManager.<SwitchToMainCity>d__24>(UnityEngine.UI.FrameManager.<SwitchToMainCity>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.View.<Open>d__31>(UnityEngine.UI.View.<Open>d__31&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.ViewContainer.<PreloadFixedUIAsync>d__5>(UnityEngine.UI.ViewContainer.<PreloadFixedUIAsync>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.ViewContainer.<PreloadPopUIAsync>d__6>(UnityEngine.UI.ViewContainer.<PreloadPopUIAsync>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.ViewContainer.<ReOpenAsync>d__15>(UnityEngine.UI.ViewContainer.<ReOpenAsync>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.ViewController.<OnOpen>d__41>(UnityEngine.UI.ViewController.<OnOpen>d__41&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.ViewController.<Open>d__36>(UnityEngine.UI.ViewController.<Open>d__36&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.ViewController.<UIPreloading>d__45>(UnityEngine.UI.ViewController.<UIPreloading>d__45&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<UnityEngine.UI.ViewManager.<MaybeCloseTopView>d__40>(UnityEngine.UI.ViewManager.<MaybeCloseTopView>d__40&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<Knight.Hotfix.Core.StateMachineBaseAsync.<TryAddState>d__14<object,object,object,object>>(Knight.Hotfix.Core.StateMachineBaseAsync.<TryAddState>d__14<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<Knight.Hotfix.Core.StateMachineBaseAsync.<TrySwitchCacheState>d__17<object,object,object,object>>(Knight.Hotfix.Core.StateMachineBaseAsync.<TrySwitchCacheState>d__17<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<byte>.Start<Knight.Hotfix.Core.StateMachineBaseAsync.<TrySwitchState>d__16<object,object,object,object>>(Knight.Hotfix.Core.StateMachineBaseAsync.<TrySwitchState>d__16<object,object,object,object>&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.FrameManager.<OpenFixedAsync>d__32>(UnityEngine.UI.FrameManager.<OpenFixedAsync>d__32&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.FrameManager.<OpenMainCityScene3DUIAsync>d__28>(UnityEngine.UI.FrameManager.<OpenMainCityScene3DUIAsync>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.FrameManager.<OpenPageUIAsync>d__19>(UnityEngine.UI.FrameManager.<OpenPageUIAsync>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.FrameManager.<OpenPageUIAsync>d__25>(UnityEngine.UI.FrameManager.<OpenPageUIAsync>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.FrameManager.<OpenPageUIAsyncBackView>d__26>(UnityEngine.UI.FrameManager.<OpenPageUIAsyncBackView>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.FrameManager.<OpenPopUIAsync>d__29>(UnityEngine.UI.FrameManager.<OpenPopUIAsync>d__29&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.FrameManager.<OpenPopupFixedUIAsync>d__34>(UnityEngine.UI.FrameManager.<OpenPopupFixedUIAsync>d__34&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.ViewContainer.<OpenFixedAsync>d__9>(UnityEngine.UI.ViewContainer.<OpenFixedAsync>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.ViewContainer.<OpenPageUIAsync>d__7>(UnityEngine.UI.ViewContainer.<OpenPageUIAsync>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.ViewContainer.<OpenPopUIAsync>d__8>(UnityEngine.UI.ViewContainer.<OpenPopUIAsync>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.ViewContainer.<OpenPopupFixedUIAsync>d__10>(UnityEngine.UI.ViewContainer.<OpenPopupFixedUIAsync>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.ViewManager.<OpenAsync>d__28>(UnityEngine.UI.ViewManager.<OpenAsync>d__28&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.ViewManager.<OpenView>d__33>(UnityEngine.UI.ViewManager.<OpenView>d__33&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.ViewManager.<OpenViewAsync>d__29>(UnityEngine.UI.ViewManager.<OpenViewAsync>d__29&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.ViewManager.<PreLoadAsync>d__25>(UnityEngine.UI.ViewManager.<PreLoadAsync>d__25&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<UnityEngine.UI.ViewManager.<PreLoadViewAsync>d__26>(UnityEngine.UI.ViewManager.<PreLoadViewAsync>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,Knight.Hotfix.Core.AsyncWrap.<WrapErrors>d__0>(System.Runtime.CompilerServices.TaskAwaiter&,Knight.Hotfix.Core.AsyncWrap.<WrapErrors>d__0&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<Knight.Hotfix.Core.AsyncWrap.<WrapErrors>d__0>(Knight.Hotfix.Core.AsyncWrap.<WrapErrors>d__0&)
		// object UnityEngine.Component.GetComponent<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// object UnityEngine.Object.Instantiate<object>(object)
		// UnityFx.Async.CompilerServices.AsyncAwaiter<object> UnityFx.Async.AsyncExtensions.GetAwaiter<object>(UnityFx.Async.IAsyncOperation<object>)
	}
}