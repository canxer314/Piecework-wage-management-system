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
