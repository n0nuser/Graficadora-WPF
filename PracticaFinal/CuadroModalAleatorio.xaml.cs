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
    /// Lógica de interacción para CuadroModalAleatorio.xaml
    /// </summary>
    public partial class CuadroModalAleatorio : Window
    {
        private ObservableCollection<Punto> listaPuntosPolinomio;

        public CuadroModalAleatorio()
        {
            InitializeComponent();
            this.listaPuntosPolinomio = new ObservableCollection<Punto>();
            listaPolinomio.ItemsSource = this.listaPuntosPolinomio;
        }

        public ObservableCollection<Punto> ListaPuntosPolinomio
        {
            get { return this.listaPuntosPolinomio; }
            set { this.listaPuntosPolinomio = value; listaPolinomio.ItemsSource = this.listaPuntosPolinomio; }
        }

        private void Lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaPolinomio.SelectedItems.Count > 0)
            {
                if (listaPolinomio.SelectedItems.Count == 1)
                {
                    if (listaPolinomio.Items.Count > 1) //Comprobación visibilidad botones subir/bajar
                    {
                        int indiceActual = listaPolinomio.SelectedIndex;
                        if (indiceActual != 0 && indiceActual != (listaPolinomio.Items.Count - 1))
                        {
                            Boton_Subir.Visibility = Visibility.Visible;
                            Boton_Bajar.Visibility = Visibility.Visible;
                        }
                        else if (indiceActual == 0)
                        {
                            Boton_Subir.Visibility = Visibility.Hidden;
                            Boton_Bajar.Visibility = Visibility.Visible;
                        }
                        else if (indiceActual == (listaPolinomio.Items.Count - 1))
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
                CajaD.Text = "";
                Boton_Subir.Visibility = Visibility.Hidden;
                Boton_Bajar.Visibility = Visibility.Hidden;
            }
        }

        private void Anadir_Click(object sender, RoutedEventArgs e)
        {
            double valueCaja1, valueCaja2, valueCaja3, valueCaja4;
            double max = 9;
            double min = -9;
            double paso = 0.1;
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
                valueCaja1 = 0;

            if (CajaB.Text != "")
            {
                if (!Double.TryParse(CajaB.Text.Replace('.', ','), out valueCaja2))
                {
                    MessageBox.Show("El valor de la casilla \"B\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }
            else
                valueCaja2 = 0;

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

            if (CajaD.Text != "")
            {
                if (!Double.TryParse(CajaD.Text.Replace('.', ','), out valueCaja4))
                {
                    MessageBox.Show("El valor de la casilla \"D\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }
            else
                valueCaja4 = 0;

            if (valueCaja1 == 0 && valueCaja2 == 0 && valueCaja3 == 0 && valueCaja4 == 0)
            {
                MessageBox.Show("Las casillas A, B, C o D no pueden estar vacías.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                error = true;
            }

            if (CajaMax.Text != "")
            {
                if (!Double.TryParse(CajaMax.Text.Replace('.', ','), out max))
                {
                    MessageBox.Show("El valor de la casilla \"Valor Max.\" no es válido.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                if (!error && max <= 0.0)
                {
                    MessageBox.Show("El valor de la casilla \"Valor Min.\".\n no puede ser negativo ó 0.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (-9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }

            if (CajaMin.Text != "")
            {
                if (!Double.TryParse(CajaMin.Text.Replace('.', ','), out min))
                {
                    MessageBox.Show("El valor de la casilla \"Valor Min.\" no es válido.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (-9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                if (!error && min >= 0.0)
                {
                    MessageBox.Show("El valor de la casilla \"Valor Min.\".\n no puede ser positivo ó 0.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (-9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }

            if (CajaPaso.Text != "")
            {
                if (!Double.TryParse(CajaPaso.Text.Replace('.', ','), out paso))
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
                Polinomio p = new Polinomio(CajaNombre.Text, min, max, paso, valueCaja1, valueCaja2, valueCaja3, valueCaja4);
                for (int i = 0; i < p.ListaPuntosPolinomio.Count; i++)
                    this.listaPuntosPolinomio.Add(p.ListaPuntosPolinomio[i]); //Si lo asignaba tal cual perdía los anteriores polinomios
            }
        }

        private void Aleatorio_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            bool error = false;
            double max = 9;
            double min = -9;
            double paso = 0.1;
            double valueCaja1, valueCaja2, valueCaja3, valueCaja4;

            if (CajaMax.Text != "")
            {
                if (!Double.TryParse(CajaMax.Text.Replace('.', ','), out max))
                {
                    MessageBox.Show("El valor de la casilla \"Valor Max.\" no es válido.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                if (!error && max <= 0.0)
                {
                    MessageBox.Show("El valor de la casilla \"Valor Min.\".\n no puede ser negativo ó 0.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (-9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }

            if (CajaMin.Text != "")
            {
                if (!Double.TryParse(CajaMin.Text.Replace('.', ','), out min))
                {
                    MessageBox.Show("El valor de la casilla \"Valor Min.\" no es válido.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (-9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                if (!error && min >= 0.0)
                {
                    MessageBox.Show("El valor de la casilla \"Valor Min.\".\n no puede ser positivo ó 0.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (-9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }

            if (CajaPaso.Text != "")
            {
                if (!Double.TryParse(CajaPaso.Text.Replace('.', ','), out paso))
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

            valueCaja1 = r.NextDouble() * (max - min) + min;
            valueCaja2 = r.NextDouble() * (max - min) + min;
            valueCaja3 = r.NextDouble() * (max - min) + min;
            valueCaja4 = r.NextDouble() * (max - min) + min;
            if (!error)
            {
                Polinomio p = new Polinomio(CajaNombre.Text, min, max, paso, valueCaja1, valueCaja2, valueCaja3, valueCaja4);
                for (int i = 0; i < p.ListaPuntosPolinomio.Count; i++)
                    this.listaPuntosPolinomio.Add(p.ListaPuntosPolinomio[i]); //Si lo asignaba tal cual perdía los anteriores polinomios
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/21220467/remove-selected-items-from-listbox-observablecollection
            var selectedFiles = listaPolinomio.SelectedItems.Cast<Punto>().ToList();
            foreach (Punto item in selectedFiles)
            {
                this.listaPuntosPolinomio.Remove(item);
            }
            Boton_Eliminar.IsEnabled = false;
        }

        private void EliminarNombre_Click(object sender, RoutedEventArgs e)
        {
            var selectedFiles = listaPolinomio.Items.Cast<Punto>().ToList();
            foreach (Punto item in selectedFiles)
            {
                if (item.Nombre == CajaEliminarNombre.Text)
                    this.listaPuntosPolinomio.Remove(item);
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
            int indiceActual = listaPolinomio.SelectedIndex;
            Punto tempP = this.listaPuntosPolinomio[indiceActual];
            this.listaPuntosPolinomio[indiceActual] = this.listaPuntosPolinomio[indiceActual - 1];
            this.listaPuntosPolinomio[indiceActual - 1] = tempP;
        }

        private void Bajar_Click(object sender, RoutedEventArgs e)
        {
            int indiceActual = listaPolinomio.SelectedIndex;
            Punto tempP = this.listaPuntosPolinomio[indiceActual];
            this.listaPuntosPolinomio[indiceActual] = this.listaPuntosPolinomio[indiceActual + 1];
            this.listaPuntosPolinomio[indiceActual + 1] = tempP;
        }

        private void lista_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //https://stackoverflow.com/questions/30737097/wpf-how-deselect-all-my-selected-items-when-click-on-empty-space-in-my-listview
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                listaPolinomio.UnselectAll();
        }
    }
}