using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;

namespace Syspharma.Data.Repositories
{
    public interface IGastoRepository
    {
        Task<List<GastoDto>> ObtenerTodos();
        Task<List<GastoDto>> ObtenerPorTurno(int turnoId);
        Task<List<GastoDto>> ObtenerHoy(int? usuarioId);
        Task<GastoDto?> ObtenerPorId(int id);
        Task<GastoDto> Crear(GastoCreateDto dto);
        Task<GastoDto> Actualizar(GastoUpdateDto dto);
        Task<bool> Eliminar(int id);
<<<<<<< Updated upstream
        Task<List<GastoDto>> ObtenerHoy(int? usuarioId);
        Task<GastoKpiDto> ObtenerKpis(DateTime? fecha);
        Task<bool> Anular(int id, string notas);
=======
        Task<bool> Anular(int id, string? motivo);
>>>>>>> Stashed changes
    }

    public class GastoRepository : IGastoRepository
    {
        private readonly SyspharmaContext _context;

        private static readonly TimeZoneInfo ColombiaZone =
            TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");

        private static DateTime AhoraColombia() =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ColombiaZone);

        public GastoRepository(SyspharmaContext context) => _context = context;

        private async Task RecalcularTotalGastosTurno(int turnoId)
        {
            var turno = await _context.Turnos.FindAsync(turnoId);
            if (turno == null) return;

            turno.TotalGastos = await _context.Gastos
                .Where(g => g.TurnoId == turnoId)
                .SumAsync(g => g.Monto);

            await _context.SaveChangesAsync();
        }

        private static GastoDto MapDto(Gasto g) => new GastoDto
        {
            Id = g.Id,
            TurnoId = g.TurnoId,
            UsuarioId = g.UsuarioId,
            UsuarioNombre = g.Usuario?.Nombre ?? "",
            Concepto = g.Concepto,
            Descripcion = g.Descripcion,
            Monto = g.Monto,
            Categoria = g.Categoria,
            Comprobante = g.Comprobante,
            FechaGasto = g.FechaGasto,
<<<<<<< Updated upstream
=======
            Anulado = g.Anulado,
            FechaAnulacion = g.FechaAnulacion,
            MotivoAnulacion = g.MotivoAnulacion
>>>>>>> Stashed changes
        };

        public async Task<List<GastoDto>> ObtenerTodos() =>
            await _context.Gastos
                .Include(g => g.Usuario)
                .OrderByDescending(g => g.FechaGasto)
                .Select(g => MapDto(g))
                .ToListAsync();

        public async Task<List<GastoDto>> ObtenerPorTurno(int turnoId) =>
<<<<<<< Updated upstream
            await _context.Gastos
                .Include(g => g.Usuario)
                .Where(g => g.TurnoId == turnoId)
                .Select(g => MapDto(g))
                .ToListAsync();
=======
            (await _context.Gastos.Include(g => g.Usuario).Where(g => g.TurnoId == turnoId && !g.Anulado).ToListAsync()).Select(MapDto).ToList();

        public async Task<List<GastoDto>> ObtenerHoy(int? usuarioId)
        {
            var hoy = DateTime.Today;
            var query = _context.Gastos.Include(g => g.Usuario)
                .Where(g => g.FechaGasto.HasValue && g.FechaGasto.Value.Date == hoy && !g.Anulado);

            if (usuarioId.HasValue)
                query = query.Where(g => g.UsuarioId == usuarioId.Value);

            return (await query.OrderByDescending(g => g.FechaGasto).ToListAsync())
                .Select(MapDto).ToList();
        }
>>>>>>> Stashed changes

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

            var gasto = new Gasto
            {
                TurnoId = dto.TurnoId,
                UsuarioId = dto.UsuarioId,
                Concepto = dto.Concepto,
                Descripcion = dto.Descripcion,
                Monto = dto.Monto,
                Categoria = dto.Categoria,
                Comprobante = dto.Comprobante,
                FechaGasto = ahora
            };

            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();

            await RecalcularTotalGastosTurno(dto.TurnoId);
            await _context.Entry(gasto).Reference(g => g.Usuario).LoadAsync();

            return MapDto(gasto);
        }

<<<<<<< Updated upstream
        public async Task<GastoDto> Actualizar(GastoUpdateDto dto)
        {
            var gasto = await _context.Gastos.FindAsync(dto.Id)
                ?? throw new Exception("Gasto no encontrado");

            gasto.Concepto = dto.Concepto;
            gasto.Descripcion = dto.Descripcion;
            gasto.Monto = dto.Monto;
            gasto.Categoria = dto.Categoria;
            gasto.Comprobante = dto.Comprobante;
            gasto.FechaGasto = dto.FechaGasto ?? gasto.FechaGasto;

            await _context.SaveChangesAsync();
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

            await RecalcularTotalGastosTurno(turnoId);
            return true;
        }

        public async Task<List<GastoDto>> ObtenerHoy(int? usuarioId)
        {
            var hoy = AhoraColombia().Date;
            var manana = hoy.AddDays(1);

            var query = _context.Gastos
                .Include(g => g.Usuario)
                .Where(g => g.FechaGasto >= hoy && g.FechaGasto < manana);

            if (usuarioId.HasValue)
                query = query.Where(g => g.UsuarioId == usuarioId.Value);

            return await query
                .OrderByDescending(g => g.FechaGasto)
                .Select(g => new GastoDto
                {
                    Id = g.Id,
                    Concepto = g.Concepto,
                    Descripcion = g.Descripcion,
                    Monto = g.Monto,
                    Categoria = g.Categoria,
                    Comprobante = g.Comprobante,
                    FechaGasto = g.FechaGasto,
                    Hora = g.FechaGasto.HasValue
                        ? g.FechaGasto.Value.ToString("hh:mm tt")
                        : null,
                    UsuarioId = g.UsuarioId,
                    UsuarioNombre = g.Usuario != null ? g.Usuario.Nombre : "",
                })
                .ToListAsync();
        }

        public async Task<GastoKpiDto> ObtenerKpis(DateTime? fecha)
        {
            var hoy = (fecha ?? AhoraColombia()).Date;
            var manana = hoy.AddDays(1);

            var gastosHoy = await _context.Gastos
                .Where(g => g.FechaGasto >= hoy && g.FechaGasto < manana)
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
            // Como no existe EstadoId en la tabla, anular = eliminar físicamente
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null) return false;

            var turnoId = gasto.TurnoId;
            _context.Gastos.Remove(gasto);
            await _context.SaveChangesAsync();

            await RecalcularTotalGastosTurno(turnoId);
=======
        public async Task<GastoDto> Actualizar(GastoUpdateDto dto) { return null; }
        public async Task<bool> Eliminar(int id) { return true; }

        public async Task<bool> Anular(int id, string? motivo)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null) return false;
            gasto.Anulado = true;
            gasto.FechaAnulacion = DateTime.Now;
            gasto.MotivoAnulacion = motivo;
            await _context.SaveChangesAsync();
>>>>>>> Stashed changes
            return true;
        }
    }
}
