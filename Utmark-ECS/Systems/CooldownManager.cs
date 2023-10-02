namespace Utmark_ECS.Systems
{
    using System;
    using System.Collections.Generic;

    public class CooldownManager
    {
        private readonly Dictionary<string, CooldownEntry> _cooldowns = new();

        public void ActivateCooldown(string actionId, float durationInSeconds)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(durationInSeconds);
            if (_cooldowns.ContainsKey(actionId))
            {
                _cooldowns[actionId].ExpirationTime = expirationTime;
            }
            else
            {
                _cooldowns[actionId] = new CooldownEntry { ExpirationTime = expirationTime };
            }
        }

        public bool IsCooldownExpired(string actionId)
        {
            if (!_cooldowns.ContainsKey(actionId)) return true; // No cooldown entry found, can execute the action.

            var cooldownEntry = _cooldowns[actionId];
            var isExpired = DateTime.UtcNow >= cooldownEntry.ExpirationTime;

            if (isExpired)
            {
                _cooldowns.Remove(actionId); // Cleanup expired cooldowns
            }

            return isExpired;
        }

        private class CooldownEntry
        {
            public DateTime ExpirationTime { get; set; }
        }
    }

}

