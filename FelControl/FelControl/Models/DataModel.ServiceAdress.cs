﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 7/29/2023 5:04:08 PM
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace FelControl
{
    public partial class ServiceAdress {

        public ServiceAdress()
        {
            this.AdressService = new Adress();
            OnCreated();
        }

        public int Id { get; set; }

        public Adress AdressService { get; set; }

        public int ServiceId { get; set; }

        public virtual Service Service { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
