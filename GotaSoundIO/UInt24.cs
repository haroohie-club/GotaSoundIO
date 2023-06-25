using GotaSoundIO.IO;

namespace GotaSoundIO
{
    /// <summary>
    /// Unsigned 24-bit integer.
    /// </summary>
    public struct UInt24 : IReadable, IWriteable
    {
        /// <summary>
        /// Max value.
        /// </summary>
        public const int MaxValue = 16777215;

        /// <summary>
        /// Min value.
        /// </summary>
        public const int MinValue = 0;

        /// <summary>
        /// Data.
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// Check for null.
        /// </summary>
        private void NullCheck()
        {
            _data ??= new byte[3];
        }

        /// <summary>
        /// Get this as an int.
        /// </summary>
        /// <returns>This as an int.</returns>
        private int GetInt()
        {
            NullCheck();
            int ret = 0;
            ret |= _data[2];
            ret |= (_data[1] << 8);
            ret |= (_data[0] << 16);
            return ret;
        }

        /// <summary>
        /// Get data from an int.
        /// </summary>
        /// <param name="val">Int to get data from.</param>
        private static UInt24 FromInt(int val)
        {
            UInt24 ret = new();
            ret.NullCheck();
            ret._data[2] = (byte)((val >> 0) & 0xFF);
            ret._data[1] = (byte)((val >> 8) & 0xFF);
            ret._data[0] = (byte)((val >> 16) & 0xFF);
            return ret;
        }

        /// <summary>
        /// Get this as a uint.
        /// </summary>
        /// <returns>This as a uint.</returns>
        private uint GetUInt()
        {
            return (uint)GetInt();
        }

        /// <summary>
        /// Get this as an int.
        /// </summary>
        /// <param name="val">Value.</param>

        public static implicit operator int(UInt24 val) => val.GetInt();

        /// <summary>
        /// Get this as a uint.
        /// </summary>
        /// <param name="val">Value.</param>

        public static implicit operator uint(UInt24 val) => val.GetUInt();

        /// <summary>
        /// Convert from an int.
        /// </summary>
        /// <param name="val">Value.</param>
        public static explicit operator UInt24(int val) => UInt24.FromInt(val);

        /// <summary>
        /// Convert from a uint.
        /// </summary>
        /// <param name="val">Value.</param>
        public static explicit operator UInt24(uint val) => UInt24.FromInt((int)val);

        /// <summary>
        /// Convert from a float.
        /// </summary>
        /// <param name="val">Value.</param>
        public static explicit operator UInt24(float val) => UInt24.FromInt((int)val);

        /// <summary>
        /// Read the data.
        /// </summary>
        /// <param name="r">The reader.</param>
        public void Read(FileReader r)
        {
            NullCheck();
            switch (r.ByteOrder)
            {
                case ByteOrder.LittleEndian:
                case ByteOrder.System:
                    _data[2] = r.ReadByte();
                    _data[1] = r.ReadByte();
                    _data[0] = r.ReadByte();
                    break;
                case ByteOrder.BigEndian:
                    _data = r.ReadBytes(3);
                    break;
            }
        }

        /// <summary>
        /// Write the data.
        /// </summary>
        /// <param name="w">The writer.</param>
        public void Write(FileWriter w)
        {
            NullCheck();
            switch (w.ByteOrder)
            {
                case ByteOrder.LittleEndian:
                case ByteOrder.System:
                    w.Write(_data[2]);
                    w.Write(_data[1]);
                    w.Write(_data[0]);
                    break;
                case ByteOrder.BigEndian:
                    w.Write(_data);
                    break;
            }
        }
    }
}
