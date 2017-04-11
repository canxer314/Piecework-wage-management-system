using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piecework_wage_management_system
{
    class DataAccessLayer
    {
        public readonly string mysqlconnectionString =
                 @"server=127.0.0.1;database=gradulation_design_db;uid=root;pwd=;charset='gbk'";

        //获取MySql的连接数据库对象。MySqlConnection
        public MySqlConnection OpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(mysqlconnectionString);
            connection.Open();
            //try
            //{
            //    connection.Open();
            //}
            //catch(Exception e)
            //{
            //    SqlConnection myCon=new SqlConnection("server=.\\szy;database=master;uid=sa;PWD=11");
            //    myCon.Open();
            //    SqlCommand myCmd = new SqlCommand("select * from sys.databases where name='SZY'",myCon);
            //    object n = myCmd.ExecuteScalar();
  
            //    if (n!=null)
            //    {
            //        MessageBox.Show("数据库szy 存在");
            //    }
            //    else
            //    {
            //        MessageBox.Show("数据库szy 不存在");
            //    }
            //    myCon.Close();
            //}

            ////检测数据库是否存在
            //    connection = new MySqlConnection("Data Source=localhost;Persist Security Info=yes;UserId=root; PWD=;");  
            //    MySqlCommand cmd = new MySqlCommand("CREATE DATABASE 你的数据库名;", conn );  
            //    conn.Open();            
            //    cmd.ExecuteNonQuery();  
            //    conn.Close();  

            ////检测数据库表是否存在
            //string sqlStr = "select count(*) from sysobjects where id = object_id('数据库名.Owner.表名')";
            //IDbConnection conn = connection;
            //if (conn.Execute(sqlStr) == 1)
            //else
            return connection;
        }

        //插入Employee对象
        public int InsertEmployee(Employee e)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into Employee values "
                     + "(@EName, @EID, @EPasswd, @Dept, @WorkShop, @Job, @Authority, @Tel)",
                     new { EName = e.Name, EID=e.EmployeeID, EPasswd=e.Password, Dept=e.Department,
                         WorkShop =e.Workshop, Job=e.Job, Authority=e.Authority, Tel=e.Telephone});
            }
        }

        //获取所有Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from Employee order by EID desc";
                return conn.Query<Employee>(query,null);
            }
        }

        //根据EName获取所有Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByEName(string eName)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from Employee where EName=@EName", new { EName = eName });
            }
        }

        //根据EID获取Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByEID(short eID)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from Employee where EID=@EID", new { EID = eID });
            }
        }

        //根据Dept获取Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByDept(string dept)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from Employee where Dept=@Dept", new { Dept = dept });
            }
        }

        //根据WorkShop获取Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByWorkShop(string workShop)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from Employee where WorkShop=@WorkShop", new { WorkShop = workShop });
            }
        }

        //根据Job获取Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByJob(string job)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from Employee where Job=@Job", new { Job = job });
            }
        }

        //根据Tel取Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByTel(string tel)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from Employee where Tel=@Tel", new { Tel = tel });
            }
        }
    }
}
