using System;

namespace STAR.Format
{
    static class RulesHelpers
    {
        public static SplitResult SplitInTwo(this ReadOnlySpan<char> text, ReadOnlySpan<char> delimiter)
        {
            var result = text.IndexOf(delimiter);
            if (result != -1) //found
            {
                var first = text.Slice(0, result);
                var last = text.Slice(result + delimiter.Length);
                return new SplitResult(first, last);
            }

            return SplitResult.CreateEmpty();
        }
    }
}