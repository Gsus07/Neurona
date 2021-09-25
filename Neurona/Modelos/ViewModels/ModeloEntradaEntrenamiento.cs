using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurona.Modelos.ViewModels
{
    class ModeloEntradaEntrenamiento
    {
        public List<List<double>> Entradas { get; init; }
        public List<double> Salidas { get; init; }
        public int PesosMaximos { get; init; }
        public double ToleranciaErrores { get; init; }
    }
}
