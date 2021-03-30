﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Practico1ADONet
{
    class Program
    {
        static void Main(string[] args)
        {

            int opcion = -1;

            while (opcion != 6)
            {
                
                Console.WriteLine(@"|----------------------------------|");
                Console.WriteLine("|       Seleccione una opcion      |");
                Console.WriteLine(@"|----------------------------------|");
                MostrarMenu();
                Console.WriteLine(@"|----------------------------------|");

                int.TryParse(Console.ReadLine(), out opcion);
                MenuOpciones(opcion);

            }
        }

        static void MostrarMenu()
        {
            Console.WriteLine("    1 - Crear Restaurante");
            Console.WriteLine("    2 - Buscar Restaurante Por Rut");
            Console.WriteLine("    3 - Actualizar Restaurante");
            Console.WriteLine("    4 - Borrar Restaurante");
            Console.WriteLine("    5 - Restaurante y Platos por Id");
            Console.WriteLine("    6 - Salir");
        }

        // estructura switch case para opciones de menu segun input 
        static void MenuOpciones(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    AltaRestaurante();
                    break;
                case 2:
                    BuscarPorRut();
                    break;
                case 3:
                    Actualizar();
                    break;
                case 4:
                    Borrar();
                    break;
                case 5:
                    BuscarPorId();
                    break;
                default:
                    Console.Clear();
                    break;
            }
        }

        static void AltaRestaurante()
        {
            // Crear dos Restaurantes con los datos solicitados por pantalla 
            for (int i = 0; i < 2; i++)
            {
                Restaurante r = new Restaurante();
                Console.WriteLine("Ingrese el rut del Restaurante: ");
                r.Rut = Console.ReadLine();
                Console.WriteLine("Ingrese la Razon Social del Restaurante: ");
                r.RazonSocial = Console.ReadLine();
                Console.WriteLine("Ingrese la Calificacion inicial del Restaurante: ");
                r.SumaCalificacion = int.Parse(Console.ReadLine());
                r.CantidadCalificacion = 1;
                r.Guardar();
            }
            Console.ReadLine();

            List<Restaurante> lista = Restaurante.LeerTodos();
            foreach (Restaurante e in lista)
            {
                Console.WriteLine(e.RazonSocial);
            }
        }

        static void BuscarPorRut()
        {
            string rut;
            string msg = "No se encontraron excursiones con esas caracteristicas";
            Console.WriteLine("Buscar por rut del Restaurante");
            Console.WriteLine("ingresar rut:");
            rut = Console.ReadLine();
            if(rut != "")
            {
                Restaurante rest = Restaurante.LeerPorRut(rut);
                Console.WriteLine(rest.RazonSocial);
            }            
        }

        static void Actualizar()
        {
            Restaurante r = new Restaurante();
            Console.WriteLine("Ingrese el rut del Restaurante para actualizar: ");
            r.Rut = Console.ReadLine();
            Console.WriteLine("Ingrese la nueva o actual Razon Social del Restaurante: ");
            r.RazonSocial = Console.ReadLine();
            Console.WriteLine("Ingrese la Calificacion nueva o actual del Restaurante: ");
            r.SumaCalificacion = int.Parse(Console.ReadLine());
            r.Actualizar();           
        }

        static void Borrar()
        {
            string rut;
            Console.WriteLine("Borrar Restaurante con este Rut: ");
            rut = Console.ReadLine();
            if (rut != "")
            {
                Restaurante rest = Restaurante.LeerPorRut(rut); //esto me gustaria hacerlo dentro del metodo de clase pero no se.
                rest.Borrar();
                Console.WriteLine(rest.RazonSocial + " ha sido borrado");
            }
        }


        static void BuscarPorId()
        {
            string rut;
            string msg = "No se encontraron excursiones con esas caracteristicas";
            Console.WriteLine("Buscar por rut del Restaurante");
            Console.WriteLine("ingresar rut:");
            rut = Console.ReadLine();
            if (rut != "")
            {
                Restaurante rest = Restaurante.LeerPorRut(rut);
                Console.WriteLine(rest.RazonSocial);
            }
        }
    }      

        public class Restaurante
        {
        #region props
    
        public int RestauranteId { get; set; }
        public string NombreFantasia { get; set; }
        public string Rut { get; set; }
            public string RazonSocial { get; set; }
            public bool Eliminado { get; set; }
            public int SumaCalificacion { get; set; }
            public int CantidadCalificacion { get; set; }
        public virtual List<Plato> Menu { get; set; }

        public void agregarPlato(Plato p)
        {
            Menu.Add(p); // definir este metodo creo.
        }
        public Restaurante()
        {
            Menu = new List<Plato>();
        }


        #endregion props
        public int Guardar()
            {
                string config = @"Server=(localdb)\ProjectsV13;DataBase=Fameliques;Integrated Security=true"; 
                //check nombre de servidor, base de datos y usuario de Sqlserver
                SqlConnection con = new SqlConnection(config); //configurar la conexion
                int afectadas = 0;
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {// aca entra cuando le paso la info de consola y hago el llamado

                        cmd.Connection = con;//asignar conexion al commando a ejecutar 

                        cmd.CommandText = "Restaurantes_Insert";//Sentencia a ejecutar, nombre storedProcedure 

                        cmd.CommandType = CommandType.StoredProcedure;//Tipo de query 
                                                                      // agregamos parametros 
                        cmd.Parameters.Add(new SqlParameter("@Rut", this.Rut));
                        cmd.Parameters.Add(new SqlParameter("@RazonSocial", this.RazonSocial));

                        cmd.Parameters.Add(new SqlParameter("@Eliminado", this.Eliminado));
                        cmd.Parameters.Add(new SqlParameter("@SumaCalificacion", this.SumaCalificacion));
                        cmd.Parameters.Add(new SqlParameter("@CantidadCalificacion", this.CantidadCalificacion));
                        con.Open();//abrimos la conexion 
                        afectadas = cmd.ExecuteNonQuery();//ejecutamos consulta 
                        con.Close(); //cerramos conexion
                    }// fin using
                }
                catch (SqlException ex)
                {
                    //loguear excepcion
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.InnerException);
                    throw (new Exception("error en acceso a datos",ex));
                }
                finally
                {
                }
                return afectadas;
            }

        public int GuardarConPlato()
        {
            string config = @"Server=(localdb)\ProjectsV13;DataBase=Fameliques;Integrated Security=true";
            //check nombre de servidor, base de datos y usuario de Sqlserver
            SqlConnection con = new SqlConnection(config); //configurar la conexion
            SqlTransaction trn = null;
            int afectadas = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {// aca entra cuando le paso la info de consola y hago el llamado

                    cmd.Connection = con;//asignar conexion al commando a ejecutar 

                    cmd.CommandText = "Restaurantes_Insert";//Sentencia a ejecutar, nombre storedProcedure 

                    cmd.CommandType = CommandType.StoredProcedure;//Tipo de query 
                                                                  // agregamos parametros 
                    cmd.Parameters.Add(new SqlParameter("@Rut", this.Rut));
                    cmd.Parameters.Add(new SqlParameter("@RazonSocial", this.RazonSocial));

                    cmd.Parameters.Add(new SqlParameter("@Eliminado", this.Eliminado));
                    cmd.Parameters.Add(new SqlParameter("@SumaCalificacion", this.SumaCalificacion));
                    cmd.Parameters.Add(new SqlParameter("@CantidadCalificacion", this.CantidadCalificacion));
                    con.Open();//abrimos la conexion 
                    trn = con.BeginTransaction(); // iniciamos la transaccion
                    cmd.Transaction = trn; //la asociamos al comando
                    //usamos executescaler ya que nos devuelve el id generado
                    this.RestauranteId = (int)cmd.ExecuteScalar();//casteamos retorno
                    //sobreescribimos la consula a ejecutar para reutilizar el objeto command
                    cmd.CommandText = "Platos Insert";
                    foreach (Plato p in this.Menu)
                    { //recorremos los platos asociados 
                        cmd.Parameters.Clear(); //limpiamos parámetros de la consulta anterior 
                        cmd.Parameters.Add(new SqlParameter("@Nombre", p.Descripcion));
                        cmd.Parameters.Add(new SqlParameter("@Descripcion", p.Descripcion));
                        cmd.Parameters.Add(new SqlParameter("@Precio", p.Precio));
                        cmd.Parameters.Add(new SqlParameter("@RestauranteId", this.RestauranteId));
                        afectadas = cmd.ExecuteNonQuery();//ejecutamos consulta 
                    }
                    trn.Commit(); // confirmamos la transaccion y las modificaciones  
                }// fin using
            }
            catch (SqlException ex)
            {
                //si hay error deshacemos los cambios y volvemos a la situacion inicial
                trn.Rollback();
            }
            finally
            {
                if(con.State == ConnectionState.Open)
                    con.Close(); //cerramos conexion
            }
            return afectadas;
        }




        public static List<Restaurante> LeerTodos()
            {
                List<Restaurante> lst = new List<Restaurante>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure; //indico que voy a ejecutar un procedimiento almacenado en la bd 
                cmd.CommandText = "Restaurantes_SelectAll"; //indico el nombre del procedimiento almacenado a ejecutar 
                string sConnectionString = @"Server=(localdb)\ProjectsV13;DataBase=Fameliques;Integrated Security=true";
                SqlConnection conn = new SqlConnection(sConnectionString);
                SqlDataReader drResults;
                cmd.Connection = conn;
                conn.Open();
                drResults = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (drResults.Read())
                {
                    Restaurante r = new Restaurante();
                    r.Rut = drResults["Rut"].ToString();
                    r.RazonSocial = drResults["RazonSocial"].ToString();
                    lst.Add(r);
                }
                drResults.Close();
                conn.Close();
                return lst;
            }

            public static Restaurante LeerPorRut(string rut)
            {
                Restaurante rest = new Restaurante();
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Add(new SqlParameter("@Rut", rut));
                cmd.CommandType = CommandType.StoredProcedure; 
                //indico que voy a ejecutar un procedimiento almacenado en la bd 
                cmd.CommandText = "Restaurantes_SelectByRut"; 
                //indico el nombre del procedimiento almacenado a ejecutar 
                string sConnectionString = @"Server=(localdb)\ProjectsV13;DataBase=Fameliques;Integrated Security=true";
                SqlConnection conn = new SqlConnection(sConnectionString);
                SqlDataReader drResults;
                cmd.Connection = conn;
                conn.Open();
                drResults = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (drResults.Read())
                {                
                    Restaurante r = new Restaurante();
                    r.Rut = drResults["Rut"].ToString();
                    r.RazonSocial = drResults["RazonSocial"].ToString();
                    rest = r;
                }
                drResults.Close();
                conn.Close();
                return rest;
            }
        

        public int Actualizar()
        {
            string config = @"Server=(localdb)\ProjectsV13;DataBase=Fameliques;Integrated Security=true";
            //check nombre de servidor, base de datos y usuario de Sqlserver
            SqlConnection con = new SqlConnection(config); //configurar la conexion

            int afectadas = 0;// que es afectadas???
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {// aca entra cuando le paso la info de consola y hago el llamado

                    cmd.Connection = con;//asignar conexion al commando a ejecutar 
                    cmd.CommandText = "Restaurantes_Update";//Sentencia a ejecutar, nombre storedProcedure 
                    cmd.CommandType = CommandType.StoredProcedure;//Tipo de query 
                    // agregamos parametros  
                    cmd.Parameters.Add(new SqlParameter("@Rut", this.Rut));
                    cmd.Parameters.Add(new SqlParameter("@RazonSocial", this.RazonSocial));
                    cmd.Parameters.Add(new SqlParameter("@Eliminado", this.Eliminado));  // esta no se la paso capaz
                    cmd.Parameters.Add(new SqlParameter("@SumaCalificacion", this.SumaCalificacion));
                    cmd.Parameters.Add(new SqlParameter("@CantidadCalificacion", this.CantidadCalificacion));
                    con.Open();//abrimos la conexion 
                    afectadas = cmd.ExecuteNonQuery();//ejecutamos consulta  // que es afectadas???
                    con.Close(); //cerramos conexion
                }// fin using
            }
            catch (SqlException ex)
            {
                //loguear excepcion
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                throw (new Exception("error en acceso a datos", ex));
            }
            finally
            {
            }
            return afectadas;
        }

        public void Borrar()
        {
            //ExecuteNonQuery
            string config = @"Server=(localdb)\ProjectsV13;DataBase=Fameliques;Integrated Security=true";
            //check nombre de servidor, base de datos y usuario de Sqlserver
            SqlConnection con = new SqlConnection(config); //configurar la conexion
            int afectadas = 0;// que es afectadas???
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;//asignar conexion al commando a ejecutar 
                    cmd.CommandText = "Restaurantes_Delete";//Sentencia a ejecutar, nombre storedProcedure 
                    cmd.CommandType = CommandType.StoredProcedure;//Tipo de query 
                    cmd.Parameters.Add(new SqlParameter("@rut", this.Rut));
                    con.Open();//abrimos la conexion 
                    afectadas = cmd.ExecuteNonQuery();//ejecutamos consulta  // que es afectadas???
                    con.Close(); //cerramos conexion
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                throw (new Exception("error en acceso a datos", ex));
            }
            finally
            {

            }

        }

        public static Restaurante BuscarPorId(string rut)
        {
            Restaurante rest = new Restaurante();
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add(new SqlParameter("@Rut", rut));
            cmd.CommandType = CommandType.StoredProcedure;
            //indico que voy a ejecutar un procedimiento almacenado en la bd 
            cmd.CommandText = "Restaurantes_SelectById";
            //indico el nombre del procedimiento almacenado a ejecutar 
            string sConnectionString = @"Server=(localdb)\ProjectsV13;DataBase=Fameliques;Integrated Security=true";
            SqlConnection conn = new SqlConnection(sConnectionString);
            SqlDataReader drResults;
            cmd.Connection = conn;
            conn.Open();
            drResults = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (drResults.Read())
            {
                Restaurante r = new Restaurante();
                r.Rut = drResults["Rut"].ToString();
                r.RazonSocial = drResults["RazonSocial"].ToString();
                rest = r;
            }
            drResults.Close();
            conn.Close();
            return rest;
        }
    }

    public class Plato
    {
        public int PlatoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public virtual Restaurante ElRestaurante { get; set; }
    }

}




