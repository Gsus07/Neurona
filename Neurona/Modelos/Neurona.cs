using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurona.Modelos
{
    public class Neurona
    {
        public Neurona(int NumeroEntradas, double ratioAprendizaje, FunctionTrigger functionTrigger)
        {
            Pesos = new List<double>(NumeroEntradas);
            RatioAprendizaje = ratioAprendizaje;
            FunctionTrigger = functionTrigger;

            Init();
        }

        public Neurona()
        {
            Pesos = new List<double>();
        }

        public List<double> Pesos { get; set; }
        public FunctionTrigger FunctionTrigger { get; set; }

        private double RatioAprendizaje { get; }

        private void Init()
        {
            Random random = new();
            for (int i = 0; i < Pesos.Capacity; i++)
            {
                Pesos.Add((random.NextDouble() * 2) - 1.0f);
            }

        }

        public void Learn(double[] entradas, double salidaEsperada)
        {
            double salida = Salida(entradas);
            double error = salidaEsperada - salida;

            for (int i = 0; i < Pesos.Count; i++)
            {
                Pesos[i] += RatioAprendizaje * error * entradas[i];
            }

        }

        public double Salida(double[] inputs)
        {
            return Prediccion(NextInput(inputs));
        }

        private double Prediccion(double entrada)
        {
            return FunctionTrigger switch
            {
                FunctionTrigger.Step => entrada > 0 ? 1.0 : 0.0,
                FunctionTrigger.Linear => entrada,
                _ => 0.0,
            };
        }

        private double NextInput(double[] entradas)
        {
            double acc = entradas.Select((t, i) => t * Pesos[i]).Sum();
            return acc;
        }
    }
}
