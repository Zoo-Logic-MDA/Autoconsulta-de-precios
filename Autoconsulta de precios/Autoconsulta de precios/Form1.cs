using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Http;

namespace Autoconsulta_de_precios
{
    public partial class Form1 : Form
    {

        //DATOS PARA LA CONEXION CON EL SERVIDOR DRAGONFISH ****************************************************
        //Refresh token generado según el proceso indicado en el manual de Dragonfish
        public string token = "sin token";
        //Nombre del servicio configurado en Dragonfish
        public string servicio = "sin servicio";
        //Dirección del servidor donde está corriendo Dragonfish y puerto definido para el servicio.
        public string servidor = "http://localhost:sin_puerto/";
        //Base de datos a consultar
        public string baseDatos = "basededatos";

        //Archivo que contiene la información de configuración de la aplicación de autoconsulta de precios;
        public string path = @"c:\Dragonfish\ConfiguracionAPI.ini";
        //Articulo a buscar (ojo que no busca equivalencias)
        public string articuloConsultado = "";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LeeConfiguracion();
            Articulo.Focus();
        }

        private string armarConsulta()
        {
            string consultaArmada = "api.Dragonfish/ConsultaStockYPrecios/";
            consultaArmada = consultaArmada + "?query=" + Articulo.Text+ " &preciocero=true&stockcero=true";
            return consultaArmada;
        }

      

        private void ConsultaryMostrarEnTextBox (string consultaArmada)
        {//La idea es buscar y mostrar en los textbox la consulta armada.

            string strRespuesta = "";
            GestionJson Json = new GestionJson();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(servidor + armarConsulta());
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("IdCliente", servicio);
            httpWebRequest.Headers.Add("Authorization", token);
            httpWebRequest.Headers.Add("BaseDeDatos", baseDatos);

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    strRespuesta = streamReader.ReadToEnd();
                    ConsultaStockyPrecios p = Json.JsonDeserialize<ConsultaStockyPrecios>(strRespuesta);
                    List<ArticuloStockyPrecios> todosLosArticulos = new List<ArticuloStockyPrecios>();
                    
                    foreach (ArticuloStockyPrecios art in p.Resultados)
                    {//Atención, si hay mas de un artículo igual en la respuesta, muestra el último.
                        Articulo.Text = art.Articulo;
                        Descripcion.Text = art.ArticuloDescripcion;
                        Precio.Text = art.Precio;
                                          
                    }

                }

            }

            catch (WebException ex)
            {
                HttpWebResponse respuesta = ex.Response as HttpWebResponse;
                if (respuesta != null)
                {
                 
                    Articulo.Text = "*****************";
                    Descripcion.Text = respuesta.StatusDescription;
                    Precio.Text = "*****************";
                    ex.Response.Close();
                }
                else
                {
                    throw;
                }
            }
        }


        public string LeeConfiguracion()
        //Determina si las habilidades están fijas, parámetro del archivo Colas.ini
        {

            GestionJson Json = new GestionJson();
            string strLinea = "";
            
            if (File.Exists(path))
            {

                using (var streamReader = new StreamReader(path))
                {
                    strLinea = streamReader.ReadToEnd();
                    ConfiguracionAPICabecera p = Json.JsonDeserialize<ConfiguracionAPICabecera>(strLinea);
                    List<ConfiguracionAPI> todasLasConfig = new List<ConfiguracionAPI>();
                    foreach (ConfiguracionAPI Config in p.Resultados)
                    {//Atención, si hay mas de una configuración en la respuesta, toma el último.
                        token = Config.Token;
                        servicio = Config.Servicio;
                        servidor = Config.Puerto;
                        baseDatos = Config.BaseDatos;
                    }
                }
            }
            else
            {
                MessageBox.Show("No se encuentra el archivo ConfiguracionAPI.ini en c:/Dragonfish/ ", "Agregue el archivo y reinicie la aplicación.", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return strLinea;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        { }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {}

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (int)Keys.Enter)
                {
                    if (articuloConsultado != "")
                    {
                        //aqui codigo para borrar lo ya cargado en el textbox
                        String s = Articulo.Text;
                        String searchString = articuloConsultado;
                        int startIndex = 0;
                        int endIndex = s.IndexOf(searchString);
                        String substring = s.Substring(startIndex, endIndex);
                        Articulo.Text = substring;
                    }

                    string consultaArmada = armarConsulta();
                    ConsultaryMostrarEnTextBox(consultaArmada);
                    articuloConsultado = Articulo.Text;

                }
            }
            catch 
            {
                Articulo.Text = "";
            }
        }

        private void Articulo_TextChanged(object sender, EventArgs e){}
    }
}
