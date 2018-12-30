using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace Ademund.Utils.DictionaryExtensions
{
    public static class DictionaryComparer
	{
		public static bool CheckForEquality<T>(this IEnumerable<T> source, IEnumerable<T> destination)
		{
			Debug.Assert(source != null, "source != null");
			Debug.Assert(destination != null, "destination != null");

			var enumerable = source as IList<T> ?? source.ToList();
			var members = destination as IList<T> ?? destination.ToList();

			if (enumerable.Count != members.Count)
			{
				return false;
			}

			var dictionary = new Dictionary<T, int>();

			foreach (var value in enumerable)
			{
				if (!dictionary.ContainsKey(value))
				{
					dictionary[value] = 1;
				}
				else
				{
					dictionary[value]++;
				}
			}

			foreach (var member in members)
			{
				if (!dictionary.ContainsKey(member))
				{
					return false;
				}

				dictionary[member]--;
			}

			return dictionary.All(kvp => kvp.Value == 0);
		}
	}

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

        [ScriptAPIVisible(true)]
        public new int Count => base.Count;

        [ScriptAPIVisible(true)]
        public new KeyCollection Keys => base.Keys;

        [ScriptAPIVisible(true)]
        public new ValueCollection Values => base.Values;

        [ScriptAPIVisible(true)]
        public new void Add(TKey key,TValue value)
        {
            base.Add(key, value);
        }

        [ScriptAPIVisible(true)]
        public new void Clear()
        {
            base.Clear();
        }

        [ScriptAPIVisible(true)]
        public new bool ContainsKey(TKey key)
        {
            return base.ContainsKey(key);
        }

        [ScriptAPIVisible(true)]
        public new bool ContainsValue(TValue value)
        {
            return base.ContainsValue(value);
        }

        [ScriptAPIVisible(true)]
        public new bool Remove(TKey key)
        {
            return base.Remove(key);
        }

        [ScriptAPIVisible(true)]
        public new bool TryGetValue(TKey key,out TValue value)
        {
            return base.TryGetValue(key, out value);
        }
    }
}
