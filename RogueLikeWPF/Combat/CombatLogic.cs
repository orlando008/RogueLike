using System;
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
            if(_playerEntity.GetCurrentDEX() > _enemyEntity.GetCurrentDEX())
            {
                _playerEntity.BeginTurn();
            }
            else if(_playerEntity.GetCurrentDEX() < _enemyEntity.GetCurrentDEX())
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

            if(isPlayer)
            {
                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Baseline movement point awarded.  Movement +1.", System.Windows.Media.Colors.LightPink));
                _playerEntity.MovementPointAdjustment(1);

                if (_playerEntity.GetCurrentDEX() > _enemyEntity.GetCurrentDEX())
                {
                    roll1PercentageChance = 75;
                }
                else
                {
                    roll1PercentageChance = 60;
                }

                if (_playerEntity.GetCurrentINT() > _enemyEntity.GetCurrentINT())
                {
                    roll2PercentageChance = 75;
                }
                else
                {
                    roll2PercentageChance = 50;
                }

                if (_ovMap.RNG.Next(1, 101) < roll1PercentageChance)
                {
                    //success
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First movement roll @" + roll1PercentageChance + "% ... succeeds.  Movement +1.", System.Windows.Media.Colors.LightPink));
                    _playerEntity.MovementPointAdjustment(1);

                    if (_ovMap.RNG.Next(1, 101) < roll2PercentageChance)
                    {
                        //success
                        _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Second movement roll @" + roll2PercentageChance + "% ... succeeds.  Movement +1.", System.Windows.Media.Colors.LightPink));
                        _playerEntity.MovementPointAdjustment(1);
                    }
                    else
                    {
                        _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Second movement roll @" + roll2PercentageChance + " % ... fails.", System.Windows.Media.Colors.LightPink));
                    }
                }
                else
                {
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First movement roll @" + roll1PercentageChance + " % ... fails.", System.Windows.Media.Colors.LightPink));
                }

                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Movement roll phase complete.  Movement points now stand at " + _playerEntity.GetCurrentMovementPoints().ToString() + ".", System.Windows.Media.Colors.LightPink));
            }
            else
            {
                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Baseline movement point awarded.  Movement +1.", System.Windows.Media.Colors.LightPink));
                _enemyEntity.MovementPointAdjustment(1);

                if (_enemyEntity.GetCurrentDEX() > _playerEntity.GetCurrentDEX())
                {
                    roll1PercentageChance = 75;
                }
                else
                {
                    roll1PercentageChance = 60;
                }

                if (_enemyEntity.GetCurrentINT() > _playerEntity.GetCurrentINT())
                {
                    roll2PercentageChance = 75;
                }
                else
                {
                    roll2PercentageChance = 50;
                }

                if (_ovMap.RNG.Next(1, 101) < roll1PercentageChance)
                {
                    //success
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First movement roll @" + roll1PercentageChance + "% ... succeeds.  Movement +1.", System.Windows.Media.Colors.LightPink));
                    _enemyEntity.MovementPointAdjustment(1);

                    if (_ovMap.RNG.Next(1, 101) < roll2PercentageChance)
                    {
                        //success
                        _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Second movement roll @" + roll2PercentageChance + "% ... succeeds.  Movement +1.", System.Windows.Media.Colors.LightPink));
                        _enemyEntity.MovementPointAdjustment(1);
                    }
                }
                else
                {
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First movement roll @" + roll1PercentageChance + " % ... fails.", System.Windows.Media.Colors.LightPink));
                }

                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Movement roll phase complete.  Movement points now stand at " + _enemyEntity.GetCurrentMovementPoints().ToString() + ".", System.Windows.Media.Colors.LightPink));
            }
        }

        public void AwardActionPoints(bool isPlayer)
        {
            int roll1PercentageChance;
            int roll2PercentageChance;

            if (isPlayer)
            {
                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Baseline action point awarded.  Action +1.", System.Windows.Media.Colors.LightPink));
                _playerEntity.ActionPointAdjustment(1);

                if (_playerEntity.GetCurrentSTR() > _enemyEntity.GetCurrentSTR())
                {
                    roll1PercentageChance = 60;
                }
                else
                {
                    roll1PercentageChance = 50;
                }

                if (_playerEntity.GetCurrentINT() > _enemyEntity.GetCurrentINT())
                {
                    roll2PercentageChance = 50;
                }
                else
                {
                    roll2PercentageChance = 25;
                }

                if (_ovMap.RNG.Next(1, 101) < roll1PercentageChance)
                {
                    //success
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First action roll @" + roll1PercentageChance + "% ... succeeds.  Action +1.", System.Windows.Media.Colors.LightPink));
                    _playerEntity.ActionPointAdjustment(1);

                    if (_ovMap.RNG.Next(1, 101) < roll2PercentageChance)
                    {
                        //success
                        _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Second action roll @" + roll2PercentageChance + "% ... succeeds.  Action +1.", System.Windows.Media.Colors.LightPink));
                        _playerEntity.ActionPointAdjustment(1);
                    }
                    else
                    {
                        _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Second action roll @" + roll2PercentageChance + " % ... fails.", System.Windows.Media.Colors.LightPink));
                    }
                }
                else
                {
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First action roll @" + roll1PercentageChance + " % ... fails.", System.Windows.Media.Colors.LightPink));
                }

                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Action roll phase complete.  Action points now stand at " + _playerEntity.GetCurrentMovementPoints().ToString() + ".", System.Windows.Media.Colors.LightPink));
            }
            else
            {
                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Baseline action point awarded.  Action +1.", System.Windows.Media.Colors.LightPink));
                _enemyEntity.ActionPointAdjustment(1);

                if (_enemyEntity.GetCurrentSTR() > _playerEntity.GetCurrentSTR())
                {
                    roll1PercentageChance = 60;
                }
                else
                {
                    roll1PercentageChance = 50;
                }

                if (_enemyEntity.GetCurrentINT() > _playerEntity.GetCurrentINT())
                {
                    roll2PercentageChance = 50;
                }
                else
                {
                    roll2PercentageChance = 25;
                }

                if (_ovMap.RNG.Next(1, 101) < roll1PercentageChance)
                {
                    //success
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First action roll @" + roll1PercentageChance + "% ... succeeds.  Action +1.", System.Windows.Media.Colors.LightPink));
                    _enemyEntity.ActionPointAdjustment(1);

                    if (_ovMap.RNG.Next(1, 101) < roll2PercentageChance)
                    {
                        //success
                        _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Second action roll @" + roll2PercentageChance + "% ... succeeds.  Action +1.", System.Windows.Media.Colors.LightPink));
                        _enemyEntity.ActionPointAdjustment(1);
                    }
                }
                else
                {
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("First action roll @" + roll1PercentageChance + " % ... fails.", System.Windows.Media.Colors.LightPink));
                }

                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("Action roll phase complete.  Action points now stand at " + _enemyEntity.GetCurrentMovementPoints().ToString() + ".", System.Windows.Media.Colors.LightPink));
            }
        }

        public void ProcessCombatEntityMovement(bool isPlayer, int direction)
        {
            if(isPlayer)
            {
                if (_playerEntity.GetCurrentMovementPoints() > 0)
                {
                    if(direction == -1)
                    {
                        if (_playerEntity.GetCombatPosition() > 1)
                        {
                            if (_enemyEntity.GetCombatPosition() != _playerEntity.GetCombatPosition() - 1)
                            {
                                _playerEntity.MovementPointAdjustment(-1);
                                _playerEntity.CombatPositionAdjustment(-1);
                                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("You back up 1 step.", System.Windows.Media.Colors.LightPink));
                            }
                            else
                            {
                                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("You cannot move there, the enemy is taking up the area.", System.Windows.Media.Colors.LightPink));
                            }

                        }
                        else
                        {
                            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("You cannot move there, a hard wall prevents you.", System.Windows.Media.Colors.LightPink));
                        }
                    }
                    else
                    {
                        if (_playerEntity.GetCombatPosition() < 11)
                        {
                            if (_enemyEntity.GetCombatPosition() != _playerEntity.GetCombatPosition() + 1)
                            {
                                _playerEntity.MovementPointAdjustment(-1);
                                _playerEntity.CombatPositionAdjustment(1);
                                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("You advanced 1 step.", System.Windows.Media.Colors.LightPink));
                            }
                            else
                            {
                                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("You cannot move there, the enemy is taking up the area.", System.Windows.Media.Colors.LightPink));
                            }

                        }
                        else
                        {
                            _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("You cannot move there, a hard wall prevents you.", System.Windows.Media.Colors.LightPink));
                        }
                    }

                }
                else
                {
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("You have no movement points remaining.", System.Windows.Media.Colors.LightPink));
                }
            }

        }

        public void ProcessCombatEntityAction(bool isPlayer, CombatAction ca)
        {
            if(isPlayer)
            {
                if(_playerEntity.GetCurrentActionPoints() > 0)
                {
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs($"You used {ca.Name}.", System.Windows.Media.Colors.LightPink));
                    _playerEntity.ActionPointAdjustment(-1);
                }
                else
                    _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs("You have no action points remaining.", System.Windows.Media.Colors.LightPink));
            }
            else
            {
                _ovMap.OnStoryMessage(new Program.StoryMessageEventArgs($"Enemy used {ca.Name}.", System.Windows.Media.Colors.LightPink));
            }
        }
    }

    public interface ICombatEntity
    {
        int GetCurrentHealthPoints();
        int GetCurrentActionPoints();
        int GetCurrentMovementPoints();
        int GetCurrentSTR();
        int GetCurrentDEX();
        int GetCurrentINT();
        int GetCombatPosition();
        void InitializeBattleValues();
        void ActionPointAdjustment(int numberOfPoints);
        void MovementPointAdjustment(int numberOfPoints);
        void BeginTurn();
        void GiveEntityTheCombatLogic(CombatLogic clog);
        void CombatPositionAdjustment(int adjustment);
    }
}
