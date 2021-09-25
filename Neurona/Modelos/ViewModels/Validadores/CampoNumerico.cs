using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Neurona.Modelos.ViewModels.Validadores
{
    class Campo_numerico: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return new ValidationResult(false, "Este campo es requerido.");
            }

            string pattern = @"^\d+$";
            Regex regex = new(pattern, RegexOptions.Singleline, TimeSpan.FromSeconds(1));

            return !regex.Match(str).Success
                ? new ValidationResult(false, "Este campo debe ser númerico")
                : ValidationResult.ValidResult;
        }
    }
}
