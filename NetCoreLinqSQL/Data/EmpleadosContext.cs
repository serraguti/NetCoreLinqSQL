using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using NetCoreLinqSQL.Models;

#region PROCEDIMIENTOS ALMACENADOS
//create procedure incrementarsalariosoficioempleados
//(@oficio nvarchar(50), @incremento int)
//as
//    update emp set salario = salario + @incremento
//	where oficio = @oficio
//go
#endregion

namespace NetCoreLinqSQL.Data
{
    public class EmpleadosContext
    {
        private SqlDataAdapter ademp;
        private DataTable tablaemp;
        private SqlConnection cn;
        private SqlCommand com;

        public EmpleadosContext()
        {
            string cadenaconexion = @"Data Source=LOCALHOST;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2021";
            this.cn = new SqlConnection(cadenaconexion);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.RefreshData();
        }

        private void RefreshData()
        {
            string cadenaconexion = @"Data Source=LOCALHOST;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2021";
            string sql = "select * from emp";
            this.ademp = new SqlDataAdapter(sql, cadenaconexion);
            this.tablaemp = new DataTable();
            this.ademp.Fill(tablaemp);
        }

        public List<string> GetOficios()
        {
            var consulta = (from datos in this.tablaemp.AsEnumerable()
                           select datos.Field<string>("OFICIO")).Distinct();
            List<string> oficios = new List<string>();
            foreach (string oficio in consulta)
            {
                oficios.Add(oficio);
            }
            return oficios;
        }

        public List<Empleado> GetEmpleadosOficio(string oficio)
        {
            var consulta = from datos in this.tablaemp.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           select datos;
            List<Empleado> empleados = new List<Empleado>();
            foreach (var row in consulta)
            {
                Empleado empleado = new Empleado();
                empleado.IdEmpleado = row.Field<int>("EMP_NO");
                empleado.Apellido = row.Field<string>("APELLIDO");
                empleado.Oficio = row.Field<string>("OFICIO");
                empleado.Salario = row.Field<int>("SALARIO");
                empleado.IdDepartamento = row.Field<int>("DEPT_NO");
                empleados.Add(empleado);
            }
            return empleados;
        }

        public int IncrementarSalarioEmpleadosOficio(string oficio, int incremento)
        {
            string sql = "incrementarsalariosoficioempleados";
            this.com.Parameters.AddWithValue("@oficio", oficio);
            this.com.Parameters.AddWithValue("@incremento", incremento);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.StoredProcedure;
            this.cn.Open();
            int results = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            this.RefreshData();
            return results;
        }

        public List<Empleado> GetEmpleados()
        {
            //PARA RECUPERAR DATOS CON LINQ, NECESITAMOS
            //HACERLO SOBRE LA COLECCION DE FILAS DE LA TABLA
            var consulta = from datos in this.tablaemp.AsEnumerable()
                           select datos;
            //AHORA MISMO EN CONSULTA TENEMOS UNA COLECCION DE ROWS
            //QUE TIENE CADA FILA UN CAMPO (FIELD)
            List<Empleado> empleados = new List<Empleado>();

            foreach (var row in consulta)
            {
                Empleado emp = new Empleado();
                //RECUPERAMOS CADA FIELD DEL ROW
                emp.IdEmpleado = row.Field<int>("EMP_NO");
                emp.Apellido = row.Field<string>("APELLIDO");
                emp.Oficio = row.Field<string>("OFICIO");
                emp.Salario = row.Field<int>("SALARIO");
                emp.IdDepartamento = row.Field<int>("DEPT_NO");
                empleados.Add(emp);
            }

            return empleados;
        }

        //REALIZAR UN METODO PARA UN SOLO EMPLEADO
        public Empleado FindEmpleado(int idempleado)
        {
            var consulta = from datos in this.tablaemp.AsEnumerable()
                           where datos.Field<int>("EMP_NO") == idempleado
                           select datos;
            //TENEMOS UN METODO PARA RECUPERAR EL PRIMER REGISTRO
            var row = consulta.First();
            Empleado emp = new Empleado();
            emp.IdEmpleado = row.Field<int>("EMP_NO");
            emp.Apellido = row.Field<string>("APELLIDO");
            emp.Oficio = row.Field<string>("OFICIO");
            emp.Salario = row.Field<int>("SALARIO");
            emp.IdDepartamento = row.Field<int>("DEPT_NO");
            return emp;
        }

        public List<Empleado> 
            GetEmpleadosOficioSalario(string oficio, int salario)
        {
            var consulta = from datos in this.tablaemp.AsEnumerable()
                           where datos.Field<int>("SALARIO") >= salario
                           && datos.Field<string>("OFICIO") == oficio
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Empleado> empleados = new List<Empleado>();
                foreach (var row in consulta)
                {
                    Empleado empleado = new Empleado();
                    empleado.IdEmpleado = row.Field<int>("EMP_NO");
                    empleado.Apellido = row.Field<string>("APELLIDO");
                    empleado.Oficio = row.Field<string>("OFICIO");
                    empleado.Salario = row.Field<int>("SALARIO");
                    empleado.IdDepartamento = row.Field<int>("DEPT_NO");
                    empleados.Add(empleado);
                }
                return empleados;
            }
        }
    }
}
