using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.Repositorio
{
    public class rSucursal
    {
        public SqlConnection ConexionSQL()
        {
            string ConnString = ConfigurationManager.ConnectionStrings["CONEXION"].ConnectionString;
            return new SqlConnection(ConnString);
        }

        public List<mSucursal> ListaSucursales(string filtro)
        {
            List<mSucursal> objLista = null;
            SqlConnection Conn = ConexionSQL();
            using (Conn)
            {
                try
                {
                    Conn.Open();
                    SqlCommand command = new SqlCommand()
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "PA_MANT_SUCURSAL",
                        Connection = Conn
                    };
                    command.Parameters.Add("@PVCH_FILTRO", SqlDbType.VarChar, 20).Value = filtro;
                    command.Parameters.Add("@PVCH_ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult);

                    if (reader != null)
                    {
                        mSucursal modelo = null;
                        objLista = new List<mSucursal>();
                        int posIdBanco = reader.GetOrdinal("ID_BANCO");
                        int posIdsucursal = reader.GetOrdinal("ID_SUCURSAL");
                        int posNombreSucursal = reader.GetOrdinal("NOM_SUCURSAL");
                        int posNombreBanco = reader.GetOrdinal("NOM_BANCO");
                        int posDireccion = reader.GetOrdinal("DIRECCION");
                        int posFechaRegistro = reader.GetOrdinal("FEC_REGISTRO");

                        while (reader.Read())
                        {
                            modelo = new mSucursal();
                            modelo.idBanco = reader.GetInt32(posIdBanco);
                            modelo.idSucursal = reader.GetInt32(posIdsucursal);
                            modelo.NombreSucursal = reader.GetString(posNombreSucursal);
                            modelo.NombreBanco = reader.GetString(posNombreBanco);
                            modelo.Direccion = reader.GetString(posDireccion);
                            modelo.Fecha_registro = reader.GetDateTime(posFechaRegistro);
                            objLista.Add(modelo);
                        }
                    }

                    reader.Close();
                    reader.Dispose();
                    command.Dispose();
                }
                catch (SqlException Ex)
                {

                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
                return objLista;
            }
        }

        public List<mSucursal> ListaSucursalesBanco(int idBanco)
        {
            List<mSucursal> objLista = null;
            SqlConnection Conn = ConexionSQL();
            using (Conn)
            {
                try
                {
                    Conn.Open();
                    SqlCommand command = new SqlCommand()
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "PA_MANT_SUCURSAL",
                        Connection = Conn
                    };
                    command.Parameters.Add("@PINT_ID_BANCO", SqlDbType.Int).Value = idBanco;
                    command.Parameters.Add("@PVCH_ACCION", SqlDbType.VarChar, 3).Value = "SEB";
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult);

                    if (reader != null)
                    {
                        mSucursal modelo = null;
                        objLista = new List<mSucursal>();                        
                        int posIdsucursal = reader.GetOrdinal("ID_SUCURSAL");
                        int posNombreSucursal = reader.GetOrdinal("NOM_SUCURSAL");
                        int posDireccion = reader.GetOrdinal("DIRECCION");

                        while (reader.Read())
                        {
                            modelo = new mSucursal();                            
                            modelo.idSucursal = reader.GetInt32(posIdsucursal);
                            modelo.NombreSucursal = reader.GetString(posNombreSucursal);
                            modelo.Direccion = reader.GetString(posDireccion);
                            objLista.Add(modelo);
                        }
                    }

                    reader.Close();
                    reader.Dispose();
                    command.Dispose();
                }
                catch (SqlException Ex)
                {

                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
                return objLista;
            }
        }

        public bool Mantenimiento(string sAccion, mSucursal modelo, ref string sMensaje)
        {
            bool bState = false;
            SqlTransaction Trans = null;

            using (SqlConnection Conn = ConexionSQL())
            {
                Conn.Open();
                Trans = Conn.BeginTransaction();
                try
                {
                    SqlCommand command = new SqlCommand()
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "PA_MANT_SUCURSAL",
                        Connection = Conn,
                        CommandTimeout = 0
                    };
                    command.Parameters.Add("@PVCH_ACCION", SqlDbType.VarChar, 3).Value = sAccion;
                    command.Parameters.Add("@PINT_ID_BANCO", SqlDbType.Int).Value = modelo.idBanco;
                    command.Parameters.Add("@PINT_ID_SUCURSAL", SqlDbType.Int).Value = modelo.idSucursal;
                    command.Parameters.Add("@PVCH_NOMBRE", SqlDbType.VarChar, 200).Value = modelo.NombreSucursal;
                    command.Parameters.Add("@PVCH_DIRECCION", SqlDbType.VarChar, 200).Value = modelo.Direccion;
                    command.Transaction = Trans;
                    command.ExecuteNonQuery();
                    bState = true;
                    Trans.Commit();
                    command.Dispose();
                }
                catch (SqlException Ex)
                {
                    sMensaje = Ex.Message;
                    Trans.Rollback();
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                        Conn.Dispose();
                    }
                }
                return bState;
            }
        }
    }
}