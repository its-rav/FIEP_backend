using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataTier.Models
{
    public partial class FEIP_be_dbContext : DbContext
    {
        //public FEIP_be_dbContext()
        //{
        //}

        public FEIP_be_dbContext(DbContextOptions<FEIP_be_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActivityType> ActivityType { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventActivity> EventActivity { get; set; }
        public virtual DbSet<EventSubscription> EventSubscription { get; set; }
        public virtual DbSet<GroupInformation> GroupInformation { get; set; }
        public virtual DbSet<GroupSubscription> GroupSubscription { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<UserInformation> UserInformation { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=SE130462;Database=FEIP_be_db;Trusted_Connection=True;User Id=sa;Password=26651199;MultipleActiveResultSets=true");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityType>(entity =>
            {
                entity.Property(e => e.ActivityTypeDescription)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentId)
                    .HasColumnName("CommentID")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.HasOne(d => d.CommentOwner)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.CommentOwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__Comment__07C12930");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__PostID__06CD04F7");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasIndex(e => e.ActivityId)
                    .HasName("UQ__Event__45F4A7F046848CFB")
                    .IsUnique();

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.ApprovalState).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.IsExpired).HasDefaultValueSql("((0))");

                entity.Property(e => e.Location)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TimeOccur).HasColumnType("datetime");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Event__GroupID__72C60C4A");
            });

            modelBuilder.Entity<EventActivity>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.ActivityTypeDescription)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.Activity)
                    .WithMany()
                    .HasPrincipalKey(p => p.ActivityId)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventActi__Activ__787EE5A0");

                entity.HasOne(d => d.ActivityType)
                    .WithMany()
                    .HasForeignKey(d => d.ActivityTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventActi__Activ__797309D9");
            });

            modelBuilder.Entity<EventSubscription>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.UserId })
                    .HasName("PK__EventSub__A83C44BAAB0CE0C4");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventSubscription)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventSubs__Event__7C4F7684");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EventSubscription)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventSubs__UserI__7D439ABD");
            });

            modelBuilder.Entity<GroupInformation>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK__GroupInf__149AF30A29695554");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.GroupManager)
                    .WithMany(p => p.GroupInformation)
                    .HasForeignKey(d => d.GroupManagerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupInfo__Group__656C112C");
            });

            modelBuilder.Entity<GroupSubscription>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.UserId })
                    .HasName("PK__GroupSub__C5E27FC0F6F568A7");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupSubscription)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupSubs__Group__6A30C649");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupSubscription)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupSubs__UserI__6B24EA82");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.PostId)
                    .HasColumnName("PostID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostContent)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Post__EventID__02084FDA");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Rolename)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserInformation>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserInfo__1788CCACE91B1C36");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__UserInfo__A9D10534245F1606")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AvatarUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserInformation)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserInfor__RoleI__628FA481");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
