using System;
using System.Data;
using MySql.Data.MySqlClient;//导入用MySql的包
using System.Configuration;
using System.Collections.Specialized;

namespace Receive.DB
{
    class TBOperation
    {
        private static NameValueCollection dbSection =
            ConfigurationManager.GetSection("DBSection") as NameValueCollection;//DB 配置项

        public int SelectLatterSeqNo(MySqlConnection conn, string type)
        {
            String maxSeqSql = "SELECT MAX(trail_seq_no) as seqNo,MAX(alarm_seq_no) AS alarmNo from gps_main.t_gps_main";
            DataSet seqData = null;
            try
            {
                seqData = DBGuid.Select(conn, maxSeqSql, "t_gps_main");
            }
            catch (Exception){}

            int latterSeq = int.Parse(Math.Pow(10, dbSection["MaxNum"].Length).ToString());
            if (type.Equals("alarm"))
            {
                latterSeq = 10000000;
            }
            if (seqData != null && seqData.Tables.Count > 0 && seqData.Tables[0].Rows.Count > 0)
            {
                DataRow seqRow = seqData.Tables[0].Rows[0];
                if (seqRow["seqNo"].ToString() != "")
                {
                    latterSeq = int.Parse(seqRow["seqNo"].ToString());
                }
                if (type.Equals("alarm"))
                {
                    if (seqRow["alarmNo"].ToString() != "")
                    {
                        latterSeq = int.Parse(seqRow["alarmNo"].ToString());
                    }

                }
            }
            return latterSeq;
        }


    }
}
