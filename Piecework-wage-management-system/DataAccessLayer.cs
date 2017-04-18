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

        //初始化数据库
        public bool DataBaseInit()
        {
            MySqlConnection conn = new MySqlConnection("Data Source=localhost;Persist Security Info=yes;UserId=root; PWD=;");  
            MySqlCommand cmd = new MySqlCommand(@"
            CREATE DATABASE IF NOT EXISTS gradulation_design_db CHARACTER SET GBK;
            USE gradulation_design_db;
            CREATE TABLE IF NOT EXISTS tbl_Administrator (
                Id SMALLINT AUTO_INCREMENT PRIMARY KEY,
                Name CHAR(8),
                Password CHAR(16),
                Authority TINYINT
            );
            CREATE TABLE IF NOT EXISTS tbl_Employee (
                Id SMALLINT UNSIGNED PRIMARY KEY,
                Name CHAR(8),
                PY_Abbr VARCHAR(8),
                Password CHAR(16),
                Dept VARCHAR(16),
                WorkShop VARCHAR(16),
                Job VARCHAR(16),
                Authority TINYINT,
                Tel CHAR(13)
            );
            CREATE TABLE IF NOT EXISTS tbl_Product (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                Name CHAR(16) UNIQUE KEY,
                PY_Abbr VARCHAR(16)
            );
            CREATE TABLE IF NOT EXISTS tbl_Procedure (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                Name CHAR(16) UNIQUE KEY,
                PY_Abbr VARCHAR(16),
                Sequence SMALLINT,
                Product_Id INT,
                CONSTRAINT fk_Product FOREIGN KEY (Product_Id)
                    REFERENCES tbl_Product (Id)
            );
            CREATE TABLE IF NOT EXISTS tbl_Value (
                Name CHAR(8) PRIMARY KEY,
                PY_Abbr VARCHAR(8),
                Unit_Price SMALLINT,
                Procedure_Id INT,
                CONSTRAINT fk_Procedure FOREIGN KEY (Procedure_Id)
                    REFERENCES tbl_Procedure (Id)
            );
            ", conn);
            try
            {
                conn.Open();            
            }
            catch(MySqlException e)
            {
                return false;
            }
            cmd.ExecuteNonQuery();
            conn.Close();
            return true;
        }

        //获取MySql的连接数据库对象。MySqlConnection
        public MySqlConnection OpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(mysqlconnectionString);
            connection.Open();
            return connection;
        }

        //插入Administrator对象
        public int InsertAdministrator(Administrator a)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Administrator values "
                     + "(@Id, @Name, @Password, @Authority)",
                     new { Id = a.AdministratorId, Name = a.Name, Password = a.Password, Authority = a.Authority });
            }
        }

        //获取所有Employee对象的集合
        public IEnumerable<Administrator> QueryAdministratorByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from tbl_Administrator order by Id desc";
                return conn.Query<Administrator>(query,null);
            }
        }

        //插入Employee对象
        public int InsertEmployee(Employee e)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Employee values "
                     + "(@Id, @Name, @Password, @Dept, @WorkShop, @Job, @Authority, @Tel)",
                     new { Id = e.EmployeeId, Name=e.Name, Password=e.Password, Dept=e.Department,
                         WorkShop =e.Workshop, Job=e.Job, Authority=e.Authority, Tel=e.Telephone});
            }
        }

        //获取所有Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from tbl_Employee order by Id desc";
                return conn.Query<Employee>(query,null);
            }
        }

        //根据Name获取所有Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByEName(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from tbl_Employee where Name=@Name", new { Name = name });
            }
        }

        //根据EmployeeID获取Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByEID(short employeeId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from tbl_Employee where Id=@EmployeeId", new { EmployeeId=employeeId });
            }
        }

        //根据Dept获取Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByDept(string dept)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from tbl_Employee where Dept=@Dept", new { Dept = dept });
            }
        }

        //根据WorkShop获取Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByWorkShop(string workShop)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from tbl_Employee where WorkShop=@WorkShop", new { WorkShop = workShop });
            }
        }

        //根据Job获取Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByJob(string job)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from tbl_Employee where Job=@Job", new { Job = job });
            }
        }

        //根据Tel取Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByTel(string tel)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from tbl_Employee where Tel=@Tel", new { Tel = tel });
            }
        }
    }
}
