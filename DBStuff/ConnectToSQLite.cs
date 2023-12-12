using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

namespace DBStuff
{
    internal class ConnectToSQLite
    {

        public SQLiteConnection con;
        private string dbfileName = "./zDBStuff.sqlite3";
        private string dbschemaFile = "../../ySQLiteDBStuffSchema.sql";  //go up 2 directories to find file
        
        internal string SQLiteconnectionstring { get; set; }

        private string connection;

        public void getconnection()
        {
            //connection = $"Data Source={dbfileName};Version=3;Compress=True;FailIfMissing=True;UTF16Encoding=True;UseUTF16Encoding=True;Cache Size=3000;Page Size=1024;Journal Mode=Persist;";
            //connection = $"Data Source={dbfileName};Version=3;New=True;Compress=True;FailIfMissing=True;Cache Size=3000;Page Size=1024;";
            connection = $"Data Source={dbfileName};Version=3;Compress=True;FailIfMissing=True;Synchronous=OFF;Journal Mode=WAL;";
            SQLiteconnectionstring = connection;

            System.Console.WriteLine(SQLiteconnectionstring);
        }

        internal ConnectToSQLite()
        {
            getconnection();
            con = new SQLiteConnection(SQLiteconnectionstring);
            if (!File.Exists($"{dbfileName}"))
            {
                SQLiteConnection.CreateFile($"{dbfileName}");
                ConOpen();
                ApplySchema();
                ConClose();
            }
            else
            {
                string version = con.ServerVersion;
                System.Console.WriteLine($"YOU ALREADY HAVE AN EXISTING SQLite Version: {version} DATABASE FILE AND WE WILL USE IT");
            }
        }


        internal void ConOpen()
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
        }
        internal void ConClose()
        {
            if (con.State != System.Data.ConnectionState.Closed)
                con.Close();
        }
        private void ApplySchema()
        {
            ConOpen();

            //you can change " = File.ReadAllText" to =@"CREAT TABLE something(OrderID   INTEGER, blah  TEXT)";
            string dbschema = File.ReadAllText($"{dbschemaFile}");
            var schemaInit = con.CreateCommand();
            schemaInit.CommandText = dbschema;
            schemaInit.ExecuteNonQuery();

            ConClose();

            System.Console.WriteLine(dbschema);
            string version = con.ServerVersion;
            System.Console.WriteLine($"WE JUST CREATED A SQLite Version: {version} DATABASE FOR YOU");
        }




    }
}
