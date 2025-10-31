namespace Knight.Core
{
    public class ProfilerTool
    {
        [System.Diagnostics.Conditional("ENABLE_GAME_PROFILER")]
        public static void BeginSample(string rName)
        {
            UnityEngine.Profiling.Profiler.BeginSample(rName);
        }
        [System.Diagnostics.Conditional("ENABLE_GAME_PROFILER")]
        public static void EndSample()
        {
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

}
