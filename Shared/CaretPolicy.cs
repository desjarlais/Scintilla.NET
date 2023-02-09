using System;

namespace ScintillaNET
{
    /// <summary>
    /// The caret policy.
    /// </summary>
    [Flags]
    public enum CaretPolicy
    {
        /// <summary>
        /// If set, we can define a slop value: caretSlop. This value defines an unwanted zone (UZ)
        /// where the caret is... unwanted. This zone is defined as a number of pixels near the
        /// vertical margins, and as a number of lines near the horizontal margins. By keeping the
        /// caret away from the edges, it is seen within its context. This makes it likely that the
        /// identifier that the caret is on can be completely seen, and that the current line is seen
        /// with some of the lines following it, which are often dependent on that line.
        /// </summary>
        Slop = NativeMethods.CARET_SLOP,

        /// <summary>
        /// If set, the policy set by CARET_SLOP is enforced... strictly. The caret is centered on the
        /// display if caretSlop is not set, and cannot go in the UZ if caretSlop is set.
        /// </summary>
        Strict = NativeMethods.CARET_STRICT,

        /// <summary>
        /// If set, the display is moved more energetically so the caret can move in the same direction
        /// longer before the policy is applied again. '3UZ' notation is used to indicate three time
        /// the size of the UZ as a distance to the margin.
        /// </summary>
        Jumps = NativeMethods.CARET_JUMPS,

        /// <summary>
        /// If not set, instead of having symmetrical UZs, the left and bottom UZs are extended up to
        /// right and top UZs respectively. This way, we favour the displaying of useful information:
        /// the beginning of lines, where most code reside, and the lines after the caret, for example,
        /// the body of a function.
        /// </summary>
        Even = NativeMethods.CARET_EVEN
    }
}
