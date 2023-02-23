namespace DeltaForce.OpenAI.Extentions.GenericExtensions
{
    /// <summary>
    /// An extension class for Enumerable Objects
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Adds the ability to an IEnumerable object to use ForEach Linq Extension.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sources"></param>
        /// <param name="predicate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ForEach<TSource>(this IEnumerable<TSource> sources, Action<TSource> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            foreach (var item in sources)
                predicate(item);
        }
    }
}
