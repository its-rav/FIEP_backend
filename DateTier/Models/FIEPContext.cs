using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataTier.Models
{
    public partial class FIEPContext : DbContext
    {
        //public FIEPContext()
        //{
        //}

        public FIEPContext(DbContextOptions<FIEPContext> options)
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
//                optionsBuilder.UseSqlServer("Server=SE130462;Database=FIEP;Trusted_Connection=True;User Id=sa;Password=26651199;MultipleActiveResultSets=true");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityType>(entity =>
            {
                entity.Property(e => e.ActivityTypeName)
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
                    .HasConstraintName("FK__Comment__Comment__60A75C0F");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__PostID__5FB337D6");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.ApprovalState).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EventName)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TimeOccur).HasColumnType("datetime");

                entity.Property(e => e.IsExpired).HasColumnType("bit");    

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Event__GroupID__4CA06362");
            });

            modelBuilder.Entity<EventActivity>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ActivityId)
                    .HasColumnName("ActivityID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.EventActivityDescription)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.HasOne(d => d.ActivityType)
                    .WithMany()
                    .HasForeignKey(d => d.ActivityTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventActi__Activ__52593CB8");

                entity.HasOne(d => d.Event)
                    .WithMany()
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventActi__Event__5165187F");
            });

            modelBuilder.Entity<EventSubscription>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.UserId })
                    .HasName("PK__EventSub__A83C44BA8B27F90A");

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
                    .HasConstraintName("FK__EventSubs__Event__5535A963");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EventSubscription)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventSubs__UserI__5629CD9C");
            });

            modelBuilder.Entity<GroupInformation>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK__GroupInf__149AF30AC85BD644");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GroupImageUrl)
                    .HasMaxLength(128)
                    .IsUnicode(false);

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
                    .HasConstraintName("FK__GroupInfo__Group__403A8C7D");
            });

            modelBuilder.Entity<GroupSubscription>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.UserId })
                    .HasName("PK__GroupSub__C5E27FC0923CD7AF");

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
                    .HasConstraintName("FK__GroupSubs__Group__44FF419A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupSubscription)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupSubs__UserI__45F365D3");
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
                    .HasConstraintName("FK__Post__EventID__5AEE82B9");
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
                    .HasName("PK__UserInfo__1788CCAC571F3792");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__UserInfo__A9D105340D167064")
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
                    .HasConstraintName("FK__UserInfor__RoleI__3D5E1FD2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
