using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;//导入用MySql的包
using System.Data;
using Common;
namespace DB.ObjectPool
{
    public sealed class DBConnectionSingletion : ObjectPool
    {
        private DBConnectionSingletion() { }

        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly DBConnectionSingletion Instance =
            new DBConnectionSingletion();

        private static string connectionString = "";

        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        protected override object Create()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        protected override bool Validate(object o)
        {
            try
            {
                MySqlConnection conn = (MySqlConnection)o;
                return !conn.State.Equals(ConnectionState.Closed);
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        protected override void Expire(object o)
        {
            try
            {
                MySqlConnection conn = (MySqlConnection)o;
                conn.Close();
            }
            catch (MySqlException) { }
        }

        public MySqlConnection BorrowDBConnection()
        {
            try
            {
                return (MySqlConnection)base.GetObjectFromPool();
            }
            catch (Exception e)
            {
                logger.Error("<<"+e.GetType().ToString()+">> "+e.Message);
                return null;         
            }
        }

        public void ReturnDBConnection(MySqlConnection conn)
        {
            base.ReturnObjectToPool(conn);
        }
    }
}