using System.Data;
using MySql.Data.MySqlClient;
using System;
using Common;
using System.Threading;
namespace Receive.DB
{
    public static class DBGuid
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static object lockObj = new object();

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="conn"></param> 连接
        /// <param name="sql"></param>  查询语句
        /// <param name="tableName"></param> 查询的表名
        /// <returns></returns>
        public static DataSet Select(MySqlConnection conn, string sql, string tableName)
        {
            MySqlDataAdapter sda = null;
            DataSet dt = null;
            try
            {
                sda = new MySqlDataAdapter(sql, conn);
                dt = new DataSet();
                sda.Fill(dt, tableName);
                //conn.Close();
                return dt;
            }
            catch (MySqlException e)
            {
                logger.Error(e.Message + " sql: " + sql);
                throw e;
            }
        }

        /// <summary>
        /// 建立执行命令语句对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static MySqlCommand GetSqlCommand(MySqlConnection conn, string sql)
        {

            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            return mySqlCommand;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="mySqlCommand"></param>
        public static void Insert(MySqlConnection conn, string sql)
        {
            try
            {
                MySqlCommand mySqlCommand = GetSqlCommand(conn, sql);

                mySqlCommand.ExecuteNonQuery();
            }
            catch (OutOfMemoryException e)
            {
                logger.Error(e.Message+" sql: "+sql);
            }
            catch (Exception ex)
            {
                logger.Error(ex.GetType().ToString() + " " + ex.Message + " sql: " + sql);
            }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="mySqlCommand"></param>
        public static void Update(MySqlConnection conn, string sql)
        {
            try
            {
                MySqlCommand mySqlCommand = GetSqlCommand(conn, sql);
                mySqlCommand.ExecuteNonQuery();
            }
            catch (OutOfMemoryException e)
            {
                logger.Error(e.Message + " sql: " + sql);
            }
            catch (Exception ex)
            {
                logger.Error(ex.GetType().ToString() + " " + ex.Message + " sql: " + sql);
                throw ex;
            }
        }

        public static void Delete(MySqlConnection conn, string sql)
        {
            try
            {
                MySqlCommand mySqlCommand = GetSqlCommand(conn, sql);

                mySqlCommand.ExecuteNonQuery();
            }
            catch (OutOfMemoryException e)
            {
                logger.Error(e.Message + " sql: " + sql);
            }
            catch (Exception ex)
            {
                logger.Error(ex.GetType().ToString() + " " + ex.Message + " sql: " + sql);
            }
        }

    }
}
