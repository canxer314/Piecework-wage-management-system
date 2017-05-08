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
                    REFERENCES tbl_Workshop (Name) ON DELETE SET NULL ON UPDATE SET NULL,
                CONSTRAINT fk_Job FOREIGN KEY (Job)
                    REFERENCES tbl_Job (Name) ON DELETE SET NULL ON UPDATE SET NULL,
                Telephone CHAR(20)
            );
            CREATE TABLE IF NOT EXISTS tbl_Product (
                Id INT PRIMARY KEY,
                Name CHAR(20) UNIQUE KEY
            );
            CREATE TABLE IF NOT EXISTS tbl_Procedure (
                Id INT PRIMARY KEY,
                Name CHAR(20) UNIQUE KEY,
                Product_Id INT,
                CONSTRAINT fk_Product FOREIGN KEY (Product_Id)
                    REFERENCES tbl_Product (Id) ON DELETE CASCADE ON UPDATE CASCADE
            );
            CREATE TABLE IF NOT EXISTS tbl_Relationship
            (
            	Id INT AUTO_INCREMENT PRIMARY KEY,
            	Sequence_Number INT,
            	Procedure_Name char(20) UNIQUE KEY,
            	CONSTRAINT fk_Procedure_Name FOREIGN KEY (Procedure_Name)
            		REFERENCES tbl_Procedure(Name) ON DELETE CASCADE ON UPDATE CASCADE,
            	Input_Output_Ratio INT,
            	Product_Id INT,
            	CONSTRAINT fk_Product_Id FOREIGN KEY (Product_Id)
                	REFERENCES tbl_Procedure (Product_Id) ON DELETE CASCADE ON UPDATE CASCADE
            );
            CREATE TABLE IF NOT EXISTS tbl_Value (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                TaskNum INT UNIQUE KEY,
                Name CHAR(20),
                Product_Name CHAR(20),
            	Product_Id INT,
            	CONSTRAINT fk_Value_Product_Id FOREIGN KEY (Product_Id)
                	REFERENCES tbl_Product (Id) ON DELETE CASCADE ON UPDATE CASCADE
            );
            CREATE TABLE IF NOT EXISTS tbl_Value_Price (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                Unit CHAR(20),
                Unit_Price FLOAT,
            	Sequence INT,
            	Value_Id INT,
            	CONSTRAINT fk_Price_Value_Id FOREIGN KEY (Value_Id)
                	REFERENCES tbl_Value (Id) ON DELETE CASCADE ON UPDATE CASCADE,
                Procedure_Id INT,
                CONSTRAINT fk_Price_ProcedureId FOREIGN KEY (Procedure_Id)
                    REFERENCES tbl_Procedure (Id) ON DELETE CASCADE ON UPDATE CASCADE
            );
            CREATE TABLE IF NOT EXISTS tbl_Assign (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                EmployeeId INT,
                CONSTRAINT fk_Reckon_EmployeeId FOREIGN KEY (EmployeeId)
                    REFERENCES tbl_Employee (Id) ON DELETE CASCADE ON UPDATE CASCADE,
            	Value_Id INT,
            	CONSTRAINT fk_Reckon_Value_Id FOREIGN KEY (Value_Id)
                	REFERENCES tbl_Value (Id) ON DELETE CASCADE ON UPDATE CASCADE,
                Procedure_Id INT,
                CONSTRAINT fk_Reckon_Procedure_Id FOREIGN KEY (Procedure_Id)
                    REFERENCES tbl_Procedure (Id) ON DELETE CASCADE ON UPDATE CASCADE
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
        public int InsertProduct(Product p)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Product values "
                     + "(@Id, @Name)",
                     new { Id = p.Id, Name = p.Name });
            }
        }

        //获取所有Product对象的集合
        public IEnumerable<Product> QueryProductByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from tbl_Product order by Id asc";
                return conn.Query<Product>(query, null);
            }
        }
        public IEnumerable<Product> QueryProductByName(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Product>("select * from tbl_Product where Name=@Name", new { Name = name });
            }
        }
        public IEnumerable<Product> QueryProductById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Product>("select * from tbl_Product where Id=@Id", new { Id = id });
            }
        }
        public int DeleteProductById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Product where Id=@Id", new { Id = id });
            }
        }
        public int InsertProcedure(Procedure p)
        {
            //Id INT PRIMARY KEY,
            //Name CHAR(20) UNIQUE KEY,
            //Product_Id INT,
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Procedure values "
                     + "(@Id, @Name, @Product_Id)",
                     new { Id = p.Id, Name = p.Name, Product_Id = p.Product_Id });
            }
        }

        //获取所有Procedure对象的集合
        public IEnumerable<Procedure> QueryProcedureByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from tbl_Procedure order by Id asc";
                return conn.Query<Procedure>(query, null);
            }
        }
        public IEnumerable<Procedure> QueryProcedureByName(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Procedure>("select * from tbl_Procedure where Name=@Name", new { Name = name });
            }
        }
        public IEnumerable<Procedure> QueryProcedureById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Procedure>("select * from tbl_Procedure where Id=@Id", new { Id = id });
            }
        }
        public IEnumerable<Procedure> QueryProcedureByProduct_Id(int p_id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Procedure>("select * from tbl_Procedure where Product_Id=@Product_Id", new { Product_Id = p_id });
            }
        }
        public int DeleteProcedureById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Procedure where Id=@Id", new { Id = id });
            }
        }
        public int InsertValue(Value v)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Value values "
                     + "(@Id, @TaskNum, @Name, @Product_Name, @Product_Id)",
                     new { Id = v.Id, TaskNum = v.TaskNum, Name = v.Name, Product_Name = v.Product_Name, Product_Id = v.Product_Id });
            }
        }

        //获取所有Value对象的集合
        public IEnumerable<Value> QueryValueByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from tbl_Value";
                return conn.Query<Value>(query, null);
            }
        }
        public IEnumerable<Value> QueryValueById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Value>("select * from tbl_Value where Id=@Id", new { Id = id });
            }
        }
        public IEnumerable<Value> QueryValueByTaskNum(int num)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Value>("select * from tbl_Value where TaskNum=@TaskNum", new { TaskNum = num });
            }
        }
        public IEnumerable<Value> QueryValueByNameAndProductId(string name, int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Value>("select * from tbl_Value where Name=@Name and Product_Id=@Product_Id", new { Name = name, Product_Id = id });
            }
        }
        public IEnumerable<Value> QueryValueByName(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Value>("select * from tbl_Value where Name=@Name", new { Name = name });
            }
        }
        public IEnumerable<Value> QueryValueByProductId(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Value>("select * from tbl_Value where Product_Id=@Product_Id", new { Product_Id = id });
            }
        }
        public int UpdateValue(Value modifiedTask)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("update tbl_Value set TaskNum=@TaskNum, Name=@Name where Id=@Id", new { TaskNum = modifiedTask.TaskNum, Name = modifiedTask.Name, Id = modifiedTask.Id });
            }
        }
        public int DeleteValueById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Value where Id=@Id", new { Id = id });
            }
        }
        public int InsertRelationship(Relationship r)
        {
            //Id INT AUTO_INCREMENT PRIMARY KEY,
            //Sequence_Number INT,
            //Procedure_Name char(20) UNIQUE KEY,
            //CONSTRAINT fk_Procedure_Name FOREIGN KEY (Procedure_Name)
            //	REFERENCES tbl_Procedure(Name),
            //Input_Output_Ratio INT,
            //Product_Id INT,
            //CONSTRAINT fk_Product_Id FOREIGN KEY (Product_Id)
            //	REFERENCES tbl_Procedure (Product_Id)
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Relationship values "
                     + "(@Id, @Sequence_Number, @Procedure_Name, @Input_Output_Ratio, @Product_Id)",
                     new { Id = r.Id, Sequence_Number = r.Sequence_Number, Procedure_Name = r.Procedure_Name, Input_Output_Ratio = r.Input_Output_Ratio, Product_Id = r.Product_Id });
            }
        }

        //获取所有Relationship对象的集合
        public IEnumerable<Relationship> QueryRelationshipByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from tbl_Relationship order by Sequence_Number";
                return conn.Query<Relationship>(query, null);
            }
        }
        public IEnumerable<Relationship> QueryRelationshipByName(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Relationship>("select * from tbl_Relationship where Procedure_Name=@Procedure_Name", new { Procedure_Name = name });
            }
        }
        public IEnumerable<Relationship> QueryRelationshipBySequence_Number(int order)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Relationship>("select * from tbl_Relationship where Sequence_Number=@Sequence_Number", new { Sequence_Number = order });
            }
        }
        public IEnumerable<Relationship> QueryRelationshipByProduct_Id(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Relationship>("select * from tbl_Relationship where Product_Id=@Product_Id order by Sequence_Number", new { Product_Id = id });
            }
        }
        public IEnumerable<Relationship> QueryRelationshipByRatio(int ratio)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Relationship>("select * from tbl_Relationship where Input_Output_Ratio=@Input_Output_Ratio", new { Input_Output_Ratio = ratio });
            }
        }
        public int DeleteRelationshipByName(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Relationship where Procedure_Name=@Procedure_Name", new { Procedure_Name = name });
            }
        }
        public IEnumerable<Procedure> QueryProcedureNotInRelationshipByProductId(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Procedure>("select * from tbl_procedure where Product_Id = @Product_Id and Name not in (select Procedure_Name from tbl_Relationship)", new { Product_Id = id });
            }
        }
        public int InsertValuePrice(ValuePrice p)
        {
            //Id INT AUTO_INCREMENT PRIMARY KEY,
            //Unit CHAR(20),
            //Unit_Price FLOAT,
            //Sequence INT,
            //CONSTRAINT fk_Sequence FOREIGN KEY (Sequence)
            //	REFERENCES tbl_Relationship (Sequence_Number) ON DELETE CASCADE ON UPDATE CASCADE,
            //Value_Id INT,
            //CONSTRAINT fk_Value_Id FOREIGN KEY (Value_Id)
            //	REFERENCES tbl_Value (Id)
            //Procedure_Id INT,
            //CONSTRAINT fk_ProcedureId FOREIGN KEY (Procedure_Id)
            //    REFERENCES tbl_Procedure (Id)
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Value_Price values "
                     + "(@Id, @Unit, @Unit_Price, @Sequence, @Value_Id, @Procedure_Id)",
                     new { Id = p.Id, Unit = p.Unit, Unit_Price = p.Unit_Price, Sequence = p.Sequence, Value_Id = p.Value_Id, Procedure_Id = p.Procedure_Id });
            }
        }
        public IEnumerable<ValuePrice> QueryValuePriceByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<ValuePrice>("select * from tbl_Value_Price order by Sequence");
            }
        }
        public IEnumerable<ValuePrice> QueryValuePriceByValueId(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<ValuePrice>("select * from tbl_Value_Price where Value_Id=@Value_Id order by Sequence", new { Value_Id = id });
            }
        }
        public int UpdatePrice(ValuePrice price)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("update tbl_Value_Price set Unit=@Unit, Unit_Price=@Unit_Price where Id=@Id", new { Unit = price.Unit, Unit_Price = price.Unit_Price, Id = price.Id });
            }
        }
        public int InsertAssign(Assign a)
        {
            //Id INT AUTO_INCREMENT PRIMARY KEY,
            //EmployeeId INT,
            //CONSTRAINT fk_Reckon_EmployeeId FOREIGN KEY (EmployeeId)
            //    REFERENCES tbl_Employee (Id) ON DELETE CASCADE ON UPDATE CASCADE,
            //Value_Id INT,
            //CONSTRAINT fk_Reckon_Value_Id FOREIGN KEY (Value_Id)
            //	REFERENCES tbl_Value (Id) ON DELETE CASCADE ON UPDATE CASCADE,
            //Procedure_Id INT,
            //CONSTRAINT fk_Reckon_Procedure_Id FOREIGN KEY (Procedure_Id)
            //    REFERENCES tbl_Procedure (Id) ON DELETE CASCADE ON UPDATE CASCADE
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Assign values "
                     + "(@Id, @EmployeeId, @Value_Id, @Procedure_Id)",
                     new { Id = a.Id, EmployeeId = a.EmployeeId, Value_Id = a.Value_Id, Procedure_Id = a.Procedure_Id });
            }
        }
        public IEnumerable<Assign> QueryAssignByValueId(int value_id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Assign>("select * from tbl_Assign where Value_Id=@Value_Id", new { Value_Id = value_id });
            }
        }
        public int DeleteAssignById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Assign where Id=@Id", new { Id = id });
            }
        }
    }
}
