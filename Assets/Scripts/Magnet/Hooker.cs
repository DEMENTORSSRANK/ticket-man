using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Element;
using UnityEngine;
using UnityEngine.UI;

namespace Magnet
{
    public class Hooker : MonoBehaviour
    {
        public Transform ropeHolder;

        public Transform magnetToFollow;

        public Transform hooker;

        public Transform parentCoins;

        public Vector2 offsetFollow;

        public float hookSpeed = 3;

        public float distanceToGetCoin = .5f;

        public float offsetWaitToHook = 1.3f;

        public float endYPos = -4;

        public PoseForCoin[] posesForCoin;

        public List<Coin> allCoins;

        public List<Coin> currentCoinsInHook = new List<Coin>();

        public MeshCollider colliderTube;

        private float startYPos;

        public static Hooker instance;

        [SerializeField] private BoxCollider stopHook;

        [SerializeField] private Button interactButton;

        public PoseForCoin[] FreePosesForCoin => posesForCoin.Where(x => !x.IsBusy).ToArray();

        public TypeMove CurrentTypeMove { get; private set; }

        public bool GotStartPosition { get; set; }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            startYPos = ropeHolder.position.y;

            for (var i = 0; i < parentCoins.childCount; i++)
                allCoins.Add(parentCoins.GetChild(i).GetComponent<Coin>());
        }

        public void DropRope()
        {
            StartCoroutine(DropingRope());
        }

        private void FixedUpdate()
        {
            FollowMagnet();

            GoingDown();

            GoingUp();

            CheckingGetCoins();
        }

        private void FollowMagnet()
        {
            if (CurrentTypeMove != TypeMove.FollowMagnet && CurrentTypeMove != TypeMove.GoingBack &&
                CurrentTypeMove != TypeMove.BackPosition)
                return;

            var ropePosition = ropeHolder.position;

            var magnetPosition = magnetToFollow.position;

            ropePosition.x = magnetPosition.x + offsetFollow.x;

            ropePosition.z = magnetPosition.z + offsetFollow.y;

            ropeHolder.position = ropePosition;
        }

        private void GoingDown()
        {
            if (CurrentTypeMove != TypeMove.GoingDown)
                return;

            var ropePosition = ropeHolder.position;

            ropePosition += Vector3.down * hookSpeed * Time.deltaTime;

            ropeHolder.position = ropePosition;
        }

        private void GoingUp()
        {
            if (CurrentTypeMove != TypeMove.GoingUp)
                return;

            var ropePosition = ropeHolder.position;

            ropePosition += Vector3.up * hookSpeed * Time.deltaTime;

            ropeHolder.position = ropePosition;
        }

        private void CheckingGetCoins()
        {
            return;

            foreach (var coin in allCoins)
            {
                if (currentCoinsInHook.Contains(coin))
                    return;

                var distance = Vector3.Distance(coin.transform.position, hooker.position);

                var isRightDistance = distance <= distanceToGetCoin;

                if (!isRightDistance)
                    continue;

                var randPos = FreePosesForCoin[Random.Range(0, FreePosesForCoin.Length)];

                randPos.IsBusy = true;

                coin.transform.position = randPos.Position;

                coin.transform.SetParent(randPos.transform);

                coin.transform.localPosition = Vector3.zero;

                coin.transform.localEulerAngles = Vector3.zero;

                currentCoinsInHook.Add(coin);
            }
        }

        private IEnumerator DropingRope()
        {
            interactButton.interactable = false;

            posesForCoin.ToList().ForEach(x => x.IsBusy = false);

            GotStartPosition = false;

            CurrentTypeMove = TypeMove.GoingDown;

            yield return new WaitUntil(() => ropeHolder.position.y <= endYPos);

            CurrentTypeMove = TypeMove.WaitingHook;

            allCoins = allCoins.Where(x => x != null).OrderBy(x =>
                Vector3.Distance(hooker.position, x.transform.position)).ToList();

            yield return new WaitForSeconds(Random.Range(.1f, .2f));

            for (var i = 0; i < 1; i++)
            {
                var coin = allCoins[i];

                if (Vector2.Distance(hooker.position, coin.transform.position) > distanceToGetCoin)
                    continue;

                var randPos = FreePosesForCoin[0];

                randPos.IsBusy = true;

                coin.transform.position = randPos.Position;

                coin.transform.SetParent(randPos.transform);

                coin.transform.localPosition = Vector3.zero;

                coin.transform.localEulerAngles = Vector3.zero;

                currentCoinsInHook.Add(coin);
            }

            yield return new WaitForSeconds(offsetWaitToHook);

            CurrentTypeMove = TypeMove.GoingUp;

            yield return new WaitUntil(() => ropeHolder.position.y >= startYPos);

            CurrentTypeMove = TypeMove.GoingBack;

            yield return new WaitUntil(() => GotStartPosition);

            yield return new WaitForSeconds(2f);

            // colliderTube.enabled = false;

            foreach (var coin in currentCoinsInHook)
            {
                coin.IsGravity = true;

                allCoins.Remove(coin);
            }

            currentCoinsInHook.Clear();

            GotStartPosition = false;

            CurrentTypeMove = TypeMove.BackPosition;

            yield return new WaitUntil(() => GotStartPosition);

            yield return new WaitForSeconds(1);

            CurrentTypeMove = TypeMove.FollowMagnet;

            interactButton.interactable = true;

            colliderTube.enabled = true;
        }

        public enum TypeMove
        {
            FollowMagnet,
            GoingDown,
            GoingUp,
            WaitingHook,
            GoingBack,
            BackPosition
        }
    }
}