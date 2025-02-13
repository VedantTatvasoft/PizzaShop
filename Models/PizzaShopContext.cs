using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pizzashop_dotnet.Models;

public partial class PizzaShopContext : DbContext
{
    public PizzaShopContext()
    {
    }

    public PizzaShopContext(DbContextOptions<PizzaShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemModifierMapping> ItemModifierMappings { get; set; }

    public virtual DbSet<Modifier> Modifiers { get; set; }

    public virtual DbSet<Modifiersgroup> Modifiersgroups { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Paymentmode> Paymentmodes { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermissionMapping> RolePermissionMappings { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<Taxis> Taxes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Waitingtoken> Waitingtokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=PizzaShop;Username=postgres;Password=Tatva@123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("categories_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Cityid).HasName("city_pkey");

            entity.HasOne(d => d.State).WithMany(p => p.Cities).HasConstraintName("city_stateid_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Countryid).HasName("country_pkey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customer_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Itemid).HasName("items_pkey");

            entity.Property(e => e.Available).HasDefaultValueSql("false");
            entity.Property(e => e.Defaulttax).HasDefaultValueSql("false");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
            entity.Property(e => e.Taxpercentage).HasDefaultValueSql("false");

            entity.HasOne(d => d.Category).WithMany(p => p.Items).HasConstraintName("items_categoryid_fkey");
        });

        modelBuilder.Entity<ItemModifierMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("item_modifier_mapping_pkey");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemModifierMappings).HasConstraintName("item_modifier_mapping_itemid_fkey");

            entity.HasOne(d => d.Modifiergroup).WithMany(p => p.ItemModifierMappings).HasConstraintName("item_modifier_mapping_modifiergroupid_fkey");
        });

        modelBuilder.Entity<Modifier>(entity =>
        {
            entity.HasKey(e => e.Modifierid).HasName("modifiers_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");

            entity.HasOne(d => d.Modifiergroup).WithMany(p => p.Modifiers).HasConstraintName("modifiers_modifiergroupid_fkey");
        });

        modelBuilder.Entity<Modifiersgroup>(entity =>
        {
            entity.HasKey(e => e.Modifiergroupid).HasName("modifiersgroup_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");

            entity.HasOne(d => d.Category).WithMany(p => p.Modifiersgroups).HasConstraintName("modifiersgroup_categoryid_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("orders_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
            entity.Property(e => e.Status).HasDefaultValueSql("false");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders).HasConstraintName("orders_customerid_fkey");

            entity.HasOne(d => d.Paymentmode).WithMany(p => p.Orders).HasConstraintName("orders_paymentmodeid_fkey");
        });

        modelBuilder.Entity<Paymentmode>(entity =>
        {
            entity.HasKey(e => e.Paymentmodeid).HasName("paymentmode_pkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Permissionid).HasName("permissions_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Ratingid).HasName("rating_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");

            entity.HasOne(d => d.Customer).WithMany(p => p.Ratings).HasConstraintName("rating_customerid_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Ratings).HasConstraintName("rating_orderid_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<RolePermissionMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_permission_mapping_pkey");

            entity.Property(e => e.Canaddedit).HasDefaultValueSql("false");
            entity.Property(e => e.Candelete).HasDefaultValueSql("false");
            entity.Property(e => e.Canview).HasDefaultValueSql("false");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissionMappings).HasConstraintName("role_permission_mapping_permissionid_fkey");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Sectionid).HasName("section_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Stateid).HasName("state_pkey");

            entity.HasOne(d => d.Country).WithMany(p => p.States).HasConstraintName("state_countryid_fkey");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Tableid).HasName("tables_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
            entity.Property(e => e.Status).HasDefaultValueSql("'available'::character varying");

            entity.HasOne(d => d.Section).WithMany(p => p.Tables).HasConstraintName("tables_sectionid_fkey");
        });

        modelBuilder.Entity<Taxis>(entity =>
        {
            entity.HasKey(e => e.Taxid).HasName("taxes_pkey");

            entity.Property(e => e.Isdefault).HasDefaultValueSql("false");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
            entity.Property(e => e.Isenable).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("now()");
            entity.Property(e => e.Createdby).HasDefaultValueSql("'Admin'::character varying");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
            entity.Property(e => e.Status).HasDefaultValueSql("true");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("now()");
            entity.Property(e => e.Updatedby).HasDefaultValueSql("'Admin'::character varying");

            entity.HasOne(d => d.CityNavigation).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_city_fkey");

            entity.HasOne(d => d.CountryNavigation).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_country_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasConstraintName("users_roleid_fkey");

            entity.HasOne(d => d.StateNavigation).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_state_fkey");
        });

        modelBuilder.Entity<Waitingtoken>(entity =>
        {
            entity.HasKey(e => e.Waitingid).HasName("waitingtoken_pkey");

            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
            entity.Property(e => e.RequestedTime).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Customer).WithMany(p => p.Waitingtokens)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("waitingtoken_customerid_fkey");

            entity.HasOne(d => d.Section).WithMany(p => p.Waitingtokens).HasConstraintName("waitingtoken_sectionid_fkey");

            entity.HasOne(d => d.Table).WithMany(p => p.Waitingtokens)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("waitingtoken_tableid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
