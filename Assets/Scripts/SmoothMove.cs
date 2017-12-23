using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMove : MonoBehaviour
{
    private SmoothPosition position;
    private SmoothRotation rotation;

    private void Awake()
    {
        position = new SmoothPosition(this);
        rotation = new SmoothRotation(this);
    }

    private void Update()
    {
        position.Update();
        rotation.Update();
    }

    public SmoothPosition Position
    {
        get { return position; }
    }

    public SmoothRotation Rotation
    {
        get { return rotation; }
    }

    public abstract class AbstractMove<T>
    {
        public delegate void DoneHandler(GameObject gameObject);
        public event DoneHandler Done;
        public event DoneHandler DoneOnce;

        protected readonly SmoothMove parent;

        protected bool moving;
        protected T start;
        protected T target;

        protected float timeTaken;
        protected float duration;

        public AbstractMove(SmoothMove parent, T initial)
        {
            this.parent = parent;
            start = initial;
            target = initial;
        }

        public void Update()
        {
            if (moving)
            {
                timeTaken += Time.deltaTime;

                // Calculate the smoothing factor.
                float timePos = timeTaken / duration * Mathf.PI - (Mathf.PI / 2); // [-1/2 pi, 1/2 pi]
                float lerpFactor = (Mathf.Sin(timePos) + 1) / 2; // [0, 1]

                DoUpdate(lerpFactor);

                // Check if we are at the target.
                if (timeTaken >= duration)
                {
                    moving = false;

                    if (Done != null)
                        Done(parent.gameObject);

                    if (DoneOnce != null)
                        DoneOnce(parent.gameObject);
                    // Reset the event.
                    DoneOnce = null;
                }
            }
        }

        /// <summary>
        /// Initialize movement.
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="target">The target position.</param>
        /// <param name="duration">The duration of the movement.</param>
        protected void Initialize(T start, T target, float duration)
        {
            this.start = start;
            this.target = target;
            this.duration = duration;

            moving = true;
            timeTaken = 0;
        }

        /// <summary>
        /// Update the movement of the object.
        /// </summary>
        /// <param name="lerpFactor">A smoothened factor which can be used to linearly interpolate between the start and target.</param>
        protected abstract void DoUpdate(float lerpFactor);

        public T Target
        {
            get { return target; }
        }
    }

    public class SmoothPosition : AbstractMove<Vector3>
    {
        public SmoothPosition(SmoothMove parent) : base(parent, parent.transform.localPosition) { }

        protected override void DoUpdate(float lerpFactor)
        {
            parent.transform.localPosition = Vector3.Lerp(start, target, lerpFactor);
        }

        /// <summary>
        /// Move to an absolute local position.
        /// </summary>
        /// <param name="position">The position to move to.</param>
        /// <param name="duration">The duration of the movement.</param>
        public void MoveTo(Vector3 position, float duration)
        {
            Initialize(parent.transform.localPosition, position, duration);
        }

        /// <summary>
        /// Move to an absolute global position.
        /// </summary>
        /// <param name="position">The position to move to.</param>
        /// <param name="duration">The duration of the movement.</param>
        public void MoveToGlobal(Vector3 position, float duration)
        {
            Vector3 local = position - (parent.transform.localPosition - parent.transform.position);
            MoveTo(local, duration);
        }

        /// <summary>
        /// Move relative to the current position.
        /// If the object is still moving, the movement will be calculated relative to the current target position.
        /// </summary>
        /// <param name="change">The change in position.</param>
        /// <param name="duration">The duration of the movement.</param>
        public void MoveRelative(Vector3 change, float duration)
        {
            Vector3 from = parent.transform.localPosition;
            float timeMultiplier = 1;

            // Account for unfinished movement.
            if (moving)
            {
                from = target;
                if (timeTaken != 0)
                    timeMultiplier = timeTaken / base.duration;
            }

            MoveTo(from + change, duration * timeMultiplier);
        }
    }

    public class SmoothRotation : AbstractMove<Quaternion>
    {
        public SmoothRotation(SmoothMove parent) : base(parent, parent.transform.localRotation) { }

        protected override void DoUpdate(float lerpFactor)
        {
            parent.transform.localRotation = Quaternion.Lerp(start, target, lerpFactor);
        }

        /// <summary>
        /// Rotate to an absolute local rotation.
        /// </summary>
        /// <param name="rotation">The rotation to rotate to.</param>
        /// <param name="duration">The duration of the rotation.</param>
        public void RotateTo(Quaternion rotation, float duration)
        {
            Initialize(parent.transform.localRotation, rotation, duration);
        }
    }
}
