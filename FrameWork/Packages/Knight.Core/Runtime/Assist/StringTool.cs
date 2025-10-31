namespace Knight.Core
{
    public static class StringTool
    {
        #region Static Method

        #endregion
        #region Expand Method
        public static string ReplaceSlash(this string rPath)
        {
            if (string.IsNullOrEmpty(rPath))
            {
                return string.Empty;
            }
            return rPath.Replace('\\', '/');
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        #endregion
    }
}
