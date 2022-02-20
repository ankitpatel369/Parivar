using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Parivar.Data.DbModel;

namespace Parivar.Data.DbContext
{
    public class ParivarDb : IdentityDbContext<FamilyUser, IdentityRole<long>, long>
    {
        public ParivarDb(DbContextOptions<ParivarDb> options)
            : base(options)
        {
            //Database.Migrate();
        }

        #region Db Set
        #region _A_
        public virtual DbSet<FamilyUser> ApplicationUser { get; set; }
        #endregion

        #region _B_
        #endregion

        #region _C_
        public virtual DbSet<CategoriesMaster> CategoriesMasters { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Countys> Countys { get; set; }
        #endregion

        #region _D_
        public virtual DbSet<Districts> Districts { get; set; }
        #endregion

        #region _E_
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        #endregion

        #region _F_
        #endregion

        #region _G_
        #endregion

        #region _H_
        #endregion

        #region _I_

        #endregion

        #region _J_
        #endregion

        #region _K_
        #endregion

        #region _L_
        #endregion

        #region _M_
        #endregion

        #region _N_
        #endregion

        #region _O_
        public virtual DbSet<FamilyMemberDetails> Organisations { get; set; }
        #endregion

        #region _P_
        #endregion

        #region _Q_
        #endregion

        #region _R_
        public virtual DbSet<RelationShipMaster> RelationShipMasters { get; set; }
        #endregion

        #region _S_
        public virtual DbSet<States> States { get; set; }
        #endregion

        #region _T_
        #endregion

        #region _U_
        #endregion

        #region _V_
        public virtual DbSet<Villages> Villages { get; set; }
        #endregion

        #region _W_
        #endregion

        #region _X_
        #endregion

        #region _Y_
        #endregion

        #region _Z_
        #endregion
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            // Change Default filed datatype & length
            modelBuilder.Entity<FamilyUser>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property(c => c.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<UserClaim>().Property(x => x.ClaimType).HasMaxLength(50);
            modelBuilder.Entity<UserClaim>().Property(x => x.ClaimValue).HasMaxLength(200);

            modelBuilder.Entity<FamilyUser>().Property(x => x.Email).HasMaxLength(100);
            modelBuilder.Entity<FamilyUser>().Property(x => x.UserName).HasMaxLength(100);
            modelBuilder.Entity<FamilyUser>().Property(x => x.PhoneNumber).HasMaxLength(20);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }


}
