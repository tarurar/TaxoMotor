namespace TM.SP.AppPages.Pipeline
{
    public interface IPipeline<T>
    {
        T GetNext();
        void PutBack(T element);
    }
}
