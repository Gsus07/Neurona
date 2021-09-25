using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurona.Modelos.ViewModels
{
    class ModeloEntradaConfiguracionNeurona
    {
        public int InputsNumber { get; init; }
        public double TrainingRate { get; init; }
        public string TriggerFunction { get; init; }
    }
}
