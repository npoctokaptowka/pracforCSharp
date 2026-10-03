using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace Clinic
{
    public static class Db
    {
        private const string ConnectionString = "Data Source=clinic.db";

        private static SqliteConnection Open()
        {
            SqliteConnection connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using (SqliteCommand pragma = connection.CreateCommand())
            {
                pragma.CommandText = "PRAGMA foreign_keys = ON;";
                pragma.ExecuteNonQuery();
            }
            return connection;
        }

        private static void AddParameters(SqliteCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null)
                return;
            foreach (KeyValuePair<string, object> p in parameters)
            {
                command.Parameters.AddWithValue("@" + p.Key, p.Value);
            }
        }

        public static DataTable Query(string sql, Dictionary<string, object> parameters = null)
        {
            using (SqliteConnection connection = Open())
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                AddParameters(command, parameters);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    DataTable table = new DataTable();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        table.Columns.Add(reader.GetName(i), typeof(object));
                    }
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        table.Rows.Add(values);
                    }
                    return table;
                }
            }
        }

        public static void Execute(string sql, Dictionary<string, object> parameters = null)
        {
            using (SqliteConnection connection = Open())
            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                AddParameters(command, parameters);
                command.ExecuteNonQuery();
            }
        }

        public static void Initialize()
        {
            Execute(@"
CREATE TABLE IF NOT EXISTS Cabinets (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Number TEXT NOT NULL UNIQUE,
    Floor INTEGER NOT NULL
);
CREATE TABLE IF NOT EXISTS Doctors (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,
    Specialty TEXT NOT NULL,
    CabinetId INTEGER NOT NULL REFERENCES Cabinets(Id) ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS Patients (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,
    BirthDate TEXT NOT NULL,
    Phone TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS Appointments (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId INTEGER NOT NULL REFERENCES Patients(Id) ON DELETE CASCADE,
    DoctorId INTEGER NOT NULL REFERENCES Doctors(Id) ON DELETE CASCADE,
    VisitTime TEXT NOT NULL,
    Complaint TEXT NOT NULL
);");

            long count = Convert.ToInt64(Query("SELECT COUNT(*) FROM Cabinets").Rows[0][0]);
            if (count == 0)
            {
                Execute(@"
INSERT INTO Cabinets (Number, Floor) VALUES ('101', 1), ('102', 1), ('205', 2), ('210', 2), ('301', 3);
INSERT INTO Doctors (FullName, Specialty, CabinetId) VALUES
    ('Иванов Пётр Сергеевич', 'Терапевт', 1),
    ('Смирнова Анна Викторовна', 'Хирург', 2),
    ('Кузнецов Олег Игоревич', 'Окулист', 3),
    ('Попова Мария Андреевна', 'Кардиолог', 4);
INSERT INTO Patients (FullName, BirthDate, Phone) VALUES
    ('Соколов Дмитрий Алексеевич', '12.03.1990', '+7 900 123-45-67'),
    ('Морозова Елена Павловна', '25.07.1985', '+7 901 234-56-78'),
    ('Волков Игорь Николаевич', '03.11.2001', '+7 902 345-67-89'),
    ('Лебедева Ольга Сергеевна', '18.01.1978', '+7 903 456-78-90');
INSERT INTO Appointments (PatientId, DoctorId, VisitTime, Complaint) VALUES
    (1, 1, '05.10.2026 09:00', 'Кашель и температура'),
    (2, 4, '05.10.2026 10:30', 'Боли в сердце'),
    (3, 3, '06.10.2026 11:00', 'Ухудшение зрения'),
    (4, 2, '07.10.2026 14:15', 'Плановый осмотр после операции');");
            }
        }
    }
}
