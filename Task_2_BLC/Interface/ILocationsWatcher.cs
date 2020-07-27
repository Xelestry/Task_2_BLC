using System;
using Task_2_BLC.EventArgs;

namespace Task_2_BLC.Interface
{
    public interface ILocationsWatcher<TModel>
    {
        event EventHandler<CreatedEventArgs<TModel>> Created;
    }
}
