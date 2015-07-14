using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.SP.AppPages.Tracker
{
    /// <summary>
    /// Контекст логгирования
    /// </summary>
    /// <typeparam name="T">Тип элемента контекста</typeparam>
    public interface ITrackingContext<T>
    {
        /// <summary>
        /// Элемент обращения из контекста
        /// </summary>
        T IncomeRequest { get; }
        /// <summary>
        /// Элекмент ТС из контекста
        /// </summary>
        T Taxi { get; }
        /// <summary>
        /// Элемент разрешения из контекста
        /// </summary>
        T License { get; }
        /// <summary>
        /// Ссылка на Web
        /// </summary>
        SPWeb Web { get; }
    }
}
