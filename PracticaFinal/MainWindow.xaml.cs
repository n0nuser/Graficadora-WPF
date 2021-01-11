using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PracticaFinal
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Gestión redimensión de elementos
        private ScaleTransform sc;
        private TranslateTransform tt;
        private TransformGroup tg;
        private double ancho, alto;
        private Line ejeX, ejeY;

        //Lista de puntos a dibujar
        private ObservableCollection<Punto> listaPuntos;

        //Ventanas modales
        private CuadroModalManual cdmm;
        private CuadroModalAleatorio cdma;
        private CuadroModalTrigonometria cdmt;
        private CuadroModalOpciones cdmo;
        

        //Variables por defecto para la polilínea
        private SolidColorBrush colorEjes = Brushes.Black;
        private SolidColorBrush colorLinea = Brushes.Black;
        private double grosorEjes = 20;
        private double grosorLinea = 90;
        private DoubleCollection dashArray = null;
        private PenLineCap inicioLinea = 0;
        private PenLineCap finLinea = 0;

        //Purgar
        private bool mouseDown = false; // Set to 'true' when mouse is held down.
        private Point mouseDownPos;
        private bool purgaON = false;

        public MainWindow() //Este es el constructor
        {
            InitializeComponent();
            this.listaPuntos = new ObservableCollection<Punto>();
            this.Loaded += cargado;
            sc = new ScaleTransform();
            tt = new TranslateTransform();
            tg = new TransformGroup();
            tg.Children.Add(tt);
            tg.Children.Add(sc);
        }

        /// <summary>
        /// Permite inicializar las ventanas modales a null y dibujar primeramente los ejes.
        /// </summary>
        private void cargado(object sender, EventArgs e)
        {
            cdmm = null;
            cdma = null;
            cdmo = null;
            cdmt = null;
            ejeX = null;
            ejeY = null;

            dibujarEjes();
        }

        /// <summary>
        /// Permite dibujar los polilínea.
        /// </summary>
        private void dibuja()
        {
            int i;
            Point p = new Point();
            Polyline linea = new Polyline();
            if (this.listaPuntos.Count > 0)
            {
                for (i = 0; i < this.listaPuntos.Count; i++)
                {
                    p.X = this.listaPuntos[i].CorX;
                    p.Y = -this.listaPuntos[i].CorY;
                    linea.Points.Add(p);
                }
                linea.Stroke = colorLinea;
                if (dashArray != null)
                    linea.StrokeDashArray = dashArray;
                linea.StrokeStartLineCap = inicioLinea;
                linea.StrokeEndLineCap = finLinea;
                linea.StrokeThickness = grosorLinea / ancho;
                linea.RenderTransform = tg;
                miCanvas.Children.Add(linea);
            }
        }

        /// <summary>
        /// Permite dibujar los ejes.
        /// </summary>
        private void dibujarEjes()
        {
            ejeX = new Line();
            ejeY = new Line();

            ejeX.Stroke = colorEjes;
            ejeX.StrokeThickness = grosorEjes / ancho;
            ejeX.X1 = -10;
            ejeX.Y1 = 0;
            ejeX.X2 = 10;
            ejeX.Y2 = 0;

            ejeX.RenderTransform = tg;

            miCanvas.Children.Add(ejeX);

            ejeY.Stroke = colorEjes;
            ejeY.StrokeThickness = grosorEjes / ancho;
            ejeY.X1 = 0;
            ejeY.Y1 = -10;
            ejeY.X2 = 0;
            ejeY.Y2 = 10;

            ejeY.RenderTransform = tg;

            miCanvas.Children.Add(ejeY);
        }

        /// <summary>
        /// Permite guardar el canvas o la gráfica como una imagen ".png".
        /// <para>Base: http://tutorialgenius.blogspot.com/2014/12/saving-window-or-canvas-as-png-bitmap.html</para>
        /// </summary>
        private void GuardarImagen_Click(object sender, RoutedEventArgs e)
        {
            Visual pantalla = miCanvas;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Grafica"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG File (.png)|*.png"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;

                if (miCanvas.Visibility == Visibility.Visible)
                {
                    Size size = new Size(this.Width, this.Height);
                    miCanvas.Measure(size);
                }
                else if (GBarras.Visibility == Visibility.Visible)
                    pantalla = GBarras;
                else if (GArea.Visibility == Visibility.Visible)
                    pantalla = GArea;
                else if (GBurbuja.Visibility == Visibility.Visible)
                    pantalla = GBurbuja;
                else if (GColumna.Visibility == Visibility.Visible)
                    pantalla = miCanvas;
                else if (GLinea.Visibility == Visibility.Visible)
                    pantalla = GLinea;
                else if (GTarta.Visibility == Visibility.Visible)
                    pantalla = GTarta;
                else if (GDispersion.Visibility == Visibility.Visible)
                    pantalla = GDispersion;

                var rtb = new RenderTargetBitmap(
                    (int)this.Width, //width
                    (int)this.Height, //height
                    96, //dpi x
                    96, //dpi y
                    PixelFormats.Pbgra32 // pixelformat
                    );
                rtb.Render(pantalla);

                var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
                enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(rtb));

                using (var stm = System.IO.File.Create(filename))
                    enc.Save(stm);
            }
        }

        /// <summary>
        /// Gestiona el momento cuando se empieza a seleccionar un área en la pantalla.
        /// <para>Base: https://stackoverflow.com/questions/1838163/click-and-drag-selection-box-in-wpf</para>
        /// </summary>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Capture and track the mouse.
            mouseDown = true;
            mouseDownPos = e.GetPosition(miGrid);
            miGrid.CaptureMouse();

            // Make the drag selection box visible.
            selectionBox.Visibility = Visibility.Visible;

            // Initial placement of the drag selection box.
            Canvas.SetLeft(selectionBox, mouseDownPos.X);
            Canvas.SetTop(selectionBox, mouseDownPos.Y);
            selectionBox.Width = 0;
            if (mouseDown)
                selectionBox.Height = 0;
        }

        /// <summary>
        /// Modifica los valores de altura y anchura del rectángulo dibujado por el usuario según mueve el ratón.
        /// </summary>
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            {
                // When the mouse is held down, reposition the drag selection box.

                Point mousePos = e.GetPosition(miGrid);
                if (mousePos.X < 0)
                    mousePos.X = 0;

                if (mousePos.X > miGrid.ActualWidth)
                    mousePos.X = miGrid.ActualWidth;

                if (mousePos.Y < 0)
                    mousePos.Y = 0;

                if (mousePos.Y > miGrid.ActualHeight)
                    mousePos.Y = miGrid.ActualHeight;

                if (mouseDownPos.X < mousePos.X)
                {
                    Canvas.SetLeft(selectionBox, mouseDownPos.X);
                    selectionBox.Width = mousePos.X - mouseDownPos.X;
                }
                else
                {
                    Canvas.SetLeft(selectionBox, mousePos.X);
                    selectionBox.Width = mouseDownPos.X - mousePos.X;
                }

                if (mouseDownPos.Y < mousePos.Y)
                {
                    Canvas.SetTop(selectionBox, mouseDownPos.Y);
                    selectionBox.Height = mousePos.Y - mouseDownPos.Y;
                }
                else
                {
                    Canvas.SetTop(selectionBox, mousePos.Y);
                    selectionBox.Height = mouseDownPos.Y - mousePos.Y;
                }
            }
        }

        /// <summary>
        /// Gestiona el momento cuando se deja de seleccionar un área en la pantalla.
        /// <para>Depende de si la variable purgaON está activa y el canvas de la polilínea es visible.</para>
        /// </summary>
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Release the mouse capture and stop tracking it.
            mouseDown = false;
            miGrid.ReleaseMouseCapture();
            Point mouseUpPos = e.GetPosition(miCanvas);
            // Hide the drag selection box.
            selectionBox.Visibility = Visibility.Collapsed;
            if (purgaON && miCanvas.Visibility == Visibility.Visible)
            {
                double XValA = (mouseDownPos.X / sc.ScaleX) - tt.X;
                double XValC = (mouseUpPos.X / sc.ScaleX) - tt.X;
                double YValA = -((mouseDownPos.Y / sc.ScaleY) - tt.Y);
                double YValC = -((mouseUpPos.Y / sc.ScaleY) - tt.Y);
                double temp;
                if (XValA != XValC) //Así evito que al hacer click se borre toda la figura
                {
                    if (XValA > XValC)
                    {
                        temp = XValA;
                        XValA = XValC;
                        XValC = temp;
                    }

                    Point posA = new Point();
                    posA.X = XValA;
                    posA.Y = YValA;

                    Point posB = new Point();
                    posB.X = XValC;
                    posB.Y = YValA;

                    Point posC = new Point();
                    posC.X = XValC;
                    posC.Y = YValC;

                    Point posD = new Point();
                    posD.X = XValA;
                    posD.Y = YValC;

                    Point[] rectangulo = new Point[] { posA, posB, posC, posD };

                    var selectedFiles = this.listaPuntos.Cast<Punto>().ToList();
                    foreach (Punto item in selectedFiles)
                    {
                        Point p = new Point();
                        p.X = item.CorX;
                        p.Y = item.CorY;
                        if (!IsInPolygon(rectangulo, p))
                        {
                            this.listaPuntos.Remove(item);
                        }
                    }
                    miCanvas.Children.Clear();
                    ejeX = null;
                    ejeY = null;
                    dibujarEjes();
                    dibuja();
                }
            }
        }

        /// <summary>IsInPolygon verifica si un punto se encuentra en el rectángulo creado por el usuario.
        /// <para>https://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon</para>
        /// </summary>
        private bool IsInPolygon(Point[] rectangle, Point testPoint)
        {
            bool isInside = false;
            int j = rectangle.Count() - 1;
            for (int i = 0; i < rectangle.Count(); i++)
            {
                if (rectangle[i].Y < testPoint.Y && rectangle[j].Y >= testPoint.Y || rectangle[j].Y < testPoint.Y && rectangle[i].Y >= testPoint.Y)
                {
                    if (rectangle[i].X + (testPoint.Y - rectangle[i].Y) / (rectangle[j].Y - rectangle[i].Y) * (rectangle[j].X - rectangle[i].X) < testPoint.X)
                        isInside = !isInside;
                }
                j = i;
            }
            return isInside;
        }

        /// <summary>
        /// Permite elegir qué ventana modal abrir y le pasa sus valores a la ventana principal, dibujando a su vez la gráfica.
        /// </summary>
        private void CDModal_Click(object sender, RoutedEventArgs e)
        {
            if (sender == CDModalManual)
            {
                cdmm = new CuadroModalManual();
                cdmm.Owner = this;
                cdmm.ListaPuntosManual = this.listaPuntos;
                cdmm.ShowDialog();

                if (cdmm.DialogResult == true)
                {
                    if (miCanvas.Visibility == Visibility.Visible)
                    {
                        VentanaPrincipal.Title = "Gráfica por puntos";
                        this.listaPuntos = cdmm.ListaPuntosManual;
                        miCanvas.Children.Clear();
                        ejeX = null;
                        ejeY = null;
                        dibujarEjes();
                        dibuja();
                    }
                    else
                        LoadBarChartData();
                }
            }

            else if (sender == CDModalAleatorio)
            {
                cdma = new CuadroModalAleatorio();
                cdma.Owner = this;
                cdma.ListaPuntosPolinomio = this.listaPuntos;
                cdma.ShowDialog();

                if (cdma.DialogResult == true)
                {
                    if (miCanvas.Visibility == Visibility.Visible)
                    {
                        VentanaPrincipal.Title = "Gráfica Polinómica";
                        this.listaPuntos = cdma.ListaPuntosPolinomio;
                        miCanvas.Children.Clear();
                        ejeX = null;
                        ejeY = null;
                        dibujarEjes();
                        dibuja();
                    }
                    else
                        LoadBarChartData();
                }
            }

            else if (sender == CDModalTrigonometria)
            {
                cdmt = new CuadroModalTrigonometria();
                cdmt.Owner = this;
                cdmt.ListaPuntosTrigonometria = this.listaPuntos;
                cdmt.ShowDialog();

                if (cdmt.DialogResult == true)
                {
                    if (miCanvas.Visibility == Visibility.Visible)
                    {
                        VentanaPrincipal.Title = "Gráfica Trigonométrica";
                        this.listaPuntos = cdmt.ListaPuntosTrigonometria;
                        miCanvas.Children.Clear();
                        ejeX = null;
                        ejeY = null;
                        dibujarEjes();
                        dibuja();
                    }
                    else
                        LoadBarChartData();
                }
            }

            if (sender == CDModalOpciones)
            {
                cdmo = new CuadroModalOpciones();
                cdmo.Owner = this;
                cdmo.colorEjes = colorEjes;
                cdmo.colorLinea = colorLinea;
                cdmo.colorBackground = (SolidColorBrush)miCanvas.Background;
                cdmo.grosorEjes = grosorEjes;
                cdmo.grosorLinea = grosorLinea;
                cdmo.InicioLinea = inicioLinea;
                cdmo.FinLinea = finLinea;
                cdmo.punteado = dashArray;
                cdmo.ShowDialog();

                if (cdmo.DialogResult == true)
                {
                    colorEjes = cdmo.colorEjes;
                    colorLinea = cdmo.colorLinea;
                    grosorEjes = cdmo.grosorEjes;
                    grosorLinea = cdmo.grosorLinea;
                    inicioLinea = cdmo.InicioLinea;
                    finLinea = cdmo.FinLinea;
                    dashArray = cdmo.punteado;
                    miCanvas.Background = cdmo.colorBackground;
                    miCanvas.Children.Clear();
                    ejeX = null;
                    ejeY = null;
                    dibujarEjes();
                    dibuja();
                }
            }
        }

        /// <summary>
        /// Cambia el texto del botón de Purga y (des)habilita la variable global que controla la Purga.
        /// </summary>
        private void Purga_Click(object sender, RoutedEventArgs e)
        {
            if (Purga.Content.ToString() == "Habilitar modo Purga")
            {
                purgaON = true;
                Purga.Content = "Deshabilitar modo Purga";
            }
            else
            {
                purgaON = false;
                Purga.Content = "Habilitar modo Purga";
            }
        }

        /// <summary>
        /// Permite elegir cómo representar los puntos.
        /// </summary>
        private void Representacion_Click(object sender, RoutedEventArgs e)
        {
            GBarras.Visibility = Visibility.Hidden;
            GArea.Visibility = Visibility.Hidden;
            GBurbuja.Visibility = Visibility.Hidden;
            GColumna.Visibility = Visibility.Hidden;
            GLinea.Visibility = Visibility.Hidden;
            GTarta.Visibility = Visibility.Hidden;
            GDispersion.Visibility = Visibility.Hidden;

            if (sender == Polilinea)
            {
                miCanvas.Visibility = Visibility.Visible;

                miCanvas.Children.Clear();
                ejeX = null;
                ejeY = null;
                dibujarEjes();
                dibuja();
            }
            else
            {
                miCanvas.Visibility = Visibility.Hidden;

                if (sender == Barras)
                {
                    GBarras.Visibility = Visibility.Visible;
                    ((AreaSeries)GArea.Series[0]).ItemsSource = null;
                    ((BubbleSeries)GBurbuja.Series[0]).ItemsSource = null;
                    ((ColumnSeries)GColumna.Series[0]).ItemsSource = null;
                    ((LineSeries)GLinea.Series[0]).ItemsSource = null;
                    ((PieSeries)GTarta.Series[0]).ItemsSource = null;
                    ((ScatterSeries)GDispersion.Series[0]).ItemsSource = null;
                }
                else if (sender == Area)
                {
                    GArea.Visibility = Visibility.Visible;
                    ((BarSeries)GBarras.Series[0]).ItemsSource = null;
                    ((BubbleSeries)GBurbuja.Series[0]).ItemsSource = null;
                    ((ColumnSeries)GColumna.Series[0]).ItemsSource = null;
                    ((LineSeries)GLinea.Series[0]).ItemsSource = null;
                    ((PieSeries)GTarta.Series[0]).ItemsSource = null;
                    ((ScatterSeries)GDispersion.Series[0]).ItemsSource = null;
                }
                else if (sender == Burbuja)
                {
                    GBurbuja.Visibility = Visibility.Visible;
                    ((BarSeries)GBarras.Series[0]).ItemsSource = null;
                    ((AreaSeries)GArea.Series[0]).ItemsSource = null;
                    ((ColumnSeries)GColumna.Series[0]).ItemsSource = null;
                    ((LineSeries)GLinea.Series[0]).ItemsSource = null;
                    ((PieSeries)GTarta.Series[0]).ItemsSource = null;
                    ((ScatterSeries)GDispersion.Series[0]).ItemsSource = null;
                }
                else if (sender == Columna)
                {
                    GColumna.Visibility = Visibility.Visible;
                    ((BarSeries)GBarras.Series[0]).ItemsSource = null;
                    ((AreaSeries)GArea.Series[0]).ItemsSource = null;
                    ((BubbleSeries)GBurbuja.Series[0]).ItemsSource = null;
                    ((LineSeries)GLinea.Series[0]).ItemsSource = null;
                    ((PieSeries)GTarta.Series[0]).ItemsSource = null;
                    ((ScatterSeries)GDispersion.Series[0]).ItemsSource = null;
                }
                else if (sender == Linea)
                {
                    GLinea.Visibility = Visibility.Visible;
                    ((BarSeries)GBarras.Series[0]).ItemsSource = null;
                    ((AreaSeries)GArea.Series[0]).ItemsSource = null;
                    ((BubbleSeries)GBurbuja.Series[0]).ItemsSource = null;
                    ((ColumnSeries)GColumna.Series[0]).ItemsSource = null;
                    ((PieSeries)GTarta.Series[0]).ItemsSource = null;
                    ((ScatterSeries)GDispersion.Series[0]).ItemsSource = null;
                }
                else if (sender == Tarta)
                {
                    GTarta.Visibility = Visibility.Visible;
                    ((BarSeries)GBarras.Series[0]).ItemsSource = null;
                    ((AreaSeries)GArea.Series[0]).ItemsSource = null;
                    ((BubbleSeries)GBurbuja.Series[0]).ItemsSource = null;
                    ((ColumnSeries)GColumna.Series[0]).ItemsSource = null;
                    ((LineSeries)GLinea.Series[0]).ItemsSource = null;
                    ((ScatterSeries)GDispersion.Series[0]).ItemsSource = null;
                }
                else if (sender == Dispersion)
                {
                    GDispersion.Visibility = Visibility.Visible;
                    ((BarSeries)GBarras.Series[0]).ItemsSource = null;
                    ((AreaSeries)GArea.Series[0]).ItemsSource = null;
                    ((BubbleSeries)GBurbuja.Series[0]).ItemsSource = null;
                    ((ColumnSeries)GColumna.Series[0]).ItemsSource = null;
                    ((LineSeries)GLinea.Series[0]).ItemsSource = null;
                    ((PieSeries)GTarta.Series[0]).ItemsSource = null;
                }

                LoadBarChartData();
            }
        }

        /// <summary>
        /// Se encarga de añadir los puntos de la listaPuntos a la lista de puntos de la gráfica visible.
        /// </summary>
        private void LoadBarChartData()
        {
            var listKVP = new List<KeyValuePair<double, double>>();
            var selectedFiles = this.listaPuntos.Cast<Punto>().ToList();
            foreach (Punto item in selectedFiles)
                listKVP.Add(new KeyValuePair<double, double>(item.CorX, item.CorY));
            
            if (GBarras.Visibility == Visibility.Visible)
                ((BarSeries)GBarras.Series[0]).ItemsSource = listKVP;
            else if (GArea.Visibility == Visibility.Visible)
                ((AreaSeries)GArea.Series[0]).ItemsSource = listKVP;
            else if (GBurbuja.Visibility == Visibility.Visible)
                ((BubbleSeries)GBurbuja.Series[0]).ItemsSource = listKVP;
            else if (GColumna.Visibility == Visibility.Visible)
                ((ColumnSeries)GColumna.Series[0]).ItemsSource = listKVP;
            else if (GLinea.Visibility == Visibility.Visible)
                ((LineSeries)GLinea.Series[0]).ItemsSource = listKVP;
            else if (GTarta.Visibility == Visibility.Visible)
                ((PieSeries)GTarta.Series[0]).ItemsSource = listKVP;
            else if (GDispersion.Visibility == Visibility.Visible)
                ((ScatterSeries)GDispersion.Series[0]).ItemsSource = listKVP;
        }

        /// <summary>
        /// Muestra un MessageBox con ayuda sobre el uso de la aplicación.
        /// </summary>
        private void Ayuda_Click(object sender, RoutedEventArgs e)
        {
            String ayuda = "¡Hola!\n" +
                "Esta aplicación sirve para representar gráficamente puntos que puedas introducir manualmente, o dados mediante parámetros imprimirá por pantalla una función polinómica (de hasta 3º grado) o una función trigonométrica (seno, coseno, tangente).\n" +
                "Para empezar a introducir estos puntos o parámetros, haga \"Click\" en la pestaña \"ARCHIVO\".\n\n" +
                "Para variar la apariencia de la polilínea existe la pestaña \"OPCIONES\" en la cual encontrará la opción de \"Apariencia\", la cual permite modificar los colores, grosor y formato de la polilínea, y un botón denominado \"Purga\", el cual al ser habilitado permite eliminar todo lo NO contenido en la selección que haga usted en la pantalla.\n\n" +
                "Y por último se encuentra la pestaña de \"REPRESENTACIÓN\", en la cual se encuentran las distintas formas de representación de la polilínea. Siendo estas la que viene por defecto, en formato gráfico de barras, de columna, de área, de líneas, de dispersión, de burbuja y de tarta (circular).\n\n" +
                "¡Espero que disfrute de la aplicación!\n" +
                "Pablo Jesús González Rubio";
            MessageBox.Show(ayuda, "Ayuda", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        /// <summary>
        /// Permite redimensionar automáticamente el canvas mediante el TranslateTransfrom y el ScalateTransform.
        /// </summary>
        private void miCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ancho = miCanvas.ActualWidth;
            alto = miCanvas.ActualHeight;

            tt.X = 10;
            tt.Y = 10;

            sc.ScaleX = ancho / 20;
            sc.ScaleY = alto / 20;
        }
    }
}
