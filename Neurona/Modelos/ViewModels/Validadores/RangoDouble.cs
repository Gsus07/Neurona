using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Neurona.Modelos.ViewModels.Validadores
{
    public class RangoDouble : ValidationRule
    {
        public double Minimo { get; set; }
        public double Maximo { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return new ValidationResult(false, "Se requiere llenar este campo");
            }

            if (!double.TryParse(str, out double val))
            {
                return new ValidationResult(false, $"El valor digitado no es un número");
            }

            if (val > Maximo)
            {
                return new ValidationResult(false, $"Recuerde que el valor digitado no debe ser mayor a {Maximo}");
            }

            return val < Minimo
                ? new ValidationResult(false, $"Recuerde que el valor digitado no debe ser menor a {Minimo}")
                : ValidationResult.ValidResult;
        }
    }
}
