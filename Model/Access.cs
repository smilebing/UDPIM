using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.SqlClient;
using KeyboardIdentify;

namespace Model
{
    public class Access
    {
        //创建连接对象
        public OleDbConnection conn { get; set; }
        



       /// <summary>
       /// 构造函数 
       /// </summary>
       /// <param name="olconn"></param>
        public Access(OleDbConnection olconn)
        {
            this.conn = olconn;
        }
        


        /// <summary>
        /// 开启数据库连接
        /// </summary>
        public void openConn()
        {
            conn.Open();
           
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void closeConn()
        {
            conn.Close();
        }


       /// <summary>
       /// 
       /// </summary>
       /// <param name="name"></param>
       /// <param name="pwd"></param>
       /// <returns>返回用户是否存在</returns>
        public bool search (string name,string pwd)
        {
            OleDbCommand search_cmd = conn.CreateCommand();
            string sqlString = "select * from [user] where username=@username and password=@password";
            search_cmd.CommandText = sqlString;
            search_cmd.Parameters.Add(new OleDbParameter("username", name));
            search_cmd.Parameters.Add(new OleDbParameter("password", pwd));


            search_cmd.ExecuteScalar(); 
            int i = Convert.ToInt32(search_cmd.ExecuteScalar());
            if(i>0)
            {
                //查询存在
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查找用户是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool search(string name)
        {
            OleDbCommand search_cmd = conn.CreateCommand();

            string sqlString = "select * from [user] where username=@username ";
            search_cmd.CommandText = sqlString;
            search_cmd.Parameters.Add(new OleDbParameter("username", name));

            search_cmd.ExecuteScalar();
            int i = Convert.ToInt32(search_cmd.ExecuteScalar());
            if (i > 0)
            {
                //查询存在
                return true;
            }
            return false;
        }


        /// <summary>
        /// 插入新用户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns>增加用户是否成功</returns>
        public bool insert(string name,string pwd)
        {
            if(search(name)==true)
            {
                return false;
            }

            OleDbCommand insert_cmd = conn.CreateCommand();


            //string sqlString = "INSERT INTO [user] (username,password) VALUES (@username,@password)";
            
            insert_cmd.CommandText = "INSERT INTO [user] ([username],[password]) VALUES(@username, @password)";

            //insert_cmd.Parameters.Add(new OleDbParameter("name", name));
            //insert_cmd.Parameters.Add(new OleDbParameter("pwd", pwd));
            insert_cmd.Parameters.AddWithValue("@username", name);
            insert_cmd.Parameters.AddWithValue("@password", pwd);

            int i = insert_cmd.ExecuteNonQuery();
            if(i>0)
            {
                //插入成功
                return true;
            }
            return false;
        }



        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns>更新是否成功</returns>
        public bool update(string name, string pwd)
        {
            if (search(name) == false)
            {
                return false;
            }

            OleDbCommand update_cmd = conn.CreateCommand();


            string sqlString = "update [user] set password=@pwd where username=@name";

            update_cmd.CommandText = sqlString;
            update_cmd.Parameters.Add(new OleDbParameter("pwd", pwd));
            update_cmd.Parameters.Add(new OleDbParameter("name", name));


            int i = update_cmd.ExecuteNonQuery();
            if (i > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 根据用户名查询用户的数字ID（主键）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户的ID，如果返回-1，则用户不存在或者查询有错误</returns>
        public int SearchUserID(string username)
        {
            string searchUserIDString = "select ID from [user] where [username]=@username";
            int userID = -1;

            OleDbCommand searchUserIDCmd = conn.CreateCommand();
            searchUserIDCmd.CommandText = searchUserIDString;
            searchUserIDCmd.Parameters.Add(new OleDbParameter("username", username));
            var reader = searchUserIDCmd.ExecuteReader();

            while(reader.Read())
            {
                userID = (int)reader["ID"];
            }

            return userID;
        }

        /// <summary>
        /// 插入键盘特征描述向量
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="v">击键特征描述向量</param>
        /// <returns>是否插入成功</returns>
        public bool InsertKeyboardData(string username, Vector v)
        {
            string insertKeyboardDataString = "insert into [keyboard] (data, userID) values (@data, @userID)";
            int userID = SearchUserID(username);

            //如果找不到该用户
            if(userID == -1)
            {
                return false;
            }

            var insertDataCmd = conn.CreateCommand();
            insertDataCmd.CommandText = insertKeyboardDataString;
            insertDataCmd.Parameters.AddWithValue("data", v.ToJson());
            insertDataCmd.Parameters.AddWithValue("userID", userID);

            return insertDataCmd.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// 获取所有保存的键盘特征向量
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<Vector> FetchKeyboardVectors(string username)
        {
            List<Vector> keyboardVectors = new List<Vector>();
            var userID = SearchUserID(username);

            var fetchCmd = conn.CreateCommand();
            fetchCmd.CommandText = "SELECT * FROM [keyboard] WHERE [userID]=@userID";
            fetchCmd.Parameters.AddWithValue("@userID", userID);
            var reader = fetchCmd.ExecuteReader();

            while (reader.Read())
            {
                keyboardVectors.Add(Vector.GetVector((string)reader["data"]));
            }

            return keyboardVectors;
        }

        /// <summary>
        /// 删除一个用户
        /// </summary>
        /// <param name="username"></param>
        public void DeleteUser(string username)
        {
            var deleteDataCmd = conn.CreateCommand();
            var deleteUserCmd = conn.CreateCommand();
            int userID = SearchUserID(username);

            deleteDataCmd.CommandText = "DELETE FROM [keyboard] WHERE [userID]=@userID";
            deleteDataCmd.Parameters.AddWithValue("@userID", userID);
            deleteDataCmd.ExecuteNonQuery();

            deleteUserCmd.CommandText = "DELETE FROM [user] WHERE [ID]=@userID";
            deleteUserCmd.Parameters.AddWithValue("@userID", userID);
            deleteUserCmd.ExecuteNonQuery();
        }

        public string GetUserPassword(string username)
        {
            var searchCmd = conn.CreateCommand();
            searchCmd.CommandText = "SELECT [password] FROM [user] WHERE [username]=@username";
            searchCmd.Parameters.AddWithValue("@username", username);
            var reader = searchCmd.ExecuteReader();

            reader.Read();
            return (string) reader["password"];
        }
    }
}
