﻿using Dapper;
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

        public bool DropDatabase()
        {
            MySqlConnection conn = new MySqlConnection("Data Source=localhost;Persist Security Info=yes;UserId=root; PWD=abcd709394;");
            MySqlCommand cmd = new MySqlCommand(@"
            DROP DATABASE IF EXISTS gradulation_design_db;
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
                    REFERENCES tbl_Workshop (Name) ON UPDATE SET NULL,
                CONSTRAINT fk_Job FOREIGN KEY (Job)
                    REFERENCES tbl_Job (Name) ON UPDATE SET NULL,
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
                    REFERENCES tbl_Product (Id) ON UPDATE CASCADE
            );
            CREATE TABLE IF NOT EXISTS tbl_Relationship
            (
            	Id INT AUTO_INCREMENT PRIMARY KEY,
            	InputProcedure char(20) UNIQUE KEY,
            	CONSTRAINT fk_InputProcedure FOREIGN KEY (InputProcedure)
            		REFERENCES tbl_Procedure(Name) ON UPDATE CASCADE,
            	OutputProcedure char(20),
            	CONSTRAINT fk_OutputProcedure FOREIGN KEY (OutputProcedure)
            		REFERENCES tbl_Procedure(Name) ON UPDATE CASCADE,
            	Input_Output_Ratio INT,
            	Product_Id INT,
            	CONSTRAINT fk_Product_Id FOREIGN KEY (Product_Id)
                	REFERENCES tbl_Procedure (Product_Id) ON UPDATE CASCADE
            );
            CREATE TABLE IF NOT EXISTS tbl_Value (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                TaskNum INT UNIQUE KEY,
                TaskDate DateTime,
                Name CHAR(20),
                Product_Name CHAR(20),
            	Product_Id INT,
            	CONSTRAINT fk_Value_Product_Id FOREIGN KEY (Product_Id)
                	REFERENCES tbl_Product (Id) ON UPDATE CASCADE
            );
            CREATE TABLE IF NOT EXISTS tbl_Value_Price (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                Unit CHAR(20),
                Unit_Price FLOAT,
            	Value_Id INT,
            	CONSTRAINT fk_Price_Value_Id FOREIGN KEY (Value_Id)
                	REFERENCES tbl_Value (Id) ON UPDATE CASCADE,
                Procedure_Id INT,
                CONSTRAINT fk_Price_ProcedureId FOREIGN KEY (Procedure_Id)
                    REFERENCES tbl_Procedure (Id) ON UPDATE CASCADE
            );
            CREATE TABLE IF NOT EXISTS tbl_Assign (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                EmployeeId INT,
                CONSTRAINT fk_Reckon_EmployeeId FOREIGN KEY (EmployeeId)
                    REFERENCES tbl_Employee (Id) ON UPDATE CASCADE,
                Price_Id INT,
                CONSTRAINT fk_Assign_Price_Id FOREIGN KEY (Price_Id)
                    REFERENCES tbl_Value_Price (Id) ON UPDATE CASCADE
            );
            CREATE TABLE IF NOT EXISTS tbl_Reckon (
                Assign_Id INT PRIMARY KEY,
                CONSTRAINT fk_Assign_Id FOREIGN KEY (Assign_Id)
                    REFERENCES tbl_Assign (Id) ON UPDATE CASCADE,
                Count INT 
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
        public int UpdateEmployee(Employee e)
        {
            using (IDbConnection conn = OpenConnection())
            {
                //Id INT PRIMARY KEY,
                //Name CHAR(20),
                //Password CHAR(20),
                //Gender VARCHAR(20),
                //Workshop CHAR(20),
                //Job CHAR(20),
                //CONSTRAINT fk_Workshop FOREIGN KEY (Workshop)
                //    REFERENCES tbl_Workshop (Name) ON DELETE SET NULL ON UPDATE SET NULL,
                //CONSTRAINT fk_Job FOREIGN KEY (Job)
                //    REFERENCES tbl_Job (Name) ON DELETE SET NULL ON UPDATE SET NULL,
                //Telephone CHAR(20)
                return conn.Execute("update tbl_Employee set Name=@Name, Password=@Password, Gender=@Gender, Workshop=@Workshop, Job=@Job, Telephone=@Telephone where Id=@Id", 
                    new { Name = e.Name, Password = e.Password, Gender = e.Gender, Workshop = e.Workshop, Job = e.Job, Telephone = e.Telephone, Id = e.Id });
            }
        }
        public int ResetEmployeePassword(Employee e)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("update tbl_Employee set Password=@Password where Id=@Id", new { Password = e.Id.ToString(), Id = e.Id});
            }
        }
        public int UpdateEmployeePassword(Employee e)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("update tbl_Employee set Password=@Password where Id=@Id", new { Password = e.Password, Id = e.Id});
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
                     + "(@Id, @TaskNum, @TaskDate, @Name, @Product_Name, @Product_Id)",
                     new { Id = v.Id, TaskNum = v.TaskNum, TaskDate = v.TaskDate, Name = v.Name, Product_Name = v.Product_Name, Product_Id = v.Product_Id });
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
                return conn.Execute("update tbl_Value set TaskNum=@TaskNum, Name=@Name where Id=@Id", 
                    new { TaskNum = modifiedTask.TaskNum, Name = modifiedTask.Name, Id = modifiedTask.Id });
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
            //InputProcedure char(20) UNIQUE KEY,
            //CONSTRAINT fk_InputProcedure FOREIGN KEY (InputProcedure)
            //	REFERENCES tbl_Procedure(Name) ON DELETE CASCADE ON UPDATE CASCADE,
            //OutputProcedure char(20),
            //CONSTRAINT fk_OutputProcedure FOREIGN KEY (OutputProcedure)
            //	REFERENCES tbl_Procedure(Name) ON DELETE CASCADE ON UPDATE CASCADE,
            //Input_Output_Ratio INT,
            //Product_Id INT,
            //CONSTRAINT fk_Product_Id FOREIGN KEY (Product_Id)
            //	REFERENCES tbl_Procedure (Product_Id) ON DELETE CASCADE ON UPDATE CASCADE
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Relationship values "
                     + "(@Id, @InputProcedure, @OutputProcedure, @Input_Output_Ratio, @Product_Id)",
                     new { Id = r.Id, InputProcedure = r.InputProcedure, OutputProcedure = r.OutputProcedure, Input_Output_Ratio = r.Input_Output_Ratio, Product_Id = r.Product_Id });
            }
        }

        //获取所有Relationship对象的集合
        public IEnumerable<Relationship> QueryRelationshipByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string query = "select * from tbl_Relationship";
                return conn.Query<Relationship>(query, null);
            }
        }
        public IEnumerable<Relationship> QueryRelationshipByInput(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Relationship>("select * from tbl_Relationship where InputProcedure=@InputProcedure", new { InputProcedure = name });
            }
        }
        public IEnumerable<Relationship> QueryRelationshipByOutput(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Relationship>("select * from tbl_Relationship where OutputProcedure=@OutputProcedure", new { OutputProcedure = name });
            }
        }
        public IEnumerable<Relationship> QueryRelationshipByProduct_Id(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Relationship>("select * from tbl_Relationship where Product_Id=@Product_Id", new { Product_Id = id });
            }
        }
        public IEnumerable<Relationship> QueryRelationshipByRatio(int ratio)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Relationship>("select * from tbl_Relationship where Input_Output_Ratio=@Input_Output_Ratio", new { Input_Output_Ratio = ratio });
            }
        }
        public int UpdateRelationship(Relationship modifiedRelationship)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("update tbl_Relationship set OutputProcedure=@OutputProcedure, Input_Output_Ratio=@Input_Output_Ratio where InputProcedure=@InputProcedure", 
                    new { OutputProcedure = modifiedRelationship.OutputProcedure, Input_Output_Ratio = modifiedRelationship.Input_Output_Ratio, InputProcedure = modifiedRelationship.InputProcedure });
            }
        }
        public int DeleteRelationshipByName(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Relationship where InputProcedure=@InputProcedure", new { InputProcedure = name });
            }
        }
        public IEnumerable<Procedure> QueryProcedureNotInRelationshipByProductId(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Procedure>("select * from tbl_procedure where Product_Id = @Product_Id and Name not in (select InputProcedure from tbl_Relationship)", 
                    new { Product_Id = id });
            }
        }
        public int InsertValuePrice(ValuePrice p)
        {
            //Id INT AUTO_INCREMENT PRIMARY KEY,
            //Unit CHAR(20),
            //Unit_Price FLOAT,
            //Value_Id INT,
            //CONSTRAINT fk_Value_Id FOREIGN KEY (Value_Id)
            //	REFERENCES tbl_Value (Id)
            //Procedure_Id INT,
            //CONSTRAINT fk_ProcedureId FOREIGN KEY (Procedure_Id)
            //    REFERENCES tbl_Procedure (Id)
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Value_Price values "
                     + "(@Id, @Unit, @Unit_Price, @Value_Id, @Procedure_Id)",
                     new { Id = p.Id, Unit = p.Unit, Unit_Price = p.Unit_Price, Value_Id = p.Value_Id, Procedure_Id = p.Procedure_Id });
            }
        }
        public IEnumerable<ValuePrice> QueryValuePriceByAll()
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<ValuePrice>("select * from tbl_Value_Price");
            }
        }
        public IEnumerable<ValuePrice> QueryValuePriceById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<ValuePrice>("select * from tbl_Value_Price where Id=@Id", new { Id = id });
            }
        }
        public IEnumerable<ValuePrice> QueryValuePriceByValueId(int v_id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<ValuePrice>("select * from tbl_Value_Price where Value_Id=@Value_Id", new { Value_Id = v_id });
            }
        }
        public int UpdatePrice(ValuePrice price)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("update tbl_Value_Price set Unit=@Unit, Unit_Price=@Unit_Price where Id=@Id", 
                    new { Unit = price.Unit, Unit_Price = price.Unit_Price, Id = price.Id });
            }
        }
        public int InsertAssign(Assign a)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Assign values "
                     + "(@Id, @EmployeeId, @Price_Id)",
                     new { Id = a.Id, EmployeeId = a.EmployeeId, Price_Id = a.Price_Id });
            }
        }
        public IEnumerable<Assign> QueryAssignByEmployeeId(int e_id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Assign>("select * from tbl_Assign where EmployeeId=@EmployeeId", new { EmployeeId = e_id });
            }
        }
        public IEnumerable<Assign> QueryAssignByValuePrice(ValuePrice v)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Assign>("select * from tbl_Assign where Price_Id=@Price_Id", new { Price_Id = v.Id });
            }
        }
        public IEnumerable<Assign> QueryAssignWhetherExsit(Assign a)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Assign>("select * from tbl_Assign where EmployeeId=@EmployeeId and Price_Id=@Price_Id", 
                    new { EmployeeId = a.EmployeeId, Price_Id = a.Price_Id });
            }
        }
        public int DeleteAssignById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Assign where Id=@Id", new { Id = id });
            }
        }
        public int DeleteAssignByValueIdAndProcedureIdAndEmployeeId(int v_id, int p_id, int e_id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Assign where Value_Id=@Value_Id and Procedure_Id=@Procedure_Id and EmployeeId=EmployeeId", 
                    new { Value_Id = v_id, Procedure_Id = p_id, EmployeeId = e_id });
            }
        }
        public int InsertReckon(Reckon r)
        {
            //Assign_Id INT PRIMAEY KEY,
            //CONSTRAINT fk_Assign_Id FOREIGN KEY (Assign_Id)
            //    REFERENCES tbl_Assign (Id) ON DELETE CASCADE ON UPDATE CASCADE,
            //Price_Id INT,
            //CONSTRAINT fk_Price_Id FOREIGN KEY (Price_Id)
            //    REFERENCES tbl_Value_Price (Id) ON DELETE CASCADE ON UPDATE CASCADE,
            //Count INT 
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("Insert into tbl_Reckon values "
                     + "(@Assign_Id, @Count)",
                     new { Assign_Id = r.Assign_Id, Count = r.Count });
            }
        }
        public IEnumerable<Reckon> QueryReckonByAssignId(int a_id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Reckon>("select * from tbl_Reckon where Assign_Id=@Assign_Id", new { Assign_Id = a_id });
            }
        }
        public int UpdateReckonCount(Reckon r)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("update tbl_Reckon set Count=@Count where Assign_Id=@Assign_Id", new { Count = r.Count, Assign_Id = r.Assign_Id });
            }
        }
        public int DeleteReckonByAssignId(int assign_id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Execute("delete from tbl_Reckon where Assign_Id=@Assign_Id", new { Assign_Id = assign_id });
            }
        }
    }
}
