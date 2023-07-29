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
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using FelControl;

namespace FelControl.Data
{

    public partial class ApplicationDbContext : IdentityDbContext<User>
    {

        public ApplicationDbContext() :
            base()
        {
            OnCreated();
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {
            OnCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured ||
                (!optionsBuilder.Options.Extensions.OfType<RelationalOptionsExtension>().Any(ext => !string.IsNullOrEmpty(ext.ConnectionString) || ext.Connection != null) &&
                 !optionsBuilder.Options.Extensions.Any(ext => !(ext is RelationalOptionsExtension) && !(ext is CoreOptionsExtension))))
            {
                optionsBuilder.UseSqlServer(@"Data Source=Dagemov\SQLEXPRESS;Initial Catalog=FelControl;Integrated Security=True;Persist Security Info=False;Password=");
            }
            CustomizeConfiguration(ref optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);

        public virtual DbSet<User> Users
        {
            get;
            set;
        }

        public virtual DbSet<UserAdress> UserAdresses
        {
            get;
            set;
        }

        public virtual DbSet<Client> Clients
        {
            get;
            set;
        }

        public virtual DbSet<Service> Services
        {
            get;
            set;
        }

        public virtual DbSet<ClientAdress> ClientAdresses
        {
            get;
            set;
        }

        public virtual DbSet<Schedule> Schedules
        {
            get;
            set;
        }

        public virtual DbSet<UserService> UserServices
        {
            get;
            set;
        }

        public virtual DbSet<ServiceAdress> ServiceAdresses
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.UserMapping(modelBuilder);
            this.CustomizeUserMapping(modelBuilder);

            this.UserAdressMapping(modelBuilder);
            this.CustomizeUserAdressMapping(modelBuilder);

            this.ClientMapping(modelBuilder);
            this.CustomizeClientMapping(modelBuilder);

            this.ServiceMapping(modelBuilder);
            this.CustomizeServiceMapping(modelBuilder);

            this.ClientAdressMapping(modelBuilder);
            this.CustomizeClientAdressMapping(modelBuilder);

            this.ScheduleMapping(modelBuilder);
            this.CustomizeScheduleMapping(modelBuilder);

            this.UserServiceMapping(modelBuilder);
            this.CustomizeUserServiceMapping(modelBuilder);

            this.ServiceAdressMapping(modelBuilder);
            this.CustomizeServiceAdressMapping(modelBuilder);

            RelationshipsMapping(modelBuilder);
            CustomizeMapping(ref modelBuilder);
        }

        #region User Mapping

        private void UserMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(@"Users");
            modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(x => x.Name).HasColumnName(@"Name").ValueGeneratedNever().HasMaxLength(80);
            modelBuilder.Entity<User>().Property(x => x.LastName).HasColumnName(@"LastName").ValueGeneratedNever();
            modelBuilder.Entity<User>().Property(x => x.TazId).HasColumnName(@"TazId").ValueGeneratedNever();
            modelBuilder.Entity<User>().Property(x => x.HourValue).HasColumnName(@"HourValue").IsRequired().ValueGeneratedNever().HasMaxLength(100);
            modelBuilder.Entity<User>().HasKey(@"Id");
        }

        partial void CustomizeUserMapping(ModelBuilder modelBuilder);

        #endregion

        #region UserAdress Mapping

        private void UserAdressMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAdress>().ToTable(@"UserAdresses");
            modelBuilder.Entity<UserAdress>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<UserAdress>().OwnsOne(t => t.adress).Property(x => x.Country).HasColumnName(@"Country").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<UserAdress>().OwnsOne(t => t.adress).Property(x => x.City).HasColumnName(@"City").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<UserAdress>().OwnsOne(t => t.adress).Property(x => x.Street).HasColumnName(@"Street").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<UserAdress>().Property(x => x.UserId).HasColumnName(@"UserId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<UserAdress>().HasKey(@"Id");
        }

        partial void CustomizeUserAdressMapping(ModelBuilder modelBuilder);

