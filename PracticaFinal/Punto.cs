using System.ComponentModel;

namespace PracticaFinal
{
    public class Punto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //Atributos
        private string nombre;

        private double corX;
        private double corY;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; OnPropertyChanged("Nombre"); }
        }

        public double CorX
        {
            get { return corX; }
            set { corX = value; OnPropertyChanged("CorX"); }
        }

        public double CorY
        {
            get { return corY; }
            set { corY = value; OnPropertyChanged("CorY"); }
        }

        // Constructor
        public Punto(string n, double x, double y)
        {
            Nombre = n;
            CorX = x;
            CorY = y;
        }

        // Método que lanza el evento PropertyChanged cuando se cambia el valor de cualquier propiedad
        private void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}