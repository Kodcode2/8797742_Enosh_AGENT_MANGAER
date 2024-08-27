using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;

namespace AgentsRest.Service
{
    public class TargetService(ApplicationDbContext context, IServiceProvider serviceProvider) : ITargetService
    {
        // מביא את הסרבר של משימות
        private IMissionService _missionService => serviceProvider.GetRequiredService<IMissionService>();
        // מייצר מטרה חדשה
            public async Task<TargetModel?> CreateNewTargetAsync(TargetDto targetDto)
        {
            // נועד לבדיקה האם המטרה כבר קימת לפי תמונה
            var exists = await context.Targets.Where(target => target.Image == targetDto.PhotoUrl).ToListAsync();
            // היה אמור לבדוק אבל השרת סימולציה מכניס את אותם תמונות
            //if (exists.Count > 0) { throw new Exception($" Target with the Image {targetDto.PhotoUrl} is alraedy exists"); }

            // מייצר מודל של מטרה
            TargetModel targetModel = new TargetModel()
            {
                TargetName = targetDto.Name,
                Position = targetDto.Position,
                Image = targetDto.PhotoUrl
            };
            // מכניס לדאטאבאיס
            await context.Targets.AddAsync(targetModel);
            // שומר שינויים
            await context.SaveChangesAsync();
            return targetModel;
        }

        // מציאת מטרה לפי ID
        public async Task<TargetModel?> FindTargetByIdAsync(int id)
        {
            TargetModel? target = await context.Targets.FirstOrDefaultAsync(target => target.Id == id);
            // אם לא מוצא מחזיר NULL
            if (target == null) { return null; }
            // מחזיר מטרה
            return target;
        }
        // מביא את כל הסוכנים
        public async Task<List<TargetModel>> GetAllTargetsAsync()
        {
			// מביא את כל הסוכנים מהדאטאבאיס

			return await context.Targets.ToListAsync();
        }
        // יצירת מילון לכיוונים
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
        // מעדכן מיקום התחלתי של מטרה
        public async Task UpdateTargetAsync(int id, LocationDto locationDto)
        {
            // אם היא ךלא קיימת מחזיר שגיאה 
            var target = await FindTargetByIdAsync(id);
            if (target == null) { throw new Exception($" Target with the id {id} dosent exists"); }
            // מעדכן מיקום
            target.X = locationDto.X;
            target.Y = locationDto.Y;
            // שומר שינויים
            await context.SaveChangesAsync();
        }

        // בודק האם סוכן/מטרה מחוץ למטריצה
        public bool CheackIfOutOfMatriza(int x,int y )
        {
            if (x < -1 || y < -1 || x > 1000 || y > 1000)
            {

            return true; 
            }
            return false;
        }
        // מוזיז מטרות
        public async Task UpdateTargetDirectionAsync(int id, DirectionDto direction)
        {
            // בודק האם קיימת מטרה אם לא מחזיר שגיאה
            TargetModel target = await FindTargetByIdAsync(id) 
            ?? throw new Exception($" Target with the id {id} dosent exists");
            // בודק האם המטרה שאצה מנסה להוזיז חיה
            if (target.Status == TargetStatus.Alive)
            {

                // בודק האם המיקום טוה
                bool isExists = deriction.TryGetValue(direction.Direction, out var result);
                if (!isExists)
                {
                    throw new Exception($" The diraction {direction.Direction} is not in Dict");
                }
                // מכניס למשתנה כיוונים
                var (x, y) = result;
                // בודק שאם נשנה כיוון האם הוא יצא מהמטריצה
                if (CheackIfOutOfMatriza(target.X + x, target.Y + y))
                {
                    throw new Exception($" You cant go out from the matriza");
                }
                // משנה מיקום מטרה
                target.X += x;
                target.Y += y;
                // בודק האם יש משימות שלא רלוונטיות
                await _missionService.CheakIfHaveMissionNotRleventTarget(target);
                // בודק האם יש התאמות חדשות למשימות
                await _missionService.CheakIfHaveMatchTarget(target);
                // שומר שינויים
                await context.SaveChangesAsync();
            }
        }
    }
}
