using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LiveDescribe.Controls
{
    public class AutoRefreshCollectionViewSource : CollectionViewSource
    {
        protected override void OnSourceChanged(object oldSource, object newSource)
        {          
            if (oldSource != null)
                UnSubscribeSourceCollectionChangedEvents(oldSource);

            if (newSource != null)
                SubscribeSourceCollectionChangedEvents(newSource);

            base.OnSourceChanged(oldSource, newSource);
        }

        private void UnSubscribeSourceCollectionChangedEvents(object source)
        {
            INotifyCollectionChanged notify = source as INotifyCollectionChanged;
            if (notify != null)
                notify.CollectionChanged -= Source_CollectionChanged;
        }

        private void SubscribeSourceCollectionChangedEvents(object source)
        {
            INotifyCollectionChanged notify = source as INotifyCollectionChanged;
            if (notify != null)
                notify.CollectionChanged += Source_CollectionChanged;
        }

        private void Source_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                    SubscribeItemEvents(item);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.NewItems)
                    UnSubscribeItemEvents(item);
            }
        }

        private void SubscribeItemEvents(object item)
        {
            INotifyPropertyChanged notify = item as INotifyPropertyChanged;
            if (notify != null)
                notify.PropertyChanged += Item_PropertyChanged;
        }

        private void UnSubscribeItemEvents(object item)
        {
            INotifyPropertyChanged notify = item as INotifyPropertyChanged;
            if (notify != null)
                notify.PropertyChanged -= Item_PropertyChanged;
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (CanRefreshSortDescrptions(e.PropertyName) || CanRefreshGroupDescriptions(e.PropertyName))
                View.Refresh();
        }

        private bool CanRefreshSortDescrptions(string PropertyName)
        {
            foreach (SortDescription sort in SortDescriptions)
            {
                if (sort.PropertyName == PropertyName)
                    return true;
            }

            return false;
        }

        private bool CanRefreshGroupDescriptions(string PropertyName)
        {
            foreach (GroupDescription group in GroupDescriptions)
            {
                PropertyGroupDescription propertyGroup = group as PropertyGroupDescription;
                if (propertyGroup != null && propertyGroup.PropertyName == PropertyName)
                    return true;
            }
            return false;
        }
    }
}
