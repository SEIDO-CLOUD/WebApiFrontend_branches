using System;

namespace Models.DTO;

public class GstUsrInfoDbDto
{
    public string Title {get;  set;}
} 


public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
}


