using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Database
{
    class Program
    {
        static void Main()
        {
            string Username;
            string Password;
            //Log in
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("What is your username?");
            Console.ForegroundColor = ConsoleColor.Gray;
            Username = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("What is your password?");
            Console.ForegroundColor = ConsoleColor.Gray;
            Password = ReadLine(true, false);

            Console.Clear();
            Showdatabases(Username, Password);
        }

        public static void Showdatabases(string UserName, string PassWord)
        {
            //This is a list with the names of databases.
            List<string> Databases = new List<string>();
            string Database = null;

            //Enter credentials
            MySql.Data.MySqlClient.MySqlConnection Connection;
            string ConnectionString;
            Connection = new MySql.Data.MySqlClient.MySqlConnection();

            ConnectionString = "server=127.0.0.1;uid=" + UserName + ";pwd=" + PassWord + ";";
            //Check if the exception is okay, if not give an error.
            try
            {
                Connection.ConnectionString = ConnectionString;
                Connection.Open();
            }
            catch (Exception ex)
            {
                Console.Write(Error(ex));
                Console.ReadLine();
                Console.Clear();
                Main();
            }

            try
            {
                //Create the connection.
                string sql = "SHOW DATABASES";
                MySqlCommand cmd = new MySqlCommand(sql, Connection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                //While there are things to read in [rdr]...
                while (rdr.Read())
                {
                    //Add them to the List.
                    Databases.Add(rdr[0].ToString());
                }
                //As long as you didn't enter a database...
                while (Database == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("These are the current databases.\nType in the name to pick one.\n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    //Read the content of the List and print them.
                    foreach (string Name in Databases)
                    {
                        Console.WriteLine(Name);
                    }
                    Console.WriteLine();
                    //Check what the entered string is and if it's correct.
                    string DatabaseInput = Console.ReadLine();
                    if (Databases.Contains(DatabaseInput))
                    {
                        Database = DatabaseInput;
                    }
                    //If you type "exit" then it will clear and close and then go to [Main].
                    else if (DatabaseInput.ToLower() == "exit")
                    {
                        Console.Clear();
                        Connection.Close();
                        Main();
                    }
                    //If it's not one of those then do this.
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(DatabaseInput + " is not a valid input");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                //If it gives an error it will automatically restart the [Showdatabases] method.
                Console.Write(Error(ex));
                Console.ReadLine();
                Showdatabases(UserName, PassWord);
            }
            //If there aren't any errors and the database name is entered correctly then go on to [Showtables].
            Connection.Close();
            Console.Clear();
            Showtables(UserName, PassWord, Database);
        }

        public static void Showtables(string UserName, string PassWord, string DataBase)
        {
            //This is a list with the names of tables.
            List<string> Tables = new List<string>();
            string Table = null;

            //Enter credentials and the name of the  database
            MySql.Data.MySqlClient.MySqlConnection ConnectionT;
            string ConnectionTString;
            ConnectionTString = "server=127.0.0.1;uid=" + UserName + ";pwd=" + PassWord + ";database=" + DataBase + ";";
            try
            {
                //Create the connection.
                ConnectionT = new MySql.Data.MySqlClient.MySqlConnection();
                ConnectionT.ConnectionString = ConnectionTString;
                ConnectionT.Open();

                string sqlT = "SHOW TABLES";
                MySqlCommand cmdT = new MySqlCommand(sqlT, ConnectionT);
                MySqlDataReader rdrT = cmdT.ExecuteReader();


                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("These are the current tables.\nType in the name to pick one.\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                //While there are things to read in [rdr]...
                while (rdrT.Read())
                {
                    //Add them to the List.
                    Tables.Add(rdrT[0].ToString());
                }
                //Print the tables.
                foreach (string Name in Tables)
                {
                    Console.WriteLine(Name);
                }
                //Keep repeating until the user has entered a correct table.
                while (Table == null)
                {

                    string TableInput = Console.ReadLine();
                    if (Tables.Contains(TableInput))
                    {
                        Table = TableInput;
                    }
                    //Go back to [Showdatabases] if you type "back".
                    else if (TableInput.ToLower() == "back")
                    {
                        Console.Clear();
                        Showdatabases(UserName, PassWord);
                    }
                    //Go back to [Main] if you type "exit".
                    else if (TableInput.ToLower() == "exit")
                    {
                        Console.Clear();
                        ConnectionT.Close();
                        Main();
                    }
                    //If it's not one of those then do this.
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(TableInput + " is not a valid input");
                        Console.ReadLine();
                        Console.Clear();
                        Showtables(UserName, PassWord, DataBase);
                    }
                }
                ConnectionT.Close();
            }
            catch (Exception ex)
            {
                Console.Write(Error(ex));
                Console.ReadLine();
                Showdatabases(UserName, PassWord);
            }
            //If there aren't any errors and the database name is entered correctly then go on to [Showdata].
            Console.Clear();
            Showdata(UserName, PassWord, DataBase, Table);

        }

        public static void Showdata(string UserName, string PassWord, string DataBase, string Table)
        {
            List<object> Data = new List<object>();

            //Two databases, one for the name and one for the actual data.
            //I couldn't get it to work with only one.
            MySql.Data.MySqlClient.MySqlConnection ConnectionDName;
            MySql.Data.MySqlClient.MySqlConnection ConnectionDData;
            string ConnectionDString;
            ConnectionDString = "server=127.0.0.1;uid=" + UserName + ";pwd=" + PassWord + ";database=" + DataBase + ";";
            try
            {
                //Create the connection.
                ConnectionDName = new MySql.Data.MySqlClient.MySqlConnection();
                ConnectionDName.ConnectionString = ConnectionDString;
                ConnectionDName.Open();

                string sqlD = "SELECT * FROM " + Table;
                MySqlCommand cmdD = new MySqlCommand(sqlD, ConnectionDName);
                MySqlDataReader rdrD = cmdD.ExecuteReader();
                int i = 0;

                //While there are things to read in [rdr]...
                while (rdrD.Read())
                {
                    //As long as [i] is not [rdrD.FieldCount]
                    while (i != rdrD.FieldCount)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        //Write the fieldname with 30 characters in between.
                        Console.Write("{0,-30} ", rdrD.GetName(i));
                        Console.ForegroundColor = ConsoleColor.Gray;
                        i++;
                    }
                }
                ConnectionDName.Close();

                //Create the connection.
                ConnectionDData = new MySql.Data.MySqlClient.MySqlConnection();
                ConnectionDData.ConnectionString = ConnectionDString;
                ConnectionDData.Open();

                string sqlDData = "SELECT * FROM " + Table;
                MySqlCommand cmdDData = new MySqlCommand(sqlDData, ConnectionDData);
                MySqlDataReader rdrDData = cmdDData.ExecuteReader();

                //While there are things to read in [rdr]...
                while (rdrDData.Read())
                {
                    Console.WriteLine();
                    for (int ii = 0; ii < rdrDData.FieldCount; ii++)
                    {
                        string s = rdrDData[ii].ToString();

                        //if [s.Lenght] = bigger than 29
                        if (s.Length > 29)
                        {
                            //Cut it to 26 characters and add "..."
                            s = s.Substring(0, 26) + "...";
                        }
                        //Skip 30 characters and write [s]
                        Console.Write("{0,-30} ", s);
                    }
                }
                Console.WriteLine();
                ConnectionDData.Close();
            }
            catch (Exception ex)
            {
                Console.Write(Error(ex));
                Console.ReadLine();
                Showdatabases(UserName, PassWord);
            }
            //Let people actually read it and go back to [Showtables]
            ReadLine(true, false);
            Console.Clear();
            Showtables(UserName, PassWord, DataBase);
        }

        public static string ReadLine(bool password, bool debug = false)
        {
            //You do add things to a string but don't display it
            string output = "";
            while (true)
            {
                ConsoleKeyInfo c;
                if (password)
                    c = Console.ReadKey(true);
                else
                    c = Console.ReadKey();
                if (c.Key == ConsoleKey.Backspace)
                {
                    if (output.Length > 1)
                    {
                        output = output.Substring(0, output.Length - 1);
                    }
                }
                else if (c.Key == ConsoleKey.Enter)
                {
                    if (debug)
                        Console.WriteLine(output);
                    return output;
                }
                else
                {
                    output += c.KeyChar;
                }
            }
        }

        public static string Error(Exception ex)
        {
            //Error message.
            string ErrorMessage;
            Console.Clear();
            if (ex.Message.Contains("Access denied"))
            {
                ErrorMessage = "Invalid username or password";
            }
            else if (ex.Message.Contains("Unable to connect"))
            {
                ErrorMessage = ex.Message;
            }
            else {
                ErrorMessage = "We have an error! " + ex;
            }
            return ErrorMessage;
        }
    }
}
