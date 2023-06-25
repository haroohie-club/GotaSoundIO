using GotaSoundIO.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotaSoundIO
{
    /// <summary>
    /// A table.
    /// </summary>
    /// <typeparam name="T">Type represented in the table.</typeparam>
    public class Table<T> : IList<T>, IReadable, IWriteable
    {
        /// <summary>
        /// Items.
        /// </summary>
        private List<T> _items = new();

        /// <summary>
        /// Get the items in the table as a list.
        /// </summary>
        /// <param name="t">Table.</param>
        public static implicit operator List<T>(Table<T> t) => t._items;

        /// <summary>
        /// Make a table from a list of items.
        /// </summary>
        /// <param name="l">List.</param>
        public static explicit operator Table<T>(List<T> l) { return new Table<T>() { _items = l }; }

        /// <summary>
        /// Read the table.
        /// </summary>
        /// <param name="r">The reader.</param>
        public void Read(FileReader r)
        {
            //Get count.
            uint count = r.ReadUInt32();

            //Types.
            if (typeof(T).Equals(typeof(ulong)))
            {
                _items = r.ReadUInt64s((int)count).ConvertTo<T>().ToList();
            }
            else if (typeof(T).Equals(typeof(uint)))
            {
                _items = r.ReadUInt32s((int)count).ConvertTo<T>().ToList();
            }
            else if (typeof(T).Equals(typeof(ushort)))
            {
                _items = r.ReadUInt16s((int)count).ConvertTo<T>().ToList();
            }
            else if (typeof(T).Equals(typeof(byte)))
            {
                _items = r.ReadBytes((int)count).ConvertTo<T>().ToList();
            }
            else if (typeof(T).Equals(typeof(long)))
            {
                _items = r.ReadInt64s((int)count).ConvertTo<T>().ToList();
            }
            else if (typeof(T).Equals(typeof(int)))
            {
                _items = r.ReadInt32s((int)count).ConvertTo<T>().ToList();
            }
            else if (typeof(T).Equals(typeof(short)))
            {
                _items = r.ReadInt16s((int)count).ConvertTo<T>().ToList();
            }
            else if (typeof(T).Equals(typeof(sbyte)))
            {
                _items = r.ReadSBytes((int)count).ConvertTo<T>().ToList();
            }
            else if (typeof(T).Equals(typeof(string)))
            {
                _items = r.ReadStrings((int)count).ConvertTo<T>().ToList();
            }
            else if (typeof(T).Equals(typeof(char)))
            {
                _items = r.ReadChars((int)count).ConvertTo<T>().ToList();
            }
            else if (typeof(T).Equals(typeof(float)))
            {
                _items = r.ReadSingles((int)count).ConvertTo<T>().ToList();
            }
            else if (typeof(T).Equals(typeof(double)))
            {
                _items = r.ReadDoubles((int)count).ConvertTo<T>().ToList();
            }

            //Custom.
            else
            {
                for (int i = 0; i < count; i++)
                {
                    _items.Add(r.Read<T>());
                }
            }
        }

        /// <summary>
        /// Write the table.
        /// </summary>
        /// <param name="w">The writer.</param>
        public void Write(FileWriter w)
        {
            //Write count.
            w.Write((uint)_items.Count);

            //Types.
            if (typeof(T).Equals(typeof(ulong)))
            {
                w.Write(_items.ConvertTo<ulong>());
            }
            else if (typeof(T).Equals(typeof(uint)))
            {
                w.Write(_items.ConvertTo<uint>());
            }
            else if (typeof(T).Equals(typeof(ushort)))
            {
                w.Write(_items.ConvertTo<ushort>());
            }
            else if (typeof(T).Equals(typeof(byte)))
            {
                w.Write(_items.ConvertTo<byte>().ToArray());
            }
            else if (typeof(T).Equals(typeof(long)))
            {
                w.Write(_items.ConvertTo<long>());
            }
            else if (typeof(T).Equals(typeof(int)))
            {
                w.Write(_items.ConvertTo<int>());
            }
            else if (typeof(T).Equals(typeof(short)))
            {
                w.Write(_items.ConvertTo<short>());
            }
            else if (typeof(T).Equals(typeof(sbyte)))
            {
                w.Write(_items.ConvertTo<byte>().ToArray());
            }
            else if (typeof(T).Equals(typeof(string)))
            {
                w.Write(_items.ConvertTo<string>());
            }
            else if (typeof(T).Equals(typeof(char)))
            {
                w.Write(_items.ConvertTo<char>().ToArray());
            }
            else if (typeof(T).Equals(typeof(float)))
            {
                w.Write(_items.ConvertTo<float>());
            }
            else if (typeof(T).Equals(typeof(double)))
            {
                w.Write(_items.ConvertTo<double>());
            }

            //Custom.
            else
            {
                foreach (var i in _items)
                {
                    w.Write(i as IWriteable);
                }
            }
        }
        //List stuff.
        #region ListStuff
        public T this[int index] { get => _items[index]; set => _items[index] = value; }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _items.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion
    }
}
