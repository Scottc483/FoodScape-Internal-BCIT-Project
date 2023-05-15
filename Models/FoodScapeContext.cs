using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Food_Scape.ViewModels;

namespace Food_Scape.Models;

public partial class FoodScapeContext : IdentityDbContext<IdentityUser>
{
    public FoodScapeContext()
    {
    }

    public FoodScapeContext(DbContextOptions<FoodScapeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventVendor> EventVendors { get; set; }

    public virtual DbSet<FsUser> FsUsers { get; set; }

    public virtual DbSet<Ipn> Ipns { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketType> TicketTypes { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Event__2DC7BD69A347E630");

            entity.ToTable("Event");

            entity.Property(e => e.EventId).HasColumnName("eventID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.AgeRestriction).HasColumnName("ageRestriction");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("imageURL");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");
        });

        modelBuilder.Entity<EventVendor>(entity =>
        {
            entity.HasKey(e => e.EventVendorId).HasName("PK__EventVen__2F64F1CFE1B1629B");

            entity.ToTable("EventVendor");

            entity.Property(e => e.EventVendorId).HasColumnName("eventVendorID");
            entity.Property(e => e.EventId).HasColumnName("eventID");
            entity.Property(e => e.VendorId).HasColumnName("vendorID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventVendors)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__EventVend__event__47FBA9D6");

            entity.HasOne(d => d.Vendor).WithMany(p => p.EventVendors)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("FK__EventVend__vendo__48EFCE0F");
        });

        modelBuilder.Entity<FsUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__FsUser__CB9A1CDF6AA008B5");

            entity.ToTable("FsUser");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("lastName");
        });

        modelBuilder.Entity<Ipn>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__IPN__A0D9EFA64E51E78E");

            entity.ToTable("IPN");

            entity.Property(e => e.PaymentId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("paymentID");
            entity.Property(e => e.Amount)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("amount");
            entity.Property(e => e.Currency)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("paymentMethod");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Ipns)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__IPN__userID__4BCC3ABA");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__2ECD6E243F76DBC6");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("reviewID");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Review1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("review");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Review__userID__3AA1AEB8");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__3333C67042C3B02A");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).HasColumnName("ticketID");
            entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.TicketType).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketTypeId)
                .HasConstraintName("FK__Ticket__ticketTy__451F3D2B");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Ticket__userID__442B18F2");
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.HasKey(e => e.TicketTypeId).HasName("PK__TicketTy__D18F5C14D3600ED8");

            entity.ToTable("TicketType");

            entity.Property(e => e.TicketTypeId).HasColumnName("ticketTypeID");
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.Discount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("discount");
            entity.Property(e => e.EventId).HasColumnName("eventID");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
            entity.Property(e => e.TicketType1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ticketType");

            entity.HasOne(d => d.Event).WithMany(p => p.TicketTypes)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__TicketTyp__event__414EAC47");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.VendorId).HasName("PK__Vendor__EC65C4E36B9055AF");

            entity.ToTable("Vendor");

            entity.Property(e => e.VendorId).HasColumnName("vendorID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("endDate");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("imageURL");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("startDate");
            entity.Property(e => e.VendorType)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("vendorType");
        });

        //OnModelCreatingPartial(modelBuilder);
    }

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    public DbSet<Food_Scape.ViewModels.RoleVM> RoleVM { get; set; } = default!;
}
