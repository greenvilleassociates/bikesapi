﻿using System;
using System.Collections.Generic;

namespace dirtbike.api.Models;

public partial class Apilog
{
    public int Id { get; set; }

    public string? Apiname { get; set; }

    public string? Apinumber { get; set; }

    public string? Eptype { get; set; }

    public int? Hashid { get; set; }

    public string? Parameterlist { get; set; }

    public string? Apiresult { get; set; }
}
