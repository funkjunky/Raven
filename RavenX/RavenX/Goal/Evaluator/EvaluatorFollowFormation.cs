#region Namespace imports

#region System
using System;
using System.Collections.Generic;
#endregion

#region Microsoft

using Microsoft.Xna.Framework;

#endregion

#region GarageGames

using GarageGames.Torque.Core;

#endregion

#region Mindcrafters
using Mindcrafters.RavenX.Goal.Composite;
using Mindcrafters.RavenX.Entity.Bot;
#endregion

#endregion


namespace Mindcrafters.RavenX.Goal.Evaluator
{

    class EvaluatorFollowFormation : Evaluator
    {
        const float myMAXDISTANCE = 1414.2f; //(float)Math.Sqrt(1000 * 1000 * 2);    //assuming the size of the field is less than, but close to 1000 by 1000.

        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors
        public EvaluatorFollowFormation(float characterBias)
            : base(characterBias)
        {
            CurrentFormation = null;
        }

        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        public Formation CurrentFormation 
        {
            get { return _currentFormation; }
            set { _currentFormation = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        //returns a score between 0 and 1 representing the desirability of the
        //strategy the concrete subclass represents
        //TODO: The desirability should also be affected by the number of teammates found. 
        //Currently I'm not precisely sure of an easy way to get the number of teammates in the game. 
        //I need to find a global that stores all the players, and go from there.
        public override float CalculateDesirability(BotEntity bot)
        {
            CurrentFormation = bot.CurrentFormation;
            /////
            //If the bot is already prepared to follow formation, and is now checking to see if it wants out.
            /////
            /////
            //If the bot is already prepared to follow formation, and is now checking to see if it is leader and ready to do formation!
            /////
            //leader
            //all members are following, and leader is ready. So leader should be standing around, while the members are alreadyin their following state.
            if (bot.FormationStatus == FormationStatuses.Ready && CurrentFormation.Leader == bot && !CurrentFormation.allMembersPreparing())
            {
                //check to see if all bots are ready. 
                bool allReady = true;
                foreach (BotEntity friend in CurrentFormation.Members)
                    if (friend.FormationStatus != FormationStatuses.Ready)
                        allReady = false;

                //If so set status to leader and send a message to all bots that the formation is ready to start. 
                if (allReady)
                {
                    bot.FormationStatus = FormationStatuses.Leading;
                    return 1 * _characterBias * 1000; //I really want this to win for testing BEWAREBEWAARE! BEEP BEEP BEEP WARNING!
                    //TODO: SEND MESSAGE HERE.
                    //send a message to all bots in CurrentFormation.Members

                }
                //They should all set status to following. and the setGoal should set this to stand still. Or just repeatidly clear subgoals.
            }
            //followers
            //if I set the goal here, then I can let a bot finish what he was doing before joining with the group. 
            //People may be waiting for him, it all depend on the desirability.
            if (bot.FormationStatus == FormationStatuses.Following || bot.FormationStatus == FormationStatuses.Leading)
            {
                //the setGoal will sort out the proper goal to assign.
                //set max desirability
                return 1 * _characterBias * 1000; //I really want this to win for testing BEWAREBEWAARE! BEEP BEEP BEEP WARNING!
            }

            float closestDistSoFar = Single.MaxValue;
            /////
            //If the bot has already received a message to form with teammates. [this can't be the leader, because the leader will never have their status set to NotReady, while having a formation]
            /////
            if (this.CurrentFormation != null && bot.FormationStatus == FormationStatuses.NotReady)
            {
                closestDistSoFar =
                    (CurrentFormation.Leader.Position - bot.Position).LengthSquared();

                closestDistSoFar /= CurrentFormation.Members.Count; //thie dist will go down, the desirability will go up according to the number of members in the formation.
                closestDistSoFar /= CurrentFormation.membersReady();    //also increase the desirability by number of members ready.
            }
            /////
            //If the bot hasn't already received a message to form with teammates.
            /////
            else
            {
                bot = null;

                //grab a list of all the opponents the owner can sense
                List<BotEntity> sensedBots
                      = bot.SensoryMemory.GetListOfRecentlySensedOpponents();

                BotEntity currentFriendlyTarget = null;

                foreach (BotEntity curBot in sensedBots)
                {
                    //make sure the bot is alive and that it is not the owner, and it is the same team.
                    if (!curBot.IsAlive || (curBot == bot) || curBot.Team != bot.Team)
                        continue;

                    float dist =
                        (curBot.Position - bot.Position).LengthSquared();

                    if (dist >= closestDistSoFar)
                        continue;

                    closestDistSoFar = dist;
                    currentFriendlyTarget = curBot;
                }

                //if we never found a target than closestDist will still be max value and we return 0 desirability.
                if (closestDistSoFar == Single.MaxValue)
                {
                    return 0;
                }
            }
            float desirability = (float)Math.Pow((closestDistSoFar / myMAXDISTANCE), -1.0f);  //the further it is the closer the fraction will be to 1, so take the inverse for desirability.

            //ensure the value is in the range 0 to 1
            desirability = (float)MathHelper.Clamp(desirability, 0, 1);

            desirability *= _characterBias;

            return desirability;
        }

        //adds the appropriate goal to the given bot's brain
        public override void SetGoal(BotEntity bot)
        {
            if(CurrentFormation == null || CurrentFormation.Leader == bot)  //is leader
            {
                                //create formation, then send it to all members of the formation, then await reply.
                if (bot.FormationStatus == FormationStatuses.NotReady)
                {
                    //grab a list of all the players the owner can sense
                    List<BotEntity> sensedBots
                            = bot.SensoryMemory.GetListOfRecentlySensedOpponents();

                    //cull that list to just the Bots on the same team.
                    List<BotEntity> friendlyBots
                            = sensedBots.FindAll(delegate(BotEntity abot) { return abot.Team == bot.Team; });

                    switch (friendlyBots.Count)
                    {
                        case 1:
                            CurrentFormation = new Formation(FormationType.BackToBack, bot, 1, friendlyBots);
                            break;
                        default:
                            CurrentFormation = new Formation(FormationType.Line, bot, 1, friendlyBots);
                            break;
                    }
                    if (CurrentFormation == null)
                        return;

                    bot.FormationStatus = FormationStatuses.Ready;
                    bot.CurrentFormation = CurrentFormation;

                    uint count = 2;
                    foreach (BotEntity abot in friendlyBots)
                    {
                        //clone formation, then change the position
                        Formation temp = new Formation(CurrentFormation);
                        temp.Position = count++;
                        abot.CurrentFormation = temp;
                    }
                }
                else if (bot.FormationStatus == FormationStatuses.Ready)
                {
                    bot.FormationStatus = FormationStatuses.NotReady;
                    throw new Exception("Not implemented yet! EvaluatorFollowFormation->SetGoal");
                }
                else if (bot.FormationStatus == FormationStatuses.Ready && CurrentFormation.Leader == bot && CurrentFormation.allMembersFollowing())
                    bot.Brain.RemoveAllSubgoals();  //this should hopefully make the bot stand still.
                else if (bot.FormationStatus == FormationStatuses.Leading)
                    bot.Brain.AddGoalLeadFormation();
            }
            else
            {
                if (bot.FormationStatus == FormationStatuses.NotReady)
                    bot.FormationStatus = FormationStatuses.Ready;
                else if (bot.FormationStatus == FormationStatuses.Ready)
                    bot.FormationStatus = FormationStatuses.NotReady;
                else if (bot.FormationStatus == FormationStatuses.Following)
                    bot.Brain.AddGoalFollowFormation(_currentFormation);
            }
        }

        public override void RenderInfo(Vector2 position, BotEntity bot)
        {
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods
        #endregion

        //======================================================================
        #region Private, protected, internal fields

        protected Formation _currentFormation;

        #endregion
    }
}
