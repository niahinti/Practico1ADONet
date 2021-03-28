using System;
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
            Console.WriteLine("    3 - Borrar Restaurante");
            Console.WriteLine("    4 - Actualizar Restaurante");
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
                    Borrar();
                    break;
                case 4:
                    Actualizar();
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
                r.Rut = Console.ReadLine(); Console.WriteLine("Ingrese la Razon Social del Restaurante: ");
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
        }

        static void Borrar()
        {
        }

        static void Actualizar()
        {
        }




    }




        public class Restaurante
        {
            public int RestauranteId { get; set; }
            public string Rut { get; set; }
            public string RazonSocial { get; set; }
            public bool Eliminado { get; set; }
            public int SumaCalificacion { get; set; }
            public int CantidadCalificacion { get; set; }
            
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

            public static List<Restaurante> LeerPorRut(string rut)
            {
                List<Restaurante> lst = new List<Restaurante>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure; 
                //indico que voy a ejecutar un procedimiento almacenado en la bd 
                cmd.CommandText = "Restaurantes_SelectByRut"; 
                //indico el nombre del procedimiento almacenado a ejecutar 
                string sConnectionString = @"Server=(localdb)\ProjectsV13;DataBase=Fameliques;Integrated Security=true";
                SqlConnection conn = new SqlConnection(sConnectionString);
                SqlDataReader drResults;
                cmd.Connection = conn;
                //cmd.Parameters.Add(new SqlParameter("@Rut", r.Rut));
                conn.Open();
                drResults = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (drResults.Read())
                {

                string drRut = drResults["Rut"].ToString();
                if (drRut == rut)
                {
                    Restaurante r = new Restaurante();
                    r.Rut = drRut;
                    r.RazonSocial = drResults["RazonSocial"].ToString();
                    lst.Add(r);
                }
                   
                }
                drResults.Close();
                conn.Close();
                return lst;
            }
        }
        
        
    }




