using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;


namespace GustavoMoraFactura
{
    public partial class frmManejoArchivos : Form
    {
        //variables globales
        public string ruta = @"C:\Facturacion\";
        public string archivo1 = "FacturaEncabezado.txt";
        public string archivo = "DetalleFactura.txt";
        public frmManejoArchivos()
        {
            InitializeComponent();
            txtEmpresa.KeyPress += SoloLetras;
            txtNombre.KeyPress += SoloLetras;
            txtApellido1.KeyPress += SoloLetras;
            txtApellido2.KeyPress += SoloLetras;



            txtIdentificacion.KeyPress += SoloNumeros;
            txtNumFactura.KeyPress += SoloNumeros;
            txtPlazo.KeyPress += SoloNumeros;
            txtPrecio.KeyPress += SoloDecimales;
            txtPrima.KeyPress += SoloDecimales;
        }

        private void frmManejoArchivos_Load(object sender, EventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Salir();
        }
        private void Salir()
        {
            Application.Exit();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
        private void Limpiar()
        {
            txtEmpresa.Clear();
            txtIdentificacion.Clear();
            txtNombre.Clear();
            txtApellido1.Clear();
            txtApellido2.Clear();
            txtNumFactura.Clear();
            txtPrecio.Clear();
            txtPrima.Clear();
            txtImpuesto.Clear();
            txtTotalFactura.Clear();
            txtPlazo.Clear();
             txtCuota.Clear();
            cmbTipoVehiculo.SelectedIndex = -1;
            //lblcantidad en su propiedad Text poner 00
            lblCantidad.Text = "00";

            //Limpiar  el ListView
            lsvContenido.Clear();
            //Crear las columnas del ListView
            lsvContenido.Columns.Add("Empresa", 100);
            lsvContenido.Columns.Add("Identificacion", 100);
            lsvContenido.Columns.Add("Nombre", 100);
            lsvContenido.Columns.Add("Apellido1", 100);
            lsvContenido.Columns.Add("Apellido2", 100);
            lsvContenido.Columns.Add("NumFactura", 100);
            lsvContenido.Columns.Add("Precio", 100);
            lsvContenido.Columns.Add("Prima", 100);
            lsvContenido.Columns.Add("Impuesto", 100);
            lsvContenido.Columns.Add("TotalFactura", 100);
            lsvContenido.Columns.Add("Seguro", 100);
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            Consultar();
        }
        private void Consultar()
        {
            try
            {
                lsvContenido.Items.Clear();
                lsvContenido.Columns.Clear();

                lsvContenido.View = View.Details; // 🔴 IMPORTANTE
                lsvContenido.GridLines = true;

                using (StreamReader leerEncabezado = new StreamReader(ruta + archivo1))
                {
                    string linea;

                    lsvContenido.Columns.Add("Empresa", 100);
                    lsvContenido.Columns.Add("Factura", 100);
                    lsvContenido.Columns.Add("Identificacion", 100);
                    lsvContenido.Columns.Add("Nombre", 100);
                    lsvContenido.Columns.Add("Apellido1", 100);
                    lsvContenido.Columns.Add("Apellido2", 100);
                    lsvContenido.Columns.Add("Vehiculo", 100);
                    lsvContenido.Columns.Add("Precio", 100);

                    while ((linea = leerEncabezado.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(linea))
                            continue;

                        string[] datos = linea.Split('|');

                        ListViewItem item = new ListViewItem(datos[0]);
                        for (int i = 1; i < datos.Length; i++)
                        {
                            item.SubItems.Add(datos[i]);
                        }

                        lsvContenido.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Consultar: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            CalcularValores();
            Guardar();
        }
        public void Guardar()
        {
            try
            {
                if (txtEmpresa.Text.Equals("") ||
                    txtNumFactura.Text.Equals("") ||
                    txtIdentificacion.Text.Equals("") ||
                    txtNombre.Text.Equals("") ||
                    txtApellido1.Text.Equals("") ||
                    txtApellido2.Text.Equals("") ||
                    txtPrecio.Text.Equals("") ||
                    txtPrima.Text.Equals("") ||
                    txtPlazo.Text.Equals(""))
                {
                    MessageBox.Show("Faltan datos por llenar", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                using (StreamWriter escribirEncabezado = new StreamWriter(ruta + archivo1, true))
                {
                    escribirEncabezado.WriteLine(txtEmpresa.Text + "|" +
                        txtNumFactura.Text + "|" +
                        txtIdentificacion.Text + "|" +
                        txtNombre.Text + "|" +
                        txtApellido1.Text + "|" +
                        txtApellido2.Text + "|" +
                        cmbTipoVehiculo.Text + "|" +
                        txtPrecio.Text + "|");
                }

                using (StreamWriter escribirDetalle = new StreamWriter(ruta + archivo, true))
                {
                    escribirDetalle.WriteLine(txtIdentificacion.Text + "|" +
                        txtNombre.Text + " " + txtApellido1.Text + " " + txtApellido2.Text + "|" +
                        txtPrecio.Text + "|" +
                        txtSeguro.Text + "|" +
                        txtInteres.Text + "|" +
                        txtPrima.Text + "|" +
                        txtImpuesto.Text + "|" +
                        txtPlazo.Text + "|" +
                        txtTotalFactura.Text + "|" +
                        txtCuota.Text + "|");
                }

                MessageBox.Show("Datos guardados correctamente", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LeerDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SoloLetras(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
            else
                e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        private void SoloDecimales(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
                return;

            if ((e.KeyChar == '.' || e.KeyChar == ',') && !((TextBox)sender).Text.Contains("."))
            {
                e.KeyChar = '.'; // Normalizar a punto
            }
            else
            {
                e.Handled = true;
            }
        }
        public void LeerDetalle()
        {
            try
            {
                if (!File.Exists(ruta + archivo))
                    return;

                lsvContenido.Items.Clear();
                CrearLista();

                using (StreamReader leerDetalle = new StreamReader(ruta + archivo))
                {
                    string linea;

                    while ((linea = leerDetalle.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(linea))
                            continue;

                        // Split correcto según cómo guardas los datos
                        string[] datos = linea.Split('|');

                        if (datos.Length < 10) continue; // validación de seguridad

                        ListViewItem item = new ListViewItem(datos[0]);
                        for (int i = 1; i < datos.Length; i++)
                        {
                            item.SubItems.Add(datos[i]);
                        }

                        lsvContenido.Items.Add(item);
                    }
                }

                lblCantidad.Text = lsvContenido.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el archivo: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void CrearLista()
        {
            lsvContenido.Clear();
            lsvContenido.Columns.Add("Identificacion", 100);
            lsvContenido.Columns.Add("Nombre Completo", 150);
            lsvContenido.Columns.Add("Precio", 100);
            lsvContenido.Columns.Add("Seguro", 100);
            lsvContenido.Columns.Add("Interes", 100);
            lsvContenido.Columns.Add("Prima", 100);
            lsvContenido.Columns.Add("Impuesto", 100);
            lsvContenido.Columns.Add("Plazo", 100);
            lsvContenido.Columns.Add("TotalFactura", 100);
            lsvContenido.Columns.Add("Cuota", 100);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Eliminar();
        }
        public void Eliminar()
        {
            try
            {
                string temporal = "temp.txt";
                string linea;
                StreamReader leerDetalle = new StreamReader(ruta + archivo);
                StreamWriter escribirTemporal = new StreamWriter(ruta + temporal);
                while (leerDetalle.Peek() != -1)
                {
                    linea = leerDetalle.ReadLine();
                    if (string.IsNullOrEmpty(linea))
                    {
                        continue;
                    }
                    string[] datos = linea.Split(',');
                    if (datos[0] != txtIdentificacion.Text)
                    {
                        escribirTemporal.WriteLine(linea);
                    }
                }
                leerDetalle.Close();
                escribirTemporal.Close();
                File.Delete(ruta + archivo);
                File.Move(ruta + temporal, ruta + archivo);
                MessageBox.Show("Archivos eliminados correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LeerDetalle();
                Limpiar();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar los archivos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void CalcularValores()
        {
            try
            {
                // Parsear entradas
                double precio = double.Parse(txtPrecio.Text);
                double prima = double.Parse(txtPrima.Text);
                int plazo = int.Parse(txtPlazo.Text);

                // Validar rangos según enunciado (opcional, pero el enunciado exige validaciones)
                if (precio < 11000000 || precio > 15000000)
                {
                    MessageBox.Show("El precio del vehículo debe estar entre 11.000.000 y 15.000.000", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (prima < 3600000)
                {
                    MessageBox.Show("La prima no puede ser menor a 3.600.000", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (plazo < 12 || plazo > 96)
                {
                    MessageBox.Show("El plazo debe estar entre 12 y 96 meses", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Cálculos
                double impuesto = precio * 0.13;
                double totalFactura = precio + impuesto;
                double montoFinanciado = totalFactura - prima;
                double interesTotal = montoFinanciado * 0.085; // interés total del 8.5%
                double cuotaSinSeguro = (montoFinanciado + interesTotal) / plazo;
                double seguroMensual = 50000;
                double cuotaMensual = cuotaSinSeguro + seguroMensual;

                // Asignar a los campos (redondear a 2 decimales si es necesario)
                txtImpuesto.Text = impuesto.ToString("F2");
                txtTotalFactura.Text = totalFactura.ToString("F2");
                txtSeguro.Text = seguroMensual.ToString("F0"); // es entero
                txtCuota.Text = cuotaMensual.ToString("F2");

                // Bloquear campos calculados (según punto 4.m)
                txtImpuesto.ReadOnly = true;
                txtTotalFactura.ReadOnly = true;
                txtSeguro.ReadOnly = true;
                txtCuota.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en cálculo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
    
