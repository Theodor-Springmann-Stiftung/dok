namespace MusenalmAPI.Data;
using Microsoft.EntityFrameworkCore;
using MusenalmAPI.Models;

public class MusenalmContext : DbContext {
    private DbContextOptions<MusenalmContext> _options;

    public MusenalmContext (DbContextOptions<MusenalmContext> options)
        : base(options)
    {
        _options = options;
    }
    public DbSet<TVOC> TVOCs { get; set; }
    public DbSet<FVOC> FVOCs { get; set; }

    public DbSet<VOC_Akteur> VOC_Akteure { get; set; }
    public DbSet<VOC_Ort> VOC_Orte { get; set; }
    public DbSet<VOC_Reihe> VOC_Reihen { get; set; }
    public DbSet<VOC_Status> VOC_Statuses { get; set; }
    public DbSet<VOC_Typ> VOC_Typen { get; set; }
    public DbSet<VOC_Paginierung> VOC_Paginierungen { get; set; }

    public DbSet<Akteur> Akteure { get; set; }
    public DbSet<Ort> Orte { get; set; }
    public DbSet<Reihe> Reihen { get; set; }
    public DbSet<Band> Baende { get; set; }
    public DbSet<Inhalt> Inhalte { get; set; }
    public DbSet<Exemplar> Exemplare { get; set; }
    
    public DbSet<REL_Exemplar_Status> REL_Exemplare_Statuses { get; set; }
    public DbSet<REL_Band_Reihe> REL_Baende_Reihen { get; set; }
    public DbSet<REL_Band_Akteur> REL_Baende_Akteue { get; set; }
    public DbSet<REL_Band_Ort> REL_Baende_Orte { get; set; }
    public DbSet<REL_Inhalt_Typ> REL_Inhalte_Typen { get; set; }
    public DbSet<REL_Inhalt_Akteur> REL_Inhalte_Akteure { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TVOC>().HasDiscriminator<string>("Tabelle");
        modelBuilder.Entity<FVOC>().HasDiscriminator<string>("Feld");

        modelBuilder.Entity<Akteur>().ToTable("Akteur");
        modelBuilder.Entity<Ort>().ToTable("Ort");
        modelBuilder.Entity<Reihe>().ToTable("Reihe");
        modelBuilder.Entity<Band>().ToTable("Band");
        modelBuilder.Entity<Inhalt>().ToTable("Inhalt");
        modelBuilder.Entity<Exemplar>().ToTable("Exemplar");

        modelBuilder.Entity<REL_Band_Reihe>().ToTable("REL_Band_Reihe");
        modelBuilder.Entity<REL_Band_Akteur>().ToTable("REL_Band_Akteur");
        modelBuilder.Entity<REL_Band_Ort>().ToTable("REL_Band_Ort");
        modelBuilder.Entity<REL_Exemplar_Status>().ToTable("REL_Exemplar_Status");
        modelBuilder.Entity<REL_Inhalt_Typ>().ToTable("REL_Inhalt_Typ");
        modelBuilder.Entity<REL_Inhalt_Akteur>().ToTable("REL_Inhalt_Akteur");       
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(LogSink.Instance.LogLine, LogLevel.Information);
    }
}