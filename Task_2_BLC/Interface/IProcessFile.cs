using System.Threading.Tasks;

namespace Task_2_BLC.Interface
{
    public interface IProcessFIle<TModel>
    {
        void MoveFileProcess(TModel item);
    }
}
