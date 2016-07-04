// 文 件: DbHelper.cs
// 作 者: yangdeyong
// 时 间: 2012年4月10日 10:12:45
// 摘 要: .NET通用数据库操作帮助类,可支持Odbc、OleDb、OracleClient、SqlClient、SqlServerCe等多种数据库提供程序操作

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace Banana.Core.Db
{
    /// <summary>
    /// 说 明: .NET通用数据库操作帮助类,可支持Odbc、OleDb、OracleClient、SqlClient、SqlServerCe等多种数据库提供程序操作
    /// 作 者: yangdeyong
    /// 时 间: 2012-04-10 10:12:45 
    /// </summary>
    /// <remarks>
    /// 作 者: yangdeyong
    /// 时 间: 2012-04-10 10:12:45 
    /// </remarks>
    public sealed class DbHelper
    {
        #region 字段属性
        private static Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
        #region 静态公有字段
        /// <summary>
        /// 获取当前数据库配置的提供程序名称值DbProviderName
        /// 
        /// </summary>
        public static readonly string DbProviderName = ConfigurationManager.AppSettings["DbProviderName"];
        /// <summary>
        /// 获取当前数据库配置的连接字符串值DbConnectionString
        /// </summary>
        public static readonly string DbConnectionString = ConfigurationManager.AppSettings["DbConnectionString"];
        #endregion

        #region 私有字段
        /// <summary>
        /// 当前默认配置的数据库提供程序DbProviderFactory
        /// </summary>
        private DbProviderFactory _dbFactory = null;

        /// <summary>
        /// 当前数据库链接DbConnection对象
        /// </summary>
        private DbConnection _dbConnection = null;

        /// <summary>
        /// 当前的数据库提供程序
        /// </summary>
        private string _dbProviderName = null;

        /// <summary>
        /// 当前的数据库连接字符串
        /// </summary>
        private string _dbConnectionString = null;
        #endregion

        #endregion

        #region 构造函数
        /// <summary>
        /// 根据配置的数据库提供程序和链接串进行初始化此对象实例
        /// </summary>
        public DbHelper()
            : this(DbHelper.DbConnectionString, DbHelper.DbProviderName)
        {
        }

        /// <summary>
        /// 根据数据库链接串和数据库提供程序名称两个参数进行初始化此对象实例
        /// </summary>
        /// <param name="connectionString">数据库连接配置字符串</param>
        /// <param name="providerName">数据库提供程序的名称</param>
        public DbHelper(string connectionString, string providerName)
        {
            if (!string.IsNullOrEmpty(providerName))
            {
                this._dbFactory = DbHelper.CreateDbProviderFactory(providerName);//创建默认配置的数据库提供程序
            }
            else
            {
                throw new ArgumentNullException("providerName", "数据库提供程序名称参数值不能为空,请在配置文件中配置该项值!");
            }

            if (!string.IsNullOrEmpty(connectionString))
            {
                this._dbConnection = DbHelper.CreateDbConnection(connectionString, providerName);//创建当前数据库链接对象
            }
            else
            {
                throw new ArgumentNullException("connectionString", "数据库链接串参数值不能为空,请在配置文件中配置该项值!");
            }

            //保存当前连接字符串和数据库提供程序名称
            this._dbConnectionString = connectionString;
            this._dbProviderName = providerName;
        }
        #endregion

        #region 方法函数

        #region 创建DbProviderFactory对象(静态方法)
        /// <summary>
        /// 根据配置的数据库提供程序的DbProviderName名称来创建一个数据库配置的提供程序DbProviderFactory对象
        /// </summary>
        public static DbProviderFactory CreateDbProviderFactory()
        {
            DbProviderFactory dbFactory = DbHelper.CreateDbProviderFactory(DbHelper.DbProviderName);

            return dbFactory;
        }

        /// <summary>
        /// 根据参数名称创建一个数据库提供程序DbProviderFactory对象
        /// </summary>
        /// <param name="dbProviderName">数据库提供程序的名称</param>
        public static DbProviderFactory CreateDbProviderFactory(string dbProviderName)
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(dbProviderName);

            return dbFactory;
        }
        #endregion

        #region 创建DbConnection对象(静态方法)
        /// <summary>
        /// 根据配置的数据库提供程序和链接串来创建数据库链接.
        /// </summary>
        public static DbConnection CreateDbConnection()
        {
            DbConnection dbConn = DbHelper.CreateDbConnection(DbHelper.DbConnectionString, DbHelper.DbProviderName);

            return dbConn;
        }

        /// <summary>
        /// 根据数据库连接字符串参数来创建数据库链接.
        /// </summary>
        /// <param name="connectionString">数据库连接配置字符串</param>
        /// <param name="dbProviderName">数据库提供程序的名称</param>
        /// <returns></returns>
        public static DbConnection CreateDbConnection(string connectionString, string dbProviderName)
        {
            DbProviderFactory dbFactory = DbHelper.CreateDbProviderFactory(dbProviderName);

            DbConnection dbConn = dbFactory.CreateConnection();
            dbConn.ConnectionString = connectionString;

            return dbConn;
        }
        #endregion

        #region 获取DbCommand对象
        /// <summary>
        /// 根据存储过程名称来构建当前数据库链接的DbCommand对象
        /// </summary>
        /// <param name="storedProcedure">存储过程名称</param>
        public DbCommand GetStoredProcedureCommond(string storedProcedure)
        {
            DbCommand dbCmd = this._dbConnection.CreateCommand();

            dbCmd.CommandText = storedProcedure;
            dbCmd.CommandType = CommandType.StoredProcedure;

            return dbCmd;
        }

        /// <summary>
        /// 根据SQL语句来构建当前数据库链接的DbCommand对象
        /// </summary>
        /// <param name="sqlQuery">SQL查询语句</param>
        public DbCommand GetSqlStringCommond(string sqlQuery)
        {
            DbCommand dbCmd = this._dbConnection.CreateCommand();

            dbCmd.CommandText = sqlQuery;
            dbCmd.CommandType = CommandType.Text;

            return dbCmd;
        }
        #endregion

        #region 添加DbCommand参数
        /// <summary>
        /// 把参数集合添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="dbParameterCollection">数据库操作集合</param>
        public void AddParameterCollection(DbCommand cmd, DbParameterCollection dbParameterCollection)
        {
            if (cmd != null)
            {
                foreach (DbParameter dbParameter in dbParameterCollection)
                {
                    cmd.Parameters.Add(dbParameter);
                }
            }
        }

        /// <summary>
        /// 把输出参数添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数的类型</param>
        /// <param name="size">参数的大小</param>
        public void AddOutParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
        {
            if (cmd != null)
            {
                DbParameter dbParameter = cmd.CreateParameter();

                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Size = size;
                dbParameter.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(dbParameter);
            }
        }

        /// <summary>
        /// 把输入参数添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数的类型</param>
        /// <param name="value">参数值</param>
        public void AddInParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            if (cmd != null)
            {
                DbParameter dbParameter = cmd.CreateParameter();

                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Value = value;
                dbParameter.Direction = ParameterDirection.Input;

                cmd.Parameters.Add(dbParameter);
            }
        }

        /// <summary>
        /// 把返回参数添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数的类型</param>
        public void AddReturnParameter(DbCommand cmd, string parameterName, DbType dbType)
        {
            if (cmd != null)
            {
                DbParameter dbParameter = cmd.CreateParameter();

                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.Add(dbParameter);
            }
        }

        /// <summary>
        /// 根据参数名称从DbCommand对象中获取相应的参数对象
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        public DbParameter GetParameter(DbCommand cmd, string parameterName)
        {
            if (cmd != null && cmd.Parameters.Count > 0)
            {
                DbParameter param = cmd.Parameters[parameterName];

                return param;
            }

            return null;
        }
        #endregion

        #region 执行SQL脚本语句
        /// <summary>
        /// 执行相应的SQL命令，返回一个DataSet数据集合
        /// </summary>
        /// <param name="sqlQuery">需要执行的SQL语句</param>
        /// <returns>返回一个DataSet数据集合</returns>
        public DataSet ExecuteDataSet(string sqlQuery)
        {
            DataSet ds = new DataSet();

            if (!string.IsNullOrEmpty(sqlQuery))
            {
                DbCommand cmd = GetSqlStringCommond(sqlQuery);

                ds = ExecuteDataSet(cmd);
            }

            return ds;
        }

        /// <summary>
        /// 执行相应的SQL命令，返回一个DataTable数据集
        /// </summary>
        /// <param name="sqlQuery">需要执行的SQL语句</param>
        /// <returns>返回一个DataTable数据集</returns>
        public DataTable ExecuteDataTable(string sqlQuery)
        {
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(sqlQuery))
            {
                DbCommand cmd = GetSqlStringCommond(sqlQuery);

                dt = ExecuteDataTable(cmd);
            }

            return dt;
        }

        /// <summary>
        /// 执行相应的SQL命令，返回一个DbDataReader数据对象，如果没有则返回null值
        /// </summary>
        /// <param name="sqlQuery">需要执行的SQL命令</param>
        /// <returns>返回一个DbDataReader数据对象，如果没有则返回null值</returns>
        public DbDataReader ExecuteReader(string sqlQuery)
        {
            if (!string.IsNullOrEmpty(sqlQuery))
            {
                DbCommand cmd = GetSqlStringCommond(sqlQuery);

                DbDataReader reader = ExecuteReader(cmd);

                return reader;
            }

            return null;
        }

        /// <summary>
        /// 执行相应的SQL命令，返回影响的数据记录数，如果不成功则返回-1
        /// </summary>
        /// <param name="sqlQuery">需要执行的SQL命令</param>
        /// <returns>返回影响的数据记录数，如果不成功则返回-1</returns>
        public int ExecuteNonQuery(string sqlQuery)
        {
            if (!string.IsNullOrEmpty(sqlQuery))
            {
                DbCommand cmd = GetSqlStringCommond(sqlQuery);

                int retVal = ExecuteNonQuery(cmd);

                return retVal;
            }

            return -1;
        }

        /// <summary>
        /// 执行相应的SQL命令，返回结果集中的第一行第一列的值，如果不成功则返回null值
        /// </summary>
        /// <param name="sqlQuery">需要执行的SQL命令</param>
        /// <returns>返回结果集中的第一行第一列的值，如果不成功则返回null值</returns>
        public object ExecuteScalar(string sqlQuery)
        {
            if (!string.IsNullOrEmpty(sqlQuery))
            {
                DbCommand cmd = GetSqlStringCommond(sqlQuery);

                object retVal = ExecuteScalar(cmd);

                return retVal;
            }

            return null;
        }

        public object ExecuteScalar(string sqlQuery, Trans trans)
        {
            if (!string.IsNullOrEmpty(sqlQuery))
            {
                DbCommand cmd = GetSqlStringCommond(sqlQuery);

                object retVal = ExecuteScalar(cmd, trans);

                return retVal;
            }

            return null;
        }

        #endregion

        #region 执行DbCommand命令
        /// <summary>
        /// 执行相应的命令，返回一个DataSet数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回一个DataSet数据集合</returns>
        public DataSet ExecuteDataSet(DbCommand cmd)
        {
            DataSet ds = new DataSet();

            if (cmd != null)
            {
                DbDataAdapter dbDataAdapter = this._dbFactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;

                dbDataAdapter.Fill(ds);
            }

            return ds;
        }

        /// <summary>
        /// 执行相应的命令，返回一个DataTable数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回一个DataTable数据集合</returns>
        public DataTable ExecuteDataTable(DbCommand cmd)
        {
            DataTable dataTable = new DataTable();

            if (cmd != null)
            {
                DbDataAdapter dbDataAdapter = this._dbFactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;

                dbDataAdapter.Fill(dataTable);
            }

            return dataTable;
        }

        /// <summary>
        /// 执行相应的命令，返回一个DbDataReader数据对象，如果没有则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回一个DbDataReader数据对象，如果没有则返回null值</returns>
        public DbDataReader ExecuteReader(DbCommand cmd)
        {
            if (cmd != null && cmd.Connection != null)
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);//当reader读取结束时自动关闭数据库链接

                return reader;
            }

            return null;
        }

        /// <summary>
        /// 执行相应的命令，返回影响的数据记录数，如果不成功则返回-1
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回影响的数据记录数，如果不成功则返回-1</returns>
        public int ExecuteNonQuery(DbCommand cmd)
        {
            if (cmd != null && cmd.Connection != null)
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                int retVal = cmd.ExecuteNonQuery();

                cmd.Connection.Close();

                return retVal;
            }

            return -1;
        }

        /// <summary>
        /// 执行相应的命令，返回结果集中的第一行第一列的值，如果不成功则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回结果集中的第一行第一列的值，如果不成功则返回null值</returns>
        public object ExecuteScalar(DbCommand cmd)
        {
            if (cmd != null && cmd.Connection != null)
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                object retVal = cmd.ExecuteScalar();

                cmd.Connection.Close();

                return retVal;
            }

            return null;
        }
        #endregion

        #region 执行DbTransaction事务
        /// <summary>
        /// 以事务的方式执行相应的命令，返回一个DataSet数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回一个DataSet数据集合</returns>
        public DataSet ExecuteDataSet(DbCommand cmd, Trans trans)
        {
            DataSet ds = new DataSet();

            if (cmd != null)
            {
                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;

                DbDataAdapter dbDataAdapter = this._dbFactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;

                dbDataAdapter.Fill(ds);
            }

            return ds;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回一个DataTable数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回一个DataTable数据集合</returns>
        public DataTable ExecuteDataTable(DbCommand cmd, Trans trans)
        {
            DataTable dataTable = new DataTable();

            if (cmd != null)
            {
                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;

                DbDataAdapter dbDataAdapter = this._dbFactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;

                dbDataAdapter.Fill(dataTable);
            }

            return dataTable;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回一个DbDataReader数据对象，如果没有则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回一个DbDataReader数据对象，如果没有则返回null值</returns>
        public DbDataReader ExecuteReader(DbCommand cmd, Trans trans)
        {
            if (cmd != null)
            {
                cmd.Connection.Close();

                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;

                DbDataReader reader = cmd.ExecuteReader();

                return reader;
            }

            return null;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回影响的数据记录数，如果不成功则返回-1
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回影响的数据记录数，如果不成功则返回-1</returns>
        public int ExecuteNonQuery(DbCommand cmd, Trans trans)
        {
            if (cmd != null)
            {
                if (cmd.Connection != trans.Connection)
                {
                    cmd.Connection.Close();
                }

                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;

                int retVal = cmd.ExecuteNonQuery();

                return retVal;
            }

            return -1;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回结果集中的第一行第一列的值，如果不成功则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回结果集中的第一行第一列的值，如果不成功则返回null值</returns>
        public object ExecuteScalar(DbCommand cmd, Trans trans)
        {
            if (cmd != null)
            {
                if (cmd.Connection != trans.Connection)
                {
                    cmd.Connection.Close();
                }

                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;

                object retVal = cmd.ExecuteScalar();

                return retVal;
            }

            return null;
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 说 明: 数据库事务操作对象
    /// 作 者: yangdeyong
    /// 时 间: 2012-04-10 10:19:23 
    /// </summary>
    /// <remarks>
    /// 作 者: yangdeyong
    /// 时 间: 2012-04-10 10:12:45 
    /// </remarks>
    public sealed class Trans : IDisposable
    {
        #region 字段属性
        private DbConnection connection = null;
        /// <summary>
        /// 获取当前数据库链接对象
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                return this.connection;
            }
        }

        private DbTransaction transaction = null;
        /// <summary>
        /// 获取当前数据库事务对象
        /// </summary>
        public DbTransaction Transaction
        {
            get
            {
                return this.transaction;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 根据配置的数据库提供程序和连接字符串来创建此事务对象
        /// </summary>
        public Trans()
            : this(DbHelper.DbConnectionString, DbHelper.DbProviderName)
        {
        }

        /// <summary>
        /// 根据数据库连接字符串来创建此事务对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dbProviderName">数据库提供程序的名称</param>
        public Trans(string connectionString, string dbProviderName)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                this.connection = DbHelper.CreateDbConnection(connectionString, dbProviderName);
                this.Connection.Open();

                this.transaction = this.Connection.BeginTransaction();
            }
            else
            {
                throw new ArgumentNullException("connectionString", "数据库链接串参数值不能为空!");
            }
        }
        #endregion

        #region 方法函数
        /// <summary>
        /// 提交此数据库事务操作
        /// </summary>
        public void Commit()
        {
            this.Transaction.Commit();

            this.Close();
        }

        /// <summary>
        /// 回滚此数据库事务操作
        /// </summary>
        public void RollBack()
        {
            this.Transaction.Rollback();

            this.Close();
        }

        /// <summary>
        /// 关闭此数据库事务链接
        /// </summary>
        public void Close()
        {
            if (this.Connection.State != System.Data.ConnectionState.Closed)
            {
                this.Connection.Close();
            }
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }
        #endregion
    }
}