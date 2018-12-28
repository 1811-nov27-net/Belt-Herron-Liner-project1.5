using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess
{
    public partial class D20CharacterDatabaseContext : DbContext
    {
        public D20CharacterDatabaseContext()
        {
        }

        public D20CharacterDatabaseContext(DbContextOptions<D20CharacterDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Campaign> Campaign { get; set; }
        public virtual DbSet<Characters> Characters { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Feats> Feats { get; set; }
        public virtual DbSet<Gamer> Gamer { get; set; }
        public virtual DbSet<Gmjunction> Gmjunction { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Skills> Skills { get; set; }
        public virtual DbSet<SpellJunction> SpellJunction { get; set; }
        public virtual DbSet<SpellSlots> SpellSlots { get; set; }
        public virtual DbSet<Spells> Spells { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.ToTable("Campaign", "d20");

                entity.HasIndex(e => e.CampaignName)
                    .HasName("UQ__Campaign__891FED51178C3944")
                    .IsUnique();

                entity.Property(e => e.CampaignName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Characters>(entity =>
            {
                entity.HasKey(e => e.CharacterId)
                    .HasName("PK__Characte__757BC9A06FA4A919");

                entity.ToTable("Characters", "d20");

                entity.Property(e => e.Ac)
                    .HasColumnName("AC")
                    .HasDefaultValueSql("((10))");

                entity.Property(e => e.CharacterName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Alignment)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Bab).HasColumnName("BAB");

                entity.Property(e => e.MaxHP).HasColumnName("MaxHP");

                entity.Property(e => e.Charisma).HasDefaultValueSql("((10))");

                entity.Property(e => e.Dexterity).HasDefaultValueSql("((10))");

                entity.Property(e => e.Ffac)
                    .HasColumnName("FFAC")
                    .HasDefaultValueSql("((10))");

                entity.Property(e => e.Intelligence).HasDefaultValueSql("((10))");

                entity.Property(e => e.Race)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Sex)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Stamina).HasDefaultValueSql("((10))");

                entity.Property(e => e.Strength).HasDefaultValueSql("((10))");

                entity.Property(e => e.TouchAc)
                    .HasColumnName("TouchAC")
                    .HasDefaultValueSql("((10))");

                entity.Property(e => e.Wisdom).HasDefaultValueSql("((10))");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_Character_Campaign");

                entity.HasOne(d => d.Gamer)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.GamerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Character_Gamer");
            });

            modelBuilder.Entity<Classes>(entity =>
            {
                entity.HasKey(e => new { e.ClassName, e.CharacterId });

                entity.ToTable("Classes", "d20");

                entity.Property(e => e.ClassName).HasMaxLength(100);

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Classes_Character");
            });

            modelBuilder.Entity<Feats>(entity =>
            {
                entity.HasKey(e => new { e.FeatName, e.CharacterId });

                entity.ToTable("Feats", "d20");

                entity.Property(e => e.FeatName).HasMaxLength(100);

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.Feats)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feats_Character");
            });

            modelBuilder.Entity<Gamer>(entity =>
            {
                entity.ToTable("Gamer", "d20");

                entity.HasIndex(e => e.UserName)
                    .HasName("UQ__Gamer__C9F284565438CC65")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Gmjunction>(entity =>
            {
                entity.HasKey(e => new { e.CampaignId, e.Gmid })
                    .HasName("PK_GM");

                entity.ToTable("GMJunction", "d20");

                entity.Property(e => e.Gmid).HasColumnName("GMId");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Gmjunction)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GM_Campaign");

                entity.HasOne(d => d.Gm)
                    .WithMany(p => p.Gmjunction)
                    .HasForeignKey(d => d.Gmid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GM_Gamer");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => new { e.ItemName, e.CharacterId });

                entity.ToTable("Inventory", "d20");

                entity.Property(e => e.ItemName).HasMaxLength(100);

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_Character");
            });

            modelBuilder.Entity<Skills>(entity =>
            {
                entity.HasKey(e => new { e.SkillName, e.CharacterId });

                entity.ToTable("Skills", "d20");

                entity.Property(e => e.SkillName).HasMaxLength(100);

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.Skills)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Skill_Character");
            });

            modelBuilder.Entity<SpellJunction>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.SpellId })
                    .HasName("PK_SJ");

                entity.ToTable("SpellJunction", "d20");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.SpellJunction)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SJ_Character");

                entity.HasOne(d => d.Spell)
                    .WithMany(p => p.SpellJunction)
                    .HasForeignKey(d => d.SpellId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SJ_Spell");
            });

            modelBuilder.Entity<SpellSlots>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.ClassName });

                entity.ToTable("SpellSlots", "d20");

                entity.Property(e => e.ClassName).HasMaxLength(100);

                entity.Property(e => e.Level0Slots).HasDefaultValueSql("((0))");

                entity.Property(e => e.Level1Slots).HasDefaultValueSql("((0))");

                entity.Property(e => e.Level2Slots).HasDefaultValueSql("((0))");

                entity.Property(e => e.Level3Slots).HasDefaultValueSql("((0))");

                entity.Property(e => e.Level4Slots).HasDefaultValueSql("((0))");

                entity.Property(e => e.Level5Slots).HasDefaultValueSql("((0))");

                entity.Property(e => e.Level6Slots).HasDefaultValueSql("((0))");

                entity.Property(e => e.Level7Slots).HasDefaultValueSql("((0))");

                entity.Property(e => e.Level8Slots).HasDefaultValueSql("((0))");

                entity.Property(e => e.Level9Slots).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.SpellSlots)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Slots_Character");
            });

            modelBuilder.Entity<Spells>(entity =>
            {
                entity.HasKey(e => e.SpellId)
                    .HasName("PK__Spells__52BE41BEE265FC74");

                entity.ToTable("Spells", "d20");

                entity.Property(e => e.Class)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SpellName)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
