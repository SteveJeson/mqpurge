using ChatServer.Codec;
using ChatServer.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using ChatServer.AnalyzeSeqnum;
using MySql.Data.MySqlClient;//导入用MySql的包
using System.Data;
using Receive.DB;
using DB.ObjectPool;
using System.Web.Configuration;
using System.Configuration;
using System.Collections.Specialized;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Collections;
using System.Threading.Tasks;

namespace Receive
{
    class Program
    {
        private static readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static ConnectionFactory factory = new ConnectionFactory();//mq 连接工厂

        private static NameValueCollection mqSec =
            ConfigurationManager.GetSection("MqSection") as NameValueCollection;//mq 配置项

        static void Main(string[] args)
        {
            try
            {
                List<IConnection> connections = GetRabbitMqConnection();
                foreach (var connection in connections)
                {
                    var channel = connection.CreateModel();               
                    var GPSQueueName = mqSec["GPSQueue"];//消息队列名
                    int GPSQueueCount = int.Parse(mqSec["GPSQueueCount"]);
                    var alarmQueueName = mqSec["AlarmQueue"];
                    int alarmQueueCount = int.Parse(mqSec["AlarmQueueCount"]);
                    for (int i = 1; i <= GPSQueueCount; i++)
                    {
                        string qName = GPSQueueName + "_" + i;
                        channel.QueuePurge(qName);
                        
                        Console.WriteLine("{0} purged!", qName);
                    }

                    for (int i = 1; i <= alarmQueueCount; i++)
                    {
                        string qName = alarmQueueName + "_" + i;
                        channel.QueuePurge(qName);
                        Console.WriteLine("{0} purged!", qName);
                    }
                }
                

            }
            catch (Exception e)
            {
                logger.Error(e.GetType() + ": " + e.Message);
            }

        }

        /// <summary>
        /// 获取rabbitmq server连接
        /// </summary>
        /// <returns></returns>
        private static List<IConnection> GetRabbitMqConnection()
        {
            try
            {
                List<IConnection> connList = new List<IConnection>();
                //从App.config获取RabbiMq配置参数
                String hostNames = mqSec["HostName"];
                String[] hostArr = hostNames.Split(',');
                foreach (var ele in hostArr)
                {
                    factory.HostName = ele;//主机
                    factory.UserName = mqSec["UserName"];//用户名
                    factory.Password = mqSec["Password"];//密码
                    factory.Port = int.Parse(mqSec["Port"]);//端口
                    
                    factory.AutomaticRecoveryEnabled = true;//允许自动恢复
                    factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);//网络恢复间隔
                    factory.RequestedHeartbeat = 30;//心跳
                    IConnection connection = factory.CreateConnection();
                    Console.WriteLine("connect to "+ ele);
                    connList.Add(connection);
                }
                
                return connList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                logger.Error(e.Message);
                return null;
            }
        }
    }
}
