using System;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Sim.View
{
    public class PlayerLoopInjector
    {
        private Action<float> OnTick;
        
        public PlayerLoopInjector(Action<float> onTick)
        {
            OnTick = onTick;
            Inject();
        }
        
        
        //-------------------UNITYENGINE.LOWLEVEL.PLAYERLOOP---------------------
        
        //Unity runs EarlyUpdate -> FixedUpdate -> PreUpdate -> Update -> PreLateUpdate -> LateUpdate -> PostLateUpdate
        //Each of these is a subsystem, a named slot that unity fills with its own callbacks.
        //PlayerLoop lets users inject their own callbacks into that sequence.
        
        //API works like this:
        //1. Get the current loop: PlayerLoop.GetCurrentPlayerLoop()
        //2 .Find the subsystem I want to inject after. For this project, after EarlyUpdate
        //3. Add my callback as a new subsystem entry
        //4. Set the modified loop back: PlayerLoop.SetPlayerLoop(loop);
        
        //callback signature must match PlayerLoopSystem.updateDelegate.
        public void Inject()
        {
            //TO inject after EarlyUpdate, just modify the top-level array. Inserting custom subsystem right after EarlyUpdate entry.
            //So the algorithm then becomes:
            //1. Get the loop (PlayerLoop.GetCurrentPlayerLoop())
            //2. Find the index of EarlyUpdate in the top-level subsystem array
            //3. Insert a new PlayerLoopSystem at that index + 1
            //4. Set the loop back.
            
            //Get current loop
            PlayerLoopSystem currentLoop = PlayerLoop.GetCurrentPlayerLoop();
            
            //Create new empty loop
            PlayerLoopSystem[] newLoop = new PlayerLoopSystem[currentLoop.subSystemList.Length + 1];
            
            //Create my subsystem
            PlayerLoopSystem simulationSystem = new PlayerLoopSystem
            {
                type = typeof(SimulationUpdateTag),
                updateDelegate = SimulationRunner,
            };

            bool inserted = false;
            for (int i = 0; i < currentLoop.subSystemList.Length; i++)
            {
                //Before inserting, copy as is
                if (!inserted)
                {
                    newLoop[i] = currentLoop.subSystemList[i];
                    
                    if (currentLoop.subSystemList[i].type == typeof(EarlyUpdate))
                    {
                        newLoop[i + 1] = simulationSystem;
                        inserted = true;
                    }
                }
                else
                {
                    newLoop[i + 1] = currentLoop.subSystemList[i];
                }
            }

            if (!inserted)
                throw new NullReferenceException("There is no Early Update????????????");
            
            currentLoop.subSystemList = newLoop;
            PlayerLoop.SetPlayerLoop(currentLoop);
        }

        private void SimulationRunner()
        {
            OnTick?.Invoke(UnityEngine.Time.deltaTime);
        }
        
        private struct SimulationUpdateTag{}
    }
    
}