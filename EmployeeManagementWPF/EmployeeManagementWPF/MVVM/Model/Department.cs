using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Text.Json.Serialization;

namespace EmployeeManagementWPF.MVVM.Model;

internal class Department : ObservableObject
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string UrlImage { get; private set; }

    [JsonConstructor]
    public Department(string id, string name, string description, string urlImage)
    {
        Id = id;
        Name = name;
        Description = description;
        UrlImage = urlImage;
    }

    public Department(string name, string description, string urlImage)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Description = description;
        UrlImage = urlImage;
    }
}
