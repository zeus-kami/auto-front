using auto_front.Models;
using auto_front.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using auto_front.Lib;

namespace auto_front.Services
{
    public class CollaboratorService
    {
        private readonly BusContext _context;

        public CollaboratorService(BusContext context)
        {
            _context = context;
        }

        public async Task<IResponse> CreateCollaborator(Collaborator collaborator)
        {
            if (string.IsNullOrWhiteSpace(collaborator.Name) ||
                string.IsNullOrWhiteSpace(collaborator.Email) ||
                string.IsNullOrWhiteSpace(collaborator.Phone) ||
                string.IsNullOrWhiteSpace(collaborator.BusId.ToString()) ||
                collaborator.BusId == Guid.Empty)
                return IResponse.Error("Dados do colaborador inválidos.");

            var bus = await _context.Buses.FindAsync(collaborator.BusId);
            if (bus == null)
                return IResponse.Error("Ônibus associado não encontrado.");

            collaborator.Bus = bus;
            _context.Collaborators.Add(collaborator);
            await _context.SaveChangesAsync();
            return IResponse.Ok(collaborator, "Colaborador criado com sucesso.");
        }

        public async Task<IResponse> GetCollaboratorById(Guid id)
        {
            if (id == Guid.Empty)
                return IResponse.Error("ID inválido.");

            var collaborator = await _context.Collaborators
                .Include(c => c.Bus)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (collaborator == null)
                return IResponse.Error("Colaborador não encontrado.");

            return IResponse.Ok(collaborator);
        }

        public async Task<IResponse> GetCollaborators()
        {
            var collaborators = await _context.Collaborators
                .Include(c => c.Bus)
                .Select(c => new
                Collaborator(
                    c.Id,
                    c.Name,
                    c.Email,
                    c.Phone,
                    c.BusId,
                    new Bus(c.Bus.Id, c.Bus.LicensePlate, c.Bus.Brand, c.Bus.Model, c.Bus.Capacity, c.Bus.Year)
                ))
                .ToListAsync();

            if (collaborators == null || collaborators.Count == 0)
                return IResponse.Error("Nenhum colaborador encontrado.");

            return IResponse.Ok(collaborators);
        }

        public async Task<IResponse> UpdateCollaborator(Guid id, Collaborator updated)
        {
            if (id == Guid.Empty)
                return IResponse.Error("ID inválido.");

            var collaborator = await _context.Collaborators.FindAsync(id);
            if (collaborator == null)
                return IResponse.Error("Colaborador não encontrado.");

            if (string.IsNullOrWhiteSpace(updated.Name) ||
                string.IsNullOrWhiteSpace(updated.Email) ||
                string.IsNullOrWhiteSpace(updated.Phone) ||
                updated.BusId == Guid.Empty)
            {
                return IResponse.Error("Dados do colaborador inválidos.");
            }

            var bus = await _context.Buses.FindAsync(updated.BusId);
            if (bus == null)
                return IResponse.Error("Ônibus associado não encontrado.");

            collaborator.Name = updated.Name;
            collaborator.Email = updated.Email;
            collaborator.Phone = updated.Phone;
            collaborator.BusId = updated.BusId;
            collaborator.Bus = bus;

            _context.Collaborators.Update(collaborator);
            await _context.SaveChangesAsync();
            return IResponse.Ok(collaborator, "Colaborador atualizado com sucesso.");
        }

        public async Task<IResponse> DeleteCollaborator(Guid id)
        {
            if (id == Guid.Empty)
                return IResponse.Error("ID inválido.");

            var collaborator = await _context.Collaborators.FindAsync(id);
            if (collaborator == null)
                return IResponse.Error("Colaborador não encontrado.");

            _context.Collaborators.Remove(collaborator);
            await _context.SaveChangesAsync();
            return IResponse.Ok(null, "Colaborador removido com sucesso.");
        }
    }
}
