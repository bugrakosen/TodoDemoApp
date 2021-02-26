using System.Collections;

namespace TodoDemoApp.API.Utils
{
    public static class HelperExtensions
    {
        /// <summary>
        /// Checks whether or not collection is null or empty. Assumes collection can be safely enumerated multiple times.
        /// </summary>
        public static bool IsNullOrEmpty(this IEnumerable @this) => @this == null || @this.GetEnumerator().MoveNext() == false;
    }
}
