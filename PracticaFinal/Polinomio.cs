using System;
using System.Collections.ObjectModel;

namespace PracticaFinal
{
    public class Polinomio
    {
        private ObservableCollection<Punto> listaPuntosPolinomio = new ObservableCollection<Punto>();
        private string name;
        private double A;
        private double B;
        private double C;
        private double D;
        private double min;
        private double max;
        private double minCanvas = -9.0;
        private double maxCanvas = 9.0;
        private double paso;

        public ObservableCollection<Punto> ListaPuntosPolinomio
        {
            get
            {
                return listaPuntosPolinomio;
            }
        }

        public Polinomio(string nombre, double rangoMin, double rangoMax, double precision, double parA, double parB, double parC, double parD)
        {
            name = nombre;
            A = parA;
            B = parB;
            C = parC;
            D = parD;
            min = rangoMin;
            max = rangoMax;
            paso = precision;
            calcular();
        }

        private void calcular()
        {
            double y;
            listaPuntosPolinomio.Clear();
            for (double x = min; x <= max; x += paso)
            {
                y = A * (Math.Pow(x, 3)) + B * (Math.Pow(x, 2)) + C * x + D;
                if (y > minCanvas && y < maxCanvas)
                    listaPuntosPolinomio.Add(new Punto(name, x, y));
            }
        }
    }
}