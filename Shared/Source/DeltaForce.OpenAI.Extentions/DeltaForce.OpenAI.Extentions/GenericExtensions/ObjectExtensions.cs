using Newtonsoft.Json;

namespace DeltaForce.OpenAI.Extentions.GenericExtensions
{
    /// <summary>
    /// General Object Extensions
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts an object into Json string.
        ///     Using Newtonsoft Library.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToJson(this object? source)
        {
            if (source is null)
                return string.Empty;

            return JsonConvert.SerializeObject(source, Formatting.Indented);
        }

        /// <summary>
        /// Converts a string into object.
        ///     Using Newtonsoft Library.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TSource? JsonToObject<TSource>(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return default;

            return JsonConvert.DeserializeObject<TSource>(source) ?? default;
        }

        /// <summary>
        /// Emulates the database's "IN" statement
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool IsIn<TSource>(this TSource source, params TSource[] options)
        {
            return options.Contains(source);
        }
    }
}
