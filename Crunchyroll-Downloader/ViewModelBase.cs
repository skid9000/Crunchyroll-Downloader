using System;
using System.Runtime.CompilerServices;

namespace CrunchyrollDownloader
{
	public class ViewModelBase<T> : PropertyChangedObject where T : class, new()
	{
		public T Model { get; protected set; }
		public ViewModelBase(T model = null)
		{
			Model = model ?? new T();
		}

		protected void SetModelProperty(object value, [CallerMemberName] string propertyName = null)
		{
			if (propertyName is null) throw new ArgumentNullException(nameof(propertyName));
			typeof(T).GetProperty(propertyName)?.SetValue(Model, value);
			OnPropertyChanged(propertyName);
		}
	}
}