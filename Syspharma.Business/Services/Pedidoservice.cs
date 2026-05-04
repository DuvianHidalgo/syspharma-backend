using Syspharma.Data.Repositories;
using Syspharma.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syspharma.Business.Services
{
    public interface IPedidoService
    {
        Task<List<PedidoDto>> ObtenerTodos();
        Task<PedidoDto?> ObtenerPorId(int id);
        Task<PedidoDto> Crear(PedidoCreateDto dto);
        Task<PedidoDto> Actualizar(PedidoUpdateDto dto);
        Task<bool> CambiarEstado(int id, int estadoId);
        Task<bool> Eliminar(int id);
        Task<List<object>> ObtenerEstados();
    }

    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _repo;
        public PedidoService(IPedidoRepository repo) => _repo = repo;

        public async Task<List<PedidoDto>> ObtenerTodos()
        {
            try
            {
                return await _repo.ObtenerTodos();
            }
            catch (Exception ex)
            {
                // Registra el error en la consola del backend
                Console.WriteLine($"Error crítico en PedidoService: {ex.Message}");
                // Devuelve una lista vacía para que el .filter() de React no falle
                return new List<PedidoDto>();
            }
        }

        public Task<PedidoDto?> ObtenerPorId(int id) => _repo.ObtenerPorId(id);
        public Task<PedidoDto> Crear(PedidoCreateDto dto) => _repo.Crear(dto);
        public Task<PedidoDto> Actualizar(PedidoUpdateDto dto) => _repo.Actualizar(dto);
        public Task<bool> CambiarEstado(int id, int estadoId) => _repo.CambiarEstado(id, estadoId);
        public Task<bool> Eliminar(int id) => _repo.Eliminar(id);
        public Task<List<object>> ObtenerEstados() => _repo.ObtenerEstados();
    }
}