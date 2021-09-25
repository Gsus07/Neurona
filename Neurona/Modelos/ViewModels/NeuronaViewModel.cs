using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;


namespace Neurona.Modelos.ViewModels
{
    public class NeuronaViewModel : ObservableObject
    {
        private Neurona _neurona;
        private string _pesos = "No inicializados";

        private string _pesosMaximos;
        private string _ratioAprendizaje = string.Empty;
        private string _funcionTrigger = string.Empty;

        private string _errorTolerance = string.Empty;

        private bool _isReadyForTraining;

        public bool IsReadyForTraining
        {
            get => _isReadyForTraining;
            set => OnPropertyChanged(ref _isReadyForTraining, value);
        }

        public IList<string> FuncionesTrigger { get; } = new List<string>
        {
            "Escalón",
            "Lineal"
        };

        private int _steps = 0;

        public NeuronaViewModel()
        {
            SetUpNeuron = new ComandoAsincrono(Init, CanSetUp);
            StartTraining = new ComandoAsincrono(TrainingNeuron);

            ErrorsSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Error de la iteración",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(1)
                    },
                    PointGeometry = DefaultGeometries.Circle,
                }
            };

            VersusSeries = new SeriesCollection
            {
                 new LineSeries
                {
                    Title = "Resultado esperado",
                    Values = new ChartValues<ObservableValue>(),
                    PointGeometry = DefaultGeometries.Circle,
                },
                  new LineSeries
                {
                    Title = "Resultado obtenido",
                    Values = new ChartValues<ObservableValue>(),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 10,
                    Stroke = Brushes.Aqua
                }
            };
        }

        public string Pesos
        {
            get => _pesos;
            set
            {
                _pesos = value;
                OnPropertyChanged(nameof(Pesos));
            }
        }

        public string RatioAprendizaje
        {
            get => _ratioAprendizaje;
            set => OnPropertyChanged(ref _ratioAprendizaje, value);
        }

        public string TriggerFunction
        {
            get => _ratioAprendizaje;
            set => OnPropertyChanged(ref _funcionTrigger, value);
        }

        public string ErrorTolerance
        {
            get => _errorTolerance;
            set => OnPropertyChanged(ref _errorTolerance, value);
        }

        public int Steps
        {
            get => _steps;
            set => OnPropertyChanged(ref _steps, value);
        }

        public string PesosMaximos
        {
            get => _pesosMaximos;
            set => OnPropertyChanged(ref _pesosMaximos, value);
        }

        public Neurona Neurona
        {
            get => _neurona;
            set
            {
                _neurona = value;
                OnPropertyChanged(nameof(Neurona));

                ErrorsSeries[0].Values.Clear();
                ErrorsSeries[0].Values.Add(new ObservableValue(1));

                RefreshViewData();
                Steps = 0;
            }
        }

        public ICommand SetUpNeuron { get; }

        public ICommand StartTraining { get; }

        public SeriesCollection ErrorsSeries { get; set; }
        public SeriesCollection VersusSeries { get; set; }

        private void RefreshViewData()
        {
            

            string pesosStr = string.Empty;
            for (int i = 0; i < _neurona.Pesos.Count; i++)
            {
                pesosStr += $"{_neurona.Pesos[i]}";
                if (i < _neurona.Pesos.Count - 1)
                {
                    pesosStr += ", ";
                }
            }

            Pesos = pesosStr;

            TriggerFunction = _neurona.FunctionTrigger switch
            {
                Modelos.FunctionTrigger.Linear => "Lineal",
                Modelos.FunctionTrigger.Step => "Escalón",
                _ => ""
            };
        }

        private bool CanSetUp()
        {
            return !string.IsNullOrWhiteSpace(_pesosMaximos) && !string.IsNullOrWhiteSpace(_ratioAprendizaje);
        }

        private Task Init(object parameter)
        {
            if (parameter is ModeloEntradaConfiguracionNeurona entradaNeurona)
            {
                var FuncionTrigger = entradaNeurona.TriggerFunction switch
                {
                    "Escalón" => Modelos.FunctionTrigger.Step,
                    "Lineal" => Modelos.FunctionTrigger.Linear,
                    _ => Modelos.FunctionTrigger.Step,
                };
                Neurona = new Neurona(entradaNeurona.InputsNumber, entradaNeurona.TrainingRate, FuncionTrigger);
            }

            return Task.CompletedTask;
        }

        private Task TrainingNeuron(object parameter)
        {
            if (parameter is not ModeloEntradaEntrenamiento neuronTraining)
            {
                return Task.CompletedTask;
            }

            int steps = 0;
            bool sw = false;

            while (!sw && (steps <= neuronTraining.PesosMaximos))
            {
                ++steps;
                ++Steps;

                List<double> patternErrors = new();
                for (int i = 0; i < neuronTraining.Entradas.Count; i++)
                {
                    double[] input = neuronTraining.Entradas[i].ToArray();
                    double result = _neurona.Salida(input);

                    double linealError = neuronTraining.Salidas[i] - result;
                    double patternError = Math.Abs(linealError);
                    patternErrors.Add(patternError);

                    if (result == neuronTraining.Salidas[i])
                    {
                        continue;
                    }

                    _neurona.Learn(input, neuronTraining.Salidas[i]);
                    RefreshViewData();
                }

                double patterErrorAverage = patternErrors.Average();
                sw = patterErrorAverage <= neuronTraining.ToleranciaErrores;

                ErrorsSeries[0].Values.Add(new ObservableValue(patterErrorAverage));
            }

            return Task.CompletedTask;
        }
    }
    
}