        #endregion

        #region Client Mapping

        private void ClientMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable(@"Clients");
            modelBuilder.Entity<Client>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Client>().Property(x => x.Name).HasColumnName(@"Name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Client>().Property(x => x.LastName).HasColumnName(@"LastName").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Client>().OwnsOne(t => t.Phones).Property(x => x.MainPhone).HasColumnName(@"MainPhone").IsRequired().ValueGeneratedNever().HasMaxLength(10);
            modelBuilder.Entity<Client>().OwnsOne(t => t.Phones).Property(x => x.Phone2).HasColumnName(@"Phone2").IsRequired().ValueGeneratedNever().HasMaxLength(10);
            modelBuilder.Entity<Client>().OwnsOne(t => t.Phones).Property(x => x.Phone3).HasColumnName(@"Phone3").IsRequired().ValueGeneratedNever().HasMaxLength(10);
            modelBuilder.Entity<Client>().HasKey(@"Id");
        }

        partial void CustomizeClientMapping(ModelBuilder modelBuilder);

        #endregion

        #region Service Mapping

        private void ServiceMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>().ToTable(@"Services");
            modelBuilder.Entity<Service>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Service>().Property(x => x.Name).HasColumnName(@"Name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Service>().Property(x => x.Description).HasColumnName(@"Description").IsRequired().ValueGeneratedNever().HasMaxLength(500);
            modelBuilder.Entity<Service>().Property(x => x.ImagenPrincipal).HasColumnName(@"ImagenPrincipal").IsRequired().ValueGeneratedNever().HasMaxLength(500);
            modelBuilder.Entity<Service>().Property(x => x.Price).HasColumnName(@"Price").IsRequired().ValueGeneratedNever().HasMaxLength(10000000);
            modelBuilder.Entity<Service>().OwnsOne(t => t.ContractService).Property(x => x.ExpeditionDate).HasColumnName(@"ExpeditionDate").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Service>().OwnsOne(t => t.ContractService).Property(x => x.Details).HasColumnName(@"Details").IsRequired().ValueGeneratedNever().HasMaxLength(500);
            modelBuilder.Entity<Service>().OwnsOne(t => t.ContractService).Property(x => x.Rules).HasColumnName(@"Rules").IsRequired().ValueGeneratedNever().HasMaxLength(500);
            modelBuilder.Entity<Service>().OwnsOne(t => t.ContractService).Property(x => x.EndDate).HasColumnName(@"EndDate").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Service>().Property(x => x.ClientId).HasColumnName(@"ClientId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Service>().HasKey(@"Id");
        }

        partial void CustomizeServiceMapping(ModelBuilder modelBuilder);

        #endregion

        #region ClientAdress Mapping

        private void ClientAdressMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientAdress>().ToTable(@"ClientAdresses");
            modelBuilder.Entity<ClientAdress>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<ClientAdress>().Property(x => x.ClientId).HasColumnName(@"ClientId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ClientAdress>().OwnsOne(t => t.Adress).Property(x => x.Country).HasColumnName(@"Country").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ClientAdress>().OwnsOne(t => t.Adress).Property(x => x.City).HasColumnName(@"City").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ClientAdress>().OwnsOne(t => t.Adress).Property(x => x.Street).HasColumnName(@"Street").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ClientAdress>().HasKey(@"Id", @"ClientId");
        }

        partial void CustomizeClientAdressMapping(ModelBuilder modelBuilder);

        #endregion

        #region Schedule Mapping

        private void ScheduleMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>().ToTable(@"Schedules");
            modelBuilder.Entity<Schedule>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Schedule>().Property(x => x.BeginHour).HasColumnName(@"BeginHour").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Schedule>().Property(x => x.EndHour).HasColumnName(@"EndHour").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Schedule>().Property(x => x.UserServiceServiceId).HasColumnName(@"UserServiceServiceId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Schedule>().Property(x => x.UserId).HasColumnName(@"UserId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Schedule>().HasKey(@"Id");
        }

        partial void CustomizeScheduleMapping(ModelBuilder modelBuilder);

        #endregion

        #region UserService Mapping

        private void UserServiceMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserService>().ToTable(@"UserServices");
            modelBuilder.Entity<UserService>().Property(x => x.ServiceId).HasColumnName(@"ServiceId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<UserService>().Property(x => x.UserId).HasColumnName(@"UserId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<UserService>().HasKey(@"ServiceId", @"UserId");
        }

        partial void CustomizeUserServiceMapping(ModelBuilder modelBuilder);

        #endregion

        #region ServiceAdress Mapping

        private void ServiceAdressMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceAdress>().ToTable(@"ServiceAdresses");
            modelBuilder.Entity<ServiceAdress>().Property(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ServiceAdress>().OwnsOne(t => t.AdressService).Property(x => x.Country).HasColumnName(@"Country").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ServiceAdress>().OwnsOne(t => t.AdressService).Property(x => x.City).HasColumnName(@"City").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ServiceAdress>().OwnsOne(t => t.AdressService).Property(x => x.Street).HasColumnName(@"Street").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ServiceAdress>().Property(x => x.ServiceId).HasColumnName(@"ServiceId").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<ServiceAdress>().HasKey(@"Id");
        }

        partial void CustomizeServiceAdressMapping(ModelBuilder modelBuilder);

        #endregion

        private void RelationshipsMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(x => x.UserAdresses).WithOne(op => op.User).HasForeignKey(@"UserId").IsRequired(true);
            modelBuilder.Entity<User>().HasMany(x => x.UserServices).WithOne(op => op.User).HasForeignKey(@"UserId").IsRequired(true);

            modelBuilder.Entity<UserAdress>().HasOne(x => x.User).WithMany(op => op.UserAdresses).HasForeignKey(@"UserId").IsRequired(true);

            modelBuilder.Entity<Client>().HasMany(x => x.ClientAdresses).WithOne(op => op.Client).HasForeignKey(@"ClientId").IsRequired(true);
            modelBuilder.Entity<Client>().HasMany(x => x.Services).WithOne(op => op.Client).HasForeignKey(@"ClientId").IsRequired(true);

            modelBuilder.Entity<Service>().HasMany(x => x.UserServices).WithOne(op => op.Service).HasForeignKey(@"ServiceId").IsRequired(true);
            modelBuilder.Entity<Service>().HasOne(x => x.Client).WithMany(op => op.Services).HasForeignKey(@"ClientId").IsRequired(true);
            modelBuilder.Entity<Service>().HasMany(x => x.ServiceAdresses).WithOne(op => op.Service).HasForeignKey(@"ServiceId").IsRequired(true);

            modelBuilder.Entity<ClientAdress>().HasOne(x => x.Client).WithMany(op => op.ClientAdresses).HasForeignKey(@"ClientId").IsRequired(true);

            modelBuilder.Entity<Schedule>().HasOne(x => x.UserService).WithMany(op => op.Schedules).HasForeignKey(@"UserServiceServiceId", @"UserId").IsRequired(true);

            modelBuilder.Entity<UserService>().HasOne(x => x.Service).WithMany(op => op.UserServices).HasForeignKey(@"ServiceId").IsRequired(true);
            modelBuilder.Entity<UserService>().HasOne(x => x.User).WithMany(op => op.UserServices).HasForeignKey(@"UserId").IsRequired(true);
            modelBuilder.Entity<UserService>().HasMany(x => x.Schedules).WithOne(op => op.UserService).HasForeignKey(@"UserServiceServiceId", @"UserId").IsRequired(true);

            modelBuilder.Entity<ServiceAdress>().HasOne(x => x.Service).WithMany(op => op.ServiceAdresses).HasForeignKey(@"ServiceId").IsRequired(true);
        }

        partial void CustomizeMapping(ref ModelBuilder modelBuilder);

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified || e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);
        }

        partial void OnCreated();
    }
}
