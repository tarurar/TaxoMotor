using System;
using TM.SP.AppPages.Strategies;

namespace TM.SP.AppPages.Pipeline
{
    /// <summary>
    /// Базовый класс работника на конвейере
    /// Задача работника: 
    ///     1. Взять элемент с конвейера
    ///     2. В зависимости от предиката обработать элемент с использованием внедренной стратегии
    ///     3. Вернуть элемент на конвейер
    /// </summary>
    /// <typeparam name="T">Класс объекта на конвейере</typeparam>
    public class BasePipelineWorker<T>: IPipelineWorker<T> where T : class
    {
        private readonly IPipeline<T> _pipeline;
        private readonly IPipelineStrategy<T> _strategy;

        public BasePipelineWorker(IPipeline<T> pipeline, IPipelineStrategy<T> strategy)
        {
            _pipeline = pipeline;
            _strategy = strategy;
        }
        /// <summary>
        /// Выполнение работы с использованием внедренной стратегии над одним элементом конвейера.
        /// Перед обработкой элемента проверяется предикат, однако взятие элемента из конвейера 
        /// и его возврат осуществляются безусловно.
        /// </summary>
        /// <param name="needHandle">Предикат, определяющий необходимость обработки элемента</param>
        /// <returns></returns>
        public virtual T RunOnce(Predicate<T> needHandle)
        {
            var o = _pipeline.GetNext();
            if (o != null)
            {
                try
                {
                    if (needHandle == null)
                    {
                        _strategy.Handle(o);
                    }
                    else
                    {
                        if (needHandle(o))
                        {
                            _strategy.Handle(o);
                        }
                    }
                }
                finally
                {
                    _pipeline.PutBack(o);
                }
            }

            return o;
        }
        /// <summary>
        /// Выполнение работы с использованием внедренной стратегии над указанным количеством 
        /// элементов либо до последнего элемента конвейера (что закончится раньше).
        /// Необходимость завершения цикла на каждом шагу проверяется предикатом. Аналог do {} while(),
        /// т .е. отработает как минимум для одного элемента конвейера
        /// </summary>
        /// <param name="count">Количество элементов для обработки</param>
        /// <param name="needHandle">Предикат необходимости обработки элемента</param>
        /// <param name="needStop">Предикат остановки конвейера</param>
        public virtual void RunMultiple(int count, Predicate<T> needHandle, Predicate<T> needStop)
        {
            for (var i = 1; i <= count; i++)
            {
                var o = RunOnce(needHandle);
                if (o != null)
                {
                    if (needStop != null)
                    {
                        if (needStop(o)) break;
                    }
                } else break;
            }
        }
    }
}
