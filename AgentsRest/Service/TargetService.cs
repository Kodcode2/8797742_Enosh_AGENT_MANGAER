using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;

namespace AgentsRest.Service
{
    public class TargetService(ApplicationDbContext context, IServiceProvider serviceProvider) : ITargetService
    {

        private IMissionService _missionService => serviceProvider.GetRequiredService<IMissionService>();
            public async Task<TargetModel?> CreateNewTargetAsync(TargetDto targetDto)
        {
            var exists = await context.Targets.Where(target => target.Image == targetDto.PhotoUrl).ToListAsync();
            if (exists.Count > 0) { throw new Exception($" Target with the Image {targetDto.PhotoUrl} is alraedy exists"); }
            TargetModel targetModel = new TargetModel()
            {
                TargetName = targetDto.Name,
                Position = targetDto.Position,
                Image = targetDto.PhotoUrl
            };
            await context.Targets.AddAsync(targetModel);
            await context.SaveChangesAsync();
            return targetModel;
        }

        public async Task<TargetModel?> FindTargetByIdAsync(int id)
        {
            TargetModel? target = await context.Targets.FirstOrDefaultAsync(target => target.Id == id);
            if (target == null) { return null; }
            return target;
        }

        public async Task<List<TargetModel>> GetAllTargetsAsync()
        {
            return await context.Targets.ToListAsync();
        }
        private readonly Dictionary<string, (int, int)> deriction = new()
        {
            {"nw" , (-1,1) },
            {"n" , (0,1) },
            { "ne" , (1,1) },
            { "w" , (-1,0) },
            { "e" , (1,0) },
            { "sw" , (-1,-1) },
            { "s" , (0,-1) },
            { "se" , (1,-1) },
        };

        public async Task UpdateTargetAsync(int id, LocationDto locationDto)
        {
            var target = await FindTargetByIdAsync(id);
            if (target == null) { throw new Exception($" Target with the id {id} dosent exists"); }
            target.X = locationDto.X;
            target.Y = locationDto.Y;
            await context.SaveChangesAsync();
        }

        public bool CheackIfOutOfMatriza(int x,int y )
        {
            if (x < -1 || y < -1 || x > 1000 || y > 1000)
            {

            return true; 
            }
            return false;
        }

        public async Task UpdateTargetDirectionAsync(int id, DirectionDto direction)
        {
            TargetModel target = await FindTargetByIdAsync(id) 
            ?? throw new Exception($" Target with the id {id} dosent exists");
            if (target.Status == TargetStatus.Alive)
            {


                bool isExists = deriction.TryGetValue(direction.Direction, out var result);
                if (!isExists)
                {
                    throw new Exception($" The diraction {direction.Direction} is not in Dict");
                }
                var (x, y) = result;

                if (CheackIfOutOfMatriza(target.X + x, target.Y + y))
                {
                    throw new Exception($" You cant go out from the matriza");
                }
                target.X += x;
                target.Y += y;
                await _missionService.CheakIfHaveMissionNotRleventTarget(target);
                await _missionService.CheakIfHaveMatchTarget(target);
                await context.SaveChangesAsync();
            }
        }
    }
}
