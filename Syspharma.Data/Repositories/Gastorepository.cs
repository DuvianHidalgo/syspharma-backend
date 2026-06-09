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
    public interface IGastoRepository
    {
        Task<List<GastoDto>> ObtenerTodos();
        Task<List<GastoDto>> ObtenerPorTurno(int turnoId);
        Task<GastoDto?> ObtenerPorId(int id);
        Task<GastoDto> Crear(GastoCreateDto dto);
        Task<GastoDto> Actualizar(GastoUpdateDto dto);
        Task<bool> Eliminar(int id);
        Task<List<GastoDto>> ObtenerHoy(int? usuarioId);
        Task<GastoKpiDto> ObtenerKpis(DateTime? fecha);
        Task<bool> Anular(int id, string notas);
    }

    public class GastoRepository : IGastoRepository
    {
        private readonly SyspharmaContext _context;

        private static readonly TimeZoneInfo ColombiaZone =
            TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");

        private static DateTime AhoraColombia() =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ColombiaZone);

        public GastoRepository(SyspharmaContext context) => _context = context;

        // ✅ Recalcula y actualiza TotalGastos del turno
        private async Task RecalcularTotalGastosTurno(int turnoId)
        {
            var turno = await _context.Turnos.FindAsync(turnoId);
            if (turno == null) return;

            turno.TotalGastos = await _context.Gastos
                .Where(g => g.TurnoId == turnoId && (g.EstadoId == 1 || g.EstadoId == null))
                .SumAsync(g => g.Monto);

            await _context.SaveChangesAsync();
        }

        private static GastoDto MapDto(Gasto g) => new GastoDto
        {
            Id = g.Id,
            TurnoId = g.TurnoId,
            UsuarioId = g.UsuarioId,
            UsuarioNombre = g.Usuario?.Nombre ?? "",
            NumeroGasto = g.NumeroGasto,
            Concepto = g.Concepto,
            Descripcion = g.Descripcion,
            Monto = g.Monto,
            Categoria = g.Categoria,
            MetodoPagoId = g.MetodoPagoId,
            MetodoPago = null,
            EstadoId = g.EstadoId,
            Subtotal = g.Subtotal,
            Iva = g.Iva,
            PorcentajeIva = g.PorcentajeIva,
            Notas = g.Notas,
            Proveedor = g.ProveedorNombre,
            Comprobante = g.Comprobante,
            FechaGasto = g.FechaGasto,
            FechaCreacion = g.FechaCreacion
        };

        public async Task<List<GastoDto>> ObtenerTodos() =>
            await _context.Gastos
                .Include(g => g.Usuario)
                .OrderByDescending(g => g.FechaGasto)
                .Select(g => MapDto(g))
                .ToListAsync();

        public async Task<List<GastoDto>> ObtenerPorTurno(int turnoId) =>
            await _context.Gastos
                .Include(g => g.Usuario)
                .Where(g => g.TurnoId == turnoId)
                .Select(g => MapDto(g))
                .ToListAsync();

        public async Task<GastoDto?> ObtenerPorId(int id)
        {
            var g = await _context.Gastos
                .Include(g => g.Usuario)
                .FirstOrDefaultAsync(x => x.Id == id);
            return g == null ? null : MapDto(g);
        }

        public async Task<GastoDto> Crear(GastoCreateDto dto)
        {
            var ahora = AhoraColombia();

            var fechaHoy = ahora.ToString("yyyyMMdd");
            var prefix = $"GAS-{fechaHoy}-";
            var ultimoNumero = await _context.Gastos
                .Where(g => g.NumeroGasto != null && g.NumeroGasto.StartsWith(prefix))
                .OrderByDescending(g => g.NumeroGasto)
                .Select(g => g.NumeroGasto)
                .FirstOrDefaultAsync();

            int nextNum = 1;
            if (ultimoNumero != null)
            {
                var numStr = ultimoNumero.Substring(prefix.Length);
                if (int.TryParse(numStr, out int num))
                    nextNum = num + 1;
            }
            var numeroGasto = $"{prefix}{nextNum:D4}";

            var gasto = new Gasto
            {
                TurnoId = dto.TurnoId,
                UsuarioId = dto.UsuarioId,
                NumeroGasto = numeroGasto,
                Concepto = dto.Concepto,
                Descripcion = dto.Descripcion,
                Monto = dto.Monto,
                Categoria = dto.Categoria,
                MetodoPagoId = dto.MetodoPagoId,
                EstadoId = 1,
                FechaGasto = ahora,  // ✅ siempre hora Colombia, ignorar dto.FechaGasto
                FechaCreacion = ahora
            };

            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();

            // ✅ Actualizar TotalGastos del turno
            await RecalcularTotalGastosTurno(dto.TurnoId);

            await _context.Entry(gasto).Reference(g => g.Usuario).LoadAsync();

            return MapDto(gasto);
        }

        public async Task<GastoDto> Actualizar(GastoUpdateDto dto)
        {
            var gasto = await _context.Gastos.FindAsync(dto.Id);
            if (gasto == null) throw new Exception("Gasto no encontrado");

            gasto.Concepto = dto.Concepto;
            gasto.Descripcion = dto.Descripcion;
            gasto.Monto = dto.Monto;
            gasto.Categoria = dto.Categoria;
            gasto.MetodoPagoId = dto.MetodoPagoId;
            gasto.Notas = dto.Notas;
            gasto.Comprobante = dto.Comprobante;
            gasto.FechaGasto = dto.FechaGasto ?? gasto.FechaGasto;

            await _context.SaveChangesAsync();

            // ✅ Actualizar TotalGastos del turno
            await RecalcularTotalGastosTurno(gasto.TurnoId);

            return MapDto(gasto);
        }

        public async Task<bool> Eliminar(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null) return false;

            var turnoId = gasto.TurnoId;
            _context.Gastos.Remove(gasto);
            await _context.SaveChangesAsync();

            // ✅ Actualizar TotalGastos del turno
            await RecalcularTotalGastosTurno(turnoId);

            return true;
        }

        public async Task<List<GastoDto>> ObtenerHoy(int? usuarioId)
        {
            var hoy = AhoraColombia().Date;
            var manana = hoy.AddDays(1);

            var query = _context.Gastos
                .Include(g => g.Usuario)
                .Where(g => g.FechaGasto >= hoy && g.FechaGasto < manana)
                .Where(g => g.EstadoId == 1 || g.EstadoId == null);

            if (usuarioId.HasValue)
                query = query.Where(g => g.UsuarioId == usuarioId.Value);

            return await query
                .OrderByDescending(g => g.FechaGasto)
                .Select(g => new GastoDto
                {
                    Id = g.Id,
                    Descripcion = g.Concepto,
                    Monto = g.Monto,
                    Categoria = g.Categoria,
                    Hora = g.FechaGasto.HasValue ? g.FechaGasto.Value.ToString("hh:mm tt") : null,
                    Fecha = g.FechaGasto,
                    MetodoPagoId = g.MetodoPagoId,
                    MetodoPago = null,
                    Observaciones = g.Notas,
                    UsuarioId = g.UsuarioId,
                    UsuarioNombre = g.Usuario != null ? g.Usuario.Nombre : "",
                    NumeroGasto = g.NumeroGasto
                })
                .ToListAsync();
        }

        public async Task<GastoKpiDto> ObtenerKpis(DateTime? fecha)
        {
            var hoy = (fecha ?? AhoraColombia()).Date;
            var manana = hoy.AddDays(1);

            var gastosHoy = await _context.Gastos
                .Where(g => g.FechaGasto >= hoy && g.FechaGasto < manana)
                .Where(g => g.EstadoId == 1 || g.EstadoId == null)
                .ToListAsync();

            return new GastoKpiDto
            {
                TotalGastosDia = gastosHoy.Sum(g => g.Monto),
                CantidadGastosDia = gastosHoy.Count,
                TotalNomina = gastosHoy.Where(g => g.Categoria == "Nómina").Sum(g => g.Monto),
                TotalServicios = gastosHoy.Where(g => g.Categoria == "Servicios Básicos").Sum(g => g.Monto),
                TotalMantenimiento = gastosHoy.Where(g => g.Categoria == "Mantenimiento").Sum(g => g.Monto)
            };
        }

        public async Task<bool> Anular(int id, string notas)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null) return false;

            var turnoId = gasto.TurnoId;
            gasto.EstadoId = 2;
            if (!string.IsNullOrEmpty(notas))
                gasto.Notas = notas;

            await _context.SaveChangesAsync();

            // ✅ Actualizar TotalGastos del turno
            await RecalcularTotalGastosTurno(turnoId);

            return true;
        }
    }
}