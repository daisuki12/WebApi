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
    public class rOrdenPago
    {
        public SqlConnection ConexionSQL()
        {
            string ConnString = ConfigurationManager.ConnectionStrings["CONEXION"].ConnectionString;
            return new SqlConnection(ConnString);
        }

        public List<mOrdenPago> ListaOrdenes(string filtro)
        {
            List<mOrdenPago> objLista = null;
            SqlConnection Conn = ConexionSQL();
            using (Conn)
            {
                try
                {
                    Conn.Open();
                    SqlCommand command = new SqlCommand()
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "PA_MANT_ORDEN",
                        Connection = Conn
                    };
                    command.Parameters.Add("@PVCH_FILTRO", SqlDbType.VarChar, 20).Value = filtro;
                    command.Parameters.Add("@PVCH_ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult);

                    if (reader != null)
                    {
                        mOrdenPago modelo = null;
                        objLista = new List<mOrdenPago>();
                        int posIdBanco = reader.GetOrdinal("ID_BANCO");
                        int posIdsucursal = reader.GetOrdinal("ID_SUCURSAL");
                        int posNRO_ORDEN_PAGO = reader.GetOrdinal("NRO_ORDEN_PAGO");
                        int posNombreSucursal = reader.GetOrdinal("NOM_SUCURSAL");
                        int posNombreBanco = reader.GetOrdinal("NOM_BANCO");
                        int posMonto = reader.GetOrdinal("MONTO");
                        int posMoneda = reader.GetOrdinal("MONEDA");
                        int posEstado = reader.GetOrdinal("ESTADO");
                        int posFecPago = reader.GetOrdinal("FEC_PAGO");

                        while (reader.Read())
                        {
                            modelo = new mOrdenPago();
                            modelo.idBanco = reader.GetInt32(posIdBanco);
                            modelo.idSucursal = reader.GetInt32(posIdsucursal);
                            modelo.idOrdenPago = reader.GetInt32(posNRO_ORDEN_PAGO);
                            modelo.nomSucursal = reader.GetString(posNombreSucursal);
                            modelo.nomBanco = reader.GetString(posNombreBanco);
                            modelo.moneda = reader.GetString(posMoneda);
                            modelo.estado = reader.GetString(posEstado);
                            modelo.monto = reader.GetDecimal(posMonto);
                            modelo.fecPago = reader.GetString(posFecPago);
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

        public List<mOrdenPago> ListaOrdenesSucursal(int idBanco, int idSucursal, string moneda)
        {
            List<mOrdenPago> objLista = null;
            SqlConnection Conn = ConexionSQL();
            using (Conn)
            {
                try
                {
                    Conn.Open();
                    SqlCommand command = new SqlCommand()
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "PA_MANT_ORDEN",
                        Connection = Conn
                    };
                    command.Parameters.Add("@PINT_ID_BANCO", SqlDbType.VarChar, 20).Value = idBanco;
                    command.Parameters.Add("@PINT_ID_SUCURSAL", SqlDbType.VarChar, 20).Value = idSucursal;
                    command.Parameters.Add("@PVCH_MONEDA", SqlDbType.VarChar, 20).Value = moneda;
                    command.Parameters.Add("@PVCH_ACCION", SqlDbType.VarChar, 3).Value = "SEA";
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult);

                    if (reader != null)
                    {
                        mOrdenPago modelo = null;
                        objLista = new List<mOrdenPago>();
                        int posIdBanco = reader.GetOrdinal("ID_BANCO");
                        int posIdsucursal = reader.GetOrdinal("ID_SUCURSAL");
                        int posNRO_ORDEN_PAGO = reader.GetOrdinal("NRO_ORDEN_PAGO");
                        int posNombreSucursal = reader.GetOrdinal("NOM_SUCURSAL");
                        int posNombreBanco = reader.GetOrdinal("NOM_BANCO");
                        int posMonto = reader.GetOrdinal("MONTO");
                        int posMoneda = reader.GetOrdinal("MONEDA");
                        int posEstado = reader.GetOrdinal("ESTADO");
                        int posFecPago = reader.GetOrdinal("FEC_PAGO");

                        while (reader.Read())
                        {
                            modelo = new mOrdenPago();
                            modelo.idBanco = reader.GetInt32(posIdBanco);
                            modelo.idSucursal = reader.GetInt32(posIdsucursal);
                            modelo.idOrdenPago = reader.GetInt32(posNRO_ORDEN_PAGO);
                            modelo.nomSucursal = reader.GetString(posNombreSucursal);
                            modelo.nomBanco = reader.GetString(posNombreBanco);
                            modelo.moneda = reader.GetString(posMoneda);
                            modelo.estado = reader.GetString(posEstado);
                            modelo.monto = reader.GetDecimal(posMonto);
                            modelo.fecPago = reader.GetString(posFecPago);
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

        public bool Mantenimiento(string sAccion, mOrdenPago modelo, ref string sMensaje)
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
                        CommandText = "PA_MANT_ORDEN",
                        Connection = Conn,
                        CommandTimeout = 0
                    };
                    command.Parameters.Add("@PVCH_ACCION", SqlDbType.VarChar, 3).Value = sAccion;
                    command.Parameters.Add("@PINT_ID_BANCO", SqlDbType.Int).Value = modelo.idBanco;
                    command.Parameters.Add("@PINT_ID_SUCURSAL", SqlDbType.Int).Value = modelo.idSucursal;
                    command.Parameters.Add("@PINT_NRO_ORDEN_PAGO", SqlDbType.Int).Value = modelo.idOrdenPago;
                    command.Parameters.Add("@PDCM_MONTO", SqlDbType.Decimal).Value = modelo.monto;
                    command.Parameters.Add("@PVCH_MONEDA", SqlDbType.VarChar, 1).Value = modelo.moneda;
                    command.Parameters.Add("@PVCH_ESTADO", SqlDbType.VarChar, 1).Value = modelo.estado;
                    command.Parameters.Add("@PDTM_FEC_PAGO", SqlDbType.DateTime).Value = modelo.fecPago;
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