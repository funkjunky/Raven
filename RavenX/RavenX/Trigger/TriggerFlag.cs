using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Entity.Objects;

namespace Mindcrafters.RavenX.Trigger
{
    public class TriggerFlag : Trigger
    {
        /// <summary>
        /// where the flag's home is
        /// </summary>
        private readonly Vector2 _homePosition;

        /// <summary>
        /// which team this flag belongs to
        /// </summary>
        private readonly GameManager.GameManager.Teams _team;

        /// <summary>
        /// is the flag currently stolen
        /// </summary>
        private bool _stolen;

        /// <summary>
        /// main construtor
        /// </summary>
        /// <param name="flagData">owner team</param>
        public TriggerFlag(FlagData flagData)
            : base(new Flag((GameManager.GameManager.Teams)flagData.Team))
        {
            Name = flagData.Name;
            Position = flagData.Position;
            BoundingRadius = flagData.Radius;
            NodeIndex = flagData.NodeIndex;
            _homePosition = flagData.Position;
            _team = (GameManager.GameManager.Teams)flagData.Team;

            //create this trigger's region of influence
            AddCircularTriggerRegion(
                Position,
                GameManager.GameManager.Instance.Parameters.DefaultGiverTriggerRange);

            EntityType = EntityTypes.Flag;
            IsActive = true;
        }

        public override void Try(BotEntity bot)
        {
            if (!IsActive ||
                !IsTouchingTrigger(bot.Position, bot.BoundingRadius) ||
                _stolen)
                return;

            if(bot.Team == _team)
            {
                if(bot.IsCarryingFlag)
                {
                    //TODO: what do we do when we capture another teams flag?
                    bot.ScoreFlag();
                }
                return;
            }

            TriggeringBot = bot;
            bot.IsCarryingFlag = _stolen = true;
        }

        public override void Update(float dt)
        {
            if (_stolen)
            {
                if (TriggeringBot != null && !TriggeringBot.IsCarryingFlag)
                {
                    SceneObject.Position = _homePosition;
                    TriggeringBot = null;
                    _stolen = false;
                }
                else
                {
                    SceneObject.Position = TriggeringBot.SceneObject.Position;
                }
            }
        }
    }
}