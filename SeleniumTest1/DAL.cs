using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Configuration;



namespace SeleniumTest_DAL
{
    public class SeleniumTest_DAL_Class
    {
        public int GetAcctInfo(string extrnlAcctId, string dbField)
        {
            try
            {
                ConnectionStringSettings _configSettings = ConfigurationManager.ConnectionStrings["GWES_connectionString"];

                DbProviderFactory _dbProvider = DbProviderFactories.GetFactory(_configSettings.ProviderName);
                //Create a connection to connect as per provided provider name
                using (DbConnection _dbConn = _dbProvider.CreateConnection())
                {
                    _dbConn.ConnectionString = _configSettings.ConnectionString;
                    _dbConn.Open();
                    //Create a command to execute
                    DbCommand _dbCommand = _dbProvider.CreateCommand();
                    _dbCommand.Connection = _dbConn;
                    _dbCommand.CommandText =
                        "SELECT " + dbField + ", ExtrnlAcctId, ShrtNm from account where extrnlacctid ='" + extrnlAcctId + "'";
                    _dbCommand.CommandType = CommandType.Text;

                    return Convert.ToInt32(_dbCommand.ExecuteScalar());
                    /* Data Reader Demo */
                    //Execute the command and store the data result-set into a data reader
                    //DbDataReader dbReader = _dbCommand.ExecuteReader();
                    
                    //Read each record from data reader at a time
                    //int AcctId =0;
                    //while (dbReader.Read())
                    //{
                    //    //bool isValidAcctId = Int32.TryParse(dbReader["Acctid"].ToString(), out AcctId);
                    //    //if (isValidAcctId)
                    //    //    return AcctId;
                    //    //else
                    //    //    return 0;

                    //    return Convert.ToInt32(dbReader["AcctId"]);
                    //    //Console.WriteLine(String.Format("{0}, {1}, {2}", dbReader["Acctid"], dbReader["ExtrnlAcctId"], dbReader["ShrtNm"]));

                    //}
                    //dbReader.Close();

                }
            }
            catch (Exception exp)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exp.Message);
                return 0;
            }


        }

        public void GetAcccountDetail(string extrnlAcctId)
        {
            try
            {
                ConnectionStringSettings _configSettings =
                    ConfigurationManager.ConnectionStrings["GWES_connectionString"];

                DbProviderFactory _dbProvider =
                    DbProviderFactories.GetFactory(_configSettings.ProviderName);
                //Create a connection to connect as per provided provider name
                using (DbConnection _dbConn = _dbProvider.CreateConnection())
                {
                    _dbConn.ConnectionString = _configSettings.ConnectionString;
                    _dbConn.Open();
                    //Create a command to execute
                    DbCommand _dbCommand = _dbProvider.CreateCommand();
                    _dbCommand.Connection = _dbConn;
                    _dbCommand.CommandText =
                        "SELECT Acctid, ExtrnlAcctId, ShrtNm from account where extrnlacctid ='" + extrnlAcctId +"'";
                    _dbCommand.CommandType = CommandType.Text;
                    /* Data Reader Demo */
                    //Execute the command and store the data result-set into a data reader
                    DbDataReader dbReader = _dbCommand.ExecuteReader();
                    //Read each record from data reader at a time
                    while (dbReader.Read())
                    {
                        Console.WriteLine(String.Format("{0}, {1}, {2}", dbReader["Acctid"],
                            dbReader["ExtrnlAcctId"], dbReader["ShrtNm"]));
                    }
                    dbReader.Close();
                    
                }
            }
            catch (Exception exp)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exp.Message);
            }

        }

        //static void Main()
        //{
        //    //try
        //    //{
        //    //    ConnectionStringSettings _configSettings =
        //    //        ConfigurationManager.ConnectionStrings["GWES_connectionString"];

        //    //    DbProviderFactory _dbProvider =
        //    //        DbProviderFactories.GetFactory(_configSettings.ProviderName);
        //    //    //Create a connection to connect as per provided provider name
        //    //    using (DbConnection _dbConn = _dbProvider.CreateConnection())
        //    //    {
        //    //        _dbConn.ConnectionString = _configSettings.ConnectionString;
        //    //        _dbConn.Open();
        //    //        //Create a command to execute
        //    //        DbCommand _dbCommand = _dbProvider.CreateCommand();
        //    //        _dbCommand.Connection = _dbConn;
        //    //        _dbCommand.CommandText =
        //    //            "SELECT TOP 5 FirstName, LastName, JobTitle FROM HumanResources.vEmployee";
        //    //        _dbCommand.CommandType = CommandType.Text;
        //    //        /* Data Reader Demo */
        //    //        //Execute the command and store the data result-set into a data reader
        //    //        DbDataReader dbReader = _dbCommand.ExecuteReader();
        //    //        //Read each record from data reader at a time
        //    //        while (dbReader.Read())
        //    //        {
        //    //            Console.WriteLine(String.Format("{0}, {1}, {2}", dbReader["FirstName"],
        //    //                dbReader["LastName"], dbReader["JobTitle"]));
        //    //        }
        //    //        dbReader.Close();
        //    //        /* Data Adaptor and Dataset Demo */
        //    //        //Execute the command and store the data result-set into a data table of a dataset
        //    //        DataSet _dataSet = new DataSet();
        //    //        DbDataAdapter _dbDataAdaptor = _dbProvider.CreateDataAdapter();
        //    //        _dbDataAdaptor.SelectCommand = _dbCommand;
        //    //        _dbDataAdaptor.Fill(_dataSet);
        //    //        //Iterate through the records and columns to get its specific values
        //    //        //A dataset may contain more than one datatable, as becuase I am using a 
        //    //        //single query to fill one datatable, I am using 0 indexer below
        //    //        foreach (DataRow _dataRow in _dataSet.Tables[0].Rows)
        //    //        {
        //    //            foreach (DataColumn _dataColumn in _dataSet.Tables[0].Columns)
        //    //            {
        //    //                Console.Write(_dataRow[_dataColumn.ColumnName] + ", ");
        //    //            }
        //    //            Console.WriteLine("");
        //    //        }
        //    //        if (_dbConn.State == ConnectionState.Open)
        //    //            _dbConn.Close();
        //    //    }
        //    //}
        //    //catch (Exception exp)
        //    //{
        //    //    Console.ForegroundColor = ConsoleColor.Red;
        //    //    Console.WriteLine(exp.Message);
        //    //}
        //}
    }
}
