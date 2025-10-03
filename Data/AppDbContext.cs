using Microsoft.EntityFrameworkCore;
using ProyectoInventario.Models;

namespace ProyectoInventario.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //  Entidades principales
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<VentaDetalle> VentaDetalles => Set<VentaDetalle>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Envio> Envios => Set<Envio>();
    public DbSet<Sucursal> Sucursales => Set<Sucursal>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // VentaDetalle: relación con Venta y Producto
        modelBuilder.Entity<VentaDetalle>()
            .HasOne(vd => vd.Venta)
            .WithMany(v => v.Detalles)
            .HasForeignKey(vd => vd.VentaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VentaDetalle>()
            .HasOne(vd => vd.Producto)
            .WithMany()
            .HasForeignKey(vd => vd.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        //  Envío: relación uno a uno con Venta
        modelBuilder.Entity<Envio>()
            .HasOne(e => e.Venta)
            .WithOne(v => v.Envio)
            .HasForeignKey<Envio>(e => e.VentaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Venta: relación con Cliente
        modelBuilder.Entity<Venta>()
            .HasOne(v => v.Cliente)
            .WithMany(c => c.Ventas)
            .HasForeignKey(v => v.ClienteId)
            .OnDelete(DeleteBehavior.SetNull);

        // relaciones futuras
        // modelBuilder.Entity<Producto>()
        //     .HasOne<Sucursal>()
        //     .WithMany()
        //     .HasForeignKey(p => p.Sucursal);

        // modelBuilder.Entity<User>()
        //     .HasOne<Sucursal>()
        //     .WithMany()
        //     .HasForeignKey(u => u.SucursalAsignada);
    }
}