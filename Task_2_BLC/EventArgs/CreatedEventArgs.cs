namespace Task_2_BLC.EventArgs
{
    public class CreatedEventArgs<TModel> : System.EventArgs
    {
        public TModel CreatedItem { get; set; }
    }
}
