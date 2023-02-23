namespace DeltaForce.OpenAI.Extentions.LiteralExtensions
{
    /// <summary>
    /// An extention class that can manipulate strings.
    /// </summary>
    public static class StringExtensions
    {
        #region Conditions
        /// <summary>
        /// Checks if the source string is Null or Empty.
        ///     It replaces the default Microsoft's class string.IsNullOrEmpty(source).
        /// </summary>
        /// <param name="source">String</param>
        /// <returns>Boolean Result</returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// Checks if the source string is Null or Empty.
        ///     It replaces the default Microsoft's class string.IsNullOrWhiteSpace(source).
        /// </summary>
        /// <param name="source">String</param>
        /// <returns>Boolean Result</returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }
        #endregion

        #region Numeric
        /// <summary>
        /// Converts a string into integer.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static int ToInt(this string source, int @default = int.MinValue)
        {
            if (string.IsNullOrEmpty(source))
                return @default;

            return int.TryParse(source, out var result) ? result : @default;
        }

        /// <summary>
        /// Converts a string into long.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static long ToLong(this string source, long @default = long.MinValue)
        {
            if (string.IsNullOrEmpty(source))
                return @default;

            return long.TryParse(source, out var result) ? result : @default;
        }

        /// <summary>
        /// Converts a string into float.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static float ToFloat(this string source, float @default = float.MinValue)
        {
            if (string.IsNullOrEmpty(source))
                return @default;

            return float.TryParse(source, out var result) ? result : @default;
        }

        /// <summary>
        /// Converts a string into double.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static double ToDouble(this string source, double @default = double.MinValue)
        {
            if (string.IsNullOrEmpty(source))
                return @default;

            return double.TryParse(source, out var result) ? result : @default;
        }

        /// <summary>
        /// Converts a string into decimal.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string source, decimal @default = decimal.MinValue)
        {
            if (string.IsNullOrEmpty(source))
                return @default;

            return decimal.TryParse(source, out var result) ? result : @default;
        }
        #endregion
    }
}
