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

        public void AwardMovementPoints()
        {

        }

        public void AwardActionPoints()
        {

        }

        public void ProcessCombatEntityMovement()
        {

        }

        public void ProcessCombatEntityAction()
        {

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
        void InitializeBattleValues();
        void ActionPointAdjustment(int numberOfPoints);
        void MovementPointAdjustment(int numberOfPoints);
        void BeginTurn();
        void GiveEntityTheCombatLogic(CombatLogic clog);
    }
}
