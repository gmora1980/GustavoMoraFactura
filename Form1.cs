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
            txtNombre.Focus();
            txtApellido1.Clear();
            txtApellido2.Clear();
            txtNumFactura.Clear();
            txtPrecio.Clear();
            txtPrima.Clear();
            txtImpuesto.Clear();
            txtTotalFactura.Clear();
            txtSeguro.Clear();
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
                StreamReader leerEncabezado = new StreamReader(ruta + archivo1);
                string linea;
                lsvContenido.Clear();
                lsvContenido.Columns.Add("Empresa", 100);
                lsvContenido.Columns.Add("Factura", 100);
                lsvContenido.Columns.Add("Identificacion", 100);
                lsvContenido.Columns.Add("Nombre", 100);
                lsvContenido.Columns.Add("Apellido1", 100);
                lsvContenido.Columns.Add("Apellido2", 100);
                lsvContenido.Columns.Add("Vehiculo", 100);
                lsvContenido.Columns.Add("Precio", 100);
                while (leerEncabezado.Peek() != -1)
                {
                    linea = leerEncabezado.ReadLine();
                    if (string.IsNullOrEmpty(linea))
                    {
                        continue;
                    }
                    string[] datos = linea.Split(',');
                    lsvContenido.Items.Add(datos[0]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[1]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[2]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[3]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[4]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[5]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[6]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[7]);
                }
                leerEncabezado.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
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
                    MessageBox.Show("Faltan datos por llenar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                    MessageBox.Show("Directorio creado de Forma correcta ", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                StreamWriter escribirEncabezado = new StreamWriter(ruta + archivo1, true);
                escribirEncabezado.WriteLine(txtEmpresa.Text + "," +
                    txtNumFactura.Text + "," +
                    txtIdentificacion.Text + "," +
                    txtNombre.Text + "," +
                    txtApellido1.Text + "," +
                    txtApellido2.Text + "," +
                    cmbTipoVehiculo.Text + "," +
                    txtPrecio.Text + ",");


                escribirEncabezado.Close();
                StreamWriter escribirDetalle = new StreamWriter(ruta + archivo, true);
                escribirDetalle.WriteLine(txtIdentificacion.Text + "," +
                    txtNombre.Text + " " + txtApellido1.Text + " " + txtApellido2.Text + "," +
                    txtPrecio.Text + "," +
                    txtSeguro.Text + "," +
                    txtInteres.Text + "," +
                    txtPrima.Text + "," +
                    txtImpuesto.Text + "," +
                    txtPlazo.Text + "," +
                    txtTotalFactura.Text + "," +
                    txtCuota.Text + ",");
                escribirDetalle.Close();
                MessageBox.Show("Datos guardados correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LeerDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void LeerDetalle()
        {
            try
            {
                StreamReader leerDetalle = new StreamReader(ruta + archivo);
                string linea;
                CrearLista();
                while (leerDetalle.Peek() != -1)
                {
                    linea = leerDetalle.ReadLine();
                    if (string.IsNullOrEmpty(linea))
                    {
                        continue;
                    }
                    string[] datos = linea.Split(',');
                    lsvContenido.Items.Add(datos[0]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[1]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[2]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[3]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[4]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[5]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[6]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[7]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[8]);
                    lsvContenido.Items[lsvContenido.Items.Count - 1].SubItems.Add(datos[9]);
                    leerDetalle.Close();
                }
                leerDetalle.Close();
                lblCantidad.Text = lsvContenido.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }

}
    
