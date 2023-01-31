using System;

namespace STAR.Format
{
    readonly ref struct SplitResult
    {
        public readonly ReadOnlySpan<char> first;
        public readonly ReadOnlySpan<char> last;

        public SplitResult(ReadOnlySpan<char> first, ReadOnlySpan<char> last)
        {
            this.first = first;
            this.last = last;
        }

        public static SplitResult CreateEmpty()
        {
            return new SplitResult(ReadOnlySpan<char>.Empty, ReadOnlySpan<char>.Empty);
        }

        public readonly bool IsEmpty()
        {
            return first.Length == 0 && last.Length == 0;
        }
    }
}
