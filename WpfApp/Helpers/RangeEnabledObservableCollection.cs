using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Helpers
{
	public class RangeEnabledObservableCollection<T> : ObservableCollection<T>
	{
		public RangeEnabledObservableCollection()
			: base()
		{
		}

		public RangeEnabledObservableCollection(IEnumerable<T> collection)
			: base(collection)
		{
		}

		public RangeEnabledObservableCollection(List<T> list)
			: base(list)
		{
		}

		public virtual void AddRange(IEnumerable<T> items)
		{
			if (items != null && items.Count() > 0)
			{
				this.CheckReentrancy();
				foreach (var item in items)
				{
					this.Items.Add(item);
				}
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		public virtual void RemoveRange(IEnumerable<T> items)
		{
			if (items != null && items.Count() > 0)
			{
				this.CheckReentrancy();
				foreach (var item in items)
					this.Items.Remove(item);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}
	}
}
