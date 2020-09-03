using UnityEngine;

namespace Magnet
{
    public class MagnetMove : MonoBehaviour
    {
        public float maxPosX = 5;

        public float maxPosZ = 5;

        [Space(5)] 
    
        [Range(.01f, 10)] public float moveSpeed = 2;
    
        [Space(10)]

        public Transform mover;

        public Transform lining;

        [Space(5)] 
    
        public Vector2 startPos;

        private static FixedJoystick Joystick => FixedJoystick.Instance;
    
        private static Hooker Hooker => Hooker.instance;

        private void Update()
        {
            DoMove();
        
            GoingToStartPos();
        }

        private void GoingToStartPos()
        {
            if (Hooker.CurrentTypeMove != Hooker.TypeMove.GoingBack)
                return;

            var liningPosition = lining.position;
        
            var targetPosition = new Vector3(startPos.x, liningPosition.y, startPos.y);
        
            liningPosition = Vector3.MoveTowards(liningPosition, targetPosition, 
                moveSpeed * Time.deltaTime);
        
            lining.position = liningPosition;

            // Update mover pos
            var moverPosition = mover.position;

            moverPosition.x = liningPosition.x;

            mover.position = moverPosition;

            if (liningPosition == targetPosition)
                Hooker.instance.GotStartPosition = true;
        }
    
        private void DoMove()
        {
            if (Hooker.CurrentTypeMove != Hooker.TypeMove.FollowMagnet)
                return;
        
            var direction = new Vector3(Joystick.Direction.x, 0, Joystick.Direction.y);
        
            var speedModifer = moveSpeed * Time.deltaTime;
        
            // Update magnet pos
            var liningPosition = lining.position;

            liningPosition += direction * speedModifer;

            liningPosition.x = Mathf.Clamp(liningPosition.x, -maxPosX, maxPosX);

            liningPosition.z = Mathf.Clamp(liningPosition.z, -maxPosZ, maxPosZ);

            lining.transform.position = liningPosition;
        
            // Update mover pos
            var moverPosition = mover.position;

            moverPosition.x = liningPosition.x;

            mover.position = moverPosition;
        }
    }
}
