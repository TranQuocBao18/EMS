﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMS.Database.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<ProductModel> Products { get; set; }
		public DbSet<UserModel> Users { get; set; }
		public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
		public DbSet<UserRoleModel> UserRoles { get; set; }
		public DbSet<RoleModel> Roles { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<EquipmentModel> Equipments { get; set; }
        public DbSet<EquipmentTypeModel> EquipmentTypes { get; set; }
        public DbSet<RotatingRequestModel> RotatingRequests { get; set; }


	}
}
