using System.Collections.ObjectModel;

namespace Calculator.model
{
    interface ICalculationChangedObserver
    {
        void OnCalculationChanged();
    }
}
