using System;

namespace STAR.Format
{
    readonly ref struct SplitResult
    {
        public readonly ReadOnlySpan<char> First { get; init; }
        public readonly ReadOnlySpan<char> Last { get; init; }

        public SplitResult(ReadOnlySpan<char> first, ReadOnlySpan<char> last)
        {
            First = first;
            Last = last;
        }

        public static SplitResult CreateEmpty()
        {
            return new SplitResult(ReadOnlySpan<char>.Empty, ReadOnlySpan<char>.Empty);
        }

        public readonly bool IsEmpty()
        {
            return First.Length == 0 && Last.Length == 0;
        }
    }
}
