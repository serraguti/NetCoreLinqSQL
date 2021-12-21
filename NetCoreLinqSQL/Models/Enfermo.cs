using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreLinqSQL.Models
{
    public class Enfermo
    {
        public int Inscripcion { get; set; }
        public String Apellido { get; set; }
        public String Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public String Nss { get; set; }
    }
}
