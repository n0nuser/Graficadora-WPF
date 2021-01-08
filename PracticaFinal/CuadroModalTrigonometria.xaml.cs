using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PracticaFinal
{
    /// <summary>
    /// Lógica de interacción para CuadroModalTrigonometria.xaml
    /// </summary>
    public partial class CuadroModalTrigonometria : Window
    {
        private ObservableCollection<Punto> listaPuntosTrigonometria;
        private double paso = 0.05;

        public CuadroModalTrigonometria()
        {
            InitializeComponent();
            this.listaPuntosTrigonometria = new ObservableCollection<Punto>();
            listaTrigonometria.ItemsSource = this.listaPuntosTrigonometria;
        }

        public ObservableCollection<Punto> ListaPuntosTrigonometria
        {
            get { return this.listaPuntosTrigonometria; }
            set { this.listaPuntosTrigonometria = value; listaTrigonometria.ItemsSource = this.listaPuntosTrigonometria; }
        }

        private void lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaTrigonometria.SelectedItems.Count > 0)
            {
                if (listaTrigonometria.SelectedItems.Count == 1)
                {
                    if (listaTrigonometria.Items.Count > 1) //Comprobación visibilidad botones subir/bajar
                    {
                        int indiceActual = listaTrigonometria.SelectedIndex;
                        if (indiceActual != 0 && indiceActual != (listaTrigonometria.Items.Count - 1))
                        {
                            Boton_Subir.Visibility = Visibility.Visible;
                            Boton_Bajar.Visibility = Visibility.Visible;
                        }
                        else if (indiceActual == 0)
                        {
                            Boton_Subir.Visibility = Visibility.Hidden;
                            Boton_Bajar.Visibility = Visibility.Visible;
                        }
                        else if (indiceActual == (listaTrigonometria.Items.Count - 1))
                        {
                            Boton_Subir.Visibility = Visibility.Visible;
                            Boton_Bajar.Visibility = Visibility.Hidden;
                        }
                    }
                }
                Boton_Eliminar.IsEnabled = true;
            }
            else
            {
                Boton_Eliminar.IsEnabled = false;
                CajaA.Text = "";
                CajaB.Text = "";
                CajaC.Text = "";

                Boton_Subir.Visibility = Visibility.Hidden;
                Boton_Bajar.Visibility = Visibility.Hidden;
            }
        }

        private void Anadir_Click(object sender, RoutedEventArgs e)
        {
            double max = 9;
            double min = -9;
            double valueCaja1, valueCaja2, valueCaja3, y = 0;
            bool error = false;

            if (CajaA.Text != "")
            {
                if (!Double.TryParse(CajaA.Text.Replace('.', ','), out valueCaja1))
                {
                    MessageBox.Show("El valor de la casilla \"A\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }
            else
                valueCaja1 = 1;

            if (CajaB.Text != "")
            {
                if (!Double.TryParse(CajaB.Text.Replace('.', ','), out valueCaja2))
                {
                    MessageBox.Show("El valor de la casilla \"B\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }
            else
                valueCaja2 = 1;

            if (CajaC.Text != "")
            {
                if (!Double.TryParse(CajaC.Text.Replace('.', ','), out valueCaja3))
                {
                    MessageBox.Show("El valor de la casilla \"C\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }
            else
                valueCaja3 = 0;

            if (Precision.Text != "")
            {
                if (!Double.TryParse(Precision.Text.Replace('.', ','), out paso))
                {
                    MessageBox.Show("El valor de la casilla \"Precisión\" no es válido.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (0.1).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                if (!error && paso < 0.000000000001)
                {
                    MessageBox.Show("El valor de la casilla \"Precisión\".\n no puede ser negativo ó 0.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (0.1).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }

            if (!error)
            {
                for (double x = min; x < max; x += paso)
                {
                    if (tipoFuncion.SelectedIndex == 0)
                        y = valueCaja1 * Math.Sin(valueCaja2 * x + valueCaja3);
                    else if (tipoFuncion.SelectedIndex == 1)
                        y = valueCaja1 * Math.Cos(valueCaja2 * x + valueCaja3);
                    else if (tipoFuncion.SelectedIndex == 2)
                        y = valueCaja1 * Math.Tan(valueCaja2 * x + valueCaja3);

                    if (y > min && y < max)
                        this.listaPuntosTrigonometria.Add(new Punto(CajaNombre.Text, x, y));
                }
            }
        }

        private void Aleatorio_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            double max = 9;
            double min = -9;
            double valueCaja1, valueCaja2, valueCaja3, y = 0;
            int funcion;
            bool error = false;

            funcion = r.Next(0, 3);
            valueCaja1 = r.NextDouble() * (max - min) + min;
            valueCaja2 = r.NextDouble() * (max - min) + min;
            valueCaja3 = r.NextDouble() * (max - min) + min;

            if (Precision.Text != "")
            {
                if (!Double.TryParse(Precision.Text.Replace('.', ','), out paso))
                {
                    MessageBox.Show("El valor de la casilla \"Precisión\" no es válido.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (0.1).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                if (!error && paso < 0.000000000001)
                {
                    MessageBox.Show("El valor de la casilla \"Precisión\".\n no puede ser negativo ó 0.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (0.1).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }

            if (!error)
            {
                for (double x = min; x < max; x += paso)
                {
                    if (funcion == 0)
                        y = valueCaja1 * Math.Sin(valueCaja2 * x + valueCaja3);
                    else if (funcion == 1)
                        y = valueCaja1 * Math.Cos(valueCaja2 * x + valueCaja3);
                    else if (funcion == 2)
                        y = valueCaja1 * Math.Tan(valueCaja2 * x + valueCaja3);

                    if (y > min && y < max)
                        this.listaPuntosTrigonometria.Add(new Punto(CajaNombre.Text, x, y));
                }
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/21220467/remove-selected-items-from-listbox-observablecollection
            var selectedFiles = listaTrigonometria.SelectedItems.Cast<Punto>().ToList();
            foreach (Punto item in selectedFiles)
            {
                this.listaPuntosTrigonometria.Remove(item);
            }
            Boton_Eliminar.IsEnabled = false;
        }

        private void EliminarNombre_Click(object sender, RoutedEventArgs e)
        {
            var selectedFiles = listaTrigonometria.Items.Cast<Punto>().ToList();
            foreach (Punto item in selectedFiles)
            {
                if (item.Nombre == CajaEliminarNombre.Text)
                    this.listaPuntosTrigonometria.Remove(item);
            }
            Boton_Eliminar.IsEnabled = false;
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Subir_Click(object sender, RoutedEventArgs e)
        {
            int indiceActual = listaTrigonometria.SelectedIndex;
            Punto tempP = this.listaPuntosTrigonometria[indiceActual];
            this.listaPuntosTrigonometria[indiceActual] = this.listaPuntosTrigonometria[indiceActual - 1];
            this.listaPuntosTrigonometria[indiceActual - 1] = tempP;
        }

        private void Bajar_Click(object sender, RoutedEventArgs e)
        {
            int indiceActual = listaTrigonometria.SelectedIndex;
            Punto tempP = this.listaPuntosTrigonometria[indiceActual];
            this.listaPuntosTrigonometria[indiceActual] = this.listaPuntosTrigonometria[indiceActual + 1];
            this.listaPuntosTrigonometria[indiceActual + 1] = tempP;
        }

        private void lista_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //https://stackoverflow.com/questions/30737097/wpf-how-deselect-all-my-selected-items-when-click-on-empty-space-in-my-listview
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                listaTrigonometria.UnselectAll();
        }
    }
}