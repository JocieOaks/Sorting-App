﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sorting_App.Data;

#nullable disable

namespace Sorting_App.Migrations
{
    [DbContext(typeof(SortingContext))]
    [Migration("20230405225042_SelectElementRelations")]
    partial class SelectElementRelations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ElementElementTag", b =>
                {
                    b.Property<int>("ElementsID")
                        .HasColumnType("int");

                    b.Property<int>("TagsID")
                        .HasColumnType("int");

                    b.HasKey("ElementsID", "TagsID");

                    b.HasIndex("TagsID");

                    b.ToTable("ElementElementTag");
                });

            modelBuilder.Entity("Sorting_App.Models.Element", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("ListID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ListID");

                    b.ToTable("Elements");
                });

            modelBuilder.Entity("Sorting_App.Models.ElementComparison", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("FirstElementID")
                        .HasColumnType("int");

                    b.Property<int?>("PreferenceDegree")
                        .HasColumnType("int");

                    b.Property<int>("SecondElementID")
                        .HasColumnType("int");

                    b.Property<int>("SortID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("FirstElementID");

                    b.HasIndex("SecondElementID");

                    b.HasIndex("SortID");

                    b.ToTable("ElementComparisons");
                });

            modelBuilder.Entity("Sorting_App.Models.ElementList", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ElementList");
                });

            modelBuilder.Entity("Sorting_App.Models.ElementTag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ListID")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ListID");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Sorting_App.Models.SelectElement", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ElementID")
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("LastTimeAsOption")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfTimesAsOption")
                        .HasColumnType("int");

                    b.Property<float>("Push")
                        .HasColumnType("real");

                    b.Property<float>("Score")
                        .HasColumnType("real");

                    b.Property<int>("SortID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ElementID");

                    b.HasIndex("SortID");

                    b.ToTable("SelectElements");
                });

            modelBuilder.Entity("Sorting_App.Models.Sort", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ElementListID")
                        .HasColumnType("int");

                    b.Property<int>("SelectionCount")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ElementListID");

                    b.ToTable("Sorts");
                });

            modelBuilder.Entity("ElementElementTag", b =>
                {
                    b.HasOne("Sorting_App.Models.Element", null)
                        .WithMany()
                        .HasForeignKey("ElementsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sorting_App.Models.ElementTag", null)
                        .WithMany()
                        .HasForeignKey("TagsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sorting_App.Models.Element", b =>
                {
                    b.HasOne("Sorting_App.Models.ElementList", "List")
                        .WithMany("Elements")
                        .HasForeignKey("ListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("List");
                });

            modelBuilder.Entity("Sorting_App.Models.ElementComparison", b =>
                {
                    b.HasOne("Sorting_App.Models.SelectElement", "FirstElement")
                        .WithMany()
                        .HasForeignKey("FirstElementID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Sorting_App.Models.SelectElement", "SecondElement")
                        .WithMany()
                        .HasForeignKey("SecondElementID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Sorting_App.Models.Sort", "Sort")
                        .WithMany("ElementComparisons")
                        .HasForeignKey("SortID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FirstElement");

                    b.Navigation("SecondElement");

                    b.Navigation("Sort");
                });

            modelBuilder.Entity("Sorting_App.Models.ElementTag", b =>
                {
                    b.HasOne("Sorting_App.Models.ElementList", "List")
                        .WithMany("Tags")
                        .HasForeignKey("ListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("List");
                });

            modelBuilder.Entity("Sorting_App.Models.SelectElement", b =>
                {
                    b.HasOne("Sorting_App.Models.Element", "Element")
                        .WithMany()
                        .HasForeignKey("ElementID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sorting_App.Models.Sort", null)
                        .WithMany("SelectElements")
                        .HasForeignKey("SortID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Element");
                });

            modelBuilder.Entity("Sorting_App.Models.Sort", b =>
                {
                    b.HasOne("Sorting_App.Models.ElementList", "ElementList")
                        .WithMany("Sorts")
                        .HasForeignKey("ElementListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ElementList");
                });

            modelBuilder.Entity("Sorting_App.Models.ElementList", b =>
                {
                    b.Navigation("Elements");

                    b.Navigation("Sorts");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("Sorting_App.Models.Sort", b =>
                {
                    b.Navigation("ElementComparisons");

                    b.Navigation("SelectElements");
                });
#pragma warning restore 612, 618
        }
    }
}
