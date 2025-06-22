using auto_front.Models;
using auto_front.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using auto_front.Lib;

namespace auto_front.Services
{
    public class BusService
    {
        private readonly BusContext _context;

        public BusService(BusContext context)
        {
            _context = context;
        }

        public async Task<IResponse> CreateBus(Bus bus)
        {
            if (string.IsNullOrWhiteSpace(bus.LicensePlate) ||
                string.IsNullOrWhiteSpace(bus.Brand) ||
                string.IsNullOrWhiteSpace(bus.Model) ||
                bus.Capacity <= 0 ||
                bus.Year <= 1900)
            {
                return IResponse.Error("Dados do ónibus inválidos.");
            }

            bool exists = await _context.Buses.AnyAsync(b => b.LicensePlate == bus.LicensePlate);
            if (exists)
                return IResponse.Error("Já existe um ônibus com esta placa.");

            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();
            return IResponse.Ok(bus, "Ônibus criado com sucesso.");
        }

        public async Task<IResponse> GetBusById(Guid id)
        {
            if (id == Guid.Empty)
                return IResponse.Error("ID inválido.");

            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
                return IResponse.Error("Ônibus não encontrado.");

            return IResponse.Ok(bus);
        }

        public async Task<IResponse> GetBuses()
        {
            var buses = await _context.Buses
                .Select(b => new Bus(b.Id, b.LicensePlate, b.Brand, b.Model, b.Capacity, b.Year))
                .ToListAsync();

            if (buses == null || buses.Count == 0)
                return IResponse.Error("Nenhum ônibus encontrado.");

            return IResponse.Ok(buses);
        }

        public async Task<IResponse> UpdateBus(Guid id, Bus updatedBus)
        {
            if (id == Guid.Empty)
                return IResponse.Error("ID inválido.");

            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
                return IResponse.Error("Ônibus não encontrado.");

            if (string.IsNullOrWhiteSpace(updatedBus.LicensePlate) ||
                string.IsNullOrWhiteSpace(updatedBus.Brand) ||
                string.IsNullOrWhiteSpace(updatedBus.Model) ||
                updatedBus.Capacity <= 0 ||
                updatedBus.Year <= 1900)
            {
                return IResponse.Error("Dados do ônibus inválidos.");
            }

            // Check for duplicate license plate (excluding current bus)
            bool exists = await _context.Buses.AnyAsync(b => b.LicensePlate == updatedBus.LicensePlate && b.Id != id);
            if (exists)
                return IResponse.Error("Já existe um ônibus com esta placa.");

            bus.LicensePlate = updatedBus.LicensePlate;
            bus.Brand = updatedBus.Brand;
            bus.Model = updatedBus.Model;
            bus.Capacity = updatedBus.Capacity;
            bus.Year = updatedBus.Year;

            _context.Buses.Update(bus);
            await _context.SaveChangesAsync();
            return IResponse.Ok(bus, "Ônibus atualizado com sucesso.");
        }

        public async Task<IResponse> DeleteBus(Guid id)
        {
            if (id == Guid.Empty)
                return IResponse.Error("ID inválido.");

            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
                return IResponse.Error("Ônibus não encontrado.");

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();
            return IResponse.Ok(null, "Ônibus removido com sucesso.");
        }
    }
}
