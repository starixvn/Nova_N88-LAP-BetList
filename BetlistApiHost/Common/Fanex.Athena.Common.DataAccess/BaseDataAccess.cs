namespace Fanex.Athena.Common.DataAccess
{
    using Fanex.Data;
    using System;

    public class BaseDataAccess
    {
        [CLSCompliant(false)]
        protected internal virtual IObjectDb GetObjectDb(string spname, int timeout = 120)
        {
            return ObjectDbFactory.CreateInstance(spname, timeout);
        }
    }
}