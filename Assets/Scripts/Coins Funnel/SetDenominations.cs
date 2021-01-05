using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Element;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; protected set; }

    private void Awake()
    {
        Instance = this as T;
    }
}

namespace Coins_Funnel
{
    public class SetDenominations : Singleton<SetDenominations>
    {
        [SerializeField] private Coin[] allCoins;
        
        // Index from 0 to 4
        [SerializeField] private int[] denominationsCounts = new int[5];

        public int GetIndexOfDenomination(int denomination)
        {
            if (!denominationsCounts.Contains(denomination))
                return 0;

            var index = denominationsCounts.ToList().IndexOf(denomination);

            return index;
        }
        
        private void SetAllDenominations()
        {
            var allCounts = new List<int>();

            for (var i = 0; i < denominationsCounts.Length; i++)
            {
                for (var g = 0; g < denominationsCounts[i]; g++)
                {
                    var i1 = i;
                    
                    allCounts.Add(i1);
                }
            }
            
            MixList(allCounts);

            for (var i = 0; i < allCoins.Length; i++)
            {
                allCoins[i].SetDenomination(allCounts[i]);
            }
        }
        
        private void Start()
        {
            SetAllDenominations();
        }

        public static void MixList<TT>(ICollection<TT> list)
        {
            var r = new System.Random();

            var mixedList = new SortedList<int, TT>();

            foreach (var item in list)
                mixedList.Add(r.Next(), item);

            list.Clear();

            for (var i = 0; i < mixedList.Count; i++)
            {
                list.Add(mixedList.Values[i]);
            }
        }
    }
}