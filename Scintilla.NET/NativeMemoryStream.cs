using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ScintillaNET;

/// <summary>
/// Like an UnmanagedMemoryStream execpt it can grow.
/// </summary>
internal sealed unsafe class NativeMemoryStream : Stream
{
    #region Fields

    private int capacity;
    private int position;
    private int length;

    #endregion Fields

    #region Methods

    protected override void Dispose(bool disposing)
    {
        if (FreeOnDispose && Pointer != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(Pointer);
            Pointer = IntPtr.Zero;
        }

        base.Dispose(disposing);
    }

    public override void Flush()
    {
        // NOP
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                this.position = (int)offset;
                break;

            default:
                throw new NotImplementedException();
        }

        return this.position;
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (this.position + count > this.capacity)
        {
            // Realloc buffer
            int minCapacity = this.position + count;
            int newCapacity = this.capacity * 2;
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;

            IntPtr newPtr = Marshal.AllocHGlobal(newCapacity);
            NativeMethods.MoveMemory(newPtr, Pointer, this.length);
            Marshal.FreeHGlobal(Pointer);

            Pointer = newPtr;
            this.capacity = newCapacity;
        }

        Marshal.Copy(buffer, offset, (IntPtr)((long)Pointer + this.position), count);
        this.position += count;
        this.length = Math.Max(this.length, this.position);
    }

    #endregion Methods

    #region Properties

    public override bool CanRead
    {
        get { throw new NotImplementedException(); }
    }

    public override bool CanSeek
    {
        get
        {
            return true;
        }
    }

    public override bool CanWrite
    {
        get
        {
            return true;
        }
    }

    public bool FreeOnDispose { get; set; }

    public override long Length
    {
        get
        {
            return this.length;
        }
    }

    public IntPtr Pointer { get; private set; }

    public override long Position
    {
        get
        {
            return this.position;
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    #endregion Properties

    #region Constructors

    public NativeMemoryStream(int capacity)
    {
        if (capacity < 4)
            capacity = 4;

        this.capacity = capacity;
        Pointer = Marshal.AllocHGlobal(capacity);
        FreeOnDispose = true;
    }

    #endregion Constructors
}
