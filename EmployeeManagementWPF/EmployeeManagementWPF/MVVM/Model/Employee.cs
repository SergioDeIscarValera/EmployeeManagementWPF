using System;
using System.Text.Json.Serialization;

namespace EmployeeManagementWPF.MVVM.Model;

internal class Employee
{
    public string Id { get; private set; }
    public string Dni { get; private set; }
    public string Name { get; private set; }
    public string UrlImage { get; private set; }
    public string Position { get; private set; }
    public string IdDepartment { get; private set; }

    [JsonConstructor]
    public Employee(string id, string dni, string name, string urlImage, string position, string idDepartment)
    {
        Id = id;
        Name = name;
        Dni = dni;
        UrlImage = urlImage;
        Position = position;
        IdDepartment = idDepartment;
    }

    public Employee(string dni, string name, string urlImage, string position, string idDepartment)
    {
        Id = Guid.NewGuid().ToString();
        Dni = dni;
        Name = name;
        UrlImage = urlImage;
        Position = position;
        IdDepartment = idDepartment;
    }
}
