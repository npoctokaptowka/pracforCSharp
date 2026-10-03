using System.Collections.Generic;

namespace Clinic
{
    public enum FieldKind
    {
        Text,
        Name,
        Int,
        Date,
        DateTime,
        Phone,
        Lookup
    }

    public class FieldInfo
    {
        public string Name;
        public string Caption;
        public FieldKind Kind;
        public int MaxLength = 100;
        public int Min;
        public int Max;
        public string LookupSql;
    }

    public class TableInfo
    {
        public string Name;
        public string Title;
        public string SelectSql;
        public List<FieldInfo> Fields;
    }

    public static class Tables
    {
        public static TableInfo Cabinets = new TableInfo
        {
            Name = "Cabinets",
            Title = "Кабинеты",
            SelectSql = "SELECT Id AS [№], Number AS [Номер кабинета], Floor AS [Этаж] FROM Cabinets ORDER BY Id",
            Fields = new List<FieldInfo>
            {
                new FieldInfo { Name = "Number", Caption = "Номер кабинета", Kind = FieldKind.Text, MaxLength = 10 },
                new FieldInfo { Name = "Floor", Caption = "Этаж", Kind = FieldKind.Int, Min = 1, Max = 20 }
            }
        };

        public static TableInfo Doctors = new TableInfo
        {
            Name = "Doctors",
            Title = "Врачи",
            SelectSql = "SELECT d.Id AS [№], d.FullName AS [ФИО], d.Specialty AS [Специальность], c.Number AS [Кабинет] " +
                        "FROM Doctors d JOIN Cabinets c ON c.Id = d.CabinetId ORDER BY d.Id",
            Fields = new List<FieldInfo>
            {
                new FieldInfo { Name = "FullName", Caption = "ФИО", Kind = FieldKind.Name },
                new FieldInfo { Name = "Specialty", Caption = "Специальность", Kind = FieldKind.Name, MaxLength = 50 },
                new FieldInfo { Name = "CabinetId", Caption = "Кабинет", Kind = FieldKind.Lookup,
                    LookupSql = "SELECT Id, Number || ' (этаж ' || Floor || ')' AS Name FROM Cabinets ORDER BY Number" }
            }
        };

        public static TableInfo Patients = new TableInfo
        {
            Name = "Patients",
            Title = "Пациенты",
            SelectSql = "SELECT Id AS [№], FullName AS [ФИО], BirthDate AS [Дата рождения], Phone AS [Телефон] FROM Patients ORDER BY Id",
            Fields = new List<FieldInfo>
            {
                new FieldInfo { Name = "FullName", Caption = "ФИО", Kind = FieldKind.Name },
                new FieldInfo { Name = "BirthDate", Caption = "Дата рождения (ДД.ММ.ГГГГ)", Kind = FieldKind.Date },
                new FieldInfo { Name = "Phone", Caption = "Телефон", Kind = FieldKind.Phone, MaxLength = 20 }
            }
        };

        public static TableInfo Appointments = new TableInfo
        {
            Name = "Appointments",
            Title = "Приёмы",
            SelectSql = "SELECT a.Id AS [№], p.FullName AS [Пациент], d.FullName AS [Врач], d.Specialty AS [Специальность], " +
                        "a.VisitTime AS [Дата и время], a.Complaint AS [Жалоба] " +
                        "FROM Appointments a JOIN Patients p ON p.Id = a.PatientId JOIN Doctors d ON d.Id = a.DoctorId ORDER BY a.Id",
            Fields = new List<FieldInfo>
            {
                new FieldInfo { Name = "PatientId", Caption = "Пациент", Kind = FieldKind.Lookup,
                    LookupSql = "SELECT Id, FullName AS Name FROM Patients ORDER BY FullName" },
                new FieldInfo { Name = "DoctorId", Caption = "Врач", Kind = FieldKind.Lookup,
                    LookupSql = "SELECT Id, FullName || ' (' || Specialty || ')' AS Name FROM Doctors ORDER BY FullName" },
                new FieldInfo { Name = "VisitTime", Caption = "Дата и время (ДД.ММ.ГГГГ ЧЧ:ММ)", Kind = FieldKind.DateTime },
                new FieldInfo { Name = "Complaint", Caption = "Жалоба", Kind = FieldKind.Text, MaxLength = 200 }
            }
        };
    }
}
