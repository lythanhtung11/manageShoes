using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ProjectWeb.Models
{
    public partial class Web1209Context : DbContext
    {
        public Web1209Context()
        {
        }

        public Web1209Context(DbContextOptions<Web1209Context> options)
            : base(options)
        {
        }

        public virtual DbSet<DetailsReceipt> DetailsReceipts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-GAFDUFD;Database=Web1209;Trusted_Connection=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DetailsReceipt>(entity =>
            {
                entity.HasKey(e => new { e.IdProduct, e.IdReceipt })
                    .HasName("PK_detailsReceipt_1");

                entity.ToTable("detailsReceipt");

                entity.Property(e => e.IdProduct).HasColumnName("ID_Product");

                entity.Property(e => e.IdReceipt).HasColumnName("ID_Receipt");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.DetailsReceipts)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_detailsReceipt_Product");

                entity.HasOne(d => d.IdReceiptNavigation)
                    .WithMany(p => p.DetailsReceipts)
                    .HasForeignKey(d => d.IdReceipt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_detailsReceipt_Receipt");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Brand).IsRequired();

                entity.Property(e => e.Condition).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.ToTable("Profile");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.Gender).IsRequired();

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.Phone).IsRequired();

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Profile_User");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("Receipt");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Receipts)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Receipt_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Datejoin).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Role).IsRequired();

                entity.Property(e => e.UserName).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
