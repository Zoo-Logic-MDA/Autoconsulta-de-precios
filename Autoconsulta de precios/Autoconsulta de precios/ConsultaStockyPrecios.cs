using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoconsulta_de_precios
{
    public class ArticuloStockyPrecios 
    {
        public string Articulo { get; set; }
        public string ArticuloDescripcion { get; set; }
        //public string Color { get; set; }
        //public string ColorDescripcion { get; set; }
        //public string Talle { get; set; }
        //public string TalleDescripcion { get; set; }
        //public string Lista { get; set; }
        public string Precio { get; set; }
        //public string Stock { get; set; }

    }

    public class ConsultaStockyPrecios 
    {
        public string TotalRegistros { get; set; }
        private List<ArticuloStockyPrecios> UnArticulo = new List<ArticuloStockyPrecios>();
        public List<ArticuloStockyPrecios> Resultados { get { return UnArticulo; } set { UnArticulo = value; } }

    }

    public class ConfiguracionAPI
    {
        public string Puerto { get; set; }
        public string Servicio { get; set; }
        public string Token { get; set; }
        public string BaseDatos { get; set; }
    }

    public class ConfiguracionAPICabecera
    {
        public string TotalRegistros { get; set; }
        private List<ConfiguracionAPI> UnaConfi = new List<ConfiguracionAPI>();
        public List<ConfiguracionAPI> Resultados { get { return UnaConfi; } set { UnaConfi = value; } }
    }
    //
}
