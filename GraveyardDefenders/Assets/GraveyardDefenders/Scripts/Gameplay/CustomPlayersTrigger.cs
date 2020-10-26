using UnityEngine;
using XD.Events;
using System.Collections.Generic;

namespace XD
{
    public class CustomPlayersTrigger : MonoBehaviour
    {
        public Evnt<PlayerCharacter> onPlayerEnter = new Evnt<PlayerCharacter>();
        public Evnt<PlayerCharacter> onPlayerExit = new Evnt<PlayerCharacter>();
        public Evnt onAllPlayersInside = new Evnt();

        public List<PlayerCharacter> playersInside = new List<PlayerCharacter>();

        private void OnTriggerEnter(Collider other)
        {
            PlayerCharacter pc = GetPlayer(other);
            if (pc != null)
            {
                if(!playersInside.Contains(pc))
                    playersInside.Add(pc);

                onPlayerEnter.Invoke(pc);
                if (playersInside.Count == PlayerInput.Instance.CurrentPlayerCount)
                    onAllPlayersInside.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerCharacter pc = GetPlayer(other);
            if (pc != null)
            {
                if (playersInside.Contains(pc))
                    playersInside.Remove(pc);

                onPlayerExit.Invoke(pc);
            }
        }

        PlayerCharacter GetPlayer(Collider other)
        {
            return other.GetComponent<PlayerCharacter>();
        }
    }
}