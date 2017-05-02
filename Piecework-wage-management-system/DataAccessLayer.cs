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
    public class DataAccessLayer
    {
        public readonly string mysqlconnectionString =
                 @"server=127.0.0.1;database=gradulation_design_db;uid=root;pwd=abcd709394;charset='gbk'";

        //初始化数据库
        public bool DataBaseInit()
        {
            MySqlConnection conn = new MySqlConnection("Data Source=localhost;Persist Security Info=yes;UserId=root; PWD=abcd709394;");
            MySqlCommand cmd = new MySqlCommand(@"
            CREATE DATABASE IF NOT EXISTS gradulation_design_db CHARACTER SET GBK;
            USE gradulation_design_db;
            CREATE TABLE IF NOT EXISTS tbl_Administrator (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                Name CHAR(20) UNIQUE KEY,
                Password CHAR(20)
            );
            CREATE TABLE IF NOT EXISTS tbl_Workshop (
                Id INT PRIMARY KEY,
                Name CHAR(20) UNIQUE KEY
            );
            CREATE TABLE IF NOT EXISTS tbl_Job (
                Id INT PRIMARY KEY,
                Name CHAR(20) UNIQUE KEY
            );
            CREATE TABLE IF NOT EXISTS tbl_Employee (
                Id INT PRIMARY KEY,
                Name CHAR(20),
                Password CHAR(20),
                Gender VARCHAR(20),
                Workshop CHAR(20),
                Job CHAR(20),
                CONSTRAINT fk_Workshop FOREIGN KEY (Workshop)
                    REFERENCES tbl_Workshop (Name),
                CONSTRAINT fk_Job FOREIGN KEY (Job)
                    REFERENCES tbl_Job (Name),
                Telephone CHAR(13)
            );
            CREATE TABLE IF NOT EXISTS tbl_Product (
                Id INT PRIMARY KEY,
                Name CHAR(20) UNIQUE KEY
            );
            CREATE TABLE IF NOT EXISTS tbl_Procedure (
                Id INT PRIMARY KEY,
                Name CHAR(20) UNIQUE KEY,
                Sequence SMALLINT,
                Product_Id INT,
                CONSTRAINT fk_Product FOREIGN KEY (Product_Id)
                    REFERENCES tbl_Product (Id)
            );
            CREATE TABLE IF NOT EXISTS tbl_Value (
                Name CHAR(20) PRIMARY KEY,
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
            catch
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
        public int InsertAdministrator(Administrator admin)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Administrator values "
                     + "(@Id, @Name, @Password)",
                     new { Id = admin.Id, Name = admin.Name, Password = admin.Password });
            }
        }

        //获取所有Employee对象的集合
        public IEnumerable<Administrator> QueryAdministratorByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from tbl_Administrator order by Id asc";
                return conn.Query<Administrator>(query, null);
            }
        }
        public IEnumerable<Administrator> QueryAdministratorByName(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Administrator>("select * from tbl_Administrator where Name=@Name", new { Name = name });
            }
        }
        public int UpdateAdministratorPasswordById(int id, string password)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("update tbl_Administrator set Password=@Password where Id=@Id", new { Password = password, Id = id });
            }
        }
        public int DeleteAdministratorById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Administrator where Id=@Id", new { Id = id });
            }
        }
        //插入Employee对象
        public int InsertEmployee(Employee e)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Employee values "
                     + "(@Id, @Name, @Password, @Gender, @WorkShop, @Job, @Telephone)",
                     new
                     {
                         Id = e.Id,
                         Name = e.Name,
                         Password = e.Password,
                         Gender = e.Gender,
                         WorkShop = e.Workshop,
                         Job = e.Job,
                         Telephone = e.Telephone
                     });
            }
        }

        //获取所有Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from tbl_Employee order by Id asc";
                return conn.Query<Employee>(query, null);
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
        public IEnumerable<Employee> QueryEmployeeByEID(int Id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from tbl_Employee where Id=@Id", new { Id = Id });
            }
        }

        //根据Dept获取Employee对象的集合
        public IEnumerable<Employee> QueryEmployeeByGender(string gender)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from tbl_Employee where Gender=@Gender", new { Gender = gender });
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
        public IEnumerable<Employee> QueryEmployeeByTel(string telephone)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Employee>("select * from tbl_Employee where Telephone=@Telephone", new { Telephone = telephone });
            }
        }
        public int UpdateEmployeePasswordById(int id, string password)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("update tbl_Employee set Password=@Password where Id=@Id", new { Password = password, Id = id });
            }
        }
        public int DeleteEmployeeById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Employee where Id=@Id", new { Id = id });
            }
        }
        public int InsertWorkshop(Workshop workshop)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Workshop values "
                     + "(@Id, @Name)",
                     new { Id = workshop.Id, Name = workshop.Name });
            }
        }

        //获取所有Workshop对象的集合
        public IEnumerable<Workshop> QueryWorkshopByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from tbl_Workshop order by Id asc";
                return conn.Query<Workshop>(query, null);
            }
        }
        public IEnumerable<Workshop> QueryWorkshopByName(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Workshop>("select * from tbl_Workshop where Name=@Name", new { Name = name });
            }
        }
        public IEnumerable<Workshop> QueryWorkshopById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Workshop>("select * from tbl_Workshop where Id=@Id", new { Id = id });
            }
        }
        public int DeleteWorkshopById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Workshop where Id=@Id", new { Id = id });
            }
        }
        public int InsertJob(Job job)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Job values "
                     + "(@Id, @Name)",
                     new { Id = job.Id, Name = job.Name });
            }
        }

        //获取所有Job对象的集合
        public IEnumerable<Job> QueryJobByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from tbl_Job order by Id asc";
                return conn.Query<Job>(query, null);
            }
        }
        public IEnumerable<Job> QueryJobByName(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Job>("select * from tbl_Job where Name=@Name", new { Name = name });
            }
        }
        public IEnumerable<Job> QueryJobById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Job>("select * from tbl_Job where Id=@Id", new { Id = id });
            }
        }
        public int DeleteJobById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Job where Id=@Id", new { Id = id });
            }
        }
    }
}
