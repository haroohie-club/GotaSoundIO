using NAudio.Wave;

namespace GotaSoundIO.Sound.Playback
{
    /// <summary>
    /// Stream for looping playback
    /// </summary>
    public class LoopStream : WaveStream
    {
        /// <summary>
        /// Source stream.
        /// </summary>
        private WaveStream _sourceStream;

        /// <summary>
        /// Loop start.
        /// </summary>
        public uint LoopStart;

        /// <summary>
        /// Loop end.
        /// </summary>
        public uint LoopEnd;

        /// <summary>
        /// Player.
        /// </summary>
        private StreamPlayer _player;

        /// <summary>
        /// Creates a new Loop stream.
        /// </summary>
        /// <param name="player">Player.</param>
        /// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end or else we will not loop to the start again.</param>
        /// <param name="loops">If to loop the playback.</param>
        /// <param name="loopStart">Loop start sample.</param>
        /// <param name="loopEnd">Loop end sample.</param>
        public LoopStream(StreamPlayer player, WaveStream sourceStream, bool loops, uint loopStart, uint loopEnd)
        {
            _player = player;
            _sourceStream = sourceStream;
            Loops = loops;
            LoopStart = loopStart;
            LoopEnd = loopEnd;
        }

        /// <summary>
        /// Use this to turn looping on or off
        /// </summary>
        public bool Loops { get; set; }

        /// <summary>
        /// Return source stream's wave format
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return _sourceStream.WaveFormat; }
        }

        /// <summary>
        /// LoopStream simply returns
        /// </summary>
        public override long Length
        {
            get { return _sourceStream.Length; }
        }

        /// <summary>
        /// LoopStream simply passes on positioning to source stream
        /// </summary>
        public override long Position
        {
            get { return _sourceStream.Position; }
            set { _sourceStream.Position = value; }
        }

        /// <summary>
        /// Current sample.
        /// </summary>
        public uint CurrentSample
        {
            get { return (uint)(_sourceStream.Position / WaveFormat.Channels / (WaveFormat.BitsPerSample / 8)); }
            set { _sourceStream.Position = value * WaveFormat.Channels * (WaveFormat.BitsPerSample / 8); }
        }

        /// <summary>
        /// The length in samples.
        /// </summary>
        public uint GetLengthInSamples
        {
            get => (uint)(_sourceStream.Length / WaveFormat.Channels / (WaveFormat.BitsPerSample / 8));
        }

        /// <summary>
        /// Read data.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;
            while (totalBytesRead < count)
            {
                int bytesRead = _sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0 || _sourceStream.Position > LoopEnd * WaveFormat.Channels * WaveFormat.BitsPerSample / 8 || _sourceStream.Position > _sourceStream.Length)
                {

                    //Break.
                    if (_sourceStream.Position == 0 || !(Loops && _player.Loop))
                    {
                        break;
                    }

                    //Loop.
                    if (Loops && _player.Loop)
                    {
                        if (CurrentSample >= LoopEnd)
                        {
                            CurrentSample = LoopStart;
                        }
                    }

                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}
