using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SustainableChemistryWeb.Models
{
    public partial class SustainableChemistryContext : DbContext
    {
        public SustainableChemistryContext()
        {
        }

        public SustainableChemistryContext(DbContextOptions<SustainableChemistryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppCatalyst> AppCatalyst { get; set; }
        public virtual DbSet<AppCompound> AppCompound { get; set; }
        public virtual DbSet<AppFunctionalgroup> AppFunctionalgroup { get; set; }
        public virtual DbSet<AppNamedreaction> AppNamedreaction { get; set; }
        public virtual DbSet<AppNamedreactionByProducts> AppNamedreactionByProducts { get; set; }
        public virtual DbSet<AppNamedreactionReactants> AppNamedreactionReactants { get; set; }
        public virtual DbSet<AppProfile> AppProfile { get; set; }
        public virtual DbSet<AppReactant> AppReactant { get; set; }
        public virtual DbSet<AppReference> AppReference { get; set; }
        public virtual DbSet<AppSolvent> AppSolvent { get; set; }
        public virtual DbSet<AuthGroup> AuthGroup { get; set; }
        public virtual DbSet<AuthGroupPermissions> AuthGroupPermissions { get; set; }
        public virtual DbSet<AuthPermission> AuthPermission { get; set; }
        public virtual DbSet<AuthUser> AuthUser { get; set; }
        public virtual DbSet<AuthUserGroups> AuthUserGroups { get; set; }
        public virtual DbSet<AuthUserUserPermissions> AuthUserUserPermissions { get; set; }
        public virtual DbSet<DjangoAdminLog> DjangoAdminLog { get; set; }
        public virtual DbSet<DjangoContentType> DjangoContentType { get; set; }
        public virtual DbSet<DjangoMigrations> DjangoMigrations { get; set; }
        public virtual DbSet<DjangoSession> DjangoSession { get; set; }
        public virtual DbSet<DjangoSite> DjangoSite { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("DataSource= SustainableChemistry.sqlite3");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<AppCatalyst>(entity =>
            {
                entity.ToTable("app_catalyst");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<AppCompound>(entity =>
            {
                entity.ToTable("app_compound");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CasNumber)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<AppFunctionalgroup>(entity =>
            {
                entity.ToTable("app_functionalgroup");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.HasIndex(e => e.Smarts)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.Smarts)
                    .IsRequired()
                    .HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<AppNamedreaction>(entity =>
            {
                entity.ToTable("app_namedreaction");

                entity.HasIndex(e => e.CatalystId)
                    .HasName("app_namedreaction_Catalyst_id_63600e1e");

                entity.HasIndex(e => e.FunctionalGroupId)
                    .HasName("app_namedreaction_Functional_Group_id_057af1bd");

                entity.HasIndex(e => e.SolventId)
                    .HasName("app_namedreaction_Solvent_id_7ec52782");

                entity.HasIndex(e => new { e.FunctionalGroupId, e.Name })
                    .HasName("app_namedreaction_Functional_Group_id_Name_5a5a6724_uniq")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AcidBase)
                    .IsRequired()
                    .HasColumnType("varchar(2)");

                entity.Property(e => e.CatalystId).HasColumnName("Catalyst_id");

                entity.Property(e => e.FunctionalGroupId).HasColumnName("Functional_Group_id");

                entity.Property(e => e.Heat)
                    .IsRequired()
                    .HasColumnType("varchar(2)");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.ReactantA)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.ReactantB)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.ReactantC)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.SolventId).HasColumnName("Solvent_id");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasColumnType("varchar(200)");

                entity.HasOne(d => d.Catalyst)
                    .WithMany(p => p.AppNamedreaction)
                    .HasForeignKey(d => d.CatalystId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.FunctionalGroup)
                    .WithMany(p => p.AppNamedreaction)
                    .HasForeignKey(d => d.FunctionalGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Solvent)
                    .WithMany(p => p.AppNamedreaction)
                    .HasForeignKey(d => d.SolventId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AppNamedreactionByProducts>(entity =>
            {
                entity.ToTable("app_namedreaction_ByProducts");

                entity.HasIndex(e => e.NamedreactionId)
                    .HasName("app_namedreaction_ByProducts_namedreaction_id_a2dc2fd2");

                entity.HasIndex(e => e.ReactantId)
                    .HasName("app_namedreaction_ByProducts_reactant_id_fc608f72");

                entity.HasIndex(e => new { e.NamedreactionId, e.ReactantId })
                    .HasName("app_namedreaction_ByProducts_namedreaction_id_reactant_id_0784f477_uniq")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.NamedreactionId).HasColumnName("namedreaction_id");

                entity.Property(e => e.ReactantId).HasColumnName("reactant_id");

                entity.HasOne(d => d.Namedreaction)
                    .WithMany(p => p.AppNamedreactionByProducts)
                    .HasForeignKey(d => d.NamedreactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Reactant)
                    .WithMany(p => p.AppNamedreactionByProducts)
                    .HasForeignKey(d => d.ReactantId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AppNamedreactionReactants>(entity =>
            {
                entity.ToTable("app_namedreaction_Reactants");

                entity.HasIndex(e => e.NamedreactionId)
                    .HasName("app_namedreaction_Reactants_namedreaction_id_b07b57ce");

                entity.HasIndex(e => e.ReactantId)
                    .HasName("app_namedreaction_Reactants_reactant_id_5118ff6c");

                entity.HasIndex(e => new { e.NamedreactionId, e.ReactantId })
                    .HasName("app_namedreaction_Reactants_namedreaction_id_reactant_id_a9ecf412_uniq")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.NamedreactionId).HasColumnName("namedreaction_id");

                entity.Property(e => e.ReactantId).HasColumnName("reactant_id");

                entity.HasOne(d => d.Namedreaction)
                    .WithMany(p => p.AppNamedreactionReactants)
                    .HasForeignKey(d => d.NamedreactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Reactant)
                    .WithMany(p => p.AppNamedreactionReactants)
                    .HasForeignKey(d => d.ReactantId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AppProfile>(entity =>
            {
                entity.ToTable("app_profile");

                entity.HasIndex(e => e.UserId)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasColumnName("address1")
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasColumnName("address2")
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.Organization)
                    .IsRequired()
                    .HasColumnName("organization")
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.AppProfile)
                    .HasForeignKey<AppProfile>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AppReactant>(entity =>
            {
                entity.ToTable("app_reactant");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.Temp2)
                    .IsRequired()
                    .HasColumnType("varchar(2)");
            });

            modelBuilder.Entity<AppReference>(entity =>
            {
                entity.ToTable("app_reference");

                entity.HasIndex(e => e.FunctionalGroupId)
                    .HasName("app_reference_Functional_Group_id_b8927bac");

                entity.HasIndex(e => e.ReactionId)
                    .HasName("app_reference_Reaction_id_bf824395");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.FunctionalGroupId).HasColumnName("Functional_Group_id");

                entity.Property(e => e.ReactionId).HasColumnName("Reaction_id");

                entity.Property(e => e.Risdata)
                    .IsRequired()
                    .HasColumnName("RISData");

                entity.HasOne(d => d.FunctionalGroup)
                    .WithMany(p => p.AppReference)
                    .HasForeignKey(d => d.FunctionalGroupId);

                entity.HasOne(d => d.Reaction)
                    .WithMany(p => p.AppReference)
                    .HasForeignKey(d => d.ReactionId);
            });

            modelBuilder.Entity<AppSolvent>(entity =>
            {
                entity.ToTable("app_solvent");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<AuthGroup>(entity =>
            {
                entity.ToTable("auth_group");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(80)");
            });

            modelBuilder.Entity<AuthGroupPermissions>(entity =>
            {
                entity.ToTable("auth_group_permissions");

                entity.HasIndex(e => e.GroupId)
                    .HasName("auth_group_permissions_0e939a4f");

                entity.HasIndex(e => e.PermissionId)
                    .HasName("auth_group_permissions_8373b171");

                entity.HasIndex(e => new { e.GroupId, e.PermissionId })
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.AuthGroupPermissions)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.AuthGroupPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AuthPermission>(entity =>
            {
                entity.ToTable("auth_permission");

                entity.HasIndex(e => e.ContentTypeId)
                    .HasName("auth_permission_417f1b1c");

                entity.HasIndex(e => new { e.ContentTypeId, e.Codename })
                    .HasName("auth_permission_content_type_id_01ab375a_uniq")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Codename)
                    .IsRequired()
                    .HasColumnName("codename")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.ContentTypeId).HasColumnName("content_type_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.ContentType)
                    .WithMany(p => p.AuthPermission)
                    .HasForeignKey(d => d.ContentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AuthUser>(entity =>
            {
                entity.ToTable("auth_user");

                entity.HasIndex(e => e.Username)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateJoined)
                    .IsRequired()
                    .HasColumnName("date_joined")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(254)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasColumnType("bool");

                entity.Property(e => e.IsStaff)
                    .IsRequired()
                    .HasColumnName("is_staff")
                    .HasColumnType("bool");

                entity.Property(e => e.IsSuperuser)
                    .IsRequired()
                    .HasColumnName("is_superuser")
                    .HasColumnType("bool");

                entity.Property(e => e.LastLogin)
                    .HasColumnName("last_login")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<AuthUserGroups>(entity =>
            {
                entity.ToTable("auth_user_groups");

                entity.HasIndex(e => e.GroupId)
                    .HasName("auth_user_groups_0e939a4f");

                entity.HasIndex(e => e.UserId)
                    .HasName("auth_user_groups_e8701ad4");

                entity.HasIndex(e => new { e.UserId, e.GroupId })
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.AuthUserGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuthUserGroups)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AuthUserUserPermissions>(entity =>
            {
                entity.ToTable("auth_user_user_permissions");

                entity.HasIndex(e => e.PermissionId)
                    .HasName("auth_user_user_permissions_8373b171");

                entity.HasIndex(e => e.UserId)
                    .HasName("auth_user_user_permissions_e8701ad4");

                entity.HasIndex(e => new { e.UserId, e.PermissionId })
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.AuthUserUserPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuthUserUserPermissions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DjangoAdminLog>(entity =>
            {
                entity.ToTable("django_admin_log");

                entity.HasIndex(e => e.ContentTypeId)
                    .HasName("django_admin_log_content_type_id_c4bce8eb");

                entity.HasIndex(e => e.UserId)
                    .HasName("django_admin_log_user_id_c564eba6");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActionFlag)
                    .HasColumnName("action_flag")
                    .HasColumnType("smallint unsigned");

                entity.Property(e => e.ActionTime)
                    .IsRequired()
                    .HasColumnName("action_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.ChangeMessage)
                    .IsRequired()
                    .HasColumnName("change_message");

                entity.Property(e => e.ContentTypeId).HasColumnName("content_type_id");

                entity.Property(e => e.ObjectId).HasColumnName("object_id");

                entity.Property(e => e.ObjectRepr)
                    .IsRequired()
                    .HasColumnName("object_repr")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.ContentType)
                    .WithMany(p => p.DjangoAdminLog)
                    .HasForeignKey(d => d.ContentTypeId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DjangoAdminLog)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DjangoContentType>(entity =>
            {
                entity.ToTable("django_content_type");

                entity.HasIndex(e => new { e.AppLabel, e.Model })
                    .HasName("django_content_type_app_label_76bd3d3b_uniq")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AppLabel)
                    .IsRequired()
                    .HasColumnName("app_label")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasColumnName("model")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<DjangoMigrations>(entity =>
            {
                entity.ToTable("django_migrations");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.App)
                    .IsRequired()
                    .HasColumnName("app")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Applied)
                    .IsRequired()
                    .HasColumnName("applied")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<DjangoSession>(entity =>
            {
                entity.HasKey(e => e.SessionKey);

                entity.ToTable("django_session");

                entity.HasIndex(e => e.ExpireDate)
                    .HasName("django_session_de54fa62");

                entity.Property(e => e.SessionKey)
                    .HasColumnName("session_key")
                    .HasColumnType("varchar(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ExpireDate)
                    .IsRequired()
                    .HasColumnName("expire_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.SessionData)
                    .IsRequired()
                    .HasColumnName("session_data");
            });

            modelBuilder.Entity<DjangoSite>(entity =>
            {
                entity.ToTable("django_site");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Domain)
                    .IsRequired()
                    .HasColumnName("domain")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");
            });
        }
    }
}
