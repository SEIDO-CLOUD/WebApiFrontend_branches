using System;

namespace Models.DTO;

public class GstUsrInfoDbDto
{
    public string Title {get;  set;}
    public int NrSeededZoos { get; set; } = 0;
    public int NrUnseededZoos { get; set; } = 0;

    public int NrSeededAnimals { get; set; } = 0;
    public int NrUnseededAnimals { get; set; } = 0;

    public int NrSeededEmployees { get; set; } = 0;
    public int NrUnseededEmployees { get; set; } = 0;
}

public class GstUsrInfoZoosDto
{
    public string Country { get; set; } = null;
    public string City { get; set; } = null;
    public int NrZoos { get; set; } = 0;
}

public class GstUsrInfoAnimalsDto
{
    public string Country { get; set; } = null;
    public string City { get; set; } = null;
    public string ZooName { get; set; } = null;
    public int NrAnimals { get; set; } = 0;
}

public class GstUsrInfoEmployeesDto
{
    public string Country { get; set; } = null;
    public string City { get; set; } = null;
    public string ZooName { get; set; } = null;
    public int NrEmployees { get; set; } = 0;
}

public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
    public List<GstUsrInfoZoosDto> Zoos { get; set; } = null;
    public List<GstUsrInfoAnimalsDto> Animals { get; set; } = null;
    public List<GstUsrInfoEmployeesDto> Employees { get; set; } = null;
}


