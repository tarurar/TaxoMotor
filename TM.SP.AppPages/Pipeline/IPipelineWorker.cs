using System;

namespace TM.SP.AppPages.Pipeline
{
    interface IPipelineWorker<T> where T:class
    {
        T RunOnce(Predicate<T> needHandle);
        void RunMultiple(int count, Predicate<T> needHandle, Predicate<T> needStop);
    }
}
