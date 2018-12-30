using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ademund.Utils.DictionaryExtensions
{
    [Serializable]
	public class DictionaryWithDefault<TKey, TValue> : Dictionary<TKey, TValue>
	{
        public TValue DefaultValue { get; set; }
        public DictionaryWithDefault() : base() { }
		public DictionaryWithDefault(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

		public DictionaryWithDefault(TValue defaultValue): base()
		{
			DefaultValue = defaultValue;
		}

		public DictionaryWithDefault(SerializationInfo info, StreamingContext context): base(info, context) { }

		public new TValue this[TKey key]
		{
			get
			{
				TValue t = DefaultValue;
				base.TryGetValue(key, out t);
				return t;
			}
			set
			{
				if (EqualityComparer<TValue>.Default.Equals(value, default(TValue)) && base.ContainsKey(key))
					base.Remove(key);
				else
					base[key] = value;
			}
		}

        public new int Count => base.Count;
        public new KeyCollection Keys => base.Keys;
        public new ValueCollection Values => base.Values;

        public new void Add(TKey key,TValue value)
        {
            base.Add(key, value);
        }

        public new void Clear()
        {
            base.Clear();
        }

        public new bool ContainsKey(TKey key)
        {
            return base.ContainsKey(key);
        }

        public new bool ContainsValue(TValue value)
        {
            return base.ContainsValue(value);
        }

        public new bool Remove(TKey key)
        {
            return base.Remove(key);
        }

        public new bool TryGetValue(TKey key,out TValue value)
        {
            return base.TryGetValue(key, out value);
        }
    }
}