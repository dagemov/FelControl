﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using the template for generating Repositories and a Unit of Work for EF Core model.
// Code is generated on: 7/29/2023 5:04:09 PM
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------
using System;

namespace FelControl
{
    public partial interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<UserAdress> UserAdresses { get; }
        IRepository<Client> Clients { get; }
        IRepository<Service> Services { get; }
        IRepository<ClientAdress> ClientAdresses { get; }
        IRepository<Schedule> Schedules { get; }
        IRepository<UserService> UserServices { get; }
        IRepository<ServiceAdress> ServiceAdresses { get; }
        void Save();
    }
}
