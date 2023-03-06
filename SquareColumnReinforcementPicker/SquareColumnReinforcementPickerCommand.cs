using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareColumnReinforcementPicker
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class SquareColumnReinforcementPickerCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Вызов формы
            SquareColumnReinforcementPickerWPF squareColumnReinforcementPickerWPF = new SquareColumnReinforcementPickerWPF();
            squareColumnReinforcementPickerWPF.ShowDialog();
            if (squareColumnReinforcementPickerWPF.DialogResult != true)
            {
                return Result.Cancelled;
            }

            return Result.Succeeded;
        }
    }
}
