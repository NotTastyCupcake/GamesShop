// <auto-generated />
using GamesShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GamesShop.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GamesShop.Models.Store.Developer", b =>
                {
                    b.Property<int>("IdDeveloper")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdDeveloper");

                    b.ToTable("Developers");
                });

            modelBuilder.Entity("GamesShop.Models.Store.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdDeveloper")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdDeveloper");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("GamesShop.Models.Store.Genre", b =>
                {
                    b.Property<int>("IdGenre")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdGenre");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("GamesShop.Models.Store.GenreAssigment", b =>
                {
                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.HasKey("GenreId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("GenreAssigments");
                });

            modelBuilder.Entity("GamesShop.Models.Store.Game", b =>
                {
                    b.HasOne("GamesShop.Models.Store.Developer", "Developer")
                        .WithMany("Games")
                        .HasForeignKey("IdDeveloper")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Developer");
                });

            modelBuilder.Entity("GamesShop.Models.Store.GenreAssigment", b =>
                {
                    b.HasOne("GamesShop.Models.Store.Game", "Game")
                        .WithMany("GenreAssigment")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamesShop.Models.Store.Genre", "Genre")
                        .WithMany("GenreAssigment")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("GamesShop.Models.Store.Developer", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("GamesShop.Models.Store.Game", b =>
                {
                    b.Navigation("GenreAssigment");
                });

            modelBuilder.Entity("GamesShop.Models.Store.Genre", b =>
                {
                    b.Navigation("GenreAssigment");
                });
#pragma warning restore 612, 618
        }
    }
}
