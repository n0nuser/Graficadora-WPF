using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PracticaFinal
{
    /// <summary>
    /// Lógica de interacción para CuadroModalOpciones.xaml
    /// </summary>
    public partial class CuadroModalOpciones : Window
    {
        private SolidColorBrush cEjes;
        private SolidColorBrush cLinea;
        private SolidColorBrush cBackground;
        private double gLinea;
        private double gEjes;
        private double anchoPunteado;
        private double espacioPunteado;
        private DoubleCollection dashArray = null;
        private PenLineCap PLCinicioLinea;
        private PenLineCap PLCfinLinea;

        public CuadroModalOpciones()
        {
            InitializeComponent();
        }

        public SolidColorBrush colorEjes
        {
            get { return cEjes; }
            set { cEjes = value; colorPickerEjes.SelectedColor = cEjes.Color; }
        }

        public SolidColorBrush colorLinea
        {
            get { return cLinea; }
            set { cLinea = value; colorPickerLineas.SelectedColor = cLinea.Color; }
        }

        public SolidColorBrush colorBackground
        {
            get { return cBackground; }
            set { cBackground = value; colorPickerBackground.SelectedColor = cBackground.Color; }
        }

        public double grosorLinea
        {
            get { return gLinea; }
            set { gLinea = value; selectionLinea.Text = gLinea.ToString(); }
        }

        public double grosorEjes
        {
            get { return gEjes; }
            set { gEjes = value; selectionEjes.Text = gEjes.ToString(); }
        }

        public DoubleCollection punteado
        {
            get { return dashArray; }
            set
            {
                dashArray = value;
                if (dashArray != null)
                {
                    dashAnchoLinea.Text = dashArray[0].ToString();
                    dashEspacioLinea.Text = dashArray[1].ToString();
                }
            }
        }

        public PenLineCap InicioLinea
        {
            get { return PLCinicioLinea; }
            set { PLCinicioLinea = value; inicioLinea.SelectedIndex = (int)PLCinicioLinea; }
        }

        public PenLineCap FinLinea
        {
            get { return PLCfinLinea; }
            set { PLCfinLinea = value; finLinea.SelectedIndex = (int)PLCfinLinea; }
        }

        private void colorPickerEjes_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (colorPickerEjes.SelectedColor.HasValue)
                cEjes = new SolidColorBrush(colorPickerEjes.SelectedColor.Value);
        }

        private void colorPickerLineas_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (colorPickerLineas.SelectedColor.HasValue)
                cLinea = new SolidColorBrush(colorPickerLineas.SelectedColor.Value);
        }

        private void colorPickerBackground_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (colorPickerBackground.SelectedColor.HasValue)
                cBackground = new SolidColorBrush(colorPickerBackground.SelectedColor.Value);
        }

        private void Predeterminado_Click(object sender, RoutedEventArgs e)
        {
            colorPickerEjes.SelectedColor = Color.FromRgb(0, 0, 0);
            colorPickerLineas.SelectedColor = Color.FromRgb(0, 0, 0);
            colorPickerBackground.SelectedColor = Color.FromRgb(255, 255, 255);
            selectionEjes.Text = gEjes.ToString();
            selectionLinea.Text = gLinea.ToString();
            dashAnchoLinea.Text = "";
            dashEspacioLinea.Text = "";
            inicioLinea.SelectedIndex = 0;
            finLinea.SelectedIndex = 0;
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            bool error = false;
            bool dash = false;

            if (selectionLinea.Text != "")
            {
                if (!Double.TryParse(selectionLinea.Text.Replace(".", ","), out gLinea))
                {
                    MessageBox.Show("El valor de la casilla \"Grosor Linea\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                else if (gLinea < 0.0)
                {
                    MessageBox.Show("El valor de la casilla \"Grosor Linea\" no puede ser menor que 0.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }
            else
                error = true;

            if (selectionEjes.Text != "")
            {
                if (!Double.TryParse(selectionEjes.Text.Replace(".", ","), out gEjes))
                {
                    MessageBox.Show("El valor de la casilla \"Grosor Ejes\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                else if (gEjes < 0.0)
                {
                    MessageBox.Show("El valor de la casilla \"Grosor Ejes\" no puede ser menor que 0.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
            }
            else
                error = true;

            if (dashAnchoLinea.Text != "")
            {
                if (!Double.TryParse(dashAnchoLinea.Text.Replace(".", ","), out anchoPunteado))
                {
                    MessageBox.Show("El valor de la casilla \"Ancho del punteado\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                else if (anchoPunteado <= 0.0)
                {
                    MessageBox.Show("El valor de la casilla \"Ancho del punteado\"\n no puede ser menor o igual que 0.\n\nSe puede dejar la casilla en blanco\n para el valor por defecto.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                else
                    dash = true;
            }

            if (dashEspacioLinea.Text != "")
            {
                if (!Double.TryParse(dashEspacioLinea.Text.Replace(".", ","), out espacioPunteado))
                {
                    MessageBox.Show("El valor de la casilla \"Espacio del punteado\" no es válido.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                else if (espacioPunteado <= 0.0)
                {
                    MessageBox.Show("El valor de la casilla \"Espacio del punteado\"\n no puede ser menor o igual que 0.\n\nSe puede dejar la casilla en blanco\n para el valor por defecto.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    error = true;
                }
                else
                    dash = true;
            }

            if (dash)
                dashArray = new DoubleCollection() { anchoPunteado, espacioPunteado };
            else
                dashArray = null;

            if (!error)
                DialogResult = true;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void inicioLinea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PLCinicioLinea = (PenLineCap)inicioLinea.SelectedIndex;
        }

        private void finLinea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PLCfinLinea = (PenLineCap)finLinea.SelectedIndex;
        }
    }
}