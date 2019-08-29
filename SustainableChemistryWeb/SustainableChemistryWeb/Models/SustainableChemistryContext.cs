using Microsoft.EntityFrameworkCore;

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

        public virtual DbSet<Catalyst> AppCatalyst { get; set; }
        public virtual DbSet<Compound> AppCompound { get; set; }
        public virtual DbSet<FunctionalGroup> AppFunctionalgroup { get; set; }
        public virtual DbSet<NamedReaction> AppNamedreaction { get; set; }
        public virtual DbSet<NamedReactionByProducts> AppNamedreactionByProducts { get; set; }
        public virtual DbSet<NamedReactionReactants> AppNamedreactionReactants { get; set; }
        public virtual DbSet<Reactant> AppReactant { get; set; }
        public virtual DbSet<Reference> AppReference { get; set; }
        public virtual DbSet<Solvent> AppSolvent { get; set; }
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

            modelBuilder.Entity<Catalyst>(entity =>
            {
                entity.ToTable("app_catalyst");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<Compound>(entity =>
            {
                entity.ToTable("app_compound");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CasNumber)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");
            });

            modelBuilder.Entity<FunctionalGroup>(entity =>
            {
                entity.ToTable("app_functionalgroup");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.HasIndex(e => e.Smarts)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

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

            modelBuilder.Entity<NamedReaction>(entity =>
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
                    .ValueGeneratedOnAdd();

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

            modelBuilder.Entity<NamedReactionByProducts>(entity =>
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
                    .ValueGeneratedOnAdd();

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

            modelBuilder.Entity<NamedReactionReactants>(entity =>
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
                    .ValueGeneratedOnAdd();

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

            modelBuilder.Entity<Reactant>(entity =>
            {
                entity.ToTable("app_reactant");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.Temp2)
                    .IsRequired()
                    .HasColumnType("varchar(2)");
            });

            modelBuilder.Entity<Reference>(entity =>
            {
                entity.ToTable("app_reference");

                entity.HasIndex(e => e.FunctionalGroupId)
                    .HasName("app_reference_Functional_Group_id_b8927bac");

                entity.HasIndex(e => e.ReactionId)
                    .HasName("app_reference_Reaction_id_bf824395");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

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

            modelBuilder.Entity<Solvent>(entity =>
            {
                entity.ToTable("app_solvent");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");
            });
        }
    }
}
