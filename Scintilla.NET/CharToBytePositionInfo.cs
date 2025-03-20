using System;

namespace ScintillaNET;

internal readonly struct CharToBytePositionInfo(int bytePosition, int charPosition, bool lowSurrogate, int nextCodePointBytePosition) : IEquatable<CharToBytePositionInfo>
{
    public readonly int BytePosition = bytePosition;
    public readonly int CharPosition = charPosition;
    public readonly bool LowSurrogate = lowSurrogate;
    public readonly int NextCodePointBytePosition = nextCodePointBytePosition;

    /// <summary>
    /// Length of this code point.
    /// </summary>
    public int Length => NextCodePointBytePosition - BytePosition;

    /// <summary>
    /// Returns <see cref="NextCodePointBytePosition"/> if <see cref="LowSurrogate"/> is true. In other
    /// words, returns the next code point position in bytes if this <see cref="CharPosition"/>
    /// points to the middle of a code point.
    /// </summary>
    public int RoundToNext => LowSurrogate ? NextCodePointBytePosition : BytePosition;

    /// <summary>
    /// Returns <c><see cref="CharPosition"/> + 1</c> if <see cref="LowSurrogate"/> is true. In other
    /// words, returns the next UTF-16 char position if this <see cref="CharPosition"/>
    /// points to the middle of a code point.
    /// </summary>
    public int RoundToNextChar => LowSurrogate ? CharPosition + 1 : CharPosition;

    public CharToBytePositionInfo(int bytePosition) : this(bytePosition, -1, false, -1) { }

    public CharToBytePositionInfo(int bytePosition, int charPosition) : this(bytePosition, charPosition, false, -1) { }

    public override bool Equals(object obj)
    {
        return obj is CharToBytePositionInfo info && Equals(info);
    }

    public bool Equals(CharToBytePositionInfo other)
    {
        return this.BytePosition == other.BytePosition &&
               this.CharPosition == other.CharPosition &&
               this.LowSurrogate == other.LowSurrogate &&
               this.NextCodePointBytePosition == other.NextCodePointBytePosition;
    }

    public override int GetHashCode()
    {
        int hashCode = 873051011;
        hashCode = hashCode * -1521134295 + this.BytePosition.GetHashCode();
        hashCode = hashCode * -1521134295 + this.CharPosition.GetHashCode();
        hashCode = hashCode * -1521134295 + this.LowSurrogate.GetHashCode();
        hashCode = hashCode * -1521134295 + this.NextCodePointBytePosition.GetHashCode();
        return hashCode;
    }

    public static bool operator ==(CharToBytePositionInfo left, CharToBytePositionInfo right) => left.Equals(right);
    public static bool operator !=(CharToBytePositionInfo left, CharToBytePositionInfo right) => !(left == right);
}
