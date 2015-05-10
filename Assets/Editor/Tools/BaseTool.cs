using UnityEngine;
using System.Collections;

namespace FFMProject
{
    abstract class BaseTool
    {
        public virtual void Start()
        {
            Debug.Log("BaseTool.Start()");
        }

        public virtual void End()
        {
            Debug.Log("BaseTool.End()");
        }

        public virtual void Update(ModelEngine engine)
        {
        }

        public abstract void OnLMBDown(ModelEngine engine);
        public abstract void OnLMBUp(ModelEngine engine);
        public abstract void OnMouseDrag(ModelEngine engine);
        public abstract void OnMouseMove(ModelEngine engine);
        public abstract void Display(ModelEngine engine);
    }
}