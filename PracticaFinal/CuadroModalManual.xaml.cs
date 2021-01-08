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
    /// Lógica de interacción para CuadroModalManual.xaml
    /// </summary>
    public partial class CuadroModalManual : Window
    {
        private ObservableCollection<Punto> listaPuntosManual;
        private int numPunto = 1;

        public CuadroModalManual()
        {
            InitializeComponent();
            this.listaPuntosManual = new ObservableCollection<Punto>();
            listaManual.ItemsSource = this.listaPuntosManual;
            Caja1.KeyDown += Modificar;
            Caja2.KeyDown += Modificar;
            Caja3.KeyDown += Modificar;
        }

        public ObservableCollection<Punto> ListaPuntosManual
        {
            get { return this.listaPuntosManual; }
            set { this.listaPuntosManual = value; listaManual.ItemsSource = this.listaPuntosManual; }
        }

        private void lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaManual.SelectedItems.Count > 0)
            {
                if (listaManual.SelectedItems.Count == 1)
                {
                    if (listaManual.Items.Count > 1) //Comprobación visibilidad botones subir/bajar
                    {
                        int indiceActual = listaManual.SelectedIndex;
                        if (indiceActual != 0 && indiceActual != (listaManual.Items.Count - 1))
                        {
                            Boton_Subir.Visibility = Visibility.Visible;
                            Boton_Bajar.Visibility = Visibility.Visible;
                        }
                        else if (indiceActual == 0)
                        {
                            Boton_Subir.Visibility = Visibility.Hidden;
                            Boton_Bajar.Visibility = Visibility.Visible;
                        }
                        else if (indiceActual == (listaManual.Items.Count - 1))
                        {
                            Boton_Subir.Visibility = Visibility.Visible;
                            Boton_Bajar.Visibility = Visibility.Hidden;
                        }
                    }
                    Boton_Modificar.IsEnabled = true;
                    Caja1.Text = this.listaPuntosManual[listaManual.SelectedIndex].Nombre;
                    Caja2.Text = this.listaPuntosManual[listaManual.SelectedIndex].CorX.ToString("F");
                    Caja3.Text = this.listaPuntosManual[listaManual.SelectedIndex].CorY.ToString("F");
                }
                Boton_Eliminar.IsEnabled = true;
            }
            else
            {
                Boton_Eliminar.IsEnabled = false;
                Boton_Modificar.IsEnabled = false;
                Caja1.Text = "";
                Caja2.Text = "";
                Caja3.Text = "";
                Boton_Subir.Visibility = Visibility.Hidden;
                Boton_Bajar.Visibility = Visibility.Hidden;
            }
        }

        private void Anadir_Click(object sender, RoutedEventArgs e)
        {
            string nombre = "";
            double valueCaja2, valueCaja3;
            bool error = false;

            if (Caja1.Text == "")
                nombre = "Punto " + numPunto.ToString("D");
            else
                nombre = Caja1.Text;

            if (Caja2.Text != "")
            {
                if (!Double.TryParse(Caja2.Text, out valueCaja2))
                {
                    MessageBox.Show("El valor de la casilla \"Coordenada X\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }
            else
                valueCaja2 = 0;

            if (Caja3.Text != "")
            {
                if (!Double.TryParse(Caja3.Text, out valueCaja3))
                {
                    MessageBox.Show("El valor de la casilla \"Coordenada Y\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }
            else
                valueCaja3 = 0;

            if (!error)
            {
                this.listaPuntosManual.Add(new Punto(nombre, valueCaja2, valueCaja3));
                numPunto++;
            }
        }

        private void Aleatorio_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            bool error = false;
            double max = 9;
            double min = -9;
            double corX, corY;

            if (CajaMax.Text != "")
            {
                if (!Double.TryParse(CajaMax.Text, out max))
                {
                    MessageBox.Show("El valor de la casilla \"Valor Max.\" no es válido.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                if (!error && max <= 0)
                {
                    MessageBox.Show("El valor de la casilla \"Valor Min.\"\n no puede ser negativo ó 0.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (-9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }

            if (CajaMin.Text != "")
            {
                if (!Double.TryParse(CajaMin.Text, out min))
                {
                    MessageBox.Show("El valor de la casilla \"Valor Min.\" no es válido.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (-9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                if (!error && min >= 0)
                {
                    MessageBox.Show("El valor de la casilla \"Valor Min.\"\n no puede ser positivo ó 0.\n\nPuede dejarse en blanco y se utilizará el valor por defecto (-9.00).", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }

            corX = r.NextDouble() * (max - min) + min;
            corY = r.NextDouble() * (max - min) + min;

            if (!error)
            {
                this.listaPuntosManual.Add(new Punto("Aleatorio", corX, corY));
                numPunto++;
            }
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (listaManual.SelectedItems.Count == 1)
            {
                double valueCaja2, valueCaja3;

                if (Double.TryParse(Caja2.Text, out valueCaja2))
                    this.listaPuntosManual[listaManual.SelectedIndex].CorX = valueCaja2;
                else
                    MessageBox.Show("El valor de la casilla \"Coordenada X\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                if (Double.TryParse(Caja3.Text, out valueCaja3))
                    this.listaPuntosManual[listaManual.SelectedIndex].CorY = valueCaja3;
                else
                    MessageBox.Show("El valor de la casilla \"Coordenada Y\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                if (Caja1.Text != "")
                    this.listaPuntosManual[listaManual.SelectedIndex].Nombre = Caja1.Text;
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            //https://stackoverflow.com/questions/21220467/remove-selected-items-from-listbox-observablecollection
            var selectedFiles = listaManual.SelectedItems.Cast<Punto>().ToList();
            foreach (Punto item in selectedFiles)
            {
                this.listaPuntosManual.Remove(item);
            }
            numPunto--;
            Boton_Eliminar.IsEnabled = false;
        }

        private void EliminarNombre_Click(object sender, RoutedEventArgs e)
        {
            var selectedFiles = listaManual.Items.Cast<Punto>().ToList();
            foreach (Punto item in selectedFiles)
            {
                if (item.Nombre == CajaEliminarNombre.Text)
                    this.listaPuntosManual.Remove(item);
            }
            Boton_Eliminar.IsEnabled = false;
        }

        private void Modificar(object sender, KeyEventArgs e)
        {
            if (listaManual.SelectedItems.Count == 1)
            {
                if (e.Key == Key.Enter)
                {
                    double valueCaja2, valueCaja3;

                    if (Double.TryParse(Caja2.Text, out valueCaja2))
                        this.listaPuntosManual[listaManual.SelectedIndex].CorX = valueCaja2;
                    else
                        MessageBox.Show("El valor de la casilla \"Coordenada X\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    if (Double.TryParse(Caja3.Text, out valueCaja3))
                        this.listaPuntosManual[listaManual.SelectedIndex].CorY = valueCaja3;
                    else
                        MessageBox.Show("El valor de la casilla \"Coordenada Y\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    if (Caja1.Text != "")
                        this.listaPuntosManual[listaManual.SelectedIndex].Nombre = Caja1.Text;
                }
            }
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
            int indiceActual = listaManual.SelectedIndex;
            Punto tempP = this.listaPuntosManual[indiceActual];
            this.listaPuntosManual[indiceActual] = this.listaPuntosManual[indiceActual - 1];
            this.listaPuntosManual[indiceActual - 1] = tempP;
        }

        private void Bajar_Click(object sender, RoutedEventArgs e)
        {
            int indiceActual = listaManual.SelectedIndex;
            Punto tempP = this.listaPuntosManual[indiceActual];
            this.listaPuntosManual[indiceActual] = this.listaPuntosManual[indiceActual + 1];
            this.listaPuntosManual[indiceActual + 1] = tempP;
        }

        private void lista_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //https://stackoverflow.com/questions/30737097/wpf-how-deselect-all-my-selected-items-when-click-on-empty-space-in-my-listview
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                listaManual.UnselectAll();
        }
    }
}