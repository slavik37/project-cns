using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Action = Interfaces.Action;

namespace WorldService
{
    public class WorldService : IWorldService
    {
        public const string TheBug = "TheBug";
        private readonly Dictionary<string, ICreature> _creatures;
        private readonly Dictionary<string, CreatureState> _creaturesStates;
        private readonly Dictionary<string, Action> _pendingActions;


        public WorldService()
        {
            _pendingActions = new Dictionary<string, Action>();
            _creatures = new Dictionary<string, ICreature>
            {
                {TheBug, new Bug()}
            };

            _creaturesStates = new Dictionary<string, CreatureState>
            {
                {TheBug, new CreatureState(50.0, 50.0, 0.0)}
            };
        }


        public void DoStep()
        {
            GetActions();
            CalculateStep();
            UpdateSensors();
        }

        private void GetActions()
        {
            foreach (var creature in _creatures)
            {
                var actions = creature.Value.GetActions();
                if (actions != null && actions.Any())
                    foreach (var action in actions)
                        _pendingActions.Add(creature.Key, action);
            }
        }

        private void CalculateStep()
        {
            foreach (var pendingAction in _pendingActions)
            {
                switch (pendingAction.Value.ActionType)
                {
                    case ActionType.Rotate:
                    {
                        var state = _creaturesStates[pendingAction.Key];
                        state.Angle += pendingAction.Value.Value;
                        _creaturesStates[pendingAction.Key] = state;
                    }
                        break;
                    case ActionType.Move:
                    {
                        var state = _creaturesStates[pendingAction.Key];
                        state.X += pendingAction.Value.Value*Math.Cos(state.Angle);
                        state.Y += pendingAction.Value.Value*Math.Sin(state.Angle);
                        _creaturesStates[pendingAction.Key] = state;
                    }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void UpdateSensors()
        {
            foreach (var creature in _creatures)
                creature.Value.UpdateSensors();
        }
    }

    struct CreatureState
    {
        public double X;
        public double Y;
        public double Angle;

        public CreatureState(double x, double y, double angle) : this()
        {
            X = x;
            Y = y;
            Angle = angle;
        }
    }
}
