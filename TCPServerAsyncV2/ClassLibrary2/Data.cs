using System.Collections.Generic;
using System.Data.SQLite;

namespace DataBaseLibrary
{
    public class Data
    {
        #region Fields
        /// <summary>
        /// Objekt niezbędny do utworzenia połączenia z bazą danych
        /// </summary>
        SQLiteConnection connect;
        /// <summary>
        /// Lista kont użytkowników
        /// </summary>
        List<Account> accounts = new List<Account>();

        /// <summary>
        /// Klasa reprezentująca konta użytkowników
        /// </summary>
        public class Account
        {
            public int? ID { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }
        #endregion

        #region Metods
        /// <summary>
        /// Metoda służąca do tworzenia połączenia i jego uruchomienia
        /// </summary>
        /// <param name="con"></param>
        public void setConn(SQLiteConnection con)
        {
            connect = con;
            connect.Open();
        }

        /// <summary>
        /// Metoda pobierająca dane i wpisująca je do listy
        /// </summary>
        public List<Account> Accounts
        {
            get
            {
                var cmd = connect.CreateCommand();
                cmd.CommandText = "SELECT * FROM Accounts";
                var reader = cmd.ExecuteReader();
                accounts.Clear();
                while (reader.Read())
                {
                    accounts.Add(new Account
                    {
                        ID = reader.GetInt32(0),
                        Login = reader.GetString(1),
                        Password = reader.GetString(2)
                    });
                }
                return accounts;
            }
        }

        /// <summary>
        /// Metoda zapisująca lub aktualizująca zmiany w bazie danych
        /// </summary>
        public void SaveOrUpdate()
        {
            var cmd = connect.CreateCommand();
            foreach (var element in accounts)
            {
                try
                {
                    cmd.CommandText =
                        $"INSERT INTO Accounts (username, password) VALUES ('{element.Login}', '{element.Password}');";
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException)
                {
                    cmd.CommandText =
                        $"UPDATE Accounts SET username='{element.Login}', password='{element.Password}' WHERE id={element.ID};";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Metoda tworząca tabele w bazie danych i wypelniająca ją podstawowymi infromacjami 
        /// </summary>
        public void CreateDatabase()
        {
            var cmd = connect.CreateCommand();

            cmd.CommandText = "DROP TABLE IF EXISTS Accounts";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"CREATE TABLE Accounts (id INTEGER PRIMARY KEY AUTOINCREMENT, username VARCHAR(20) UNIQUE NOT NULL, password VARCHAR(64) NOT NULL);";
            cmd.ExecuteNonQuery();

            accounts.Add(new Account{Login = "admin", Password = "password"});
            accounts.Add(new Account{Login = "user", Password = "user"});

            SaveOrUpdate();
        }
        #endregion
    }
}
