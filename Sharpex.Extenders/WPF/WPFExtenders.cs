using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sharpex.Extenders.WPF
{
    public static class WPFExtenders
    {
        /// <summary>
        /// Remove an object from the datagrid, even when bound by an itemssource
        /// </summary>
        /// <param name="grid">The grid to remove the object from</param>
        /// <param name="data">The object to remove from the grid</param>
        public static void RemoveDataBoundObject(this DataGrid grid, object data)
        {
            ObservableCollection<object> obsCollection = new ObservableCollection<object>(grid.ItemsSource as IEnumerable<object>);

            obsCollection.Remove(data);
            grid.ItemsSource = obsCollection;
        }
    }
}
