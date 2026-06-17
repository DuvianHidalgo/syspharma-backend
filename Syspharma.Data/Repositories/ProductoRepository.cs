using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syspharma.Data.Repositories
{
    public interface IProductoRepository
    {
        Task<List<ProductoDto>> ObtenerTodos();
        Task<ProductoDto?> ObtenerPorId(int id);
        Task<ProductoDto> Crear(ProductoCreateDto dto);
        Task<ProductoDto> Actualizar(ProductoUpdateDto dto);
        Task<bool> CambiarEstado(int id, bool estado);
        Task<bool> Eliminar(int id);
        Task<List<ProductoDto>> ProximosAVencer(int dias);
    }

    public class ProductoRepository : IProductoRepository
    {
        private readonly SyspharmaContext _context;

        public ProductoRepository(SyspharmaContext context)
        {
            _context = context;
        }

        private static ProductoDto MapToDto(Producto p) => new ProductoDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            CategoriaId = p.CategoriaId,
            CategoriaNombre = p.Categoria?.Nombre,
            ProveedorId = p.ProveedorId,
            ProveedorNombre = p.Proveedor?.Nombre,
            Precio = p.Precio,
            PrecioCompra = p.PrecioCompra,
            Stock = p.Stock,
            CodigoBarras = p.CodigoBarras,
            Imagen = p.Imagen,
            Estado = p.Estado,
            FechaCreacion = p.FechaCreacion,
            UltimaActualizacion = p.UltimaActualizacion,
<<<<<<< Updated upstream

            // --- NUEVO MAPEO DEL MEDICAMENTO ---
            Medicamento = p.ProductoMedicamento != null ? new ProductoMedicamentoDto
            {
                Id = p.ProductoMedicamento.Id,
                ProductoId = p.ProductoMedicamento.ProductoId,
                Composicion = p.ProductoMedicamento.Composicion,
                Concentracion = p.ProductoMedicamento.Concentracion,
                Presentacion = p.ProductoMedicamento.Presentacion,
                ViaAdministracion = p.ProductoMedicamento.ViaAdministracion,
                RegistroSanitario = p.ProductoMedicamento.RegistroSanitario,
                RequiereFormula = p.ProductoMedicamento.RequiereFormula
            } : null
=======
            FechaVencimientoProxima = p.FechaVencimientoProxima
>>>>>>> Stashed changes
        };

        public async Task<List<ProductoDto>> ObtenerTodos()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Include(p => p.ProductoMedicamento) // <-- NUEVO: Incluir medicamento
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria.Nombre,
                    ProveedorId = p.ProveedorId,
                    ProveedorNombre = p.Proveedor != null ? p.Proveedor.Nombre : null,
                    Precio = p.Precio,
                    PrecioCompra = p.PrecioCompra,
                    Stock = p.Stock,
                    CodigoBarras = p.CodigoBarras,
                    Imagen = p.Imagen,
                    Estado = p.Estado,
                    FechaCreacion = p.FechaCreacion,
                    UltimaActualizacion = p.UltimaActualizacion,
<<<<<<< Updated upstream

                    // --- NUEVO: Proyección del medicamento en la lista ---
                    Medicamento = p.ProductoMedicamento != null ? new ProductoMedicamentoDto
                    {
                        Id = p.ProductoMedicamento.Id,
                        ProductoId = p.ProductoMedicamento.ProductoId,
                        Composicion = p.ProductoMedicamento.Composicion,
                        Concentracion = p.ProductoMedicamento.Concentracion,
                        Presentacion = p.ProductoMedicamento.Presentacion,
                        ViaAdministracion = p.ProductoMedicamento.ViaAdministracion,
                        RegistroSanitario = p.ProductoMedicamento.RegistroSanitario,
                        RequiereFormula = p.ProductoMedicamento.RequiereFormula
                    } : null
=======
                    FechaVencimientoProxima = p.FechaVencimientoProxima
>>>>>>> Stashed changes
                })
                .ToListAsync();
        }

        public async Task<ProductoDto?> ObtenerPorId(int id)
        {
            var p = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Include(p => p.ProductoMedicamento) // <-- NUEVO: Incluir medicamento
                .FirstOrDefaultAsync(p => p.Id == id);

            return p == null ? null : MapToDto(p);
        }

        public async Task<List<ProductoDto>> ProximosAVencer(int dias)
        {
            var limite = DateTime.Today.AddDays(dias);
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Where(p => p.FechaVencimientoProxima != null
                         && p.FechaVencimientoProxima <= limite
                         && p.Estado == true)
                .OrderBy(p => p.FechaVencimientoProxima)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria.Nombre,
                    ProveedorId = p.ProveedorId,
                    ProveedorNombre = p.Proveedor != null ? p.Proveedor.Nombre : null,
                    Precio = p.Precio,
                    PrecioCompra = p.PrecioCompra,
                    Stock = p.Stock,
                    CodigoBarras = p.CodigoBarras,
                    Imagen = p.Imagen,
                    Estado = p.Estado,
                    FechaCreacion = p.FechaCreacion,
                    UltimaActualizacion = p.UltimaActualizacion,
                    FechaVencimientoProxima = p.FechaVencimientoProxima
                })
                .ToListAsync();
        }

        public async Task<ProductoDto> Crear(ProductoCreateDto dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                CategoriaId = dto.CategoriaId,
                ProveedorId = dto.ProveedorId,
                Precio = dto.Precio,
                PrecioCompra = dto.PrecioCompra,
                Stock = dto.Stock ?? 0,
                CodigoBarras = dto.CodigoBarras,
                Imagen = dto.Imagen,
                Estado = true,
                FechaCreacion = DateTime.Now,
                UltimaActualizacion = DateTime.Now
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            // --- NUEVO: Guardado de los detalles de medicamento ---
            if (dto.EsMedicamento && dto.Medicamento != null)
            {
                var medicamento = new ProductoMedicamento
                {
                    ProductoId = producto.Id,
                    Composicion = dto.Medicamento.Composicion,
                    Concentracion = dto.Medicamento.Concentracion,
                    Presentacion = dto.Medicamento.Presentacion,
                    ViaAdministracion = dto.Medicamento.ViaAdministracion,
                    RegistroSanitario = dto.Medicamento.RegistroSanitario,
                    RequiereFormula = dto.Medicamento.RequiereFormula
                };

                _context.ProductoMedicamentos.Add(medicamento);
                await _context.SaveChangesAsync();
            }

            return await ObtenerPorId(producto.Id) ?? MapToDto(producto);
        }

        public async Task<ProductoDto> Actualizar(ProductoUpdateDto dto)
        {
            var producto = await _context.Productos.FindAsync(dto.Id);

            if (producto == null)
                throw new Exception("Producto no encontrado");

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.CategoriaId = dto.CategoriaId;
            producto.ProveedorId = dto.ProveedorId;
            producto.Precio = dto.Precio;
            producto.PrecioCompra = dto.PrecioCompra;
            producto.Stock = dto.Stock ?? producto.Stock;
            producto.CodigoBarras = dto.CodigoBarras;
            producto.Imagen = dto.Imagen;
            producto.UltimaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();

            // --- NUEVO: Gestión de actualización del detalle de medicamento ---
            var medicamentoExistente = await _context.ProductoMedicamentos
                .FirstOrDefaultAsync(pm => pm.ProductoId == producto.Id);

            if (dto.EsMedicamento && dto.Medicamento != null)
            {
                if (medicamentoExistente == null)
                {
                    // Si antes era producto normal y ahora es medicamento, creamos el detalle
                    var nuevoMedicamento = new ProductoMedicamento
                    {
                        ProductoId = producto.Id,
                        Composicion = dto.Medicamento.Composicion,
                        Concentracion = dto.Medicamento.Concentracion,
                        Presentacion = dto.Medicamento.Presentacion,
                        ViaAdministracion = dto.Medicamento.ViaAdministracion,
                        RegistroSanitario = dto.Medicamento.RegistroSanitario,
                        RequiereFormula = dto.Medicamento.RequiereFormula
                    };
                    _context.ProductoMedicamentos.Add(nuevoMedicamento);
                }
                else
                {
                    // Si ya existía, actualizamos sus campos adicionales
                    medicamentoExistente.Composicion = dto.Medicamento.Composicion;
                    medicamentoExistente.Concentracion = dto.Medicamento.Concentracion;
                    medicamentoExistente.Presentacion = dto.Medicamento.Presentacion;
                    medicamentoExistente.ViaAdministracion = dto.Medicamento.ViaAdministracion;
                    medicamentoExistente.RegistroSanitario = dto.Medicamento.RegistroSanitario;
                    medicamentoExistente.RequiereFormula = dto.Medicamento.RequiereFormula;
                }
                await _context.SaveChangesAsync();
            }
            else if (!dto.EsMedicamento && medicamentoExistente != null)
            {
                // Si ya no es medicamento, removemos el detalle de la base de datos
                _context.ProductoMedicamentos.Remove(medicamentoExistente);
                await _context.SaveChangesAsync();
            }

            return await ObtenerPorId(producto.Id) ?? MapToDto(producto);
        }

        public async Task<bool> CambiarEstado(int id, bool estado)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            producto.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
