﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows
{
    public class CombatLogic
    {
        ICombatEntity _playerEntity;
        ICombatEntity _enemyEntity;
        private OverallMap _ovMap;

        public CombatLogic(OverallMap ovMap)
        {
            _playerEntity = ovMap.ThePlayer;
            _enemyEntity = ovMap.CurrentCombatUnit;
            _ovMap = ovMap;
            
            _playerEntity.GiveEntityTheCombatLogic(this);
            _enemyEntity.GiveEntityTheCombatLogic(this);
        }

        public void InitiateBattle()
        {
            _playerEntity.InitializeBattleValues();
            _enemyEntity.InitializeBattleValues();

            //Figure out who goes first
            if(_playerEntity.GetCombatProperties().BattleDEX > _enemyEntity.GetCombatProperties().BattleDEX)
            {
                _playerEntity.BeginTurn();
            }
            else if(_playerEntity.GetCombatProperties().BattleDEX < _enemyEntity.GetCombatProperties().BattleDEX)
            {
                _enemyEntity.BeginTurn();
            }
            else
            {
                if (_ovMap.RNG.Next(1, 3) == 1)
                    _playerEntity.BeginTurn();
                else
                    _enemyEntity.BeginTurn();
            }
        }

        public void AwardMovementPoints(bool isPlayer)
        {
            int roll1PercentageChance;
            int roll2PercentageChance;

            ICombatEntity tmpCurrentEntity = (isPlayer ? _playerEntity : _enemyEntity);
            ICombatEntity tmpOppositionEntity = (isPlayer ? _enemyEntity : _playerEntity);

            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Baseline movement point awarded.  Movement +1.", System.Windows.Media.Colors.LightPink));
            tmpCurrentEntity.GetCombatProperties().CurrentMovementPoints += 1;

            if (tmpCurrentEntity.GetCombatProperties().BattleDEX > tmpOppositionEntity.GetCombatProperties().BattleDEX)
            {
                roll1PercentageChance = 75;
            }
            else
            {
                roll1PercentageChance = 60;
            }

            if (tmpCurrentEntity.GetCombatProperties().BattleINT > tmpOppositionEntity.GetCombatProperties().BattleINT)
            {
                roll2PercentageChance = 60;
            }
            else
            {
                roll2PercentageChance = 50;
            }

            roll1PercentageChance += tmpCurrentEntity.GetCombatProperties().PercentIncreaseToMovementRolls;
            roll2PercentageChance += tmpCurrentEntity.GetCombatProperties().PercentIncreaseToMovementRolls;

            if (_ovMap.RNG.Next(1, 101) < roll1PercentageChance)
            {
                //success
                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First movement roll @" + roll1PercentageChance + "% ... succeeds.  Movement +1.", System.Windows.Media.Colors.LightPink));
                tmpCurrentEntity.GetCombatProperties().CurrentMovementPoints += 1;

                if (_ovMap.RNG.Next(1, 101) < roll2PercentageChance)
                {
                    //success
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Second movement roll @" + roll2PercentageChance + "% ... succeeds.  Movement +1.", System.Windows.Media.Colors.LightPink));
                    tmpCurrentEntity.GetCombatProperties().CurrentMovementPoints += 1;
                }
                else
                {
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Second movement roll @" + roll2PercentageChance + " % ... fails.", System.Windows.Media.Colors.LightPink));

                    if (tmpCurrentEntity.GetCombatProperties().PercentChanceToReRollFailedMovement > 0)
                    {
                        if (_ovMap.RNG.Next(1, 101) < tmpCurrentEntity.GetCombatProperties().PercentChanceToReRollFailedMovement)
                        {
                            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve movement roll @" + roll2PercentageChance + " % was awarded due to equipment 'granting % chance to re-roll failed movement'.", System.Windows.Media.Colors.LightPink));
                            if (_ovMap.RNG.Next(1, 101) < roll2PercentageChance)
                            {
                                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve movement roll ... succeeds.  Movement +1.", System.Windows.Media.Colors.LightPink));
                                tmpCurrentEntity.GetCombatProperties().CurrentMovementPoints += 1;
                            }
                            else
                            {
                                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve movement roll ... fails.", System.Windows.Media.Colors.LightPink));
                            }
                        }
                    }
                }
            }
            else
            {
                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First movement roll @" + roll1PercentageChance + " % ... fails.", System.Windows.Media.Colors.LightPink));

                if (tmpCurrentEntity.GetCombatProperties().PercentChanceToReRollFailedMovement > 0)
                {
                    if (_ovMap.RNG.Next(1, 101) < tmpCurrentEntity.GetCombatProperties().PercentChanceToReRollFailedMovement)
                    {
                        _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve movement roll @" + roll1PercentageChance + " % was awarded due to equipment 'granting % chance to re-roll failed movement'.", System.Windows.Media.Colors.LightPink));
                        if (_ovMap.RNG.Next(1, 101) < roll1PercentageChance)
                        {
                            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve movement roll ... succeeds.  Movement +1.", System.Windows.Media.Colors.LightPink));
                            tmpCurrentEntity.GetCombatProperties().CurrentMovementPoints += 1;
                        }
                        else
                        {
                            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve movement roll ... fails.", System.Windows.Media.Colors.LightPink));
                        }
                    }
                }
            }

            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Movement roll phase complete.  Movement points now stand at " + tmpCurrentEntity.GetCombatProperties().CurrentMovementPoints.ToString() + ".", System.Windows.Media.Colors.LightPink));
        }

        public void AwardActionPoints(bool isPlayer)
        {
            int roll1PercentageChance;
            int roll2PercentageChance;
            ICombatEntity tmpCurrentEntity = (isPlayer ? _playerEntity : _enemyEntity);
            ICombatEntity tmpOppositionEntity = (isPlayer ? _enemyEntity : _playerEntity);

            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Baseline action point awarded.  action +1.", System.Windows.Media.Colors.LightPink));
            tmpCurrentEntity.GetCombatProperties().CurrentActionPoints += 1;

            if (tmpCurrentEntity.GetCombatProperties().BattleSTR > tmpOppositionEntity.GetCombatProperties().BattleSTR)
            {
                roll1PercentageChance = 60;
            }
            else
            {
                roll1PercentageChance = 50;
            }

            if (tmpCurrentEntity.GetCombatProperties().BattleINT > tmpOppositionEntity.GetCombatProperties().BattleINT)
            {
                roll2PercentageChance = 60;
            }
            else
            {
                roll2PercentageChance = 50;
            }

            roll1PercentageChance += tmpCurrentEntity.GetCombatProperties().PercentIncreaseToActionRolls;
            roll2PercentageChance += tmpCurrentEntity.GetCombatProperties().PercentIncreaseToActionRolls;

            if (_ovMap.RNG.Next(1, 101) < roll1PercentageChance)
            {
                //success
                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First action roll @" + roll1PercentageChance + "% ... succeeds.  action +1.", System.Windows.Media.Colors.LightPink));
                tmpCurrentEntity.GetCombatProperties().CurrentActionPoints += 1;

                if (_ovMap.RNG.Next(1, 101) < roll2PercentageChance)
                {
                    //success
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Second action roll @" + roll2PercentageChance + "% ... succeeds.  action +1.", System.Windows.Media.Colors.LightPink));
                    tmpCurrentEntity.GetCombatProperties().CurrentActionPoints += 1;
                }
                else
                {
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Second action roll @" + roll2PercentageChance + " % ... fails.", System.Windows.Media.Colors.LightPink));

                    if (tmpCurrentEntity.GetCombatProperties().PercentChanceToReRollFailedAction > 0)
                    {
                        if (_ovMap.RNG.Next(1, 101) < tmpCurrentEntity.GetCombatProperties().PercentChanceToReRollFailedAction)
                        {
                            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve action roll @" + roll2PercentageChance + " % was awarded due to equipment 'granting % chance to re-roll failed action'.", System.Windows.Media.Colors.LightPink));
                            if (_ovMap.RNG.Next(1, 101) < roll2PercentageChance)
                            {
                                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve action roll ... succeeds.  action +1.", System.Windows.Media.Colors.LightPink));
                                tmpCurrentEntity.GetCombatProperties().CurrentActionPoints += 1;
                            }
                            else
                            {
                                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve action roll ... fails.", System.Windows.Media.Colors.LightPink));
                            }
                        }
                    }
                }
            }
            else
            {
                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First action roll @" + roll1PercentageChance + " % ... fails.", System.Windows.Media.Colors.LightPink));

                if (tmpCurrentEntity.GetCombatProperties().PercentChanceToReRollFailedAction > 0)
                {
                    if (_ovMap.RNG.Next(1, 101) < tmpCurrentEntity.GetCombatProperties().PercentChanceToReRollFailedAction)
                    {
                        _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve action roll @" + roll1PercentageChance + " % was awarded due to equipment 'granting % chance to re-roll failed action'.", System.Windows.Media.Colors.LightPink));
                        if (_ovMap.RNG.Next(1, 101) < roll1PercentageChance)
                        {
                            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve action roll ... succeeds.  action +1.", System.Windows.Media.Colors.LightPink));
                            tmpCurrentEntity.GetCombatProperties().CurrentActionPoints += 1;
                        }
                        else
                        {
                            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Reprieve action roll ... fails.", System.Windows.Media.Colors.LightPink));
                        }
                    }
                }
            }

            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Action roll phase complete.  action points now stand at " + tmpCurrentEntity.GetCombatProperties().CurrentActionPoints.ToString() + ".", System.Windows.Media.Colors.LightPink));
        }

        public void ProcessCombatEntityMovement(bool isPlayer, int direction)
        {
            ICombatEntity tmpCurrentEntity = (isPlayer ? _playerEntity : _enemyEntity);
            ICombatEntity tmpOppositionEntity = (isPlayer ? _enemyEntity : _playerEntity);

            if (tmpCurrentEntity.GetCombatProperties().CurrentMovementPoints > 0)
            {
                if (tmpCurrentEntity.GetCombatProperties().CombatPosition + direction == tmpOppositionEntity.GetCombatProperties().CombatPosition)
                {
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Cannot move there, a unit is taking up the area.", System.Windows.Media.Colors.LightPink));
                }
                else
                {
                    if ((tmpCurrentEntity.GetCombatProperties().CombatPosition + direction <= 0) || (tmpCurrentEntity.GetCombatProperties().CombatPosition + direction > 11))
                    {
                        _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Cannot move there.", System.Windows.Media.Colors.LightPink));
                    }
                    else
                    {
                        tmpCurrentEntity.GetCombatProperties().CurrentMovementPoints -= 1;
                        tmpCurrentEntity.GetCombatProperties().CombatPosition += direction;
                    }
                }
            }
            else
            {
                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("No movement points remaining.", System.Windows.Media.Colors.LightPink));
                return;
            }
     
            EndTurnIfEntityOutOfThingsToDo(isPlayer);
        }

        public void ProcessCombatEntityAction(bool isPlayer, CombatAction ca)
        {
            ICombatEntity tmpCurrentEntity = (isPlayer ? _playerEntity : _enemyEntity);
            ICombatEntity tmpOppositionEntity = (isPlayer ? _enemyEntity : _playerEntity);

            string pronoun = (isPlayer ? "You" : "Enemy");

            if (tmpCurrentEntity.GetCombatProperties().CurrentActionPoints > 0)
            {
                if (ca.Range > 0 && Math.Abs(tmpCurrentEntity.GetCombatProperties().CombatPosition - tmpOppositionEntity.GetCombatProperties().CombatPosition) > ca.Range)
                {
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs($"Must be within range {ca.Range} to use this action.", System.Windows.Media.Colors.LightPink));
                    return;
                }

                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs($"{pronoun} used {ca.Name}.", System.Windows.Media.Colors.LightPink));
                tmpCurrentEntity.GetCombatProperties().CurrentActionPoints -= 1;
            }
            else
            {
                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("No action points remaining.", System.Windows.Media.Colors.LightPink));
                return;
            }

            EndTurnIfEntityOutOfThingsToDo(isPlayer);
        }

        public void EndTurnIfEntityOutOfThingsToDo(bool isPlayer)
        {
            ICombatEntity tmpCurrentEntity = (isPlayer ? _playerEntity : _enemyEntity);

            if (tmpCurrentEntity.GetCombatProperties().CurrentMovementPoints == 0 &&
                tmpCurrentEntity.GetCombatProperties().CurrentActionPoints == 0)
            {
                EndTurn(isPlayer);
            }
        }

        public void EndTurn(bool isPlayer)
        {
            string pronoun = (isPlayer ? "Your" : "Enemy");

            ICombatEntity tmpCurrentEntity = (isPlayer ? _playerEntity : _enemyEntity);
            tmpCurrentEntity.GetCombatProperties().CurrentMovementPoints = 0;
            tmpCurrentEntity.GetCombatProperties().CurrentActionPoints = 0;

            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs($"***{pronoun} turn ends***", System.Windows.Media.Colors.LightPink));

            ICombatEntity tmpOppositionEntity = (isPlayer ? _enemyEntity : _playerEntity);
            tmpOppositionEntity.BeginTurn();
        }
    }

    public interface ICombatEntity
    {
        void InitializeBattleValues();
        void BeginTurn();
        void GiveEntityTheCombatLogic(CombatLogic clog);
        CombatProperties GetCombatProperties();
    }
}
