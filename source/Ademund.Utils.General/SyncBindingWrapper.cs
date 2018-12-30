using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System;

namespace Ademund.Utils
{
    public class SyncBindingWrapper<T> : INotifyPropertyChanged, IDisposable
    {
        private readonly INotifyPropertyChanged _source;
        private readonly PropertyInfo _property;
        private readonly SynchronizationContext _context;

        public event PropertyChangedEventHandler PropertyChanged;

        public T Value => (T)_property.GetValue(_source, null);

        public SyncBindingWrapper(INotifyPropertyChanged source, string propertyName)
        {
            _source = source;
            _property = _source.GetType().GetProperty(propertyName);
            _context = SynchronizationContext.Current;
            _source.PropertyChanged += OnSourcePropertyChanged;
        }

        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != _property.Name)
                return;

            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;

            _context.Send(_ => propertyChanged(this, e), null);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _source.PropertyChanged -= OnSourcePropertyChanged;
            }
        }
    }
}