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
    class CampoFlotante: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value as string;
            if (string.IsNullOrEmpty(str))
            {
                return new ValidationResult(false, "Este campo es necesario");
            }

            string pattern = @"([0-9]*[.])?[0-9]+";
            Regex regex = new(pattern, RegexOptions.Singleline, TimeSpan.FromSeconds(1));

            return !regex.Match(str).Success
                ? new ValidationResult(false, "Recuerde que este debe ser un valor numerico")
                : ValidationResult.ValidResult;
        }
    }
}
