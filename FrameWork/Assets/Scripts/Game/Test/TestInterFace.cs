using System;
using System.Collections.Generic;
using Knight.Core;
using UnityEngine;

namespace Game.Battle
{
    public interface IManagerInterface
    {
        public void Initialize();
        public void LogicUpdate();
        public void RenderUpdate();
        public void Destroy();
    }

    public abstract class ManagerBase
    {
        public abstract void Initialize();
    }

    public class FishM : ManagerBase
    {
        public override void  Initialize()
        {
            
        }
    }

    public class Human
    {
        public int Hungry = 100;

        public Human()
        {
             this.Hungry = 1;
        }
        public virtual void Eat()
        {
            this.Hungry += 1;
        }
    }

    public class Student:Human
    {
        //public new int Hungry = 1;
        public Student()
        {
            this.Hungry = 100;
        }

        public override void Eat()
        {
            base.Eat();
        }
    }

    public class FishManager : IManagerInterface
    {
        public void Initialize()
        {
            Human a = new Student();
            a.Eat();
        }

        public void LogicUpdate()
        {
        }

        public void RenderUpdate()
        {
        }

        public void Destroy()
        {
        }
    }

    public enum ManagerState
    {
        Fish
    }
    public class Managers
    {
        public Dictionary<ManagerState, IManagerInterface> ManagerDic = new Dict<ManagerState, IManagerInterface>();

        public void Test()
        {
            this.Add(ManagerState.Fish,new FishManager());
        }

        public void Add(ManagerState rState,IManagerInterface rIns)
        {
            this.ManagerDic.Add(rState,rIns);
        }
    }

    public class TestInterFace : MonoBehaviour
    {
        public void Start()
        {
            var B = new Human();
            Debug.Log(B.Hungry);
            B.Eat();
            Debug.Log(B.Hungry);
            
            var A = new Student();
            Debug.Log(A.Hungry);
            A.Eat();
            Debug.Log(A.Hungry);
            Debug.Log(B.Hungry);
        }
    }
}