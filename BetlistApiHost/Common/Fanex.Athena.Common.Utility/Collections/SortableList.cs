namespace Fanex.Athena.Common.Utility.Collections
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class SortableList<T> : List<T>
    {
        private string _propertyName;
        private bool _ascending;

        public void Sort(string propertyName, bool ascending)
        {
            UpdateSortingData(propertyName, ascending);

            // Apply and set the sort, if items to sort
            var comparer = new PropertyComparer<T>(propertyName, _ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
            this.Sort(comparer);
        }

        private void UpdateSortingData(string propertyName, bool ascending)
        {
            // Flip the properties if the parameters are the same.
            if (_propertyName == propertyName && _ascending == ascending)
            {
                _ascending = !ascending;
            }
            else
            {
                // Else, new properties are set with the new values.
                _propertyName = propertyName;
                _ascending = ascending;
            }
        }
    }
}