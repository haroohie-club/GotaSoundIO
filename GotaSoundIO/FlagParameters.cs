using GotaSoundIO.IO;

namespace GotaSoundIO
{
    /// <summary>
    /// Has optional parameters that are enabled by bit flags.
    /// </summary>
    public class FlagParameters : IReadable, IWriteable
    {
        /// <summary>
        /// Parameters.
        /// </summary>
        private uint?[] _parameters = new uint?[32];

        /// <summary>
        /// Get parameter.
        /// </summary>
        /// <param name="bit">Bit index.</param>
        /// <returns>Parameter at bit index.</returns>
        public uint? this[int bit] { get { return _parameters[bit]; } set { _parameters[bit] = value; } }

        /// <summary>
        /// Read the item.
        /// </summary>
        /// <param name="r">The reader.</param>
        public void Read(FileReader r)
        {
            uint mask = r.ReadUInt32();
            for (int i = 0; i < 32; i++)
            {
                if ((mask & (0b1 << i)) > 0)
                {
                    _parameters[i] = r.ReadUInt32();
                }
                else
                {
                    _parameters[i] = null;
                }
            }
        }

        /// <summary>
        /// Write the item.
        /// </summary>
        /// <param name="w">The writer.</param>
        public void Write(FileWriter w)
        {
            uint mask = 0;
            for (int i = 0; i < 32; i++)
            {
                if (_parameters[i] != null)
                {
                    mask |= (uint)(0b1 << i);
                }
            }
            w.Write(mask);
            foreach (var p in _parameters)
            {
                if (p != null)
                {
                    w.Write(p.Value);
                }
            }
        }
    }
}
